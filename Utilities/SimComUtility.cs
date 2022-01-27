using System.IO;
using System.IO.Ports;
using System.Text;

namespace SimCom.Utilities;

public class SimComUtility
{
    private string _enter = Environment.NewLine;

    private SerialPort _port = new SerialPort();

    public SimComUtility()
    {
        _port.PortName = "/dev/ttyUSB0";
    }


    public SimComUtility(string portName)
    {
        _port.PortName = portName;
    }

    public bool sendSms(string phoneNumber, string msg)
    {
        try
        {
            _port.Close();
            _port.Open();
        }
        catch (System.Exception exception)
        {
            throw exception;
        }

        _port.Write("AT" + _enter);
        Thread.Sleep(100);

        _port.Write("AT+CSCS=\"HEX\"" + _enter);
        Thread.Sleep(100);

        _port.Write("AT+CSMP=49,167,0,8" + _enter);
        Thread.Sleep(100);

        _port.Write("AT+CMGF=1" + _enter);
        Thread.Sleep(100);

        _port.Write("AT+CMGS=\"" + phoneNumber + "\"" + _enter);
        Thread.Sleep(100);

        string unicodedMessage = convertToHex(msg);

        _port.Write(unicodedMessage + (char)26 + _enter);
        Thread.Sleep(100);

        var response = _port.ReadExisting();

        _port.Close();

        bool smsSent = !response.Contains("ERROR");
        return smsSent;
    }

    private string convertToHex(string text)
    {
        byte[] ba = Encoding.Unicode.GetBytes(text);
        var hexString = BitConverter.ToString(ba);
        string[] ha = hexString.Split("-");

        for (var i = 0; i < ha.Length; i += 2)
        {
            var temp = ha[i];
            ha[i] = ha[i + 1];
            ha[i + 1] = temp;
        }

        hexString = String.Join("", ha);
        return hexString;
    }

}