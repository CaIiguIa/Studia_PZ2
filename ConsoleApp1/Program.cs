using System.Xml;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

[XmlRoot(ElementName = "Tweet")]
public class Tweet
{
    public Tweet()
    {
    }

    public Tweet(string? text, string? userName, string? linkToTweet, string? firstLinkUrl, string? createdAt,
        string? tweetEmbedCode)
    {
        Text = text;
        UserName = userName;
        LinkToTweet = linkToTweet;
        FirstLinkUrl = firstLinkUrl;
        CreatedAt = createdAt;
        TweetEmbedCode = tweetEmbedCode;
    }

    [XmlAttribute(AttributeName = "Text")] public string? Text { get; set; }

    [XmlAttribute(AttributeName = "UserName")]
    public string? UserName { get; set; }

    [XmlAttribute(AttributeName = "LinkToTweet")]
    public string? LinkToTweet { get; set; }

    [XmlAttribute(AttributeName = "FirstLinkUrl")]
    public string? FirstLinkUrl { get; set; }

    [XmlAttribute(AttributeName = "CreatedAt")]
    public string? CreatedAt { get; set; }

    [XmlAttribute(AttributeName = "TweetEmbedCode")]
    public string? TweetEmbedCode { get; set; }

    public override string ToString()
    {
        return Text;
    }
}

[XmlRoot("ArrayOfTweets")]
class Tweets
{
    public Tweets()
    {
        data = new List<Tweet>();
    }

    [XmlElement("ArrayOfTweets")] public List<Tweet> data { get; set; }
}

class Pro
{
    public static void Main(string[] args)
    {
        // TODO read data from file

        var tweets = new List<Tweet>();

        foreach (var line in System.IO.File.ReadLines(
                      "C:\\Users\\sdjur\\Documents\\Programowanie\\Rider\\C#\\Projekt1\\PierwszyProjekt\\ConsoleApp1\\favorite-tweets.jsonl"))
        {
            tweets.Add(JsonSerializer.Deserialize<Tweet>(line));
        }

        Console.WriteLine(tweets.Count);


        // TODO to XML
        TweetXML(tweets,
            "C:\\Users\\sdjur\\Documents\\Programowanie\\Rider\\C#\\Projekt1\\PierwszyProjekt\\ConsoleApp1\\favorite-tweets.xml",
            false);

        // TODO from XML
        List<Tweet> t2 = TweetXML(null,
            "C:\\Users\\sdjur\\Documents\\Programowanie\\Rider\\C#\\Projekt1\\PierwszyProjekt\\ConsoleApp1\\favorite-tweets.xml",
            true);
        Console.WriteLine("\nDeserialized from XML: " + t2[0].Text + "\n");

        // TODO sorting by username
        var t_username = new List<Tweet>(tweets);
        sortByUsername(t_username);
        // for (int i = 0; i < t2.Count; i++)
        // {
        // Console.WriteLine(tweets[i].UserName + "     " + t2[i].UserName);   
        // }

        // TODO sorting by date
        var t_date = new List<Tweet>(tweets);
        sortByDate(t_date);
        // for (int i = 0; i < t2.Count; i++)
        // {
        // Console.WriteLine(tweets[i].CreatedAt + "     " + t2[i].CreatedAt);
        // }

        // TODO Oldest and newest tweet

        Console.WriteLine("\nOldest: " + t_date[0].Text);
        Console.WriteLine("\nNewest: " + t_date.Last().Text);


        // TODO słownik
        var dict = new Dictionary<String, List<Tweet>>();

        foreach (var tweet in tweets)
        {
            if (dict.ContainsKey(tweet.UserName))
            {
                dict[tweet.UserName].Add(tweet);
            }
            else
            {
                dict.Add(tweet.UserName, new List<Tweet>());
            }
        }


        // TODO słownik częstości

        var freq = new Dictionary<String, int>();
        foreach (var tweet in tweets)
        {
            foreach (var word in tweet.Text.Split(' '))
            {
                if (freq.ContainsKey(word))
                {
                    freq[word] += 1;
                }
                else
                {
                    freq.Add(word, 1);
                }
            }
        }

        var ordered = freq.OrderBy(x => -x.Value).ToDictionary(x => x.Key, x => x.Value);

        Console.WriteLine("\n\nMost frequent:");

        int count = 0;
        foreach (var key in ordered.Keys)
        {
            if (count == 10)
            {
                break;
            }

            if (key.Length >= 5)
            {
                count++;

                Console.WriteLine(count + ". " + key + ": " + ordered[key]);
            }
        }


        // TODO IDF
        var idf = new Dictionary<String, Double>();
        foreach (var tweet in tweets)
        {
            var occured = new List<String>();
            foreach (var word in tweet.Text.Split(' '))
            {
                if (!occured.Contains(word))
                {
                    occured.Add(word);
                    if (idf.ContainsKey(word))
                    {
                        idf[word] += 1;
                    }
                    else
                    {
                        idf.Add(word, 1);
                    }
                }
            }
        }

        var idf_sorted = idf.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        Console.WriteLine("\nIDF:");
        int cnt = 0;
        foreach (var key in idf_sorted.Keys)
        {
            if (cnt == 10) break;
            cnt++;

            idf[key] = Math.Log(tweets.Count / idf[key]);
            Console.WriteLine(cnt + ". " + key + ":  " + idf[key]);
        }
    }

    public static List<Tweet> TweetXML(List<Tweet> tweets, string path, bool read)
    {
        // System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<Tweet>));

        if (read) // if you want to read from file
        {
            // XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
            //
            // using (FileStream stream = File.OpenRead(path))
            // {
            //     tweets = (List<Tweet>)serializer.Deserialize(stream);
            // }
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<Tweet>));

            using (StreamReader reader = new StreamReader(path))
            {
                return (List<Tweet>)x.Deserialize(reader);
            }
        }
        else // if you want to write to file
        {
            // var xmlSavePath = new XElement("Tweets",
            //     from tw in tweets
            //     select new XElement("Tweet",
            //         new XElement("Text", tw.Text),
            //         new XElement("UserName", tw.UserName),
            //         new XElement("LinkToTweet", tw.LinkToTweet),
            //         new XElement("FirstLinkUrl", tw.FirstLinkUrl),
            //         new XElement("CreatedAt", tw.CreatedAt),
            //         new XElement("TweetEmbedCode", tw.TweetEmbedCode)
            //     ));
            // xmlSavePath.Save(path);

            // XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
            //
            // using (FileStream stream = File.OpenWrite(path))
            // {
            //     serializer.Serialize(stream, tweets);
            // }

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(List<Tweet>));
            using (StreamWriter writer = File.CreateText(path))
            {
                x.Serialize(writer, tweets);
            }
        }

        return null;
    }

    public static void sortByUsername(List<Tweet> tweets)
    {
        tweets.Sort((x, y) => x.UserName.CompareTo(y.UserName));
    }


    public static void sortByDate(List<Tweet> tweets)
    {
        tweets.Sort((x, y) => DateTime.Parse(x.CreatedAt.Replace("at ", ","))
            .CompareTo(DateTime.Parse(y.CreatedAt.Replace("at ", ","))));
    }
}