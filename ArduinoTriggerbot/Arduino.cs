using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoTriggerbot
{
    class Arduino
    {
        public SerialPort serialPort;

        public Arduino(SerialPort port)
        {
            serialPort = port;
            serialPort.Open();
        }

        public void SendCommand(string command)
        {
            serialPort.WriteLine(command);
        }
    }
}
