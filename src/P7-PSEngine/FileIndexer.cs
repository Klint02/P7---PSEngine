using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CloudFileIndexer

{
    public class InvertedIndex
    {
        private Dictionary<string, Dictionary<string, TermInfo>> invertedIndex = new Dictionary<string, Dictionary<string, TermInfo>>();
        private Dictionary<string, int> documentFrequency = new Dictionary<string, int>();

        public void AddTerm(string term, string docId, int position)
        {
            term = term.ToLower();

            if (!invertedIndex.ContainsKey(term))
            {
                invertedIndex[term] = new Dictionary<string, TermInfo>();
                documentFrequency[term] = 0;
            }

            if (!invertedIndex.ContainsKey(docId)) 
            {
                invertedIndex[term][docId] = new TermInfo();
                documentFrequency[term]++;
            }

            invertedIndex[term][docId].TermFrequency++;
            invertedIndex[term][docId].Positions.Add(position);
        }


         public void IndexDocument(string docId, string content)
        {
            // Tokenize content and track position
            var tokens = Regex.Split(content.ToLower(), @"\W+"); // Splitting by non-word characters
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (!string.IsNullOrEmpty(token))
                {
                    AddTerm(token, docId, i); // Use the position in the document as `i`
                }
            }
        }

        public Dictionary<string, TermInfo> GetTermInfo(string term) 
        {
            term = term.ToLower();
            return invertedIndex.ContainsKey(term) ? invertedIndex[term] : null;
        }

        public void DisplayIndex()
        {
            foreach (var termEntry in invertedIndex)
            {
//                Console.WriteLine($"Term: {termEntry.Key}");
                Console.WriteLine($"Term: {termEntry.Key}, Total Frequency: {GetDocumentFrequency(termEntry.Key)}");
                foreach (var docEntry in termEntry.Value)
                {
                    var info = docEntry.Value;
                    Console.WriteLine($"\tDocId: {docEntry.Key}, TF: {info.TermFrequency}, Positions: {string.Join(", ", info.Positions)}");
                }
            }
        }

        public string GetIndexData()
        {
            // Sort positions in each TermInfo for consistent ordering
            foreach (var termEntry in invertedIndex)
            {
                foreach (var docEntry in termEntry.Value)
                {
                    docEntry.Value.Positions.Sort();
                }
            }

            // Serialize the entire invertedIndex to JSON
            return JsonConvert.SerializeObject(invertedIndex, Formatting.Indented);
        }

        public int GetDocumentFrequency(string term)
        {
            term = term.ToLower();
            return documentFrequency.ContainsKey(term) ? documentFrequency[term] : 0;
        }
    }

    public class TermInfo
    {
        public int TermFrequency {get; set;} = 0;
        public List<int> Positions {get; set;} = new List<int>();
    }
}