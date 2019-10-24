using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using MccDaq;
using ErrorDefs;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Ivi.Visa.Interop;
using System.IO.Ports;


namespace mfg_527
{
    public struct TestData
    {
        public int step;
        public string name;
        public string method_name;
        public bool qual;
        public int upper;
        public int lower;
        public int measurement;
        public bool pass;
        // Pointer to the function to invoke
        public MethodInfo testinfo;


        public TestData(int step, string name, string method_name, bool qual, int upper, int lower, int measurement, bool pass, MethodInfo function)
        {
            this.step = step;
            this.name = name;
            this.method_name = method_name;
            this.qual = qual;
            this.upper = upper;
            this.lower = lower;
            this.measurement = measurement;
            this.pass = pass;
            this.testinfo = function;
            
        }
    }
/******************************************************************************************************************************************
 *                                               Functional Test Class
 ******************************************************************************************************************************************/
    public partial class FunctionalTest
    {
        //Test specific data --> To be stored in results file and in database
        private string serial;             //Test serial number
        private string location;
        private int eqid;
        private int user_id;
        private DateTime date;
        private DateTime time;


        private GPIO _gpio;
        public List<TestData> Tests = new List<TestData>();
        private readonly ConcurrentQueue<string> _queue;
        private bool cancel_request = false;


        
        /************************************************************************************************************
         * Functional Test Class Constructor
         * 
         * Parameters: - ConcurrentQueue<string> _queue--> The message passing data structure used between the calling object (presumably GUI)
         *             - String serial--> The serial number of the board in which this test has been initialized
         * 
         * **********************************************************************************************************/
        public FunctionalTest(ConcurrentQueue<string> _queue, string serial, string location, int eqid, string user_id) {
            
            //Create the list of tests needed on startup
            this.GetTests();

            //Create the queue used for passing messages between threads
            this._queue = _queue;

            //Initialize class variables
            this.serial = serial;
            this._gpio = new GPIO();
        }
        /************************************************************************************************************
         * Functional Test Class Destructor
         * 
         * - Disconnects from the GPIO module, Power Supply, and Multimeter so that the next test can use the resources
         * 
         * **********************************************************************************************************/
        ~FunctionalTest()
        {
            _queue.Enqueue("Garbage collected");

            //TODO: Add the destructor tasks
        }

/******************************************************************************************************************************************
 *                  MESSAGE PASSING UTILITIES
 ******************************************************************************************************************************************/

        /************************************************************************************************************
         * ClearInput
         * 
         * Function: Clears the message buffer between the main thread and the test thread. 
         * 
         * Arguments: None
         * 
         * Returns: None
         * 
         * **********************************************************************************************************/
        private void ClearInput()
        {
            string message;
            while (!this._queue.IsEmpty)
            {
                //Queue is not empty. Pending inputs or a pending cancel. Need to check to see if it is a cancel
                this._queue.TryPeek(out message);
                if(message != "cancel")
                {
                    //If message is not a cancel, remove the message. Don't care if it's there 
                    this._queue.TryDequeue(out message);
                }
                else
                {
                    //Message is a cancel, need to exit function so that the program can read the message
                    this.cancel_request = true;
                    break;
                }

            }
            return;
        }
        /************************************************************************************************************
         * ReceiveInput
         * 
         * Function: Pops a message from the queue or waits until the queue has received a message.
         * 
         * Arguments: None
         * 
         * Returns: string message - message popped from the queue.
         * 
         * **********************************************************************************************************/
        private string ReceiveInput()
        {
            string message;

            while (this._queue.IsEmpty)
            {
                //Do nothing, block until a message is received.
            }
            this._queue.TryDequeue(out message);
            
            return message;

        }


/******************************************************************************************************************************************
*                                               TEST RUNNING FUNCTIONS
******************************************************************************************************************************************/

        /************************************************************************************************************
         * RunTest() - Runs the list of tests determined by the functional test
         * 
         * Parameters: - progress --> Progress interface variable. Indicates the percentage of the test that is complete
         *             - message  --> Progress interface variable. Used to update the text in the output box.  
         *             - TestList --> List of TestStep. Used to tell RunTest which tests need to be run.
         * **********************************************************************************************************/
        public void RunTest(IProgress<int> progress, IProgress<string> message, List<TestData> TestList)
        {
            int i = 0;
            string str;
            
            // For each test in the test list, run the function
            // Pass the message object to each test so that the tests can update the display as needed
            foreach(TestData test in TestList)
            {
                this._queue.TryDequeue(out str);
                if (str == "cancel")
                {
                    //progress.Report(100);
                    break;
                }
                else
                {
                    //TODO: Remove this delay when tests are added
                    Task.Delay(100).Wait();
                    var param = new object[] { message, test.upper, test.lower };
                    message.Report("Starting " + test.testinfo.Name);   //Indicate which test is being run
                    progress.Report((i * 100) / (TestList.Count)); // Indicate the progress made

                    test.testinfo.Invoke(this, param);

                    i++;
                }
            }
            return;
        }
        /************************************************************************************************************
         * Program() - Runs the programming section only
         * 
         * Parameters: - progress --> Progress interface variable. Indicates the percentage of the test that is complete
         *             - message  --> Progress interface variable. Used to update the text in the output box.  
         *             
         * **********************************************************************************************************/
        public bool  Program(IProgress<int> progress, IProgress<string> message)
        {
            //TODO: add code to Confirm that Flashpro and Uniflash are installed in the correct place

   
            bool success;
            if (this.CPLD_Program(message))
            {
                success = true;
                if (this.CPLD_Verify(message))
                {
                    success = true;
                    message.Report("CPLD programming done");
                    if (this.Hercules_Program(message))
                    {
                        success = true;
                        message.Report("Hercules program successful");
                        if (this.SOM_Program(message))
                        {
                            success = true;
                            message.Report("SOM Programmed okay");
                        }
                        else
                        {
                            success = false;
                            message.Report("Failed to program SOM");
                        }
                    }
                    else
                    {
                        success = false;
                        message.Report("Failed to upload code to herculues");
                    }
                }
                else
                {
                    success = false;
                    message.Report("CPLD verification failed");
                }
                
            }
            else
            {
                success = false;
                message.Report("CPLD program failed");
            }
        
            return success;
        }

/******************************************************************************************************************************************
 *                                               DATA ANALYSIS FUNCTIONS
 ******************************************************************************************************************************************/

        /*
         * GetTests
         * 
         * Function: Reads from the specs.txt file to generate a list of tests that are available to the user. This list is the tests that will
         * run during a full functional test.
         * 
         * Arguments: None
         * 
         * Returns: None - Update the class variable Tests
         *
         */
        private void GetTests()
        {
            //Get list of methods in this class
            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
           
            bool qual;
            MethodInfo function=null;
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Configuration\specs.txt"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int step_num = 0;
                string[] row = parser.ReadFields();

                while (!parser.EndOfData)
                {
                    row = parser.ReadFields();
                    string name = row[0];
                    string method_name = row[1];
                    if (row[2].ToLower() == "yes"){
                        qual = true;
                    }
                    else
                    {
                        qual = false;
                    }

                    int upper = Convert.ToInt32(row[3]);
                    int lower = Convert.ToInt32(row[4]);

                    for (int j = 0; j<methods.Length;j++){
                        
                        if (methods[j].Name == method_name){
                            function = methods[j];
                            break;
                        }
                    }
                    TestData step = new TestData(step_num, name, method_name, qual, upper, lower, 0, false, function);
                    this.Tests.Add(step);
                    step_num++;    
                }
            }
            return;
        }

    }
}
