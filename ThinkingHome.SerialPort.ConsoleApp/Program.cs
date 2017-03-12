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
            var cmdService = MTRFXXAdapter.BuildCommand(
                MTRFXXAdapter.MTRFMode.Service, MTRFXXAdapter.MTRFAction.SendCommand, MTRFXXAdapter.MTRFRepeatCount.NoRepeat,
                0, MTRFXXAdapter.MTRFCommand.Off, MTRFXXAdapter.MTRFDataFormat.NoData, null);

            var cmdOn = MTRFXXAdapter.BuildCommand(
                MTRFXXAdapter.MTRFMode.TXF, MTRFXXAdapter.MTRFAction.SendCommand, MTRFXXAdapter.MTRFRepeatCount.One,
                0, MTRFXXAdapter.MTRFCommand.On, MTRFXXAdapter.MTRFDataFormat.NoData, null);

            var cmdOff = MTRFXXAdapter.BuildCommand(
                MTRFXXAdapter.MTRFMode.TXF, MTRFXXAdapter.MTRFAction.SendCommand, MTRFXXAdapter.MTRFRepeatCount.Three,
                0, MTRFXXAdapter.MTRFCommand.Off, MTRFXXAdapter.MTRFDataFormat.NoData, null);

            using (var device = SerialDevice.Create("/dev/cu.usbserial-AI04XT35", BaudRate.B9600))
            {
                device.Open();

                device.DataReceived += DeviceOnDataReceived;

                Console.WriteLine("write: {0}", string.Join(".", cmdService.Select(b => b.ToString("x2"))));
                device.Write(cmdService);
                Thread.Sleep(500);

                for (var i = 0; i < 3; i++)
                {
                    Console.WriteLine("write: {0}", string.Join(".", cmdOn.Select(b => b.ToString("x2"))));
                    device.Write(cmdOn);
                    Thread.Sleep(2000);

                    Console.WriteLine("write: {0}", string.Join(".", cmdOff.Select(b => b.ToString("x2"))));
                    device.Write(cmdOff);
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
