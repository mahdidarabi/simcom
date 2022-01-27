using System.Text;
using SimCom.Utilities;

Console.WriteLine("App Started...\n");

SimComUtility simCom = new SimComUtility();
try
{
    bool result = simCom.sendSms("+989337983009", "تست");
    Console.WriteLine(result.ToString());
}
catch (System.Exception)
{
    Console.WriteLine("ERROR");
}


