using System;
using System.Linq;
using System.Threading;
using ThinkingHome.SerialPort.ConsoleApp.Internal;

namespace ThinkingHome.SerialPort.ConsoleApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
            }

            Console.WriteLine(new string('-', 10));

            Send();
        }

        private static void Send()
        {
            using (var device = new MTRFXXAdapter("/dev/cu.usbserial-AI04XT35"))
            {
                device.DataReceived += DeviceOnDataReceived;

                device.Open();

                device.SendCommand(MTRFMode.Service, MTRFAction.SendCommand, 0, MTRFCommand.Off);

                Thread.Sleep(500);

                for (var i = 0; i < 3; i++)
                {
                    device.SendCommand(MTRFMode.TXF, MTRFAction.SendCommand, 0, MTRFCommand.On);
                    Thread.Sleep(2000);

                    device.SendCommand(MTRFMode.TXF, MTRFAction.SendCommand, 0, MTRFCommand.Off);
                    Thread.Sleep(2000);
                }

                Console.WriteLine("done");
                Console.ReadLine();
            }

            Console.WriteLine("exit");
        }

        private static void DeviceOnDataReceived(object o, byte[] bytes)
        {
            Console.WriteLine("read:  {0}", string.Join(".", bytes.Select(b => b.ToString("x2"))));
        }
    }
}
