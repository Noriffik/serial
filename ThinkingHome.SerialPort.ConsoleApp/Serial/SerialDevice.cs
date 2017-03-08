using System;
using System.Runtime.InteropServices;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    public abstract class SerialDevice : IDisposable
    {
        public abstract void Open();

        public abstract void Close();

        public abstract void Write(byte[] buf);

        public abstract void Dispose();

        public static SerialDevice Create(string portName, BaudRate baudRate)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsSerialDevice(portName, baudRate);
            }
            else
            {
                return new UnixSerialDevice(portName, baudRate);
            }
        }
    }
}
