using System.Text;

namespace MathGirl.Contents;

/// <summary>
/// Behandelt alle Systemeinstellungen für das Spiel MathGirl.
/// </summary>
public class SystemSettings
{
    /// <summary>
    /// Speichert die grösste Zahl, welche zum Rechnen verwendet werden soll.
    /// </summary>
    private int _largestNumber;
    /// <summary>
    /// Speichert das Passwort in verschlüsselter Form.
    /// </summary>
    private string _password = "";
    /// <summary>
    /// Enthält alle mathematischen Operatoren, die zum Rechnen verwenden darf.
    /// </summary>
    public char[]? MathOperators;
    /// <summary>
    /// Speichert den 
    /// </summary>
    private readonly string _path = "Contents/configuration.mg"; //"Contents/configuration.mg";

    /// <summary>
    /// Der Konstruktor initialisiert alle Variablen mit den Werten aus der configuration.mg Datei.
    /// </summary>
    public SystemSettings()
    {
        string[] contents = File.ReadAllText(_path).Split("\n");

        foreach (string configValue in contents)
        {
            if (configValue == "")
            {
                break;
            }
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
        string toSaveMathOperator = this.MathOperatorToString();
        
        string[] newSettings = new string[]
        {
            "LargestNumber: " + this._largestNumber,
            "Password: " + this._password,
            "MathOperators: " + toSaveMathOperator
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

    public string GetMathOperators()
    {
        return this.MathOperatorToString();
    }

    public void SetMathOperators(string newMathOperators)
    {
        string newMathOperatorsForArray = "";
        string newMathOperatorsTrim = newMathOperators.Trim();
        char lastSign = newMathOperators[newMathOperatorsTrim.Length - 1];

        if (lastSign == ',')
        {
            newMathOperatorsTrim = newMathOperatorsTrim.Substring(0, newMathOperatorsTrim.Length - 1);
        }
        
        MathOperatorToArray(newMathOperatorsTrim);
        
        SaveNewSettings();
    }

    private void MathOperatorToArray(string operators)
    {
        string[] operatorsArray = operators.Split(',');

        this.MathOperators = new char[operatorsArray.Length];

        for (int i = 0; i < operatorsArray.Length; i++)
        {
            if (operatorsArray[i] != "")
            {
                char activOperator = Convert.ToChar(operatorsArray[i].Trim());
                this.MathOperators[i] = activOperator;
            }
        }
    }
    private string MathOperatorToString()
    {
        string saveString = "";
        for (int i = 0; i < this.MathOperators.Length; i++)
        {
            saveString += this.MathOperators[i] + ",";
        }

        saveString = saveString.Substring(0, saveString.Length - 1);

        return saveString;
    }
}