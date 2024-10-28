/*using Routing;*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
/*var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
//
Router router = new (app);
*/

class Program
{
    static void Main()
    {
        // Excerpts from file from Google Drive
        var files = new List<FileData> {
            new FileData { Id = "1UPgwbDBof6fDihyQnABN4eAIOhoL5XUs821NuI7fNmE", Name = "lærer Daphne" },
            new FileData { Id = "1YJ5-RQigS6N0K3WSygMa-ONETw0Y129bm8aL8-l5hmE", Name = "Tilmelding (Efterår 2024) (optælling)" },
            new FileData { Id = "17jRh94Hvz7lJpTDW6xtp_rVGj-i0QIAT4qXPgcGENxk", Name = "Mail til forældre - gult band / guitar" },
            new FileData { Id = "1RafPeBmPKj7Lfvac__5BkRflLbRZKc15ASS7U1J9FCQ", Name = "Mail til forældre - bas/klaver v. kasper"},
            new FileData { Id = "1sG6UeBDVCG3J10j_GuBWsnXS-0jF7rU5W9xd4ArxqY4", Name = "Mail til forældre - klaver"},
            new FileData { Id = "1nuM4-JIPJ-XL_gD-u3nzH7uETAt2ItygZgMpkOnIM_s", Name = "Besked om opstart på intra"}
        };

        // Creating dictionary to store inverted index of tokens and file IDs
        // Key: token, Value: list of file IDs (could probably be stored as an integer instead)
        Dictionary<string, List<string>> invertedIndex = new Dictionary<string, List<string>>();

        // Loop through each file
        foreach (var file in files)
        {
            // Tokenize the file name (split by spaces, punctuation, etc.)
            // The tokenization process should be more complex also using lemmatization and stemming
            var tokens = Tokenize(file.Name);

            // Add each token to the inverted index
            foreach (var token in tokens)
            {
                if (!invertedIndex.ContainsKey(token))
                {
                    invertedIndex[token] = new List<string>();
                }

                // Add the file ID to the list of files for this token
                invertedIndex[token].Add(file.Id);
            }
        }

        // Display the inverted index
        foreach (var entry in invertedIndex)
        {
            Console.WriteLine($"Token: {entry.Key} -> Files: {string.Join(", ", entry.Value)}");
        }
    }

    // Tokenization method
    static List<string> Tokenize(string fileName)
    {
        // Use regular expression to split by spaces, punctuation, etc.
        // Here we use a simple regex to split by non-word characters
        string[] tokens = Regex.Split(fileName.ToLower(), @"\W+");
        return new List<string>(tokens);
    }
}

// Example file data class
class FileData
{
    public string Id { get; set; }
    public string Name { get; set; }
}
