using Microsoft.Extensions.Configuration;

namespace MathGirl;

// Globale Variablen
// Soll später durch eine Datei mit den gespeicherten Werten ersetzt werden.
public static class Global
{
    // Speichert die grösste Zahl, die für die Rechnungen benutzt werden soll.
    public static int LargestNumber = 10;
    public static bool Continue = true;

    public static char[] MathOperators = new char[]
    {
        '+',
        '-'
    };

    public static void Initialize(IConfiguration config)
    {
        LargestNumber = config.GetValue<int>("LargestNumber");
    }
}