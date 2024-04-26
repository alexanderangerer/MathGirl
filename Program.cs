using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Threading.Channels;

namespace MathGirl;

class Program
{
    // Globale Variablen
    // Soll später durch eine Datei mit den gespeicherten Werten ersetzt werden.
    public static class Global
    {
        // Speichert die grösste Zahl, die für die Rechnungen benutzt werden soll.
        public static int LargestNumber = 20;
        public static bool Continue = true;

        public static char[] MathOperators = new char[]
        {
            '+',
            '-'
        };
    }

    static void Main(string[] args)
    {
        var contents = File.ReadAllText("configuration.json");
        var json = JsonSerializer.Deserialize<JsonElement>(contents, JsonSerializerOptions.Default);
        var largestNumberElement = json.GetProperty("LargestNumber");
        Global.LargestNumber = largestNumberElement.GetInt32();

        bool isRuning = true;


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

    static void CreateTask()
    {
        bool inputResult = false;
        //bool continueCalc = true;
        string[] newNumber = new string[3];
        int number1;
        int number2;
        char mathOperator;

        while (Global.Continue)
        {
            newNumber = NumberDetermine();
            number1 = Convert.ToInt32(newNumber[0]);
            number2 = Convert.ToInt32(newNumber[1]);
            mathOperator = Convert.ToChar(newNumber[2]);

            inputResult = ShowCalculation(number1, number2, mathOperator);

            while (!inputResult && Global.Continue)
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
        Global.Continue = true;
    }

    static void ChangeSettings()
    {
        byte inputUser = ShowSettings();

        switch (inputUser)
        {
            case 0:
                break;
            case 1:
                Console.WriteLine("Aktueller Wert: {0}", Global.LargestNumber);
                Console.Write("Neue Grösste Zahl: ");
                Global.LargestNumber = Convert.ToInt32(Console.ReadLine());
                break;
        }

        Console.Clear();
    }

    static string[] NumberDetermine()
    {
        Random rndNumber = new Random();
        int number1 = rndNumber.Next(0, Global.LargestNumber);
        int number2 = rndNumber.Next(0, Global.LargestNumber);
        int mathOperatorIndex = rndNumber.Next(0, 100);
        string[] retunrArray = new string[3];
        char mathOperator;

        if (mathOperatorIndex % 2 == 0)
            mathOperator = Global.MathOperators[0];
        else
            mathOperator = Global.MathOperators[1];

        // Damit keine Negativen Ergebnisse entstehen, müssen die Zahlen eventuell gekehrt werden.
        if (number1 < number2 && mathOperator == '-')
        {
            int temp = number1;
            number1 = number2;
            number2 = temp;
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
            Global.Continue = false;
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

    static byte ShowSettings()
    {
        byte input;
        Console.Write("Passwort: ");
        if (Console.ReadLine() == "aaPhoto80")
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