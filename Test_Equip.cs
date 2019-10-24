using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace mfg_527
{
    class Test_Equip
    {
        private readonly int QUERY_DELAY = 500; //millisecond delay for SCPI queries
        string name;
        string comm;
        SerialPort device;
        int address;
        /* Test_Equip Constructor: 
         * Initializes a Test Equipment SCPI compatible device over RS232, GPIB, LAN, or USB
         */
        public Test_Equip(string comm, string address)
        {
            if (comm == "RS232")
            {
                Byte[] byte_name = new Byte[32];
                this.comm = comm;
                this.device = new SerialPort(address, 9600, Parity.None, 8, StopBits.One);
                name = this.Query("*IDN?\r");
                              
                this.device.Write("SYST:REM");
            }
            else if (comm == "GPIB")
            {
                //TODO: GPIB constructor
            }
            else if (comm == "USB")
            {
                //TODO: USB constructor
            }
            else if (comm == "LAN")
            {                
                //TODO: LAN constructor
            }
            
        }

        public string Get_Volts()
        {
            string volt_str = "";

            volt_str = this.Query(":MEAS:VOLT:DC?");
            
            return volt_str;
        }
        public string Get_Amps()
        {
            string amp_str = "";

            amp_str = this.Query(":MEAS:CURR:DC?");

            return amp_str;
        }

        private string Query(string cmd)
        {
            string response = "";
            byte[] byte_response = new byte[16];
            if(this.comm == "RS232")
            {
                this.device.Write(cmd +"\r");
                Thread.Sleep(this.QUERY_DELAY);
                int num = this.device.Read(byte_response, 0, byte_response.Length);
                response = Encoding.ASCII.GetString(byte_response, 0, byte_response.Length);

            }

            return response;
        }

    }
}
