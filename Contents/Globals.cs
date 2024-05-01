using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Threading.Channels;

namespace MathGirl.Contents;

public class Globals
{
    public Globals()
    {
        var contents = File.ReadAllText("configuration.json");
        var json = JsonSerializer.Deserialize<JsonElement>(contents, JsonSerializerOptions.Default);

        var largestNumberElement = json.GetProperty("LargestNumber");
    }

//Globals.LargestNumber = largestNumberElement.GetInt32();
// Globale Variablen
// Soll später durch eine Datei mit den gespeicherten Werten ersetzt werden.
    public static int LargestNumber = 

    public static string Password = "Lenia2016";

// Speichert die grösste Zahl, die für die Rechnungen benutzt werden soll.
    public static char[] MathOperators = new char[]
    {
        '+',
        '-'
    };
}