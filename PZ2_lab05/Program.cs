
class PZ2_lab05
{
    static EventWaitHandle manualResetEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
    private static Stack<string> signal = new Stack<string>();
    public static int bufor = 0;
    public static List<String> szukanie = new List<string>();

    public static void Main(string[] args)
    {
        // zad1();
        // zad2();
        // zad3("05");
        zad4(10);
    }

    public static void zad1()
    {
        Console.WriteLine("aaaa");
        //TODO: zadanie 1
        Random random = new Random(Environment.TickCount);
        int n = 3;
        int m = 3;
        List<Producent> producenci = new List<Producent>();

        for (int i = 0; i < n; i++)
        {
            var producent = new Producent(i.ToString(), random.Next(10000));
            producent.wątek = new Thread(new ThreadStart(producent.ThreadMaker));

            producenci.Add(producent);
        }

        List<Konsument> konsumenci = new List<Konsument>();

        for (int i = 0; i < m; i++)
        {
            var konsument = new Konsument(i.ToString(), random.Next(10000));
            konsument.wątek = new Thread(new ThreadStart(konsument.ThreadMaker));

            konsumenci.Add(konsument);
        }

        foreach (var prod in producenci)
        {
            prod.wątek.Start();
            prod.wątek.IsBackground = false;
        }

        foreach (var kon in konsumenci)
        {
            kon.wątek.Start();
            kon.wątek.IsBackground = false;
        }

        string? s = null;
        do
        {
            s = Console.ReadLine();
        } while (!s.Equals("q"));


        foreach (var kon in konsumenci)
        {
            kon.Stop = true;
        }

        foreach (var prod in producenci)
        {
            prod.Stop = true;
        }

        while (true)
        {
            bool cont = false;
            Thread.Sleep(1000);
            foreach (var prod in producenci)
            {
                if (prod.wątek.IsAlive)
                {
                    cont = true;
                    break;
                }
            }
            foreach (var kon in konsumenci)
            {
                if (kon.wątek.IsAlive)
                {
                    cont = true;
                    break;
                }
            }

            if (!cont) break;
        }

        Console.WriteLine("Wszystkie wątki zatrzymane");
    }

    public static void zad2()
    {
        Katalogowanie k = new Katalogowanie("kat", 0);
        k.wątek = new Thread(new ThreadStart(k.ThreadMaker));
        k.wątek.Start();
        k.wątek.IsBackground = false;
        string? s = null;
        do
        {
            s = Console.ReadLine();
        } while (!s.Equals("q"));

        k.Stop = true;
        while(k.wątek.IsAlive) Thread.Sleep(1000);
        Console.WriteLine("Koniec procesu");
    }

    public static void zad3(string pat)
    {
        Szukacz k = new Szukacz("kat", 0, pat);
        k.wątek = new Thread(new ThreadStart(k.ThreadMaker));
        k.wątek.Start();
        k.wątek.IsBackground = false;
        while (k.wątek.IsAlive)
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("Znalezione pliki ze wzorcem: \"" +pat+"\"");
        foreach (var plik in szukanie)
        {
            Console.WriteLine("-> " +plik);
        }
    }
    
    public static void zad4(int n)
    {
        List<Buforowanie> buforrrrr = new List<Buforowanie>();
        for (int i = 0; i < n; i++)
        {
            Buforowanie k = new Buforowanie(i.ToString(), 0);
            k.wątek = new Thread(new ThreadStart(k.ThreadMaker));    
            buforrrrr.Add(k);
        }

        foreach (var th in buforrrrr)
        {
            th.wątek.Start();
            th.wątek.IsBackground = false;
        }

        while (bufor!=n)
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("Wszystkie wątki rozpoczęte");
        
        foreach (var th in buforrrrr)
        {
            th.Stop = true;
        }

        while (bufor!=0)
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("Wszystkie wątki zatrzymane");
    }
    class Producent
    {
        public string Nazwa;
        public int Opoznienie;
        ThreadStart? ThreadStart = null;
        public bool Stop = false;
        public Thread wątek;
        private Dictionary<string, int> data = new Dictionary<string, int>();

        public Producent(string Nazwa, int Opoznienie)
        {
            this.Nazwa = Nazwa;
            this.Opoznienie = Opoznienie;
        }

