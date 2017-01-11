using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace ThinkingHome.SerialPort
{
    public class SerialPort
    {
        public static string [] GetPortNames ()
        {
            var result = new List<string>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var subkey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM"))
                {
                    if (subkey != null) {

                        foreach (var value in subkey.GetValueNames()) {

                            var port = subkey.GetValue(value, "").ToString();

                            if (!string.IsNullOrEmpty(port))
                            {
                                result.Add(port);
                            }
                        }
                    }
                }
            }
            else
            {
                // regex from https://github.com/4refr0nt/ESPlorer/blob/8c32b5512adc31d8ce896b57fd671a89dc65c34d/ESPlorer/src/jssc/SerialPortList.java
                var regex = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    ? new Regex("tty.(serial|usbserial|usbmodem).*|cu.*", RegexOptions.Compiled)
                    : new Regex("(ttyS|ttyUSB|ttyACM|ttyAMA|rfcomm|ttyO)[0-9]{1,3}", RegexOptions.Compiled);

                result.AddRange(
                    Directory.GetFiles("/dev/", "tty*")
                        .Where(dev => regex.IsMatch(dev)));
            }

            return result.ToArray();
        }
    }
}