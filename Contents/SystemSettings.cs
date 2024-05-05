using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace MathGirl.Contents;

public class SystemSettings
{
    public int LargestNumber;
    private string Password;
    public char[] MathOperators;
    
    public SystemSettings()
    {
        string path = "Contents/configuration.mg";
        
        string[] contents = File.ReadAllText(path).Split("\n");

        foreach (string configValue in contents)
        {
            if (configValue != "{" && configValue != "}")
            {
                SplitConfig(configValue);
            }
            
        }
    }

    private void SplitConfig(string configItem)
    {
        char[] trimChar = { '\"', '[', ']', ',', ' ' };
        
        string[] contents = configItem.Split(":");
        string configName = contents[0];
        string configValue = contents[1].Trim(trimChar);

        switch (configName)
        {
            case "LargestNumber":
                this.LargestNumber = Convert.ToInt32(configValue);
                break;
            case "Password":
                this.Password = configValue;
                break;
            case "MathOperators":
                MathOperatorToArray(configValue);
                break;
        }
    }

    public string GetPassword()
    {
        // Encode - Verschlüsseln
        // var utf8byte = System.Text.Encoding.UTF8.GetBytes(password);
        // Console.WriteLine(utf8byte);
        // var decode = System.Convert.ToBase64String(utf8byte);
        // Console.WriteLine(decode);
        
        // Decode - Entschlüsseln
        var encodePassword = Convert.FromBase64String(this.Password);
        return Encoding.UTF8.GetString(encodePassword);
    }
    private void MathOperatorToArray(string operators)
    {
        string[] operatorsArray = operators.Split(',');

        this.MathOperators = new char[operatorsArray.Length];

        for (int i = 0; i < operatorsArray.Length; i++)
        {
            char activOperator = Convert.ToChar(operatorsArray[i].Trim());
            this.MathOperators[i] = activOperator;
        }
        
    }
}