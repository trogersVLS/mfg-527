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

namespace mfg_527
{
    
    public class NameValue
    {

        private string dataName;
        private string dataValue;

        public NameValue(string dataName, string dataValue)
        {
            DataName = dataName;
            DataValue = dataValue;
        }

        public string DataName
        {
            get { return dataName; }
            set { dataName = value; }
        }

        public string DataValue
        {
            get { return dataValue; }
            set { dataValue = value; }
        }

        public override string ToString()
        {
            return dataName;
        }

    }
    public class GPIO
    {
        MccDaq.MccBoard gpio_board;
        int numChannels;
        MccDaq.ErrorInfo err;
        MccDaq.DigitalPortType[] Ports = {DigitalPortType.FirstPortA, DigitalPortType.FirstPortB, DigitalPortType.FirstPortC,
                                          DigitalPortType.SecondPortA, DigitalPortType.SecondPortB, DigitalPortType.SecondPortCH, DigitalPortType.SecondPortCL,
                                          DigitalPortType.ThirdPortA, DigitalPortType.ThirdPortB, DigitalPortType.ThirdPortCH, DigitalPortType.ThirdPortCL };
        DigitalIO.clsDigitalIO dig_props = new DigitalIO.clsDigitalIO();
        public GPIO()
        {
            
            
            this.gpio_board = new MccDaq.MccBoard(0);
            
            InitUL();

                



            //this.gpio_board.DBitOut(PortNum, 3, DigitalLogicState.High );

        

        }

        public void setBit(DigitalPortType port, int bit, DigitalLogicState val)
        {
            this.gpio_board.DBitOut(port, bit, val);
        }

        public void setPort(DigitalPortType port, ushort val)
        {
            this.gpio_board.DOut(port, val);
        }

        private void InitUL()
        {
            //  Initiate error handling
            //   activating error handling will trap errors like
            //   bad channel numbers and non-configured conditions.
            //   Parameters:
            //     MccDaq.ErrorReporting.PrintAll :all warnings and errors encountered will be printed
            //     MccDaq.ErrorHandling.StopAll   :if an error is encountered, the program will stop

            clsErrorDefs.ReportError = MccDaq.ErrorReporting.PrintAll;
            clsErrorDefs.HandleError = MccDaq.ErrorHandling.StopAll;
            this.err = MccDaq.MccService.ErrHandling
                (ErrorReporting.PrintAll, ErrorHandling.StopAll);

            this.err = this.gpio_board.BoardConfig.GetDiNumDevs(out this.numChannels);



            if (this.numChannels != 0)
            {
                for (int i = 0; i < (numChannels - 1); i++)
                {
                    err = this.gpio_board.DConfigPort(Ports[i], DigitalPortDirection.DigitalOut);
                    this.setPort(Ports[i], 0);
                }
            }
            else
            {
                //GPIO is not configured
            }
        }
    }
    public class DMM
    {   

        public DMM()
        {

        }
    }
    public class PPS
    {
        public PPS()
        {

        }
    }
    public class TestStep
    {
        public int step;
        public string name;
        public bool qual;
        public int lowerBound;
        public int upperBound;
        public MethodInfo testinfo;
        

        public TestStep(int number, string name, bool qual, int lowerBound, int upperBound, MethodInfo function)
        {
            this.step = number;
            this.name = name;
            this.qual = qual;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.testinfo = function;
        }

    }
    public class TestResult
    {
        public TestResult(int number, string name, bool pass, int measure)
        {
            StepNumber = number;
            Name = name;
            Result = pass;
            Measurement = measure;
        }
        public TestResult(int number, string name, bool pass)
        {

        }

        public int StepNumber { get; set; }
        public string Name { get; set; }
        public bool Result { get; set; }
        public int Measurement { get; set; }
    }
    public class FunctionalTest
    {

        public string SerialNumber;             //Test serial number
        public string status_msg = "Ready";
        private PPS _pps;
        private DMM _dmm;
        private GPIO _gpio;
        public List<TestStep> Tests = new List<TestStep>();
        private readonly ConcurrentQueue<string> _queue;



