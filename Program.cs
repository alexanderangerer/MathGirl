namespace MathGirl;

class Program
{
    // Globale Variablen
    // Soll später durch eine Datei mit den gespeicherten Werten ersetzt werden.
    public static class Global
    {
        // Speichert die grösste Zahl, die für die Rechnungen benutzt werden soll.
        public static int LargestNumber = 20;
    }

    static void Main(string[] args)
    {
        bool isRuning = true;


        while (isRuning)
        {
            byte inputUser = ShowMainMenu();

            switch (inputUser)
            {
                case 1:
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
        Random rndNumber = new Random();
        int number1 = rndNumber.Next(0, Global.LargestNumber);

        Console.WriteLine(number1);
        Random oZufall = new Random();
        byte[] aPuffer = new byte[5];
        
        Console.WriteLine("Zufallszahlen (Ganzzahlen 0 bis 1000):");
        for (int i = 0; i < 5; i++)
            Console.WriteLine(oZufall.Next(0, 1001));       // Zahlen von 0 bis 1000 (!)
        
        Console.WriteLine();
        
        Console.WriteLine("Array mit byte-Zufallszahlen:");
        oZufall.NextBytes(aPuffer);
        foreach (byte bPufferEintrag in aPuffer)
            Console.WriteLine(bPufferEintrag);
        
        Console.WriteLine();
        
        Console.WriteLine("Zufallszahlen (Kommazahl 0.0 bis 1.0):");
        for (int i = 0; i < 5; i++)
            Console.WriteLine(oZufall.NextDouble());
        
        Console.ReadKey();
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
    }

    static byte ShowMainMenu()
    {
        byte input;

        Console.WriteLine("1 - Spiel starten");
        Console.WriteLine("2 - Programm beendet");
        Console.WriteLine();
        Console.WriteLine("8 - Einstellungen");

        input = Convert.ToByte(Console.ReadLine());
        return input;
    }

    static byte ShowSettings()
    {
        byte input;
        Console.Write("Passwort: ");
        if (Console.ReadLine() == "aaPhoto80")
        {
            Console.WriteLine("1 - Grösste Zahl festlegen");

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