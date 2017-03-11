using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using ThinkingHome.SerialPort.ConsoleApp.Serial;

namespace ThinkingHome.SerialPort.ConsoleApp
{
    internal class Program
    {
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
                0, //cmd
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

                Console.WriteLine("======");
                Console.WriteLine("write: {0}", string.Join(".", data.Select(b => b.ToString("x2"))));
                device.Write(data);
                Thread.Sleep(1000);

                Console.WriteLine("======");
                Console.WriteLine("write: {0}", string.Join(".", data.Select(b => b.ToString("x2"))));
                device.Write(data);
                Thread.Sleep(1000);

                Console.WriteLine("======");
                Console.WriteLine("write: {0}", string.Join(".", data.Select(b => b.ToString("x2"))));
                device.Write(data);
                Thread.Sleep(1000);

                Console.WriteLine("======");
                Console.WriteLine("write: {0}", string.Join(".", data.Select(b => b.ToString("x2"))));
                device.Write(data);
                Thread.Sleep(1000);
                Console.ReadLine();
            }

            Console.WriteLine("done");
        }

        public static void Main(string[] args)
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
            }

            Console.WriteLine("=====");

            Send();
        }

//        IntPtr ptr = Marshal.AllocHGlobal(80);
//        var x = read(fd, ptr, 80);
//
//        Console.WriteLine(Marshal.PtrToStringAnsi(ptr));


//        IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
//        Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
//        Call unmanaged code
//        Marshal.FreeHGlobal(unmanagedPointer);


//        private static void WriteArray(byte[] data)
//        {
//            Console.WriteLine(string.Join("", data.Select(x => x.ToString("x2"))));
//        }

//                byte bbb = 0;
//                IntPtr ptr = Marshal.AllocHGlobal(1);
//
//                while (true)
//                {
//                    read(fd, ptr, 1);
//                    Console.Write(Marshal.PtrToStringAnsi(ptr));
//                }

    }
}
