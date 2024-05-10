// using System.Threading.Channels;

using System.Threading.Channels;
using MathGirl.Contents;
// Muss noch erforscht werden.
// using System.Text.Json;

namespace MathGirl;

class Program
{
    /// <summary>
    /// Ist true, solange das Spiel weiterläuft.
    /// </summary>
    static bool _isRuning = true;
    

    static void Main(string[] args)
    {
        SystemSettings sysSettings = new SystemSettings();
        
        while (_isRuning)
        {
            byte inputUser = ShowMainMenu();

            switch (inputUser)
            {
                case 1:
                    Console.Clear();
                    CreateTask(sysSettings);
                    break;
                case 2:
                    _isRuning = false;
                    break;
                case 8:
                    // Globale Einstellungen anpassen.
                    ChangeSettings(sysSettings);
                    break;
                default:
                    Console.WriteLine("Keine gültige Eingabe.");
                    break;
            }
        }
    }
        
    /// <summary>
    /// Beschreibung der Methode
    /// </summary>
    static void CreateTask(SystemSettings sysSettings)
    {
        bool inputResult = false;
        //bool continueCalc = true;
        string[] newNumber = new string[3];
        int number1;
        int number2;
        char mathOperator;

        while (_isRuning)
        {
            newNumber = NumberDetermine(sysSettings);
            number1 = Convert.ToInt32(newNumber[0]);
            number2 = Convert.ToInt32(newNumber[1]);
            mathOperator = Convert.ToChar(newNumber[2]);

            inputResult = ShowCalculation(number1, number2, mathOperator);

            while (!inputResult && _isRuning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Leider ist das falsch. Versuche es nochmals.");
                inputResult = ShowCalculation(number1, number2, mathOperator);
                Console.ResetColor();
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sehr gut. Das war richtig. Weiter gehts.");
            Console.ResetColor();
        }

        Console.Clear();
        _isRuning = true;
    }

    /// <summary>
    /// Zeigt die gespeicherten Einstellungen an.
    /// </summary>
    static void ChangeSettings(SystemSettings sysSettings)
    {
        // Hier wird das Settings-Menü aufgerufen und die Wahl des Users zurückgegeben.
        byte inputUser = ShowSettings(sysSettings);

        // Der eingegebene Wert wird geprüft und entsprechend abgeglichen.
        switch (inputUser)
        {
            case 0:
                break;
            case 1:
                // Hier wird die grösste Nummer für die Rechenoperatoren gespeichert.
                Console.WriteLine("Aktueller Wert: {0}", sysSettings.LargestNumber);
                Console.Write("Neue Grösste Zahl: ");
                sysSettings.LargestNumber = Convert.ToInt32(Console.ReadLine());
                break;
            case 2:
                break;
            case 3:
                string neuPassword = DetermineNewPassword();
                if (neuPassword != "")
                    sysSettings.SetPassword(neuPassword); 
                break;
            default:
                Console.WriteLine("Keine gültige Eingabe!");
                break;
        }

        // Bevor das nächste Menü angezeigt wird, wird der Bildschirm geleert.
        Console.Clear();
    }

    static string DetermineNewPassword()
    {
        Console.Write("Neues Passwort eingeben: ");
        string firstInputPassword = Console.ReadLine();
        Console.Write("Passwort nochmals eingeben: ");
        string secondInputPassword = Console.ReadLine();

        if (firstInputPassword == secondInputPassword)
        {
            return firstInputPassword;
        }

        return "";
    }
    
    static string[] NumberDetermine(SystemSettings sysSettings)
    {
        Random rndNumber = new Random();
        int number1 = rndNumber.Next(0, sysSettings.LargestNumber);
        int number2 = rndNumber.Next(0, sysSettings.LargestNumber);
        int mathOperatorIndex = rndNumber.Next(0, 100);
        string[] retunrArray = new string[3];
        char mathOperator;

        if (mathOperatorIndex % 2 == 0)
            mathOperator = sysSettings.MathOperators[0];
        else
            mathOperator = sysSettings.MathOperators[1];

        // Damit keine Negativen Ergebnisse entstehen, müssen die Zahlen eventuell gekehrt werden.
        if (number1 < number2 && mathOperator == '-')
        {
            (number1, number2) = (number2, number1);
        }

        retunrArray[0] = Convert.ToString(number1);
        retunrArray[1] = Convert.ToString(number2);
        retunrArray[2] = Convert.ToString(mathOperator);

        return retunrArray;
    }

    static bool ShowCalculation(int number1, int number2, char mathOperator)
    {
        int inputResult;
        int calculation = 0;

        Console.WriteLine("Beendet mit -1");
        Console.Write("Was ergibt: {0} {1} {2} = ", number1, mathOperator, number2);
        try
        {
            inputResult = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            return false;
            throw;
        }
        
        // inputResult = Convert.ToInt32(Console.ReadLine());
        
        switch (mathOperator)
        {
            case '+':
                calculation = number1 + number2;
                break;
            case '-':
                calculation = number1 - number2;
                break;
        }

        if (inputResult == -1)
        {
            _isRuning = false;
        }

        if (inputResult == calculation)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Zeigt das Hauptmenü 
    /// </summary>
    /// <returns></returns>
    static byte ShowMainMenu()
    {
        byte input;

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("*** MathGirl ***");
        Console.WriteLine("****************");
        Console.WriteLine();
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[1] Spiel starten");
        Console.WriteLine("[2] Programm beenden");
        Console.WriteLine();
        Console.WriteLine("[8] Einstellungen");
        Console.ResetColor();

        Console.Write("Aktion: ");
        input = Convert.ToByte(Console.ReadLine());
        return input;
    }

    /// <summary>
    /// Zeigt das Einstellungs-Menü an und nimmt die Eingabe des Benutzers entgegen.
    /// </summary>
    /// <returns>Die Menüwahl des Users.</returns>
    static byte ShowSettings(SystemSettings sysSettings)
    {
        byte input;
        // Damit die Einstellungen angepasst werden können, muss ein Passwort eingegeben werden.
        Console.Write("Passwort: ");
        string inputPassword = "";
        
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            inputPassword += key.KeyChar;
            Console.Write("*");
        }
        
        if (inputPassword == sysSettings.GetPassword())
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("*** Einstellungen ***");
            Console.WriteLine("*********************");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[1] Grösste Zahl festlegen");
            Console.WriteLine("[2] Operatoren anpassen");
            Console.WriteLine("[3] Passwort ändern");
            Console.WriteLine();
            Console.WriteLine("[0] Zurück");
            Console.ResetColor();

            // Nimmt die Auswahl des Users auf und gibt diese zurück.
            input = Convert.ToByte(Console.ReadLine());
            return input;
        }
        else
        {
            Console.WriteLine("Falsches Passwort!");
            return 0;
        }
    }
}