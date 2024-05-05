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
    private string path = "Contents/configuration.mg";
    
    public SystemSettings()
    {
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

    private void SaveNewSettings()
    {
        string[] newSettings = new string[]
        {
            "LargestNumber: " + this.LargestNumber,
            "Password: " + this.Password,
            "MathOperators: [" + this.MathOperators + "]"
        };
        
        File.WriteAllLines(path, newSettings);
    }

    public string GetPassword()
    {
        // Decode - Entschlüsseln
        var encodePassword = Convert.FromBase64String(this.Password);
        return Encoding.UTF8.GetString(encodePassword);
    }

    public void SetPassword(string newPassword)
    {
        // Encode - Verschlüsseln
        var utf8byte = System.Text.Encoding.UTF8.GetBytes(newPassword);
        this.Password = System.Convert.ToBase64String(utf8byte);
        
        SaveNewSettings();
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