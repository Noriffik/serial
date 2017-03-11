using System;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    [Flags]
    public enum OpenFlags
    {
        //
        // One of these
        //
        O_RDONLY = 0,
        O_WRONLY = 1,
        O_RDWR = 2,

        O_NONBLOCK = 4,

        //
        // Or-ed with zero or more of these
        //
//        O_CREAT = 64, //Octal - 0100
//        O_EXCL = 128, // Octal - 0200
//        O_NOCTTY = 256, // Octal - 0400
//        O_TRUNC = 512, //Octal - 01000
//        O_APPEND = 1024, //Octal -02000
//        O_NONBLOCK = 2048, // Octal -04000
//        O_SYNC = 1052672, //Octal -04010000
    }
}
