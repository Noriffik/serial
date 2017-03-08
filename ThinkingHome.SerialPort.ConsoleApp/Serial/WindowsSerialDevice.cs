using System;

namespace ThinkingHome.SerialPort.ConsoleApp.Serial
{
    public class WindowsSerialDevice : SerialDevice
    {
        public WindowsSerialDevice(string portName, BaudRate baudRate)
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buf)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
