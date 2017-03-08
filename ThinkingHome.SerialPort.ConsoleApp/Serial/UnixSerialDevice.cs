using System;
using System.Runtime.InteropServices;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    public class UnixSerialDevice : SerialDevice
    {
        private readonly string portName;
        private readonly BaudRate baudRate;

        private int? fd;

        public UnixSerialDevice(string portName, BaudRate baudRate)
        {
            this.portName = portName;
            this.baudRate = baudRate;
        }

        public override void Open()
        {
            int fd = Libc.open(portName, OpenFlags.O_RDWR | OpenFlags.O_NOCTTY | OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                throw new Exception($"failed to open port ({portName})");
            }

            byte[] termiosData = new byte[256];

            Libc.tcgetattr(fd, termiosData);
            Libc.cfsetspeed(termiosData, BaudRate.B9600);
            Libc.tcsetattr(fd, 0, termiosData);

            this.fd = fd;
        }

        public override void Close()
        {
            if (fd.HasValue)
            {
                Libc.close(fd.Value);
            }
        }

        public override void Write(byte[] buf)
        {
            if (fd.HasValue)
            {
                IntPtr ptr = Marshal.AllocHGlobal(buf.Length);
                Marshal.Copy(buf, 0, ptr, buf.Length);

                Libc.write(fd.Value, ptr, buf.Length);
            }
            else
            {
                throw new Exception();
            }
        }

        public override void Dispose()
        {
            Close();
        }
    }
}
