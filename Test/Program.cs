// See https://aka.ms/new-console-template for more information

class Test
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        List<string> pl = Directory.GetFiles("C:\\Users\\sdjur\\Desktop", "*", SearchOption.TopDirectoryOnly).ToList();
        List<string> dir = Directory.GetDirectories("C:\\Users\\sdjur\\Desktop", "*", SearchOption.TopDirectoryOnly).ToList();
        foreach (var t in pl)
        {
            Console.WriteLine(t);
        }
        foreach (var t in dir)
        {
            Console.WriteLine(t);
        }
    }
}