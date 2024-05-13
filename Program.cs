// using System.Threading.Channels;

using System.Reflection;
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
        // Environment.CurrentDirectory: Enthält oder gibt das Arbeitsverzeichnis an.
        // Path.GetDirectoryName: Ermittelt Informationen über das angegebene Verzeichnis.
        // Assembly.GetEntryAssembly(): Ruft die ausführbare Prozessordatei auf.
        // ?.Location) ?? "~"
        // Diese Zeile Code setzt den in der App benutzten relativen Pfad auf jenen in welchen die
        // App gestartet wurde.
        Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? "~";
        
        // Hier wird die Systemeinstellung für das Programm initialisiert.
        // Der Konstruktor liest auch gleich die Daten aus der configuration.mg ein.
        SystemSettings sysSettings = new SystemSettings();
        
        // Solange die Variable true ist, wird die Schleife weiter durchlaufen.
        while (_isRuning)
        {
            // Über eine Methode wird das Menü angezeigt und eine Wahl vom Benutzer entgegen genommen.
            byte inputUser = ShowMainMenu();

            // Der zurückgegebene Wert wird auf seinen Wert geprüft.
            switch (inputUser)
            {
                // Das Spiel wird gestartet.
                case 1:
                    Console.Clear();
                    CreateTask(sysSettings);
                    break;
                // Das Programm wird beendet
                case 2:
                    _isRuning = false;
                    break;
                // Ruft die Einstellungen auf
                case 8:
                    ChangeSettings(sysSettings);
                    break;
                // Alle anderen Eingaben werden nicht geprüft.
                default:
                    Console.WriteLine("Keine gültige Eingabe.");
                    break;
            }
        }
    }
        
    /// <summary>
    /// Hier wird eine mathematische Rechnung erzeugt.
    /// </summary>
    static void CreateTask(SystemSettings sysSettings)
    {
        // Speichert ob der Benutzer die Aufgabe korrekt gelöst hat.
        bool inputResult = false;
        // Speichert die Zahlen und den mathematischen Operator
        string[] newNumber = new string[3];
        // Speichert die beiden Zahlen ..
        int number1;
        int number2;
        // .. und den mathematischen Operator.
        char mathOperator;

        // Solange die globale Variable true ist, wird die Schleife weiter durchlaufen.
        while (_isRuning)
        {
            // Zuerst werden die Zahlen und der math. Operator ermittelt
            newNumber = NumberDetermine(sysSettings);
            // Das Array wird auf die 3 Variablen erteilt.
            number1 = Convert.ToInt32(newNumber[0]);
            number2 = Convert.ToInt32(newNumber[1]);
            mathOperator = Convert.ToChar(newNumber[2]);

            // Nun wird die math. Rechnung ausgegeben und die Eingabe zurückgegeben.
            inputResult = ShowCalculation(number1, number2, mathOperator);

            // Wurde die math. Rechnung falsch beantwortet und die globale Variable steht auf true,
            // so wird eine Fehlermeldung ausgegeben.
            while (!inputResult && _isRuning)
            {
                // Die Konsole wird zuerst geleert und die Farbe für den Text angepasst.
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Leider ist das falsch. Versuche es nochmals.");
                inputResult = ShowCalculation(number1, number2, mathOperator);
                // Die Farbeinstellungen werden wieder zurückgenommen.
                Console.ResetColor();
            }

            // Ist die Antwort korrekt, wird auch hier die Textfarbe angepasst ...
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sehr gut. Das war richtig. Weiter gehts.");
            // ... und am Schluss wieder zurückgesetzt.
            Console.ResetColor();
        }

        // Danach wird die Konsole wieder geleert.
        Console.Clear();
        _isRuning = true;
    }

    /// <summary>
    /// Zeigt das Menü für die Einstellungen.
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
                Console.WriteLine("Aktueller Wert: {0}", sysSettings.GetLargestNumber());
                Console.Write("Neue Grösste Zahl: ");
                sysSettings.SetLargestNumber(Convert.ToInt32(Console.ReadLine()));
                break;
            case 2:
                // Hier werden die Operatoren angezeigt und neue entgegen genommen.
                Console.WriteLine("Aktuelle eingesetzte Operatoren: {0}", sysSettings.GetMathOperators());
                Console.Write("Operatoren durch , getrennt eingeben:");
                sysSettings.SetMathOperators(Console.ReadLine());
                break;
            case 3:
                // Ein neues Passwort wird abgefragt.
                string neuPassword = DetermineNewPassword();
                if (neuPassword != "")
                    sysSettings.SetPassword(neuPassword); 
                break;
            default:
                // Es wurde keine gültige Taste gedrückt.
                Console.WriteLine("Keine gültige Eingabe!");
                break;
        }

        // Bevor das nächste Menü angezeigt wird, wird der Bildschirm geleert.
        Console.Clear();
    }

    /// <summary>
    /// Nimmt das neue Passwort entgegen und prüft die Eingabe.
    /// </summary>
    /// <returns>Das Passwort wenn alles korrekt eingegeben wurde, ansonsten wird ein Leerstring zurückgegeben.</returns>
    static string DetermineNewPassword()
    {
        // Das Passwort wird zweimal abgefragt und geprüft.
        Console.Write("Neues Passwort eingeben: ");
        string firstInputPassword = Console.ReadLine();
        Console.Write("Passwort nochmals eingeben: ");
        string secondInputPassword = Console.ReadLine();

        // Sind beide identisch, wird dieses zurückgegeben.
        if (firstInputPassword == secondInputPassword)
        {
            return firstInputPassword;
        }

        // Wurden zwei unterschiedliche Passwörter eingegeben, wird ein Leerstring zurückgegeben.
        return "";
    }
    
    /// <summary>
    /// Ermittelt die Zahlen und den mathematischen Opertor. Zusätzlich wird sichergestellt, dass bei
    /// substraktionen oder divisionen keine negativen Ergebnisse geliefert werden.
    /// </summary>
    /// <param name="sysSettings">Das Objekt zu den Systemeinstellungen.</param>
    /// <returns>Das Array mit den Zahlen und dem Operator.</returns>
    static string[] NumberDetermine(SystemSettings sysSettings)
    {
        // Damit Zufallszahlen ermittelt werden können, braucht es ein entsprechendes Objekt.
        Random rndNumber = new Random();
        // Es werden zwei Zahlen in den festgelegten Grenzen (LargestNumber) ermittelt. Die Methode Next
        // nutzt bei der oberen Grenze nicht den ganzen Wert. Die obere Grenze ist nicht einschliesslich.
        int number1 = rndNumber.Next(0, sysSettings.GetLargestNumber() + 1);
        int number2 = rndNumber.Next(0, sysSettings.GetLargestNumber() + 1);
        // Auch der mathematische Operator wird so ermittelt.
        int mathOperatorIndex = rndNumber.Next(0, sysSettings.MathOperators.Length);
        string[] retunrArray = new string[3];
        char mathOperator;

        // Der math. Operator wird aus der Systemeinstellungs-Klasse geholt.
        mathOperator = sysSettings.MathOperators[mathOperatorIndex];

        // Damit keine Negativen Ergebnisse entstehen, müssen die Zahlen eventuell gekehrt werden.
        if (number1 < number2 && mathOperator == '-')
        {
            (number1, number2) = (number2, number1);
        }
        // Damit auch bei den Divisionen keine negativen Zahlen entstehen, müssen die Zahlen
        // eventuell gewechselt werden.
        else if (number1 < number2 && mathOperator == '/')
        {
                (number1, number2) = (number2, number1);
        }

        // Beide Zahlen und der Operator werden in einem Array gespeichert und zurückgegeben.
        retunrArray[0] = Convert.ToString(number1);
        retunrArray[1] = Convert.ToString(number2);
        retunrArray[2] = Convert.ToString(mathOperator);

        return retunrArray;
    }

    /// <summary>
    /// Stellt die mathematische Rechnung zusammen und gibt sie aus. Danach wird die eingabe geprüft.
    /// </summary>
    /// <param name="number1">Die erste Zahl.</param>
    /// <param name="number2">Die zweite Zahl.</param>
    /// <param name="mathOperator">Der mathematische Operator.</param>
    /// <returns>true, wenn der Benutzer die Rechnung korrekt gelöst hat, andernfalls false.</returns>
    static bool ShowCalculation(int number1, int number2, char mathOperator)
    {
        int inputResult;
        int calculation = 0;

        // Eine Anweisung und die math. Rechnung wird in der Konsole ausgegeben.
        Console.WriteLine("Beendet mit -1");
        Console.Write("Was ergibt: {0} {1} {2} = ", number1, mathOperator, number2);
        
        // Wird hier etwas falschen eingegeben, löst die ein Fehler aus. 
        try
        {
            // Die eingabe wird in einen Integer umgewandelt.
            inputResult = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            return false;
            throw;
        }
        
        // Der Rechnungsaufbau unterscheidet sich je nach Operator.
        switch (mathOperator)
        {
            case '+':
                calculation = number1 + number2;
                break;
            case '-':
                calculation = number1 - number2;
                break;
            case '*':
                calculation = number1 * number2;
                break;
            case '/':
                calculation = number1 / number2;
                break;
        }

        // -1 beendet das Spielt
        if (inputResult == -1)
        {
            _isRuning = false;
        }

        // Ist die Benutzereingabe und das errechnete Resultat identisch, wird true zurückgegeben, ...
        if (inputResult == calculation)
        {
            return true;
        }
        // ... andernfalls false.
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Zeigt das Hauptmenü des Spieles an.
    /// </summary>
    /// <returns>Die vom Benutzer eingegebene Zahl.</returns>
    static byte ShowMainMenu()
    {
        byte input;

        // Zuerst wird die Konsole geleert.
        Console.Clear();

        // Anschliessend wird der Spieltitel in Farbe ausgegeben ...
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("*** MathGirl ***");
        Console.WriteLine("****************");
        Console.WriteLine();
        Console.ResetColor();

        // ... danach in einer anderen Farbe das Menü.
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[1] Spiel starten");
        Console.WriteLine("[2] Programm beenden");
        Console.WriteLine();
        Console.WriteLine("[8] Einstellungen");
        Console.ResetColor();

        // Die Benutzereingabe wird in eine positive Ganzzahl konvertiert und zurückgegeben.
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
        
        // In dieser Endlosschleife wird jeder vom Benutzer eingegebene Taste abgefangen.
        while (true)
        {
            // Die Benutzereingabe wird in einer key Variable gespeichert.
            var key = Console.ReadKey(true);
            // Wurde Enter gerückt, wird die Schleife abgebrochen.
            if (key.Key == ConsoleKey.Enter)
                break;
            // Ansonsten wird der Wert der gedrückten Taste in der Variable gespeichert.
            inputPassword += key.KeyChar;
            // Für den Benutzer sichtbar wird ein * angezeigt.
            Console.Write("*");
        }
        
        // Nun wird das Einstellungs-Menü angezeigt.
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
        // Wird help eingegeben, wird ein Passwort-zurücksetzten-Modus gestartet.
        else if (inputPassword == "help")
        {
            // Das Passwort wird auf admin zurückgesetzt.
            Console.WriteLine("");
            Console.WriteLine("Das Passwort wurde auf \"admin\" zurückgesetzt");
            sysSettings.SetPassword("admin");
            Console.ReadKey();
            return 0;
        }
        else
        {
            Console.WriteLine("Falsches Passwort!");
            return 0;
        }
    }
}