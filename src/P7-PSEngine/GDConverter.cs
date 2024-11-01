using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GDConverter
{
    static void Main()
    {
        // Path to the original JSON file
        string filePath = "testgoogle.json";

        // Read JSON from the file
        string jsonContent = File.ReadAllText(filePath);

        // Parse JSON into a JObject
        JObject jsonObj = JObject.Parse(jsonContent);

        // Iterate over the "files" array and replace each "id"
        foreach (var file in jsonObj["files"])
        {
            file["id"] = Guid.NewGuid().ToString();  // Replace ID with a new unique ID
        }

        // Convert JObject back to JSON string with formatting
        string updatedJsonContent = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

        // Output the updated JSON to console (or save it back to a file)
        Console.WriteLine(updatedJsonContent);

        // Optionally, save the updated JSON back to a file
        File.WriteAllText("testgooglefinal.json", updatedJsonContent);
    }
}
