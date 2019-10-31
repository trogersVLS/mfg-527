/* Tests.cs
 * Partial class FunctionalTest
 * 
 * - To be used with FunctionalTest.cs
 * 
 * Author: Taylor Rogers
 * Date: 10/23/2019
 * 
 */

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

/*********************************************************************************************************************************************************
    * FunctionalTest - This file contains all of the test step code needed for the functional test. The program can run any combination of these test steps
    * 
    *********************************************************************************************************************************************************/
    partial class FunctionalTest
    {
        /*******************************************************************************************************************************  
         *  CPLD_Verify
         *  
         *  Function: Performs a verification of the CPLD firmware on the board using a TCL Script run using Flashpro.exe
         *      Assumptions: Flashpro.exe must be installed in the default location on the C: drive.
         *  
         *  Arguments: IProgress<string> message - This argument allows the function to send updates back to the GUI thread
         *  
         *  Returns: bool success - returns true is the verification is successful, returns false if the verification is not successful
         * 
         *******************************************************************************************************************************/
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
            if (CPLD_LogRead(ResultFilePath, Verify_Success))
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

        /*  CPLD_Program
         *  
         *  Function: Performs a verification of the CPLD firmware on the board using a TCL Script run using Flashpro.exe
         *      Assumptions: Flashpro.exe must be installed in the default location on the C: drive.
         *  
         *  Arguments: IProgress<string> message - This argument allows the function to send updates back to the GUI thread
         *  
         *  Returns: bool success - returns true is the verification is successful, returns false if the verification is not successful
         * 
         */
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

            Program_CMD = "script:" + ProgramScriptPath + " logfile:" + ResultFilePath;

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
            if (CPLD_LogRead(ResultFilePath, Program_Success))
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
        private bool CPLD_LogRead(string path, string pass)
        {
            bool success;
            string file;
            
            file = File.ReadAllText(path);

            if (file.Contains(pass))
            {
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
        private bool SOM_Program(IProgress<string> message)
        {
            bool success;
            if (true)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }
        private int test_software_install(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_touch_cal(IProgress<string> message, TestData test)
        {
            return 1;
        }

        private int test_mfg_install(IProgress<string> message, TestData test)
        {
            return 1;
        }

        private int test_power_on(IProgress<string> message, TestData test)
        {
            return 1;
        }

        private string test_pre_use(IProgress<string> message, TestData test)
        {
            //Assumptions - Unit has been powered on
            //            - Unit is connected to telemetry
            //            - Unit has an electronic solenoid valve and connected to a burn-in lung
            //
            string str;
            string result = "";

            //Confirm assumptions and correct if wrong
            //Device is powered on



            //Clear the queue
            this.ClearInput();

            //Blocking until user input is given --> Possible options are: "yes", "no" and "cancel"
            message.Report("Follow the on-screen instructions");
            if (!this.cancel_request)
            {   
                //
                str = ReceiveInput();
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
            }
            return result;
        }

        private string test_lcd(IProgress<string> message, TestData test)
        {
            //Assumptions - Unit has been powered on
            string str;
            string result = "--";

            //Clear the queue
            this.ClearInput();

            //Blocking until user input is given --> Possible options are: "yes", "no" and "cancel"
            message.Report("Is the LCD screen clear?");
            if (!this.cancel_request)
            {
                str = ReceiveInput();
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
            }
            return result;
        }
        private bool test_voltage(IProgress<string> message, TestData test)
        {
            string str_value;
            bool value_available;
            float upper=0;
            float lower=0;
            //Get parameters from test data object
            value_available = test.parameters.TryGetValue("upper", out str_value);
            if (value_available)
            {
                upper = float.Parse(str_value, System.Globalization.NumberStyles.Float);
            }
            
            value_available = test.parameters.TryGetValue("upper", out str_value);
            if (value_available)
            {
                lower = float.Parse(str_value, System.Globalization.NumberStyles.Float);
            }

            if (this.DMM.Connected)
            {
                float measured = this.DMM.Get_Volts();
                try
                {
                    message.Report("Measured: " + measured.ToString() + " V");

                    if ((measured < (upper)) && (measured > (lower)))
                    {
                        message.Report("Test PASS");
                    }
                    else
                    {
                        message.Report("Test FAIL");
                    }

                    //TODO: Log results
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                message.Report("Multimeter is not connected");
                return false;
            }
            

        }

        
        private int test_spi1_bus(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_i2c_vent(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_blower(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_exhalation(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_dac(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_sov(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_oxygen(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_extO2_on(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_exto2_off(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_cough(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_neb(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_suction(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_low_fan_volt(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_low_fan_freq(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_high_fan_volt(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_high_fan_freq(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_alarm_silence(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_ambient_pressure(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_ambient_temperature(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_microphone(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_speaker(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_piezo(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external_ac(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_internal_battery(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external_battery_1(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external_battery_2(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_internal_chg_cc(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_internal_chg_cv(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external1_chg_cc(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external1_chg_cv(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external2_chg_cc(IProgress<string> message, TestData test)
        {
            return 1;
        }
        private int test_external2_chg_cv(IProgress<string> message, TestData test)
        {
            return 1;
        }


    }
}