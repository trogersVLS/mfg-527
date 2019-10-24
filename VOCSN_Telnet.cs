using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace mfg_527
{
    public class Telnet
    {
        int cmd_port = 5000;
        int shell_port = 23;
        int telemetry_port = 5001;

        TcpClient vocsn_cmd;
        NetworkStream stream_cmd;
        bool connected = false;
        string _ip_address;
        public Telnet(string _ip_address)
        {
            this._ip_address = _ip_address;
            try
            {
                this.Connect(this._ip_address);
            }
            catch (Exception e)
            {
                this.connected = false;
            }
        }
        ~Telnet()
        {
            this.Close();
        }
        /*read_until:
         * Reads from the telnet stream until the specified string.
         * 
         */
        private String read_until(string str)
        {
            String response = "";
            int tempByte;

            while (true)
            {
                tempByte = this.stream_cmd.ReadByte();
                if (tempByte != (-1))
                {

                    response += (char)tempByte;
                }
                if (response.Contains(str))
                {
                    break;
                }

            }

            return response;

        }
        /****************************************************************
         * Command
         * Sends a command over port 5000;
         * 
         * **************************************************************/
        public List<String> Command(string message)
        {
            List<String> responseData = new List<String>();
            Byte[] command = System.Text.Encoding.ASCII.GetBytes(message);
            if (message == "exit")
            {
                this.stream_cmd.Write(command, 0, command.Length); //Send the command
                responseData.Add("Successful Exit");
            }
            else
            {
                this.stream_cmd.Write(command, 0, command.Length); //Send the command
                responseData.AddRange(this.read_until("$vserver>").Split(new string[] { "\r","\n" }, StringSplitOptions.None)); // Wait and receive the response.
                responseData.ForEach(i => i.Trim());
            }
            return responseData;
        }
        /* Connect
         * Connects to the telnet port at the specificied ip address
         */
        private bool Connect(String _ip_address)
        {
            string responseString;
            Byte[] response = new Byte[256];
            int bytes;
            try
            {
                // Create a TcpClient connection to VOCSN
                this.vocsn_cmd = new TcpClient(_ip_address, this.cmd_port);

                //Get the VOCSN network stream.
                this.stream_cmd = this.vocsn_cmd.GetStream();
                bytes = this.stream_cmd.Read(response, 0, response.Length);
                responseString = System.Text.Encoding.ASCII.GetString(response, 0, bytes);

                if (bytes == 0 || responseString != "$vserver> ")
                {
                    Console.WriteLine("Unable to connect");
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }


            return true;
        }


        private void Close()
        {
            try
            {
                this.Command("exit");
                this.vocsn_cmd.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}
