namespace PtgExpressHub.ConsoleRunner;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Repeat("Hello", 5));
        Console.ReadKey();
    }

    static string Repeat(string pattern, int timesToRepeat)
    {
        string result = string.Empty;

        for (int i = 0; i < timesToRepeat; i++)        
            result += pattern;        

        return result;
    }
}
