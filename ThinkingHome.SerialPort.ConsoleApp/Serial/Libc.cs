using System;
using System.Runtime.InteropServices;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    public static class Libc
    {
        [DllImport("libc")]
        public static extern int getpid();

        [DllImport("libc")]
        public static extern int tcgetattr(int fd, [Out] byte[] termios_data);

        [DllImport("libc")]
        public static extern int open(string pathname, OpenFlags flags);

        [DllImport("libc")]
        public static extern int close(int fd);

        [DllImport("libc")]
        public static extern int read(int fd, IntPtr buf, int count);

        [DllImport("libc")]
        public static extern int write(int fd, IntPtr buf, int count);

        [DllImport("libc")]
        public static extern int tcsetattr(int fd, int optional_actions, byte[] termios_data);

        [DllImport("libc")]
        public static extern int cfsetspeed(byte[] termios_data, SerialSpeed speed);
    }
}