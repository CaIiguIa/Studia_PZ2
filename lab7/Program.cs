using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

class Program
{
    public static void Main(string[] args)
    {
        string[] arguments1_0 = { "0" };
        string[] arguments1_1 =
        {
            "1", @"C:\Users\sdjur\Desktop\zadanie1\taxi.txt",
            @"C:\Users\sdjur\Desktop\zadanie1\taxi_encoded.txt"
        };
        string[] arguments1_2 =
        {
            "2", @"C:\Users\sdjur\Desktop\zadanie1\taxi_encoded.txt",
            @"C:\Users\sdjur\Desktop\zadanie1\taxi_decoded.txt"
        };

        // zad1(arguments1_0);
        // zad1(arguments1_1);
        // zad1(arguments1_2);

        string[] arguments2 =
        {
            @"C:\Users\sdjur\Desktop\zadanie2\taxi.txt", @"C:\Users\sdjur\Desktop\zadanie2\taxi_hash.txt",
            "SHA256"
        };

        // zad2(arguments2);// stworz
        // zad2(arguments2);//sprawdz

        string[] arguments3 =
        {
            @"C:\Users\sdjur\Desktop\zadanie3\taxi.txt", @"C:\Users\sdjur\Desktop\zadanie3\taxi_podpis.txt",
        };

        // zad3(arguments3); // stworz podpis
        zad3(arguments3); // sprawdz podpis

        string[] arguments4_zaszyfruj =
        {
            @"C:\Users\sdjur\Desktop\zadanie4\taxi.txt", @"C:\Users\sdjur\Desktop\zadanie4\taxi_sym.txt",
            "taxi", "0"
        };

        string[] arguments4_odszyfruj =
        {
            @"C:\Users\sdjur\Desktop\zadanie4\taxi_sym.txt", @"C:\Users\sdjur\Desktop\zadanie4\taxi_decoded.txt",
            "taxi", "1"
        };

        // zad4(arguments4_zaszyfruj);
        // zad4(arguments4_odszyfruj);
    }

