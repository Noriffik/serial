namespace ThinkingHome.SerialPort.ConsoleApp.Internal
{
    public enum MTRFMode : byte
    {
        TX = 0,
        RX = 1,
        TXF = 2,
        RXF = 3,
        Service = 4,
        Update = 5
    }
}
