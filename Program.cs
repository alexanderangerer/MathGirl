using System.Threading.Channels;
using MathGirl.Contents;
// Muss noch erforscht werden.
using System.Text.Json;

namespace MathGirl;

class Program
{
    /// <summary>
    /// Ist true, solange das Spiel weiterläuft.
    /// </summary>
    static bool isRuning = true;
    

    static void Main(string[] args)
    {
        while (isRuning)
        {
            byte inputUser = ShowMainMenu();

            switch (inputUser)
            {
                case 1:
                    Console.Clear();
                    CreateTask();
                    break;
                case 2:
                    isRuning = false;
                    break;
                case 8:
                    // Globale Einstellungen anpassen.
                    ChangeSettings();
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
    static void CreateTask()
    {
        bool inputResult = false;
        //bool continueCalc = true;
        string[] newNumber = new string[3];
        int number1;
        int number2;
        char mathOperator;

        while (isRuning)
        {
            newNumber = NumberDetermine();
            number1 = Convert.ToInt32(newNumber[0]);
            number2 = Convert.ToInt32(newNumber[1]);
            mathOperator = Convert.ToChar(newNumber[2]);

            inputResult = ShowCalculation(number1, number2, mathOperator);

            while (!inputResult && isRuning)
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
        isRuning = true;
    }

    /// <summary>
    /// Zeigt die gespeicherten Einstellungen an.
    /// </summary>
    static void ChangeSettings()
    {
        // Hier wird das Settings-Menü aufgerufen und die Wahl des Users zurückgegeben.
        byte inputUser = ShowSettings();

        // Der eingegebene Wert wird geprüft und entsprechend abgeglichen.
        switch (inputUser)
        {
            case 0:
                break;
            case 1:
                // Hier wird die grösste Nummer für die Rechenoperatoren gespeichert.
                Console.WriteLine("Aktueller Wert: {0}", Globals.LargestNumber);
                Console.Write("Neue Grösste Zahl: ");
                Globals.LargestNumber = Convert.ToInt32(Console.ReadLine());
                break;
            default:
                Console.WriteLine("Keine gültige Eingabe!");
                break;
        }

        // Bevor das nächste Menü angezeigt wird, wird der Bildschirm geleert.
        Console.Clear();
    }

    static string[] NumberDetermine()
    {
        Random rndNumber = new Random();
        int number1 = rndNumber.Next(0, Globals.LargestNumber);
        int number2 = rndNumber.Next(0, Globals.LargestNumber);
        int mathOperatorIndex = rndNumber.Next(0, 100);
        string[] retunrArray = new string[3];
        char mathOperator;

        if (mathOperatorIndex % 2 == 0)
            mathOperator = Globals.MathOperators[0];
        else
            mathOperator = Globals.MathOperators[1];

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

        Console.Write("Was ergibt: {0} {1} {2} = ", number1, mathOperator, number2);
        inputResult = Convert.ToInt32(Console.ReadLine());

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
            isRuning = false;
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
    static byte ShowSettings()
    {
        byte input;
        // Damit die Einstellungen angepasst werden können, muss ein Passwort eingegeben werden.
        Console.Write("Passwort: ");
        
        if (Console.ReadLine() == Globals.Password)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("*** Einstellungen ***");
            Console.WriteLine("*********************");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[1] Grösste Zahl festlegen");
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