    public static void zad1(string[] args)
    {
        string filePublicKey = @"C:\Users\sdjur\Desktop\zadanie1\publiczny_zad1_polecenie0.dat";
        string filePrivateKey = @"C:\Users\sdjur\Desktop\zadanie1\prywatny_zad1_polecenie0.dat";
        string? publicKey = null;
        string? privateKey = null;

        if (args[0].Equals("0")) // stworz klucze
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Niewłaściwa liczba argumentów");
                return;
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();


            publicKey = rsa.ToXmlString(false);
            File.WriteAllText(filePublicKey, publicKey);


            privateKey = rsa.ToXmlString(true);
            File.WriteAllText(filePrivateKey, privateKey);
            Console.WriteLine("Stworzono klucze");
        }
        else if (args[0].Equals("1")) //zaszyfruj dane
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Niewłaściwa liczba argumentów");
                return;
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("Plik do zaszysfrowania nie istnieje");
                return;
            }

            publicKey = File.ReadAllText(filePublicKey);
            string dataToEncrypt = File.ReadAllText(args[1]);
            EncryptText(publicKey, dataToEncrypt,
                args[2]);
        }
        else if (args[0].Equals("2")) //odszyfruj dane
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Niewłaściwa liczba argumentów");
                return;
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("Plik do odszysfrowania nie istnieje");
                return;
            }

            privateKey = File.ReadAllText(filePrivateKey);


            File.WriteAllText(args[2], DecryptData(privateKey, args[1]));
            Console.WriteLine("Dane zostały odszyfrowane");
        }
        else
        {
            Console.WriteLine("Niewłaściwy typ polecenia");
            return;
        }
    }

    public static void zad2(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Niewłaściwa liczba argumentów");
            return;
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine("Plik do weryfikacji nie istnieje");
            return;
        }

        if (!args[2].Equals("SHA256") && !args[2].Equals("SHA512") && !args[2].Equals("MD5"))
        {
            Console.WriteLine("Nieprawidłowy tryb hashowania");
        }

        if (File.Exists(args[1])) // sprawdz czy zgodny
        {
            string plik = File.ReadAllText(args[0]);
            string hash = File.ReadAllText(args[1]);
            string hashToCheck = "";
            switch (args[2])
            {
                case "SHA256":
                    hashToCheck = skrotSHA256(plik);
                    break;
                case "SHA512":
                    hashToCheck = skrotSHA512(plik);
                    break;
                case "MD5":
                    hashToCheck = skrotMD5(plik);
                    break;
            }

            if (hash.Equals(hashToCheck))
            {
                Console.WriteLine("Plik zgodny");
            }
            else
            {
                Console.WriteLine("Plik niezgodny");
            }
        }
        else // stworz plik z haszem
        {
            string plik = File.ReadAllText(args[0]);
            string hashToWrite = "";
            switch (args[2])
            {
                case "SHA256":
                    hashToWrite = skrotSHA256(plik);
                    break;
                case "SHA512":
                    hashToWrite = skrotSHA512(plik);
                    break;
                case "MD5":
                    hashToWrite = skrotMD5(plik);
                    break;
            }

            File.WriteAllText(args[1], hashToWrite);
            Console.WriteLine("Stworzono plik z hashem");
        }
    }

    public static void zad3(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Niewłaściwa liczba argumentów");
            return;
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine("Plik do weryfikacji nie istnieje");
            return;
        }

        string filePublicKey = @"C:\Users\sdjur\Desktop\zadanie1\publiczny_zad1_polecenie0.dat";
        string filePrivateKey = @"C:\Users\sdjur\Desktop\zadanie1\prywatny_zad1_polecenie0.dat";

        string fileA = File.ReadAllText(args[0]);
        string key = File.ReadAllText(filePrivateKey);

        using SHA256 algSkroto = SHA256.Create();
        byte[] dane = Encoding.ASCII.GetBytes(fileA);

        byte[] hash = algSkroto.ComputeHash(dane);
        RSAParameters parametryAlgorytmuRSA;
        byte[] podpisanySkrot;

        using (RSA rsa = RSA.Create())
        {
            rsa.FromXmlString(key);
            // parametryAlgorytmuRSA = rsa.ExportParameters(true);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));
            podpisanySkrot = rsaFormatter.CreateSignature(hash);
        }

        if (File.Exists(args[1])) // sprawdz czy zgodny
        {
            string fileB = File.ReadAllText(args[1]);
            if (fileB.Equals(Encoding.UTF8.GetString(podpisanySkrot)))
            {
                Console.WriteLine("Poprawny podpis");
            }
            else
            {
                Console.WriteLine("Niepoprawny podpis");
            }
        }
        else // stworz plik z haszem
        {
            File.WriteAllBytes(args[1], podpisanySkrot);
            Console.WriteLine("Stworzono plik z hashem");
        }
    }

    public static void zad4(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Niewłaściwa liczba argumentów");
            return;
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine("Plik do weryfikacji nie istnieje");
            return;
        }

        byte[] napis = File.ReadAllBytes(args[0]);
        byte[] salt = "aaaaaaaa"u8.ToArray();
        byte[] initVector = "aaaaaaaaaaaaaaaa"u8.ToArray();
        int liczbaIteracji = 2000;

        if (args[3].Equals("0")) // zaszyfruj i zapisz
        {
            // byte[] utfD1 = new UTF8Encoding(false).GetBytes(napis);
            byte[] zaszyfrowane = Szyfruj(args[2], salt, initVector, liczbaIteracji, napis);
            File.WriteAllBytes(args[1], zaszyfrowane);
            Console.WriteLine("Zaszyfrowano i zapisano");
        }
        else if (args[3].Equals("1")) // odszyfruj i zapisz
        {
            // byte[] zaszyfrowane = new UTF8Encoding(false).GetBytes(napis);
            byte[] rozszyfrowane = Rozszyfruj(args[2], salt, initVector, liczbaIteracji, napis);
            File.WriteAllBytes(args[1], rozszyfrowane);
            Console.WriteLine("Odszyfrowano i zapisano");
        }
        else Console.WriteLine("Niewłaściwy typ operacji");
    }

    static void EncryptText(string kluczPubliczny, string tekst, string nazwaPliku)
    {
        UnicodeEncoding byteConverter = new UnicodeEncoding();
        byte[] daneDoZaszyfrowania = byteConverter.GetBytes(tekst);
        byte[] zaszyfrowaneDane;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(kluczPubliczny);
            zaszyfrowaneDane = rsa.Encrypt(daneDoZaszyfrowania, false);
        }

        File.WriteAllBytes(nazwaPliku, zaszyfrowaneDane);
        Console.WriteLine("Dane zostały zaszyfrowane");
    }

    static string DecryptData(string privateKey, string fileName)
    {
        byte[] daneDoOdszyfrowania = File.ReadAllBytes(fileName);
        byte[] odszyfrowaneDane;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);
            odszyfrowaneDane = rsa.Decrypt(daneDoOdszyfrowania, false);
        }

        UnicodeEncoding byteConverter = new UnicodeEncoding();
        return byteConverter.GetString(odszyfrowaneDane);
    }

    static String skrotSHA256(String napis)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = SHA256.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(napis));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    static String skrotSHA512(String napis)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = SHA512.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(napis));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    static String skrotMD5(String napis)
    {
        Encoding enc = Encoding.UTF8;
        var hashBuilder = new StringBuilder();
        using var hash = MD5.Create();
        byte[] result = hash.ComputeHash(enc.GetBytes(napis));
        foreach (var b in result)
            hashBuilder.Append(b.ToString("x2"));
        return hashBuilder.ToString();
    }

    public static byte[]? Rozszyfruj(String haslo, byte[] salt, byte[] initVector, int iteracje, byte[] dane)
    {
        Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(haslo, salt, iteracje, HashAlgorithmName.SHA256);
        Aes decAlg = Aes.Create();
        decAlg.Key = k1.GetBytes(16);
        decAlg.IV = initVector;
        MemoryStream decryptionStreamBacking = new MemoryStream();
        CryptoStream decrypt = new CryptoStream(
            decryptionStreamBacking, decAlg.CreateDecryptor(),
            CryptoStreamMode.Write);
        decrypt.Write(dane, 0, dane.Length);
        decrypt.Flush();
        decrypt.Close();
        k1.Reset();
        return decryptionStreamBacking.ToArray();
    }

    public static byte[]? Szyfruj(String haslo, byte[] salt, byte[] initVector, int iteracje,
        byte[] daneDoZaszyfrowania)
    {
        Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(haslo, salt, iteracje,
            HashAlgorithmName.SHA256);
        Aes encAlg = Aes.Create();
        encAlg.IV = initVector;
        encAlg.Key = k1.GetBytes(16);
        MemoryStream encryptionStream = new MemoryStream();
        CryptoStream encrypt = new CryptoStream(encryptionStream,
            encAlg.CreateEncryptor(), CryptoStreamMode.Write);
        encrypt.Write(daneDoZaszyfrowania, 0, daneDoZaszyfrowania.Length);
        encrypt.FlushFinalBlock();
        encrypt.Close();
        byte[] edata1 = encryptionStream.ToArray();
        k1.Reset();
        return edata1;
    }
}