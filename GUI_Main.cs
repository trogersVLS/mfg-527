/* GUI_Main.cs
 * Partial class GUI_Main
 * 
 * - To be used with GUI_Main.Designer.cs (a visual studio generated file)
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 */
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace mfg_527
{


    public partial class GUI_Main : Form
    {
        //Global Settings
        string location;
        int eqid;
        string mfg_code;
        string user_id;
        string serial;

        FunctionalTest test;
        private readonly ConcurrentQueue<string> message_queue = new ConcurrentQueue<string>();
        public GUI_Main()
        {

            //Initialize Settings
            XmlNode x = this.GetSettings();

            //User Login
            LoginForm login_screen = new LoginForm();
            this.user_id = login_screen.ShowForm(x);
            
            
            

            InitializeComponent();
            //Hide the unused buttons until a serial number is entered
            this.Button_Run.Hide();
            this.Button_Yes.Hide();
            this.Button_No.Hide();
            this.Check_Program.Hide();
            this.Check_FullTest.Hide();
            this.console_debugOutput.Text = "Enter the Serial Number";
            this.Label_Username.Text = this.user_id;

        }

        private XmlNode GetSettings()
        {
            //open the settings.xml file

            XmlDocument settings = new XmlDocument();
            
            settings.Load(@"..\..\Configuration\settings.xml");
            XmlNode settings_node = settings.DocumentElement.ChildNodes[0];
            XmlNode user_ids = settings.DocumentElement.ChildNodes[1];
            this.eqid = Convert.ToInt32(settings_node.Attributes["eqid"].Value);
            this.location = settings_node.Attributes["location"].Value;
            this.mfg_code = settings_node.Attributes["mfg_code"].Value;




            //Get User_Id number
            //Prompt user for id and password

            //foreach(XmlNode x in user_ids)
            //{
            //    //if(x.Attributes["username"] == "a")
            //    //{
            //    //    this.user_id = Convert.ToInt32(x.Attributes["id"]);
            //    //}
            //}


            return user_ids;

        }


        /******************************************************************************************************
         * Button_Run_Click(): event handler for run test button click. 
         * 
         * Based on the state of the checkboxes, this event handler will initiate a long running async task
         * 
         * Program && FunctionalTest --> Runs the FunctionalTest.RunTest in an asynchronous task
         * Program --> Runs the FunctionalTest.Program() method in an asynchronous task
         * FunctionalTest -- > Runs the FunctionalTest.RunTest() method in an asynchronous task, won't program the board
         * None checked --> Does nothing. Displays a message on the debug console.
         * 
         * 
         * This function will wait until the task finishes before exiting. // Not sure if that is a good thing or not yet
         * 
         * ****************************************************************************************************/
        private async void Button_Run_Click(object sender, EventArgs e)
        {
            //async progress variables. Update the linked property when changed, hooking the appropriate event handler to update the GUI.
            var progress = new Progress<int>(i => StatusBar.Value = i);
            var messages = new Progress<string>(s => this.console_debugOutput.AppendText("\n" + s));


            this.StatusBar.Value = 0;


            //If this button says Run
            if (this.Button_Run.Text == "Run")
            {   //Change button text 
                this.Button_Run.Text = "Cancel";

                if (Check_FullTest.Checked & Check_Program.Checked)
                {
                    console_debugOutput.Text = "Program and Functional Test";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                                                TaskCreationOptions.LongRunning);
                    this.StatusBar.Value = 100;

                }
                else if (Check_FullTest.Checked & !Check_Program.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    console_debugOutput.Text = "Functional Test only";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    //TODO:Get the selected task and pass to function.
                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                                                                    TaskCreationOptions.LongRunning);


                    this.StatusBar.Value = 100;


                }
                else if (!Check_FullTest.Checked & Check_Program.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    console_debugOutput.Text = "Just gonna program this board now ....";
                    
                    await Task.Factory.StartNew(() => this.test.Program(progress, messages),
                                                TaskCreationOptions.LongRunning);
                    this.StatusBar.Value = 100;


                }
                else
                {
                    console_debugOutput.Text = "Ya gotta click one of the boxes dummy";
                }
            }
            else
            {   //Clear the queue and send message to cancel functional test thread.
                while(!this.message_queue.IsEmpty)
                {
                    this.message_queue.TryDequeue(out string str);
                }
                this.message_queue.Enqueue("cancel");
                
            }
            //Reset state to exit method
            this.Button_Yes.Hide();
            this.Button_No.Hide();
            this.Button_Run.Text = "Run";

            // Reset GUI?
            this.ConfirmReset();

        }
        private void ConfirmReset()
        {
            

            var user_confirmation = MessageBox.Show("Test is finished, save output locally?", "Save!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(user_confirmation == DialogResult.Yes)
            {
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "All Fiels|*.*";
                sv.Title = "Save test output";
                sv.ShowDialog();
                this.Reset_GUI();
            }


            
        }
        /******************************************************************************************************
         * Field_SerialNumber_KeyUp
         * - Method checks to see if the user has finished inputting a serial number by pressing <Enter>.
         *   This allows the user to use a scanner to enter the serial number of the boards.
         *   
         *   TODO: Create file for saved configuration settings. This will allow for each installation of this 
         *   program to be configured specifically for the manufacturer
         *   
         *   Serial number format: @ A # # $ # # #
         *   @ - Manufacturer code
         *   # - A number
         *   $ - Letter to indicate the two week period in which the board was made
         *   
         * 
         * ****************************************************************************************************/
        private void Field_SerialNumber_KeyUp(object sender, KeyEventArgs e)
        {   

            //Only update the serial number if the user hits enter.
            //Creates an instance of the functional test
            if (e.KeyData == Keys.Enter)
            {   
                //TODO: Check if the serial is valid and/or has changed.
                this.console_debugOutput.Text = "Serial Number =  " + this.Field_SerialNumber.Text;
                //Uncheck all checkboxes;
                this.Uncheck_All();
                this.Button_Run.Show();
                this.Check_Program.Show();
                this.Check_FullTest.Show();
                this.Check_SingleTest.Show();
                
                this.test = new FunctionalTest(this.message_queue, this.serial, this.location, this.eqid, this.user_id);
            }
           

        }
        /******************************************************************************************************
         * Console_DebugOutput_TextChanged   
         * - Method to scroll the rich text box to the end after each text change
         * 
         * 
         * ****************************************************************************************************/
        private void Console_debugOutput_TextChanged(object sender, EventArgs e)
        {
            this.console_debugOutput.SelectionStart = this.console_debugOutput.Text.Length;
            this.console_debugOutput.ScrollToCaret();
        }
        /******************************************************************************************************
         * Button_Yes_Click   
         * - Adds the message "yes" to the event handler for the Functional Test Thread to handle
         * 
         * 
         * ****************************************************************************************************/
        private void Button_Yes_Click(object sender, EventArgs e)
        {
            message_queue.Enqueue("yes");
        }
        /******************************************************************************************************
         * Button_No_Click   
         * - Adds the message "no" to the event handler for the Functional Test Thread to handle
         * 
         * 
         * ****************************************************************************************************/
        private void Button_No_Click(object sender, EventArgs e)
        {
            message_queue.Enqueue("no");
        }        
        /******************************************************************************************************
         * Check_SingleTest_CheckedChanged  
         * - When Check_SingleTest is checked, the dropdown list appears and on the first check, the dropdown
         * list is populated with all of the test names that are available.
         * - When Check_SingleTest is unchecked, the dropdown lists is hidden, but the test names are preserved.
         * 
         * ****************************************************************************************************/

        private void Check_SingleTest_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_SingleTest.Checked)
            {   
                if(Dropdown_Test_List.Items.Count == 0)
                {
                    //Get test names
                    List<TestData> test_names = this.test.Tests;
                    foreach (TestData i in test_names) {
                        this.Dropdown_Test_List.Items.Add(i.name);
                    }
                }

                Dropdown_Test_List.Show();
                
            }
            else
            {
                Dropdown_Test_List.Hide();
            }
        }


        /******************************************************************************************************
         * Uncheck_All
         * - Utility method to uncheck all of the check boxes
         * 
         * ****************************************************************************************************/
        private void Uncheck_All()
        {
            this.Check_FullTest.Checked = false;
            this.Check_SingleTest.Checked = false;
            this.Check_Program.Checked = false;

        }
        /******************************************************************************************************
         * Reset_GUI
         * - Utility method to reset the GUI to a start-up state. Should be called after a test has finished.
         * 
         * ****************************************************************************************************/
        private void Reset_GUI()
        {
            this.Uncheck_All();

            //Hide buttons
            this.Button_No.Hide();
            this.Button_Yes.Hide();
            this.Button_Run.Hide();
            //Hide Checkboxes
            this.Check_FullTest.Hide();
            this.Check_Program.Hide();
            this.Check_SingleTest.Hide();
            //Clear serial number box
            this.Field_SerialNumber.ResetText();
            //Reset status bar
            this.StatusBar.Value = 0;
            //Reset Debug_Output
            this.console_debugOutput.ResetText();
        }
    }

  
}
