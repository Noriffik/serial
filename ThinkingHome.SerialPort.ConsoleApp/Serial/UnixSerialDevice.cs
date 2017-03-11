using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    public class UnixSerialDevice : SerialDevice
    {
        private readonly string portName;
        private readonly BaudRate baudRate;
        private Task reading;
        private CancellationTokenSource cts;

        private int? fd;

        public UnixSerialDevice(string portName, BaudRate baudRate)
        {
            this.portName = portName;
            this.baudRate = baudRate;
        }

        public override void Open()
        {
            int fd = Libc.open(portName, OpenFlags.O_RDWR | OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                throw new Exception($"failed to open port ({portName})");
            }

            byte[] termiosData = new byte[256];

            Libc.tcgetattr(fd, termiosData);
            Libc.cfsetspeed(termiosData, baudRate);
            Libc.tcsetattr(fd, 0, termiosData);

            cts = new CancellationTokenSource();

            reading = Task.Run(() =>
            {
                byte[] buf = new byte[1700];
                IntPtr ptr = Marshal.AllocHGlobal(1700);

                string xxx = String.Empty;

                while (true)
                {
                    int res = Libc.read(fd, ptr, 1700);

                    if (res != -1)
                    {
                        Marshal.Copy(ptr, buf, 0, res);

                        for (var i = 0; i <= res; i++)
                        {
                            xxx += "." + buf[i].ToString("x2");

                            if (xxx.Length / 3 == 18)
                            {
                                Console.WriteLine("read: {0}", xxx);
                                xxx = String.Empty;
                                buf = new byte[1700];
                            }
                        }
                    }

                    Thread.Sleep(50);

                    cts.Token.ThrowIfCancellationRequested();
                }
            }, cts.Token);

            this.fd = fd;
        }

        public override void Close()
        {
            if (fd.HasValue)
            {
                cts.Cancel();
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