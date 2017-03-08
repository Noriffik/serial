using System;
using System.Runtime.InteropServices;
using ThinkingHome.SerialPort.ConsoleApp.Serial;

namespace ThinkingHome.SerialPort.ConsoleApp
{
    internal class Program
    {
        private static void Send()
        {
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

            var portName = "/dev/cu.usbserial-AI04XT35";

            int fd = Libc.open(portName, OpenFlags.O_RDWR | OpenFlags.O_NOCTTY | OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                Console.WriteLine($"failed to open port ({fd})");
            }
            else
            {
                Console.WriteLine($"open: {fd}");

                byte[] termios_data = new byte[256];

                Libc.tcgetattr(fd, termios_data);
                Libc.cfsetspeed(termios_data, SerialSpeed.B9600);
                Libc.tcsetattr(fd, 0, termios_data);

                IntPtr ptr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, ptr, data.Length);

                Libc.write(fd, ptr, data.Length);

//                byte bbb = 0;
//                IntPtr ptr = Marshal.AllocHGlobal(1);
//
//                while (true)
//                {
//                    read(fd, ptr, 1);
//                    Console.Write(Marshal.PtrToStringAnsi(ptr));
//                }

                Libc.close(fd);
            }

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
    }
}