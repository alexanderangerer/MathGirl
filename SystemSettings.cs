using System.Text;

namespace MathGirl.Contents;

public class SystemSettings
{
    private int _largestNumber;
    private string _password = "";
    public char[]? MathOperators;
    private readonly string _path = "Contents/configuration.mg"; //"Contents/configuration.mg";

    public SystemSettings()
    {
        string[] contents = File.ReadAllText(_path).Split("\n");

        foreach (string configValue in contents)
        {
            SplitConfig(configValue);
        }
    }

    private void SplitConfig(string configItem)
    {
        char[] trimChar = { '\"', '[', ']', ',', ' ' };

        string[] contents = configItem.Split(":");
        var configName = contents[0];
        var configValue = contents[1].Trim(trimChar);

        switch (configName)
        {
            case "LargestNumber":
                this._largestNumber = Convert.ToInt32(configValue);
                break;
            case "Password":
                this._password = configValue;
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
            "LargestNumber: " + this._largestNumber,
            "Password: " + this._password,
            "MathOperators: " + this.MathOperators
        };

        File.WriteAllLines(_path, newSettings);
    }

    public string GetPassword()
    {
        // Decode - Entschlüsseln
        var encodePassword = Convert.FromBase64String(this._password);
        return Encoding.UTF8.GetString(encodePassword);
    }

    public void SetPassword(string newPassword)
    {
        // Encode - Verschlüsseln
        var utf8Byte = Encoding.UTF8.GetBytes(newPassword);
        this._password = Convert.ToBase64String(utf8Byte);

        SaveNewSettings();
    }

    public int GetLargestNumber()
    {
        return this._largestNumber;
    }

    public void SetLargestNumber(int newLargestNumber)
    {
        this._largestNumber = newLargestNumber;
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