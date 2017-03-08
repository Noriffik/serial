using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThinkingHome.SerialPort.ConsoleApp
{
    public enum Speed : long
    {
        B1200 = 1200,
        B2400 = 2400,
        B4800 = 4800,
        B9600 = 9600,
        B19200 = 19200,
        B38400 = 38400,
        B57600 = 57600,
        B1152000 = 115200,
    }

    [Flags]
    public enum OpenFlags
    {
        //
        // One of these
        //
        O_RDONLY = 0,
        O_WRONLY = 1,
        O_RDWR = 2,

        //
        // Or-ed with zero or more of these
        //
        O_CREAT = 64, //Octal - 0100
        O_EXCL = 128, // Octal - 0200
        O_NOCTTY = 256, // Octal - 0400
        O_TRUNC = 512, //Octal - 01000
        O_APPEND = 1024, //Octal -02000
        O_NONBLOCK = 2048, // Octal -04000
        O_SYNC = 1052672, //Octal -04010000
    }


    internal class Program
    {
        [DllImport("libc")]
        private static extern int getpid();

        [DllImport("libc")]
        private static extern int tcgetattr(int fd, [Out] byte[] termios_data);

        [DllImport("libc")]
        private static extern int open(string pathname, OpenFlags flags);

        [DllImport("libc")]
        private static extern int close(int fd);

        [DllImport("libc")]
        private static extern int read(int fd, IntPtr buf, int count);

        [DllImport("libc")]
        private static extern int write(int fd, IntPtr buf, int count);

        [DllImport("libc")]
        private static extern int tcsetattr(int fd, int optional_actions, byte[] termios_data);

        [DllImport("libc")]
        private static extern int cfsetspeed(byte[] termios_data, Speed speed);

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

            int fd = open(portName, OpenFlags.O_RDWR | OpenFlags.O_NOCTTY | OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                Console.WriteLine($"failed to open port ({fd})");
            }
            else
            {
                Console.WriteLine($"open: {fd}");

                byte[] termios_data = new byte[256];

                tcgetattr(fd, termios_data);
                cfsetspeed(termios_data, Speed.B9600);
                tcsetattr(fd, 0, termios_data);

                IntPtr ptr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, ptr, data.Length);

                write(fd, ptr, data.Length);

//                byte bbb = 0;
//                IntPtr ptr = Marshal.AllocHGlobal(1);
//
//                while (true)
//                {
//                    read(fd, ptr, 1);
//                    Console.Write(Marshal.PtrToStringAnsi(ptr));
//                }

                close(fd);
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
            return;
        }

//                IntPtr ptr = Marshal.AllocHGlobal(80);
//                var x = read(fd, ptr, 80);
//
//                Console.WriteLine(Marshal.PtrToStringAnsi(ptr));


//            IntPtr unmanagedPointer = Marshal.AllocHGlobal(bytes.Length);
//            Marshal.Copy(bytes, 0, unmanagedPointer, bytes.Length);
//            Call unmanaged code
//            Marshal.FreeHGlobal(unmanagedPointer);


        private static void WriteArray(byte[] data)
        {
            Console.WriteLine(string.Join("", data.Select(x => x.ToString("x2"))));
        }
    }
}