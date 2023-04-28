using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.RegularExpressions;

class Lab6Server
{
    public static void Main(string[] args)
    {
        // zad1();
        // zad2();
        zad3();
    }

    public static void zad1()
    {
        Console.WriteLine("Zadanie 1");
        //włącz serwer
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Socket socketSerwera = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        socketSerwera.Bind(localEndPoint);
        socketSerwera.Listen(100);

        //dodaj klienta
        Socket socketKlienta = socketSerwera.Accept();

        //odbierz wiadomość
        byte[] bufor = new byte[1_024];
        int received = socketKlienta.Receive(bufor, 0);
        Console.WriteLine("Odebrano " + received + " bajtów");
        String wiadomoscKlienta = Encoding.UTF8.GetString(bufor, 0, received);
        Console.WriteLine(wiadomoscKlienta);

        //odpisz klientowi
        string odpowiedz =
            "odczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałem";
        var echoBytes = Encoding.UTF8.GetBytes(odpowiedz);
        var i = socketKlienta.Send(echoBytes, 0);
        Console.WriteLine("Wysłano " + i + " bajtów");
        //zakończ pracę
        try
        {
            socketSerwera.Shutdown(SocketShutdown.Both);
            socketSerwera.Close();
        }
        catch (SocketException)
        {
            socketSerwera.Close();
        }
        catch (ObjectDisposedException)
        {
            socketSerwera.Close();
        }
    }

    public static void zad2()
    {
        Console.WriteLine("Zadanie 2");
        //włącz serwer
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Socket socketSerwera = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        socketSerwera.Bind(localEndPoint);
        socketSerwera.Listen(100);

        //dodaj klienta
        Socket socketKlienta = socketSerwera.Accept();

        //odbierz długość wiadomości
        byte[] buforDługosci = new byte[4];
        int dlugosc = socketKlienta.Receive(buforDługosci, 0);
        String messageLength = Encoding.UTF8.GetString(buforDługosci, 0, dlugosc);
        int d = Int32.Parse(messageLength);

        //odbierz wiadomość
        byte[] bufor = new byte[d];
        int received = socketKlienta.Receive(bufor, 0);
        Console.WriteLine("Odebrano " + received + " bajtów\n");
        String wiadomoscKlienta = Encoding.UTF8.GetString(bufor, 0, received);
        Console.WriteLine(wiadomoscKlienta);

        //wyślij klientowi długość wiadomości
        string odpowiedz =
            "odczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałemodczytałem";
        var echoBytes = Encoding.UTF8.GetBytes(odpowiedz);
        var sendLen = echoBytes.Length;
        socketKlienta.Send(Encoding.UTF8.GetBytes("" + sendLen));

        //odpisz klientowi
        var i = socketKlienta.Send(echoBytes, 0);
        Console.WriteLine("\nWysłano " + i + " bajtów");

        //zakończ pracę
        try
        {
            socketSerwera.Shutdown(SocketShutdown.Both);
            socketSerwera.Close();
        }
        catch (SocketException)
        {
            socketSerwera.Close();
        }
        catch (ObjectDisposedException)
        {
            socketSerwera.Close();
        }
    }

    public static void zad3()
    {
        Console.WriteLine("Zadanie 3");
        //włącz serwer
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Socket socketSerwera = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        socketSerwera.Bind(localEndPoint);
        socketSerwera.Listen(100);

        //dodaj klienta
        Socket socketKlienta = socketSerwera.Accept();

        String request = "";
        String my_dir = @"C:\Users\sdjur\Desktop";
        Regex inRegex = new Regex(@"in .*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        while (true)
        {
            Console.WriteLine("\nAwaiting request....");
            request = ReceiveRequest(socketKlienta);

            
            if (request.Equals("list"))
            {
                Console.WriteLine("Sending list of files and directories in: " + my_dir);
                SendFilesAndDirsInFolder(my_dir, socketKlienta);
            }
            else if (inRegex.Matches(request).Count > 0)
            {
                String name = request.Substring(3);
                Console.WriteLine("Searching in: " + name);
                List<string> files = Directory.GetFiles(my_dir, "*", SearchOption.TopDirectoryOnly).ToList();
                List<string> dir = Directory.GetDirectories(my_dir, "*", SearchOption.TopDirectoryOnly).ToList();

                for (int i = 0; i < files.Count; i++)
                {
                    files[i] = files[i].Replace(my_dir, "");
                }

                for (int i = 0; i < dir.Count; i++)
                {
                    dir[i] = dir[i].Replace(my_dir, "");
                }

                if (name.Equals(".."))
                {
                    if (!my_dir.Equals("C:"))
                    {
                        my_dir = my_dir.Substring(0, my_dir.LastIndexOf(@"\"));
                        SendFilesAndDirsInFolder(my_dir, socketKlienta);
                    }
                    else
                    {
                        SendMessage("Nie można przejść do katalogu wyżej", socketKlienta);
                    }                    
                }
                else if (dir.Contains(name))
                {
                    my_dir += name;
                    SendFilesAndDirsInFolder(my_dir, socketKlienta);
                }
                else
                {
                    SendMessage("Podana ścieżka nie istnieje", socketKlienta);
                }
            }
            else if (request.Equals("!end"))
            {
                Console.WriteLine("Kończę pracę");
                break;
            }
            else
            {
                //napisz że zla komenda
                SendMessage("Nieprawidłowa komenda", socketKlienta);
            }
        }

        //zakończ pracę
        try
        {
            socketSerwera.Shutdown(SocketShutdown.Both);
            socketSerwera.Close();
        }
        catch (SocketException)
        {
            socketSerwera.Close();
        }
        catch (ObjectDisposedException)
        {
            socketSerwera.Close();
        }
    }

    public static void SendMessage(String message, Socket clientSocket)
    {
        //wyślij długość wiaodmości
        var echoBytes = Encoding.UTF8.GetBytes(message);
        var sendLen = echoBytes.Length;
        clientSocket.Send(Encoding.UTF8.GetBytes("" + sendLen));

        //wyślij właściwą wiadomość
        clientSocket.Send(echoBytes, 0);
    }

    public static String ReceiveRequest(Socket clientSocket)
    {
        //odbierz długość wiadomości
        byte[] buforDługosci = new byte[4];
        int dlugosc = clientSocket.Receive(buforDługosci, 0);
        String messageLength = Encoding.UTF8.GetString(buforDługosci, 0, dlugosc);
        int d = Int32.Parse(messageLength);

        //odbierz wiadomość
        byte[] bufor = new byte[d];
        int received = clientSocket.Receive(bufor, 0);
        return Encoding.UTF8.GetString(bufor, 0, received);
    }

    public static void SendFilesAndDirsInFolder(String my_dir, Socket clientSocket)
    {
        Console.WriteLine("Ścieżka: " + my_dir);
        List<string> files = Directory.GetFiles(my_dir, "*", SearchOption.TopDirectoryOnly).ToList();
        List<string> dir = Directory.GetDirectories(my_dir, "*", SearchOption.TopDirectoryOnly).ToList();

        String response = ("\n"+my_dir+"\nFiles: \n");
        for (int i = 0; i < files.Count; i++)
        {
            files[i] = files[i].Replace(my_dir, "");
            response += (files[i] + "\n");
        }

        response += ("\nDirectories:\n");
        for (int i = 0; i < dir.Count; i++)
        {
            dir[i] = dir[i].Replace(my_dir, "");
            response += (dir[i] + "\n");
        }

        SendMessage(response, clientSocket);
    }
}