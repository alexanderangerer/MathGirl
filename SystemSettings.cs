using System.Text;

namespace MathGirl;

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
    /// Speichert den Pfad der Datei. Durch AppContext.BaseDirectory wird der Pfad bis zur App
    /// ermittelt und mit dem bekannten Pfad zur Einstellungsdatei kombiniert.
    /// </summary>
    private readonly string _path = Path.Combine(AppContext.BaseDirectory, "Contents/configuration.mg");

    /// <summary>
    /// Der Konstruktor initialisiert alle Variablen mit den Werten aus der configuration.mg Datei.
    /// </summary>
    public SystemSettings()
    {
        // Die Datei wird geöffnet, alle Zeilen eingelesen und in ein Array umgewandelt.
        string[] contents = File.ReadAllText(_path).Split("\n");

        // Jedes Array wird druchlaufen um in einer Methode die Werte auszulesen.
        foreach (string configValue in contents)
        {
            if (configValue == "")
            {
                break;
            }
            // Das Entnehmen der Informationen geschieht in einer Methode.
            SplitConfig(configValue);
        }
    }

    /// <summary>
    /// Die einzelnen Zeilen der configuration.mg werden hier getrennt in Schlüssel und Werte.
    /// </summary>
    /// <param name="configItem">Schlüssel Wert-Paar, getrennt durch ein Doppelpunkt</param>
    private void SplitConfig(string configItem)
    {
        // Die Zeichen, welche aus dem übergebenen configItem entfernt werden müssen.
        char[] trimChar = { '\"', '[', ']', ',', ' ' };

        // Nun wird der Schlüssel und der Wert je in einem String gespeichert.
        string[] contents = configItem.Split(":");
        var configName = contents[0].Trim(trimChar);
        var configValue = contents[1].Trim(trimChar);

        // Nun wird geprüft, um welchen Schlüssel es sich handelt und entsprechend wird 
        // die Variable mit dem Wert gefüllt.
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

    /// <summary>
    /// Hier werden die Werte, welche aktuell in den Variablen gespeichert sind, in die Datei geschrieben.
    /// </summary>
    private void SaveNewSettings()
    {
        // Zuerst muss der Array, in welchem die mathematischen Operatoren gespeichert werden
        // in einen String umgewandelt.
        string toSaveMathOperator = this.MathOperatorToString();
        
        // Es wird nun ein String-Array mit allen Werten gebildet.
        string[] newSettings = new string[]
        {
            "LargestNumber: " + this._largestNumber,
            "Password: " + this._password,
            "MathOperators: " + toSaveMathOperator
        };

        // Der bereits vorhandene Werte werden mit den neuen überschrieben.
        File.WriteAllLines(_path, newSettings);
    }

    /// <summary>
    /// Liefert das aktuelle Passwort.
    /// </summary>
    /// <returns>Das entschlüsselte Passwort.</returns>
    public string GetPassword()
    {
        // Decode - Entschlüsseln
        var encodePassword = Convert.FromBase64String(this._password);
        // Liefert das entschlüsselte Passwort zurück.
        return Encoding.UTF8.GetString(encodePassword);
    }

    /// <summary>
    /// Setzt ein neues Passwort.
    /// </summary>
    /// <param name="newPassword">Das neue Passwort, vor der Verschlüsselung.</param>
    public void SetPassword(string newPassword)
    {
        // Encode - Verschlüsseln
        var utf8Byte = Encoding.UTF8.GetBytes(newPassword);
        this._password = Convert.ToBase64String(utf8Byte);

        // Alle Einstellungen werden gespeichert. 
        SaveNewSettings();
    }

    /// <summary>
    /// Liefert die grösste Zahl die verwendet wird.
    /// </summary>
    /// <returns>Die grösste Zahl.</returns>
    public int GetLargestNumber()
    {
        return this._largestNumber;
    }

    /// <summary>
    /// Setzt die grösste zu verwendende Zahl.
    /// </summary>
    /// <param name="newLargestNumber">Die Zahl die neu als Limite gelten soll.</param>
    public void SetLargestNumber(int newLargestNumber)
    {
        this._largestNumber = newLargestNumber;
        
        // Alle Einstellungen werden gespeichert. 
        SaveNewSettings();
    }

    /// <summary>
    /// Liefert die mathematischen Operatoren, die aktuell verwendet werden.
    /// </summary>
    /// <returns>Liefert einen String mit den verwendeten mathematischen Operatoren.</returns>
    public string GetMathOperators()
    {
        return this.MathOperatorToString();
    }

    /// <summary>
    /// Speichert die übergebenen mathematischen Operatoren.
    /// </summary>
    /// <param name="newMathOperators">Die neuen Operatoren.</param>
    public void SetMathOperators(string newMathOperators)
    {
        // Im übergebenen String werden eventuell vorhandene Leerzeichen entfernt.
        string newMathOperatorsTrim = newMathOperators.Trim();
        // Das letzte Zeichen des Strings wird gespeichert.
        char lastSign = newMathOperators[newMathOperatorsTrim.Length - 1];

        // Sollte das letzte Zeichen ein , (Komma) sein, muss dieses entfernt werden.
        if (lastSign == ',')
        {
            newMathOperatorsTrim = newMathOperatorsTrim.Substring(0, newMathOperatorsTrim.Length - 1);
        }
        
        // In dieser Methode werden die Operatoren in ein Array umgewandelt, was dann
        // in der entsprechenden Variable gespeichert wird.
        MathOperatorToArray(newMathOperatorsTrim);
        
        // Alle Einstellungen werden gespeichert. 
        SaveNewSettings();
    }

    /// <summary>
    /// Wandelt die mathematischen Operatoren in ein Array um.
    /// </summary>
    /// <param name="operators">Die umzuwandelnden Operatoren in einem String.</param>
    private void MathOperatorToArray(string operators)
    {
        // Der String wird in einen Array verwandelt.
        string[] operatorsArray = operators.Split(',');

        // Die globale Variable wird neu definiert, anhand der länge
        // des gerade gebildeten Arrays.
        this.MathOperators = new char[operatorsArray.Length];
        
        for (int i = 0; i < operatorsArray.Length; i++)
        {
            // Ist der aktuelle Wert im Array kein Leerstring ...
            if (operatorsArray[i] != "")
            {
                // ... wird dieser zu einem Char konvertiert und Leerzeichen entfernt.
                char activOperator = Convert.ToChar(operatorsArray[i].Trim());
                // Danach wird die in der globalen Variable gespeichert.
                this.MathOperators[i] = activOperator;
            }
        }
    }
    
    /// <summary>
    /// Wandelt den Array mit den mathematischen Operatoren in ein String um.
    /// </summary>
    /// <returns>Die mathematischen Operatoren als String, durch , (kommas) getrennt.</returns>
    private string MathOperatorToString()
    {
        string saveString = "";
        
        // Jeder Arraywert wird in einem String gespeichert und mit einem , (Komma) versehen.
        for (int i = 0; i < this.MathOperators.Length; i++)
        {
            saveString += this.MathOperators[i] + ",";
        }

        // Das zuletzt hinzugefügte , (Komma) muss wieder entfernt werden.
        saveString = saveString.Substring(0, saveString.Length - 1);

        return saveString;
    }
}