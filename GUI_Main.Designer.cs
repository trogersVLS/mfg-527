namespace mfg_527
{
    partial class GUI_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_Run = new System.Windows.Forms.Button();
            this.field_serialNumber = new System.Windows.Forms.TextBox();
            this.label_serialNumber = new System.Windows.Forms.Label();
            this.label_Title = new System.Windows.Forms.Label();
            this.check_program = new System.Windows.Forms.CheckBox();
            this.check_test = new System.Windows.Forms.CheckBox();
            this.console_debugOutput = new System.Windows.Forms.RichTextBox();
            this.Button_Yes = new System.Windows.Forms.Button();
            this.Button_No = new System.Windows.Forms.Button();
            this.StatusBar = new System.Windows.Forms.ProgressBar();
            this.check_SingleTest = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.Dropdown_Test_List = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Button_Run
            // 
            this.Button_Run.Location = new System.Drawing.Point(8, 225);
            this.Button_Run.Name = "Button_Run";
            this.Button_Run.Size = new System.Drawing.Size(103, 42);
            this.Button_Run.TabIndex = 1;
            this.Button_Run.Text = "Run";
            this.Button_Run.UseVisualStyleBackColor = true;
            this.Button_Run.Click += new System.EventHandler(this.Button_Run_Click);
            // 
            // field_serialNumber
            // 
            this.field_serialNumber.BackColor = System.Drawing.SystemColors.Window;
            this.field_serialNumber.Location = new System.Drawing.Point(87, 76);
            this.field_serialNumber.Name = "field_serialNumber";
            this.field_serialNumber.Size = new System.Drawing.Size(150, 20);
            this.field_serialNumber.TabIndex = 3;
            this.field_serialNumber.TextChanged += new System.EventHandler(this.Field_serialNumber_TextChanged);
            this.field_serialNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Field_serialNumber_KeyUp);
            // 
            // label_serialNumber
            // 
            this.label_serialNumber.AutoSize = true;
            this.label_serialNumber.Location = new System.Drawing.Point(9, 76);
            this.label_serialNumber.Name = "label_serialNumber";
            this.label_serialNumber.Size = new System.Drawing.Size(73, 13);
            this.label_serialNumber.TabIndex = 4;
            this.label_serialNumber.Text = "Serial Number";
            this.label_serialNumber.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Font = new System.Drawing.Font("Avenir Heavy", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Title.Location = new System.Drawing.Point(159, 9);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(456, 64);
            this.label_Title.TabIndex = 5;
            this.label_Title.Text = "Control Board Test";
            this.label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // check_program
            // 
            this.check_program.AutoSize = true;
            this.check_program.Location = new System.Drawing.Point(12, 110);
            this.check_program.Name = "check_program";
            this.check_program.Size = new System.Drawing.Size(65, 17);
            this.check_program.TabIndex = 7;
            this.check_program.Text = "Program";
            this.check_program.UseVisualStyleBackColor = true;
            this.check_program.CheckedChanged += new System.EventHandler(this.Check_program_CheckedChanged);
            // 
            // check_test
            // 
            this.check_test.AutoSize = true;
            this.check_test.Location = new System.Drawing.Point(12, 133);
            this.check_test.Name = "check_test";
            this.check_test.Size = new System.Drawing.Size(99, 17);
            this.check_test.TabIndex = 8;
            this.check_test.Text = "Functional Test";
            this.check_test.UseVisualStyleBackColor = true;
            this.check_test.CheckedChanged += new System.EventHandler(this.Check_test_CheckedChanged);
            // 
            // console_debugOutput
            // 
            this.console_debugOutput.Location = new System.Drawing.Point(267, 76);
            this.console_debugOutput.Name = "console_debugOutput";
            this.console_debugOutput.ReadOnly = true;
            this.console_debugOutput.Size = new System.Drawing.Size(521, 488);
            this.console_debugOutput.TabIndex = 9;
            this.console_debugOutput.Text = "";
            this.console_debugOutput.TextChanged += new System.EventHandler(this.Console_debugOutput_TextChanged);
            // 
            // Button_Yes
            // 
            this.Button_Yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Button_Yes.Location = new System.Drawing.Point(12, 468);
            this.Button_Yes.Name = "Button_Yes";
            this.Button_Yes.Size = new System.Drawing.Size(91, 96);
            this.Button_Yes.TabIndex = 10;
            this.Button_Yes.Text = "Yes";
            this.Button_Yes.UseVisualStyleBackColor = false;
            this.Button_Yes.Visible = false;
            this.Button_Yes.Click += new System.EventHandler(this.Button_Yes_Click);
            // 
            // Button_No
            // 
            this.Button_No.BackColor = System.Drawing.Color.Red;
            this.Button_No.Location = new System.Drawing.Point(134, 468);
            this.Button_No.Name = "Button_No";
            this.Button_No.Size = new System.Drawing.Size(91, 96);
            this.Button_No.TabIndex = 11;
            this.Button_No.Text = "No";
            this.Button_No.UseVisualStyleBackColor = false;
            this.Button_No.Visible = false;
            this.Button_No.Click += new System.EventHandler(this.Button_No_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(267, 570);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(521, 23);
            this.StatusBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.StatusBar.TabIndex = 12;
            // 
            // check_SingleTest
            // 
            this.check_SingleTest.AutoSize = true;
            this.check_SingleTest.Location = new System.Drawing.Point(12, 157);
            this.check_SingleTest.Name = "check_SingleTest";
            this.check_SingleTest.Size = new System.Drawing.Size(79, 17);
            this.check_SingleTest.TabIndex = 13;
            this.check_SingleTest.Text = "Single Test";
            this.check_SingleTest.UseVisualStyleBackColor = true;
            this.check_SingleTest.Visible = false;
            this.check_SingleTest.CheckedChanged += new System.EventHandler(this.Check_SingleTest_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(134, 225);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 42);
            this.button3.TabIndex = 14;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            // 
            // Dropdown_Test_List
            // 
            this.Dropdown_Test_List.FormattingEnabled = true;
            this.Dropdown_Test_List.Location = new System.Drawing.Point(116, 155);
            this.Dropdown_Test_List.Name = "Dropdown_Test_List";
            this.Dropdown_Test_List.Size = new System.Drawing.Size(121, 21);
            this.Dropdown_Test_List.TabIndex = 15;
            this.Dropdown_Test_List.Visible = false;
            // 
            // GUI_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(222)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(801, 605);
            this.Controls.Add(this.Dropdown_Test_List);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.check_SingleTest);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.Button_No);
            this.Controls.Add(this.Button_Yes);
            this.Controls.Add(this.label_Title);
            this.Controls.Add(this.label_serialNumber);
            this.Controls.Add(this.console_debugOutput);
            this.Controls.Add(this.Button_Run);
            this.Controls.Add(this.field_serialNumber);
            this.Controls.Add(this.check_test);
            this.Controls.Add(this.check_program);
            this.Name = "GUI_Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.GUI_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Run;
        private System.Windows.Forms.TextBox field_serialNumber;
        private System.Windows.Forms.Label label_serialNumber;
        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.CheckBox check_program;
        private System.Windows.Forms.CheckBox check_test;
        private System.Windows.Forms.RichTextBox console_debugOutput;
        private System.Windows.Forms.Button Button_Yes;
        private System.Windows.Forms.Button Button_No;
        private System.Windows.Forms.ProgressBar StatusBar;
        private System.Windows.Forms.CheckBox check_SingleTest;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox Dropdown_Test_List;
    }
}

