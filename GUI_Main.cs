using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;

namespace mfg_527
{


    public partial class GUI_Main : Form
    {
        
        CancellationTokenSource cancel;
        static AutoResetEvent _restart = new AutoResetEvent(false);
        FunctionalTest test;
        private readonly ConcurrentQueue<string> event_handler = new ConcurrentQueue<string>();
        public GUI_Main()
        {

            this.cancel = new CancellationTokenSource();
            
            InitializeComponent();
            //Hide the unused buttons until a serial number is entered
            this.Button_runTest.Hide();
            this.Button_Yes.Hide();
            this.Button_No.Hide();
            this.check_program.Hide();
            this.check_test.Hide();
            this.console_debugOutput.Text = "Enter the Serial Number";
        }
        /******************************************************************************************************
         * button_runTest_Click(): event handler for run test button click. 
         * 
         * Based on the state of the checkboxes, this event handler will initiate a long running function
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
        private async void button_runTest_Click(object sender, EventArgs e)
        {   
            this.StatusBar.Value = 0;
            this.Button_Yes.Show();
            this.Button_No.Show();

            //Get checkbox values
            if (this.Button_runTest.Text != "Cancel")
            {
                
                if (check_test.Checked & check_program.Checked)
                {
                    this.Button_runTest.Text = "Cancel";
                    console_debugOutput.Text = "Program and Functional Test";

                    var progress = new Progress<float>(s => StatusBar.Value = (int)s);
                    var messages = new Progress<string>(s => this.console_debugOutput.AppendText("\n" + s));

                    await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                                                TaskCreationOptions.LongRunning);
                    this.StatusBar.Value = 100;
                    this.Button_runTest.Text = "Run Test";
                }
                else if (check_test.Checked & !check_program.Checked)
                {
                    this.Button_runTest.Text = "Cancel";
                    console_debugOutput.Text = "Functional Test only";


                    this.Button_runTest.Text = "Run Test";
                }
                else if (!check_test.Checked & check_program.Checked)
                {
                    this.Button_runTest.Text = "Cancel";
                    console_debugOutput.Text = "Just gonna program this board now ....";
                    var progress = new Progress<float>(s => StatusBar.Value = (int)s);
                    var messages = new Progress<string>(s => this.console_debugOutput.AppendText("\n" + s));

                    await Task.Factory.StartNew(() => this.test.Program(progress, messages),
                                                TaskCreationOptions.LongRunning);
                    this.StatusBar.Value = 100;

                    this.Button_runTest.Text = "Run Test";
                }
                else
                {
                    console_debugOutput.Text = "Ya gotta click one of the boxes dummy";
                }
            }
            else
            {
                this.cancel.Cancel();
                this.Button_runTest.Text = "Run Test";
            }


        }
        private async void button_runStep_Click(object sender, EventArgs e)
        {
            var progress = new Progress<float>(s => StatusBar.Value = (int)s);
            var messages = new Progress<string>(s => this.console_debugOutput.AppendText("\n" + s));
            await Task.Factory.StartNew(() => this.test.RunTest(progress, messages, this.test.Tests),
                            TaskCreationOptions.LongRunning);

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

        private void Label_Title_Click(object sender, EventArgs e)
        {

        }

        private void Field_serialNumber_KeyUp(object sender, KeyEventArgs e)
        {   

            //Only update the serial number if the user hits enter.
            if (e.KeyData == Keys.Enter)
            {
                console_debugOutput.Text = "Serial Number =  " + this.field_serialNumber.Text;
                this.Button_runTest.Show();
                this.check_program.Show();
                this.check_test.Show();
                this.check_SingleTest.Show();
                this.test = new FunctionalTest(event_handler, field_serialNumber.Text);
            }
            //this.test.SerialNumber = this.field_serialNumber.Text;

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
    }
}
