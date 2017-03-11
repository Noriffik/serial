using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    public class UnixSerialDevice : SerialDevice
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken CancellationToken => cts.Token;

        private int? fd;
        private Task reading;

        public UnixSerialDevice(string portName, BaudRate baudRate) : base(portName, baudRate)
        {
        }

        public override void Open()
        {
            // open serial port
            int fd = Libc.open(portName, Libc.OpenFlags.O_RDWR | Libc.OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                throw new Exception($"failed to open port ({portName})");
            }

            // set baud rate
            byte[] termiosData = new byte[256];

            Libc.tcgetattr(fd, termiosData);
            Libc.cfsetspeed(termiosData, baudRate);
            Libc.tcsetattr(fd, 0, termiosData);

            // start reading
            reading = Task.Run(() =>
            {
                IntPtr ptr = Marshal.AllocHGlobal(1024);

                while (!CancellationToken.IsCancellationRequested)
                {
                    int res = Libc.read(fd, ptr, 1024);

                    if (res != -1)
                    {
                        byte[] buf = new byte[res];
                        Marshal.Copy(ptr, buf, 0, res);

                        OnDataReceived(buf);
                    }

                    Thread.Sleep(50);
                }
            }, CancellationToken);

            this.fd = fd;
        }

        public override bool IsOpened => fd.HasValue;

        public override void Close()
        {
            if (!fd.HasValue)
            {
                throw new Exception();
            }

            cts.Cancel();
            reading.Wait(CancellationToken);
            Libc.close(fd.Value);
        }

        public override void Write(byte[] buf)
        {
            if (!fd.HasValue)
            {
                throw new Exception();
            }

            IntPtr ptr = Marshal.AllocHGlobal(buf.Length);
            Marshal.Copy(buf, 0, ptr, buf.Length);
            Libc.write(fd.Value, ptr, buf.Length);
        }
    }
}