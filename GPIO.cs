using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MccDaq;
using ErrorDefs;

namespace mfg_527
{
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

}
