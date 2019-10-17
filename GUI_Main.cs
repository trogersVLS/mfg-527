using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Collections.Generic;

namespace mfg_527
{


    public partial class GUI_Main : Form
    {
        

        FunctionalTest test;
        private readonly ConcurrentQueue<string> event_handler = new ConcurrentQueue<string>();
        public GUI_Main()
        {   
            InitializeComponent();
            //Hide the unused buttons until a serial number is entered
            this.Button_Run.Hide();
            this.Button_Yes.Hide();
            this.Button_No.Hide();
            this.check_program.Hide();
            this.check_test.Hide();
            this.console_debugOutput.Text = "Enter the Serial Number";
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
            var progress = new Progress<float>(s => StatusBar.Value = (int)s);
            var messages = new Progress<string>(s => this.console_debugOutput.AppendText("\n" + s));


            this.StatusBar.Value = 0;


            //Get checkbox values
            if (this.Button_Run.Text != "Cancel")
            {
                
                if (check_test.Checked & check_program.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    console_debugOutput.Text = "Program and Functional Test";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                                                TaskCreationOptions.LongRunning);
                    this.StatusBar.Value = 100;
                    this.Button_Yes.Hide();
                    this.Button_No.Hide();

                    this.Button_Run.Text = "Run Test";
                }
                else if (check_test.Checked & !check_program.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    console_debugOutput.Text = "Functional Test only";
                    this.Button_Yes.Show();
                    this.Button_No.Show();

                    //TODO:Get the selected tasks and create a list of them to pass as list of tests to run.


                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                                                                    TaskCreationOptions.LongRunning);


                    this.StatusBar.Value = 100;
                    this.Button_Yes.Hide();
                    this.Button_No.Hide();

                    this.Button_Run.Text = "Run Test";
                }
                else if (!check_test.Checked & check_program.Checked)
                {
                    this.Button_Run.Text = "Cancel";
                    console_debugOutput.Text = "Just gonna program this board now ....";
                    
                    await Task.Factory.StartNew(() => this.test.Program(progress, messages),
                                                TaskCreationOptions.LongRunning);
                    this.StatusBar.Value = 100;
                    this.Button_Yes.Hide();
                    this.Button_No.Hide();

                    this.Button_Run.Text = "Run Test";
                }
                else
                {
                    console_debugOutput.Text = "Ya gotta click one of the boxes dummy";
                }
            }
            else
            {   //Clear the queue and send message to cancel functional test thread.
                while(!this.event_handler.IsEmpty)
                {
                    this.event_handler.TryDequeue(out string str);
                }
                this.event_handler.Enqueue("cancel");
                //this.Button_runTest.Text = "Run Test";
            }


        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DomainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void Button_runProgram_Click(object sender, EventArgs e)
        {
            
        }

        private void Button_runTest_Click_1(object sender, EventArgs e)
        {

        }

        private void Check_test_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Check_program_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Field_serialNumber_TextChanged(object sender, EventArgs e)
        {


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
        private void Field_serialNumber_KeyUp(object sender, KeyEventArgs e)
        {   

            //Only update the serial number if the user hits enter.
            //Creates an instance of the functional test
            if (e.KeyData == Keys.Enter)
            {   
                //TODO: Check if the serial is valid and/or has changed.
                this.console_debugOutput.Text = "Serial Number =  " + this.field_serialNumber.Text;
                this.Button_Run.Show();
                this.check_program.Show();
                this.check_test.Show();
                this.check_SingleTest.Show();
                
                this.test = new FunctionalTest(this.event_handler, this.field_serialNumber.Text);
            }
           

        }

        private void Console_debugOutput_TextChanged(object sender, EventArgs e)
        {
            this.console_debugOutput.SelectionStart = this.console_debugOutput.Text.Length;
            this.console_debugOutput.ScrollToCaret();
        }

        private void GUI_Main_Load(object sender, EventArgs e)
        {

        }

        private void Button_Yes_Click(object sender, EventArgs e)
        {
            event_handler.Enqueue("yes");
        }

        private void Button_No_Click(object sender, EventArgs e)
        {
            event_handler.Enqueue("no");
        }

        private void Check_SingleTest_CheckedChanged(object sender, EventArgs e)
        {
            if (check_SingleTest.Checked)
            {   
                if(Dropdown_Test_List.Items.Count == 0)
                {
                    //Get test names
                    List<TestStep> test_names = this.test.Tests;
                    foreach (TestStep i in test_names) {
                        this.Dropdown_Test_List.Items.Add(i.name);
                    }
                }
                else
                {

                }
                Dropdown_Test_List.Show();
                
            }
            else
            {
                Dropdown_Test_List.Hide();
            }
        }
    }
}
