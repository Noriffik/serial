using System;
using System.Threading.Tasks.Dataflow;

namespace ThinkingHome.SerialPort.ConsoleApp
{
    public class MTRFXXAdapter
    {
        private const byte START_MARKER = 171;
        private const byte STOP_MARKER = 172;

        public enum MTRFMode : byte
        {
            TX = 0,
            RX = 1,
            TXF = 2,
            RXF = 3,
            Service = 4,
            Update = 5
        }

        public enum MTRFAction : byte
        {
            SendCommand = 0,
            SendBroadcastCommand = 1,
            ReadResponseBuffer = 2,
            StartBinding = 3,
            StopBinding = 4,
            ClearChannel = 5,
            ClearAllChannels = 6,
            Unbind = 7,
            SendTargetedCommand = 8
        }

        public enum MTRFRepeatCount : byte
        {
            NoRepeat = 0,
            One = 1,
            Two = 2,
            Three = 3
        }

        public enum MTRFCommand : byte
        {
            Off = 0,
            On = 2,
        }

        public enum MTRFDataFormat : byte
        {
            NoData = 0
        }

        public static byte[] BuildCommand(MTRFMode mode, MTRFAction action, MTRFRepeatCount repeatCount, byte channel,
            MTRFCommand command, MTRFDataFormat format, byte[] data, UInt32 target = 0)
        {
            byte actionAndRepeatCount = (byte) ((byte) action | ((byte) repeatCount << 6));

            var res = new byte[]
            {
                START_MARKER,         // 0: start marker
                (byte) mode,          // 1: device mode
                actionAndRepeatCount, // 2: action & repeat count
                0,                    // 3: reserved
                channel,              // 4: channel
                (byte) command,       // 5: command
                (byte) format,        // 6: data format
                0,0,0,0,              // 7..10: data
                0,0,0,0,              // 11..14: target device id
                0,                    // 15: checksum
                STOP_MARKER           // 16: stop marker
            };

            for (int i = 0; i < 15; i++) res[15] += res[i];

            return res;
        }
    }
}