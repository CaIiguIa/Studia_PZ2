using System.Net;
using System.Net.Sockets;
using System.Text;

class Lab6Klient
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
        //połącz się
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Socket socket = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        socket.Connect(localEndPoint);

        //wyślij wiadomość    
        string wiadomosc =
            "Why_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxi";
        byte[] wiadomoscBajty = Encoding.UTF8.GetBytes(wiadomosc);
        var i = socket.Send(wiadomoscBajty, 0);
        Console.WriteLine("Wysłano " + i + " bajtów");

        //odbierz odpowiedź
        var bufor = new byte[1_024];
        int liczbaBajtów = socket.Receive(bufor, 0);
        Console.WriteLine("Odebrano " + liczbaBajtów + " bajtów");
        String odpowiedzSerwera = Encoding.UTF8.GetString(bufor, 0, liczbaBajtów);
        Console.WriteLine(odpowiedzSerwera);

        //zakończ
        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (SocketException)
        {
            socket.Close();
        }
        catch (ObjectDisposedException)
        {
            socket.Close();
        }
    }

    public static void zad2()
    {
        Console.WriteLine("Zadanie 2");
        //połącz się
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Socket socket = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        socket.Connect(localEndPoint);

        //wyślij długość wiadomości
        string wiadomosc =
            "Why_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxiWhy_are_YOU_driving_the_taxi";
        byte[] wiadomoscBajty = Encoding.UTF8.GetBytes(wiadomosc);
        byte[] lenBytes = Encoding.UTF8.GetBytes("" + wiadomoscBajty.Length);
        socket.Send(lenBytes, 0);


        //wyślij wiadomość    
        var i = socket.Send(wiadomoscBajty, 0);
        Console.WriteLine("Wysłano " + i + " bajtów\n");

        //odbierz długość wiadomości
        byte[] buforDługosci = new byte[4];
        int dlugosc = socket.Receive(buforDługosci, 0);
        String messageLength = Encoding.UTF8.GetString(buforDługosci, 0, dlugosc);
        int d = Int32.Parse(messageLength);

        //odbierz odpowiedź
        var bufor = new byte[d];
        int liczbaBajtów = socket.Receive(bufor, 0);
        Console.WriteLine("\nOdebrano " + liczbaBajtów + " bajtów");
        String odpowiedzSerwera = Encoding.UTF8.GetString(bufor, 0, liczbaBajtów);
        Console.WriteLine(odpowiedzSerwera);

        //zakończ
        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (SocketException)
        {
            socket.Close();
        }
        catch (ObjectDisposedException)
        {
            socket.Close();
        }
    }

    public static void zad3()
    {
        Console.WriteLine("Zadanie 3");
        //połącz się
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        Socket socket = new(
            localEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        socket.Connect(localEndPoint);

        String request = "";

        while (true)
        {
            Console.WriteLine("\nWpisz komendę:");
            request = Console.ReadLine();
            SendRequest(request, socket);
            if (request.Equals("!end")) break;
            String response = ReceiveResponse(socket);
            Console.WriteLine(response);
        }

        //zakończ
        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (SocketException)
        {
            socket.Close();
        }
        catch (ObjectDisposedException)
        {
            socket.Close();
        }
    }

    public static void SendRequest(String message, Socket server)
    {
        //wyślij długość wiadomości
        byte[] wiadomoscBajty = Encoding.UTF8.GetBytes(message);
        byte[] lenBytes = Encoding.UTF8.GetBytes("" + wiadomoscBajty.Length);
        server.Send(lenBytes, 0);

        //wyślij wiadomość    
        server.Send(wiadomoscBajty, 0);
    }

    public static String ReceiveResponse(Socket server)
    {
        //odbierz długość wiadomości
        byte[] buforDługosci = new byte[4];
        int dlugosc = server.Receive(buforDługosci, 0);
        String messageLength = Encoding.UTF8.GetString(buforDługosci, 0, dlugosc);
        int d = Int32.Parse(messageLength);

        //odbierz odpowiedź
        var bufor = new byte[d];
        int liczbaBajtów = server.Receive(bufor, 0);
        return Encoding.UTF8.GetString(bufor, 0, liczbaBajtów);
    }
}