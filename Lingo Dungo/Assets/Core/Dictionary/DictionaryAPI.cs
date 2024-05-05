using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;


public class DictionaryAPI
{


	private static readonly HttpClient client = new HttpClient();
	
	private static async Task Main()
    {
		Console.Write("Enter word: ");
		string query = Console.ReadLine();

		string url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + query;

        try
        {
			var response = await client.GetStringAsync(url);
			List<Word>? word = JsonSerializer.Deserialize<List<Word>>(response);
			Console.WriteLine("Definition: {0}", word[0].meanings[0].definitions[0].definition);
        }
        catch (HttpRequestException e)
        {
			Console.WriteLine("Error: {0}", e);
        }
	}
}



public class Word
{
	public List<Meaning>? meanings { get; set; }
}
public class Meaning
{
	public List<Definition>? definitions { get; set; }
}
public class Definition
{
	public string? definition { get; set; }
}