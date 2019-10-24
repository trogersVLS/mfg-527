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
            this.Field_SerialNumber = new System.Windows.Forms.TextBox();
            this.label_serialNumber = new System.Windows.Forms.Label();
            this.Check_Program = new System.Windows.Forms.CheckBox();
            this.Check_FullTest = new System.Windows.Forms.CheckBox();
            this.console_debugOutput = new System.Windows.Forms.RichTextBox();
            this.Button_Yes = new System.Windows.Forms.Button();
            this.Button_No = new System.Windows.Forms.Button();
            this.StatusBar = new System.Windows.Forms.ProgressBar();
            this.Check_SingleTest = new System.Windows.Forms.CheckBox();
            this.Dropdown_Test_List = new System.Windows.Forms.ComboBox();
            this.Label_User = new System.Windows.Forms.Label();
            this.Label_Username = new System.Windows.Forms.Label();
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
            // Field_SerialNumber
            // 
            this.Field_SerialNumber.BackColor = System.Drawing.SystemColors.Window;
            this.Field_SerialNumber.Location = new System.Drawing.Point(87, 76);
            this.Field_SerialNumber.Name = "Field_SerialNumber";
            this.Field_SerialNumber.Size = new System.Drawing.Size(150, 20);
            this.Field_SerialNumber.TabIndex = 3;
            this.Field_SerialNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Field_SerialNumber_KeyUp);
            // 
            // label_serialNumber
            // 
            this.label_serialNumber.AutoSize = true;
            this.label_serialNumber.Location = new System.Drawing.Point(9, 76);
            this.label_serialNumber.Name = "label_serialNumber";
            this.label_serialNumber.Size = new System.Drawing.Size(73, 13);
            this.label_serialNumber.TabIndex = 4;
            this.label_serialNumber.Text = "Serial Number";
            // 
            // Check_Program
            // 
            this.Check_Program.AutoSize = true;
            this.Check_Program.Location = new System.Drawing.Point(12, 110);
            this.Check_Program.Name = "Check_Program";
            this.Check_Program.Size = new System.Drawing.Size(65, 17);
            this.Check_Program.TabIndex = 7;
            this.Check_Program.Text = "Program";
            this.Check_Program.UseVisualStyleBackColor = true;
            // 
            // Check_FullTest
            // 
            this.Check_FullTest.AutoSize = true;
            this.Check_FullTest.Location = new System.Drawing.Point(12, 133);
            this.Check_FullTest.Name = "Check_FullTest";
            this.Check_FullTest.Size = new System.Drawing.Size(99, 17);
            this.Check_FullTest.TabIndex = 8;
            this.Check_FullTest.Text = "Functional Test";
            this.Check_FullTest.UseVisualStyleBackColor = true;
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
            // Check_SingleTest
            // 
            this.Check_SingleTest.AutoSize = true;
            this.Check_SingleTest.Location = new System.Drawing.Point(12, 157);
            this.Check_SingleTest.Name = "Check_SingleTest";
            this.Check_SingleTest.Size = new System.Drawing.Size(79, 17);
            this.Check_SingleTest.TabIndex = 13;
            this.Check_SingleTest.Text = "Single Test";
            this.Check_SingleTest.UseVisualStyleBackColor = true;
            this.Check_SingleTest.Visible = false;
            this.Check_SingleTest.CheckedChanged += new System.EventHandler(this.Check_SingleTest_CheckedChanged);
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
            // Label_User
            // 
            this.Label_User.AutoSize = true;
            this.Label_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_User.Location = new System.Drawing.Point(5, 9);
            this.Label_User.Name = "Label_User";
            this.Label_User.Size = new System.Drawing.Size(37, 13);
            this.Label_User.TabIndex = 16;
            this.Label_User.Text = "User:";
            // 
            // Label_Username
            // 
            this.Label_Username.AutoSize = true;
            this.Label_Username.Location = new System.Drawing.Point(41, 9);
            this.Label_Username.Name = "Label_Username";
            this.Label_Username.Size = new System.Drawing.Size(41, 13);
            this.Label_Username.TabIndex = 17;
            this.Label_Username.Text = "<User>";
            // 
            // GUI_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(220)))), ((int)(((byte)(222)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(805, 605);
            this.Controls.Add(this.Label_Username);
            this.Controls.Add(this.Label_User);
            this.Controls.Add(this.Dropdown_Test_List);
            this.Controls.Add(this.Check_SingleTest);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.Button_No);
            this.Controls.Add(this.Button_Yes);
            this.Controls.Add(this.label_serialNumber);
            this.Controls.Add(this.console_debugOutput);
            this.Controls.Add(this.Button_Run);
            this.Controls.Add(this.Field_SerialNumber);
            this.Controls.Add(this.Check_FullTest);
            this.Controls.Add(this.Check_Program);
            this.Name = "GUI_Main";
            this.Text = "Control Board Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Run;
        private System.Windows.Forms.TextBox Field_SerialNumber;
        private System.Windows.Forms.Label label_serialNumber;
        private System.Windows.Forms.CheckBox Check_Program;
        private System.Windows.Forms.CheckBox Check_FullTest;
        private System.Windows.Forms.RichTextBox console_debugOutput;
        private System.Windows.Forms.Button Button_Yes;
        private System.Windows.Forms.Button Button_No;
        private System.Windows.Forms.ProgressBar StatusBar;
        private System.Windows.Forms.CheckBox Check_SingleTest;
        private System.Windows.Forms.ComboBox Dropdown_Test_List;
        private System.Windows.Forms.Label Label_User;
        private System.Windows.Forms.Label Label_Username;
    }
}

