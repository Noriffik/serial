using System;
using System.Linq;
using System.Threading;
using ThinkingHome.SerialPort.ConsoleApp.Serial;

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
            var data3 = new byte[17] {
                171, //start
                4, //mode
                0, //cmd mode
                0,
                0, // channel
                0, //cmd
                0, //fmt
                0,0,0,0, //data
                0,0,0,0, //address
                0,
                172 //end
            };
            var data = new byte[17] {
                171, //start
                2, //mode
                0, //cmd mode
                0,
                0, // channel
                2, //cmd
                0, //fmt
                0,0,0,0, //data
                0,0,0,0, //address
                0,
                172 //end
            };

            int sum = 0;
            for (int i = 0; i < 15; i++) sum += data[i];

            data[15] = (byte)(sum % 256);

            using (var device = SerialDevice.Create("/dev/cu.usbserial-AI04XT35", BaudRate.B9600))
            {
                device.Open();

                device.DataReceived += DeviceOnDataReceived;

                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine("write: {0}", string.Join(".", data.Select(b => b.ToString("x2"))));
                    device.Write(data);
                    Thread.Sleep(500);
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
