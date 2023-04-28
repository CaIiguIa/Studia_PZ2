using System.Runtime.InteropServices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("");
        List<List<string>>? dane;
        List<string> types;
        List<bool> isNullable;
        SqliteConnection con = new SqliteConnection("Data Source=database.db;");
        try
        {
            con.Open();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        dane = zad1("plik.csv", ",");
        types = zad2(dane);

        
        zad3(dane[0], types, "cos", con);
        Console.WriteLine("");
        zad4(dane, types, "cos", con);
        
        zad5("cos", con);
    }

    public static List<List<string>> zad1(string path, string separator)
    {
        int last = -1;
        List<List<string>> data = new List<List<string>>();
        foreach (string line in File.ReadLines(path))
        {
            data.Add(new List<string>());
            last += 1;
            foreach (var cell in line.Split(separator))
            {
                data[last].Add(cell);
            }
        }

        string header = "Nazwy kolumn:\n";
        foreach (var v in data[0])
        {
            header += v;
            header += "\n";
        }

        Console.WriteLine(header);

        return data;
    }

    public static List<string> zad2(List<List<string>> data)
    {
        //count null

        //check values
        short result = 0;
        List<string> types = new List<string>();

        for (int col = 0; col < data[0].Count; col++)
        {
            //cast int

            bool castable = true;

            for (int row = 1; row < data.Count; row++)
            {
                if (!data[row][col].Equals("") && !Int16.TryParse(data[row][col], out result))
                {
                    castable = false;
                    break;
                }
            }

            if (castable) types.Add("INTEGER");
            else
            {
                //else cast double
                castable = true;

                double res = 0;
                for (int row = 1; row < data.Count; row++)
                {
                    if (!data[row][col].Equals("") && !double.TryParse(data[row][col].Replace(".", ","), out res))
                    {
                        castable = false;
                        break;
                    }
                }

                if (castable) types.Add("REAL");
                else types.Add("TEXT");
            }
        }

        for (int col = 0; col < data[0].Count; col++)
        {
            bool nullbl = false;
            for (int row = 1; row < data.Count; row++)
            {
                if (data[row][col].Equals(""))
                {
                    nullbl = true;
                    break;
                }
            }

            if (!nullbl)
            {
                types[col] += " NOT NULL";
            }
        }
        
        string header = "Typy:\n";
        foreach (var v in types)
        {
            header += v;
            header += "\n";
        }

        Console.WriteLine(header);

        return types;
    }

    public static void zad3(List<string> colnames, List<string> types, string name, SqliteConnection connection)
    {
        SqliteCommand delTableCmd = connection.CreateCommand();
        delTableCmd.CommandText = "DROP TABLE IF EXISTS " + "\""+name+ "\"";
        delTableCmd.ExecuteNonQuery();

        SqliteCommand createTableCmd = connection.CreateCommand();
        createTableCmd.CommandText =
            "CREATE TABLE \"" + name + "\" (";
        for (int i = 0; i < colnames.Count; i++)
        {
            createTableCmd.CommandText += "\"" + colnames[i] + "\" " + types[i];
            if (i < colnames.Count - 1) createTableCmd.CommandText += ", ";
        }

        createTableCmd.CommandText += ")";
        Console.WriteLine(createTableCmd.CommandText);

        createTableCmd.ExecuteNonQuery();
    }

    public static void zad4(List<List<string>> data, List<string> types, string name, SqliteConnection connection)
    {
        try
        {
            using (var transaction = connection.BeginTransaction())
            {
                SqliteCommand insertCmd = connection.CreateCommand();
                insertCmd.CommandText =
                    "INSERT INTO " + name + "(";

                for (int i = 0; i < data[0].Count; i++)
                {
                    insertCmd.CommandText += data[0][i];
                    if (i < data[0].Count - 1) insertCmd.CommandText += ", ";
                }

                insertCmd.CommandText += ") VALUES ";

                for (int row = 1; row < data.Count; row++)
                {
                    insertCmd.CommandText += "(";
                    for (int col = 0; col < data[0].Count; col++)
                    {
                        if (data[row][col].Equals("")) insertCmd.CommandText += "NULL";
                        else if (types[col].Contains("REAL")) insertCmd.CommandText += data[row][col].Replace(",", ".");
                        else if (types[col].Contains("TEXT"))
                        {
                            insertCmd.CommandText += ("\"" + data[row][col] + "\"");
                        }
                        else {
                            insertCmd.CommandText += data[row][col];
                        }

                        if (col < data[0].Count - 1) insertCmd.CommandText += ", ";
                    }

                    insertCmd.CommandText += ")";
                    if (row < data.Count - 1) insertCmd.CommandText += ", ";
                }

                Console.WriteLine(insertCmd.CommandText);
                insertCmd.ExecuteNonQuery();

                transaction.Commit();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void zad5(string name, SqliteConnection connection)
    {
        List<List<string>> readData = new List<List<string>>();
        SqliteCommand selectCmd = connection.CreateCommand();
        selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM " + name;
        
        SqliteDataReader r = selectCmd.ExecuteReader();
        
        readData.Add(new List<string>());
        for (var i = 0; i < r.FieldCount; i++)
        {
            readData[0].Add(r.GetName(i));
        }

        int readIndex = 0;
        while (r.Read()){
            readData.Add(new List<string>());
            readIndex += 1;
            for (var i = 0; i < r.FieldCount; i++)
            {            
                readData[readIndex].Add(Convert.ToString(r[r.GetName(i)]));
            }
        }

        Console.WriteLine("\n\nData read from database:\n");
        string data = "";
        for (int row = 0; row < readData.Count; row++)
        {
            for (int col = 0; col < readData[row].Count; col++)
            {
                if (row==0) data += readData[row][col];
                else if (readData[row][col].Equals("")) data += "NULL";
                else
                {
                    if (r.GetFieldType(col).ToString().Equals("System.Double"))
                    {
                        data += readData[row][col].Replace(",", ".");
                    }
                    else if (r.GetFieldType(col).ToString().Equals("System.String"))
                    {
                        data += "\""+readData[row][col]+"\"";
                    }
                    else data += readData[row][col];
                }
                if (col < readData[row].Count - 1) data += ",";
            }

            data += "\n";
        }

        Console.WriteLine(data);
    }
}