        /************************************************************************************************************
         * Functional Test Class Constructor
         * 
         * Parameters: - ConcurrentQueue<string> _queue--> The message passing data structure used between the calling object (presumably GUI)
         *             - String serial--> The serial number of the board in which this test has been initialized
         * 
         * **********************************************************************************************************/
        public FunctionalTest(ConcurrentQueue<string> _queue, string serial) {
            
            //Create the list of tests needed on startup
            int y = this.getTests();

            //Create the queue used for passing messages between threads
            this._queue = _queue;

            //Initialize class variables
            this.SerialNumber = serial;

            //Connect to test equipment
            this._pps = new PPS();
            this._dmm = new DMM();
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
        /************************************************************************************************************
         * RunTest() - Runs the list of tests determined by the functional test
         * 
         * Parameters: - progress --> Progress interface variable. Indicates the percentage of the test that is complete
         *             - message  --> Progress interface variable. Used to update the text in the output box.  
         *             - TestList --> List of TestStep. Used to tell RunTest which tests need to be run.
         * **********************************************************************************************************/
        public void RunTest(IProgress<float> progress, IProgress<string> message, List<TestStep> TestList)
        {
            int i = 0;
            string str;
            
            // For each test in the test list, run the function
            // Pass the message object to each test so that the tests can update the display as needed
            foreach(TestStep test in TestList)
            {
                this._queue.TryDequeue(out str);
                if (str == "cancel")
                {
                    break;
                }
                else
                {
                    Task.Delay(500).Wait();
                    var param = new object[] { message, test.lowerBound, test.upperBound };
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
        public bool  Program(IProgress<float> progress, IProgress<string> message)
        {
            //TODO: add code to Confirm that Flashpro is installed in the correct place\

   
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

        private bool CPLD_Verify(IProgress<string> message)
        {
            string VerifyScriptPath;
            string ResultFilePath;
            string Verify_CMD;
            string Verify_Success = "Executing action VERIFY PASSED";
            bool success;
            //The path to the CPLD_Program script is always two directories up from the executing path.
            VerifyScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            VerifyScriptPath = VerifyScriptPath.Remove(VerifyScriptPath.LastIndexOf("\\")); //Up one directory
            VerifyScriptPath = VerifyScriptPath.Remove(VerifyScriptPath.LastIndexOf("\\")); // Up two directories
            ResultFilePath = VerifyScriptPath + "\\ProgramLoad\\CPLDLoad\\Results\\VerifyResult.txt";
            VerifyScriptPath = VerifyScriptPath + "\\ProgramLoad\\CPLDLoad\\cpld_verify.tcl";

            Verify_CMD = "script:" + VerifyScriptPath + " logfile:" + ResultFilePath;

            System.Diagnostics.Process cpld_cmd = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo cpld_info = new System.Diagnostics.ProcessStartInfo();
            cpld_info.FileName = "C:\\Microsemi\\Program_Debug_v11.9\\bin\\flashpro.exe";
            cpld_info.Arguments = Verify_CMD;
            cpld_info.RedirectStandardOutput = true;
            cpld_info.UseShellExecute = false;
            cpld_cmd.StartInfo = cpld_info;
            cpld_cmd.Start();
            //string output = cpld_cmd.StandardOutput.ReadToEnd();
            message.Report("Starting programmer ...");
            while (!File.Exists(ResultFilePath))
            {
                Thread.Sleep(2000);
                message.Report("...");
            }
            if (CPLD_Success(ResultFilePath, Verify_Success))
            {
                message.Report("CPLD Verify Successful");
                success = true;
            }
            else
            {
                message.Report("CPLD Verify unsuccessful");
                success = false;
            }


            return success;
        }
        private bool CPLD_Program(IProgress<string> message)
        {
            string ProgramScriptPath;
            string ResultFilePath;
            string Program_CMD;
            string Program_Success = "Executing action PROGRAM PASSED";
            bool success;
            //The path to the CPLD_Program script is always two directories up from the executing path.
            ProgramScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            ProgramScriptPath = ProgramScriptPath.Remove(ProgramScriptPath.LastIndexOf("\\")); //Up one directory
            ProgramScriptPath = ProgramScriptPath.Remove(ProgramScriptPath.LastIndexOf("\\")); // Up two directories
            ResultFilePath = ProgramScriptPath + "\\ProgramLoad\\CPLDLoad\\Results\\ProgramResult.txt";
            ProgramScriptPath = ProgramScriptPath + "\\ProgramLoad\\CPLDLoad\\cpld_program.tcl";

            Program_CMD =  "script:" + ProgramScriptPath + " logfile:" + ResultFilePath;

            System.Diagnostics.Process cpld_cmd = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo cpld_info = new System.Diagnostics.ProcessStartInfo();
            cpld_info.FileName = "C:\\Microsemi\\Program_Debug_v11.9\\bin\\flashpro.exe";
            cpld_info.Arguments = Program_CMD;
            cpld_info.RedirectStandardOutput = true;
            cpld_info.UseShellExecute = false;
            cpld_cmd.StartInfo = cpld_info;
            cpld_cmd.Start();
            //string output = cpld_cmd.StandardOutput.ReadToEnd();
            message.Report("Starting programmer ...");
            while (!File.Exists(ResultFilePath))
            {
                Thread.Sleep(2000);
                message.Report("...");
            }
            if (CPLD_Success(ResultFilePath, Program_Success))
            {
                message.Report("CPLD Program Successful");
                success = true;
            }
            else
            {
                message.Report("CPLD Program unsuccessful");
                success = false;
            }
           

            return success;
        }
        private bool CPLD_Success(string path, string pass)
        {
            bool success;
            string file;

            file = File.ReadAllText(path);

            if (file.Contains(pass)){
                success = true;
            }
            else
            {
                success = false;
            }

            File.Delete(path);
            return success;
        }
        private bool Hercules_Program(IProgress<string> message)
        {
            string HerculesScriptPath;
            string Hercules_CMD;
            string cmd_output;
            
            bool success;
            //The path to the CPLD_Program script is always two directories up from the executing path.
            HerculesScriptPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            HerculesScriptPath = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); //Up one directory
            HerculesScriptPath = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\")); // Up two directories
            HerculesScriptPath = HerculesScriptPath + "\\ProgramLoad\\HerculesLoad\\dslite.bat";

            Hercules_CMD = "/c " + HerculesScriptPath;

            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo cmd_info = new System.Diagnostics.ProcessStartInfo("cmd");
            cmd_info.FileName = "cmd.exe";
            cmd_info.Arguments = Hercules_CMD;
            cmd_info.CreateNoWindow = true;
            cmd_info.RedirectStandardOutput = true;
            cmd_info.RedirectStandardError = true;
            cmd_info.UseShellExecute = false;
            cmd_info.WorkingDirectory = HerculesScriptPath.Remove(HerculesScriptPath.LastIndexOf("\\"));
            cmd.StartInfo = cmd_info;
            message.Report("Starting Hercules programmer ...");
            cmd.Start();
            
            
           
            cmd_output = cmd.StandardOutput.ReadToEnd();

            message.Report("Programmer exit");
            

            if (cmd_output.Contains("Program verification successful"))
            {
                success = true;
            }
            else
            {
                success = false;
            }


            return success;
        }
        public void RunStep(TestStep step)
        {   

            return;
        }


        private int getTests()
        {
            //Get list of methods in this class
            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
           
            bool qual;
            MethodInfo function=null;
            using (TextFieldParser parser = new TextFieldParser(@"..\..\Configuration\specs.txt"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int i = 0;
                string[] row = parser.ReadFields();

                while (!parser.EndOfData)
                {
                    row = parser.ReadFields();
                    string name = row[0];
                    if (row[1].ToLower() == "yes"){
                        qual = true;
                    }
                    else
                    {
                        qual = false;
                    }
                    for(int j = 0; j<methods.Length;j++){
                        
                        if (methods[j].Name == name){
                            function = methods[j];
                            break;
                        }
                    }
                    TestStep step = new TestStep(i, name, qual, Convert.ToInt32(row[2]), Convert.ToInt32(row[3]), function);
                    this.Tests.Add(step);
                    i++;    
                }
            }
            return 1;
        }

        private int getDMM() {

            return 1;
        }

        private int getPPS() {

            return 1;
        }

        private int test_software_install(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_touch_cal(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_mfg_install(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_power_on(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private string test_pre_use(IProgress<string> message,int upperbound, int lowerbound)
        {
            string str;
            string result = "UNKN";
            message.Report("Start the pre-use test");
            while (!_queue.TryDequeue(out str)) ;
            if (str == "yes")
            {
                message.Report("Test Passed!");
                result = "PASS";
            }
            else if (str == "no")
            {
                message.Report("Test Failed");
                result = "FAIL";
            }
            return result;
        }

        private string test_lcd(IProgress<string> message,int upperbound, int lowerbound)
        {
            string str;
            string result = "UNKN";

            //Blocking until user input is given --> Possible options are: "yes", "no" and "cancel"
            message.Report("Does the LCD look clear?");
            while (true) {
                this._queue.TryDequeue(out str);
                if (str == "yes")
                {
                    message.Report("Test Passed!");
                    result = "PASS";
                    break;
                }
                else if (str == "no")
                {
                    message.Report("Test Failed");
                    result = "FAIL";
                    break;
                }
                else if( str == "cancel")
                {
                    break;
                }
                else {
                    //Do nothing
                }
            }
            return result;
        }

        private int test_3V3_HOT(IProgress<string> message, int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_5V0_HOT(IProgress<string> message, int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_5V0_SMPS(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_12V0_SMPS(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_3V3_SMPS(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_1V2_SMPS(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }

        private int test_3V3_LDO(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_2V048_VREF(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_30V0_SMPS(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_36V0_SMPS(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_spi1_bus(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_i2c_vent(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_blower(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_exhalation(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_dac(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_sov(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_oxygen(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_extO2_on(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_exto2_off(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_cough(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_neb(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_suction(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_low_fan_volt(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_low_fan_freq(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_high_fan_volt(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_high_fan_freq(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_alarm_silence(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_ambient_pressure(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_ambient_temperature(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_microphone(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_speaker(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_piezo(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external_ac(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_internal_battery(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external_battery_1(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external_battery_2(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_internal_chg_cc(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_internal_chg_cv(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external1_chg_cc(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external1_chg_cv(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external2_chg_cc(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
        private int test_external2_chg_cv(IProgress<string> message,int upperbound, int lowerbound)
        {
            return 1;
        }
    }
}
