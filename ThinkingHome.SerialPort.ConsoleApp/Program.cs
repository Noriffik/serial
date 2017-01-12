using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ThinkingHome.SerialPort.ConsoleApp
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct termios
    {
        public UInt32 c_iflag;
        public UInt32 c_oflag;
        public UInt32 c_cflag;
        public UInt32 c_lflag;
        public Byte c_cc;
    }

    [Flags]
    public enum OpenFlags {
        //
        // One of these
        //
        O_RDONLY    = 0,
        O_WRONLY    = 1,
        O_RDWR      = 2,

        //
        // Or-ed with zero or more of these
        //
        O_CREAT     = 64, //Octal - 0100
        O_EXCL      = 128, // Octal - 0200
        O_NOCTTY    = 256, // Octal - 0400
        O_TRUNC     = 512, //Octal - 01000
        O_APPEND    = 1024, //Octal -02000
        O_NONBLOCK  = 2048, // Octal -04000
        O_SYNC      = 1052672, //Octal -04010000

        //
        // These are non-Posix, think of a way of exposing
        // this for Linux users.
        //

        // O_NOFOLLOW  = 512,
        // O_DIRECTORY = 1024,
        // O_DIRECT    = 2048,
        // O_ASYNC     = 4096,
        // O_LARGEFILE = 8192
    }

    internal class Program
    {
        [DllImport ("libc")]
        private static extern int getpid();

        [DllImport ("libc")]
        private static extern int tcgetattr(int fd, ref termios termios_p);

        [DllImport ("libc")]
        private static extern int open(string pathname, OpenFlags flags);

        [DllImport ("libc")]
        private static extern int close(int fd);

        [DllImport ("libc")]
        private static extern int read(int fd, IntPtr buf, UInt32 count);

        public static void Main(string[] args)
        {
            var file = "/Users/dima117a/init2.lua";
            var portName2 = "/dev/tty.Bluetooth-Incoming-Port";

            int fd = open(file, OpenFlags.O_RDWR | OpenFlags.O_NOCTTY | OpenFlags.O_NONBLOCK);
            int fd2 = open("/Users/dima117a/init3.lua", OpenFlags.O_RDWR | OpenFlags.O_NOCTTY | OpenFlags.O_NONBLOCK);

            if(fd == -1) {
                Console.WriteLine( $"failed to open port ({fd})");
            }
            else
            {
                IntPtr ptr = Marshal.AllocHGlobal(80);
                var x = read(fd, ptr, 80);

                Console.WriteLine(Marshal.PtrToStringAnsi(ptr));

                Console.WriteLine(fd);
                Console.WriteLine(fd2);

                termios obj = new termios();
                tcgetattr(fd, ref obj);

                Console.WriteLine($"{obj.c_iflag}, {obj.c_oflag}");

                Console.WriteLine(close(fd));
                Console.WriteLine(close(fd2));
            }

            return;

            Console.WriteLine(getpid());

            foreach (var portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
            }

            Console.WriteLine("===");
        }
    }
}