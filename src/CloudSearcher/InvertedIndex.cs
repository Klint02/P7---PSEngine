using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using DocumentMetadata = CloudSearcher.DocumentMetadata;

namespace CloudSearcher

{
    // Class to store data related to a term in the inverted index
    public class TermData
    {
        public string Term { get; set; } = ""; // The term
        public int DocumentFrequency { get; set; } = 0; // Number of documents containing the term
        public int TotalTermFrequency { get; set; } = 0; // Total number of times the term appears across all documents
        public Dictionary<string, DocumentData> Documents { get; set; } = new Dictionary<string, DocumentData>(); // List of documents containing the term 
    }
    // Class
    public class DocumentData
    {
        public string DocID { get; set; } = ""; // Document ID
        public int TermFrequency { get; set; } = 0; // Number of times the term appears in the document
        public List<int> Positions { get; set; } = new List<int>(); // List of positions where the term appears in the document
    }

    public class InvertedIndex
    {
        // Dictionary to store the inverted index
        // Key: term, Value: TermData object
        public Dictionary<string, TermData> invertedIndex { get; set; } = new Dictionary<string, TermData>();

        /*

        // Dictionary to store the inverted index
        // Key: term, Value: dictionary of document IDs and term info
        public Dictionary<string, Dictionary<string, TermInfo>> invertedIndex = new Dictionary<string, Dictionary<string, TermInfo>>();
        // Dictionary to store the document frequency of each term
        public Dictionary<string, int> documentFrequency = new Dictionary<string, int>();
        // Dictionary to store metadata of each document
        public Dictionary<string, DocumentMetadata> documentMetadata = new Dictionary<string, DocumentMetadata>();
*/

        // Method to add a term to the inverted index
                // Method to add a term to the inverted index

        public void AddOrUpdateTerm(string term, string docId, int position)
        {
            term = term.ToLower();

            if (!invertedIndex.ContainsKey(term))
            {
                invertedIndex[term] = new TermData  // Create a new TermData object
                {
                    Term = term,
                    DocumentFrequency = 0,
                    TotalTermFrequency = 0
                };
            }

            var termData = invertedIndex[term];

            if (!termData.Documents.ContainsKey(docId))
            {
                termData.Documents[docId] = new DocumentData { DocID = docId };
                termData.DocumentFrequency++;
            }

            var docData = termData.Documents[docId];
            docData.TermFrequency++;
            docData.Positions.Add(position);

            termData.TotalTermFrequency++;
        }
/*
        // Method to add Metadata
        public void AddOrUpdateMetadata(string docId, string fileName)
        {
            if (documentMetadata.ContainsKey(docId))
            {
                documentMetadata[docId].Filename = fileName;
            }
            else
            {
                documentMetadata[docId] = new DocumentMetadata
                {
                    DocID = docId,
                    Filename = fileName
                };
            }
       }

        // Method to get the filename of a document
        public string GetFileName(string docId)
        {
            if (string.IsNullOrEmpty(docId))
            {
                return "No document ID provided";
            }
            else if (!documentMetadata.ContainsKey(docId))
            {
                return "Document ID not found";
            }
            if (documentMetadata.TryGetValue(docId, out var metadata))
            {
                if (metadata != null)
                {
                    return metadata.Filename;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "No metadata found";
            }
        }
*/


        // Method to get the term info for a term
 /*       public Dictionary<string, TermInfo>? GetTermInfo(string term) 
        {
            term = term.ToLower();
            if (invertedIndex.ContainsKey(term))
            {
                return invertedIndex[term];
            }
            else 
            {
                return null;
            }
        }
*/
        // Method to get the document frequency of a term
        public int GetDocumentFrequency(string term)
        {
            term = term.ToLower();
            return invertedIndex.ContainsKey(term) ? invertedIndex[term].DocumentFrequency : 0;
        }

        // Method to index a document
         public void IndexDocument(string docId, string content)
        {
            // Tokenize content and track position
            var tokens = Regex.Split(content.ToLower(), @"\W+"); // Splitting by non-word characters
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (!string.IsNullOrEmpty(token))
                {
                    AddOrUpdateTerm(token, docId, i); // Use the position in the document as `i`
                    //AddOrUpdateMetadata(docId, content); // Add metadata for the document
                }
            }
        }

        // Method to return all information about a term 
        public string GetTermData(string term)
        {
            term = term.ToLower();
            if (invertedIndex.ContainsKey(term))
            {
                return JsonConvert.SerializeObject(invertedIndex[term], Formatting.Indented);
            }
            else
            {
                return "Term not found";
            }
        }

        // Method to return all IDs of documents that contain a term
        public List<string> GetDocumentIds(string term)
        {
            term = term.ToLower();
            if (invertedIndex.ContainsKey(term))
            {
                return new List<string>(invertedIndex[term].Documents.Keys);
            }
            else
            {
                return new List<string>();
            }
        }

        // Method to print the inverted index
        public void DisplayIndex()
        {
            foreach (var termEntry in invertedIndex)
            {

                Console.WriteLine($"Term: {termEntry.Key}, Total Frequency: {GetDocumentFrequency(termEntry.Key)}");
                foreach (var docEntry in termEntry.Value.Documents)
                {
                    var info = docEntry.Value;
                    Console.WriteLine($"\tDocId: {docEntry.Key}, TF: {info.TermFrequency}, Positions: {string.Join(", ", info.Positions)}");
                }
            }
        }

        // Method to get the index data as a JSON string
        public string GetIndexData()
        {
            // Sort positions in each TermInfo for consistent ordering
            foreach (var termEntry in invertedIndex)
            {
                foreach (var docEntry in termEntry.Value.Documents)
                {
                    docEntry.Value.Positions.Sort();
                }
            }

            // Serialize the entire invertedIndex to JSON
            return JsonConvert.SerializeObject(invertedIndex, Formatting.Indented);
        }
        

    }


} 