        public void ThreadMaker()
        {
            Console.WriteLine("Start producenta " + Nazwa);
            while (!Stop)
            {
                Thread.Sleep(Opoznienie);
                manualResetEvent.Set();
                signal.Push(Nazwa);
                // Console.WriteLine("Wysłanie: producent " + Nazwa);
            }

            // Console.WriteLine("-p: " + Nazwa);
        }
    }

    class Konsument
    {
        public string Nazwa;
        public int Opoznienie;
        public bool Stop = false;
        ThreadStart? ThreadStart = null;
        public Thread wątek;
        private Dictionary<string, int> data = new Dictionary<string, int>();


        public Konsument(string Nazwa, int Opoznienie)
        {
            this.Nazwa = Nazwa;
            this.Opoznienie = Opoznienie;
        }

        public void ThreadMaker()
        {
            Console.WriteLine("Start konsumenta " + Nazwa);
            string s;
            while (!Stop)
            {
                Thread.Sleep(Opoznienie);
                s = "";
                manualResetEvent.WaitOne();
                if (signal.TryPop(out s)) ;
                {
                    if (s != null)
                    {
                        if (data.Keys.Contains(s)) data[s] += 1;
                        else data[s] = 1;
                    }
                }

                // Console.WriteLine("Otrzymanie: konsument " + Nazwa);
            }

            printData();

            // Console.WriteLine("-k: Zatrzymanie konsumenta " + Nazwa);
        }

        public void printData()
        {
            Console.Write("-k" + Nazwa + ": [");
            foreach (var key in data.Keys)
            {
                Console.Write(key + ": " + data[key] + ", ");
            }

            Console.Write("]\n");
        }
    }

    class Katalogowanie
    {
        public string Nazwa;
        public int Opoznienie;
        public bool Stop = false;
        ThreadStart? ThreadStart = null;
        public Thread wątek;
        private List<String> pliki = new List<string>();
        List<string> filesToRemove = new List<string>();


        public Katalogowanie(string Nazwa, int Opoznienie)
        {
            this.Nazwa = Nazwa;
            this.Opoznienie = Opoznienie;
        }

        public void ThreadMaker()
        {
            while (!Stop)
            {
                List<string> pl =
                    Directory.GetFiles("C:\\Users\\sdjur\\Desktop", "*", SearchOption.AllDirectories).ToList();
                foreach (var plik in pl)
                {
                    if (!pliki.Contains(plik))
                    {
                        Console.WriteLine("Nowy plik: " + plik);
                        pliki.Add(plik);
                    }
                }

                foreach (var plik in pliki)
                {
                    if (!pl.Contains(plik))
                    {
                        filesToRemove.Add(plik);
                    }
                }

                foreach (var plik in filesToRemove)
                {
                    Console.WriteLine("Usunięto plik: " + plik);
                    pliki.Remove(plik);
                }
                filesToRemove.Clear();
                Thread.Sleep(1000);
            }
        }
    }

    class Buforowanie
    {
        public string Nazwa;
        public int Opoznienie;
        public bool Stop = false;
        ThreadStart? ThreadStart = null;
        public Thread wątek;


        public Buforowanie(string Nazwa, int Opoznienie)
        {
            this.Nazwa = Nazwa;
            this.Opoznienie = Opoznienie;
        }

        public void ThreadMaker()
        {
            Console.WriteLine("+"+Nazwa);
            Interlocked.Increment(ref bufor);
            // bufor++;
            while (!Stop)
            {
                Thread.Sleep(1000);
            }
            Console.WriteLine("-"+Nazwa);
            Interlocked.Decrement(ref bufor);
            // bufor--;
        }
    }

    class Szukacz
    {
        public string Nazwa;
        public int Opoznienie;
        public bool Stop = false;
        ThreadStart? ThreadStart = null;
        public Thread wątek;
        private String pattern;

        public Szukacz(string Nazwa, int Opoznienie, string pat)
        {
            this.Nazwa = Nazwa;
            this.Opoznienie = Opoznienie;
            this.pattern = pat;
        }

        public void ThreadMaker()
        {
            List<string> pl =
                Directory.GetFiles("C:\\Users\\sdjur\\Desktop", "*", SearchOption.AllDirectories).ToList();
            foreach (var plik in pl)
            {
                int index = plik.LastIndexOf("\\");
                if (plik.Substring(index).IndexOf(pattern)!=-1) szukanie.Add(plik);
            }
        }
    }
}