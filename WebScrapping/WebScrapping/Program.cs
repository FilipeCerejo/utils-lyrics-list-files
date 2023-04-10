using System.Collections.Generic;
using Aspose.Html;
using Aspose.Html.Dom;
using WebScrapping;

string site = "https://www.letras.mus.br";
string lyricsUrl = "/novo-hinario-adventista";
List<Song> songList = new();

//Url and Name
using (var lyricsDocument = new HTMLDocument($"{site}{lyricsUrl}"))
{
    var songNode = lyricsDocument.Evaluate("//*[@class='song-name']", lyricsDocument, null, Aspose.Html.Dom.XPath.XPathResultType.Any, null);
    int i = 0;
    for (Node node; (node = songNode.IterateNext()) != null;)
    {
        songList.Add(new Song()
        {
            Name = node.TextContent,
            Url = $"{site}{node.Attributes["href"].Value}"
        });
        if (++i == 10) break;
    }
}

//Lyrics
foreach(Song song in songList)
{
    try
    {
        var songDocument = new HTMLDocument(song.Url);

        var songLyricsNode = songDocument.Evaluate("//*[@class='cnt-letra']//p", songDocument, null, Aspose.Html.Dom.XPath.XPathResultType.Any, null);

        for (Node node; (node = songLyricsNode.IterateNext()) != null;)
        {
            Node line = node.FirstChild;
            song.Lyrics.AppendLine(line.TextContent);

            while (line.NextSibling != null)
            {
                line = line.NextSibling;
                if (!string.IsNullOrWhiteSpace(line.TextContent))
                {
                    song.Lyrics.AppendLine(line.TextContent);
                }
            }

            song.Lyrics.AppendLine();
        }

        if (song.Lyrics.Length < 10)
        {
            throw new Exception("Musica pequena....");
        }

        songLyricsNode = null;
        songDocument = null;
    }
    catch(Exception ex)
    {
        Console.WriteLine($"Erro {song.Name}. " + ex.Message);
    }
}

//Number map
Dictionary<string, int> numberMap = new();
numberMap.Add("A Ceia do Senhor", 44);

//Number match
foreach (Song song in songList)
{
    if (numberMap.ContainsKey(song.Name))
    {
        song.Number = numberMap[song.Name];
    }
    else
    {
        Console.WriteLine($"Erro {song.Name}. SEM NÚMERO");
    }
}

//Create files
foreach (Song song in songList)
{
    using(StreamWriter sw = new StreamWriter($"C:\\source\\personal\\WebScrapping\\WebScrapping\\Result\\{song.Title}.txt", false))
    {
        sw.WriteLine(song.Lyrics);
    }
}