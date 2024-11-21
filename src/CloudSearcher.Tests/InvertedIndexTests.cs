using System;
using System.Collections.Generic;
using Xunit;
using CloudSearcher;

namespace CloudSearcher.Tests
{
    public class InvertedIndexTests
    {
        /*
        // Test AddMetadata method
        [Fact]
        public void AddOrUpdateMetadata_ShouldAddMetadata()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string fileName = "file1.txt";

            // Act
            index.AddOrUpdateMetadata(docId, fileName);

            // Assert
            Assert.True(index.documentMetadata.ContainsKey(docId));
            Assert.Equal(fileName, index.documentMetadata[docId].Filename);
            Assert.False(index.documentMetadata.ContainsKey("doc2"));
        }

        // Test that adding metadata for an existing document ID updates the filename
        [Fact]
        public void AddOrUpdateMetadata_ShouldUpdateMetadata()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string fileName = "file1.txt";
            string newFileName = "file2.txt";
            index.AddOrUpdateMetadata(docId, fileName);

            // Act
            index.AddOrUpdateMetadata(docId, newFileName);

            // Assert
            Assert.Equal(newFileName, index.documentMetadata[docId].Filename);
        }


        // Test GetFileName method
        [Fact]
        public void GetFileName_ShouldReturnCorrectFileName()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string fileName = "file1.txt";
            index.AddOrUpdateMetadata(docId, fileName);

            // Act
            var result = index.GetFileName(docId);

            // Assert
            Assert.Equal(fileName, result);
        }

        [Fact]
        public void GetFileName_ShouldReturnNotFoundMessage()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";

            // Act
            var result = index.GetFileName(docId);

            // Assert
            Assert.Equal("Document ID not found", result);
        }
/*
        [Fact]
        public void GetFileName_ShouldReturnNoDocumentIdMessage()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "";

            // Act
            var result = index.GetFileName(docId);

            // Assert
            Assert.Equal("No document ID provided", result);
        }

        [Fact]
        public void GetFileName_ShouldReturnNoFilenameMessage()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string fileName = "";
            index.AddOrUpdateMetadata(docId, fileName);

            // Act
            var result = index.GetFileName(docId);
            Console.WriteLine(result);

            // Assert
            Assert.Equal("", result);
        }
/*
        [Fact]
        public void GetFileName_ShouldReturnNoMetadataMessage()
        {
            // Arrange
            var index = new InvertedIndex();

            // Act
            var result = index.GetFileName("");

            // Assert
            Assert.Equal("No metadata found", result);
        }
*/
        // Test AddTerm method
        [Fact]
        public void AddTerm_ShouldAddTerm()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            int position = 1;

            // Act
            index.AddOrUpdateTerm(term, docId, position);

            // Assert
            Assert.True(index.invertedIndex.ContainsKey("term1"));
            Assert.True(index.invertedIndex[term].Documents.ContainsKey("doc1"));
            Assert.Equal(1, index.invertedIndex[term].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex[term].Documents[docId].Positions[0]);
            Assert.Equal(1, index.GetDocumentFrequency(term));
        }
        
        // Test that adding a document ID that already exists in the inverted index does 
        //not add a new entry but updates the existing entry 
        [Fact]
        public void AddTerm_ShouldUpdateTerm()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            string docID2 = "doc2";
            int position = 1;
            index.AddOrUpdateTerm(term, docId, position);
            index.AddOrUpdateTerm(term, docId, 2);

            // Act
            index.AddOrUpdateTerm(term, docID2, 2);

            // Assert
            Assert.True(index.invertedIndex.ContainsKey("term1"));
            Assert.True(index.invertedIndex[term].Documents.ContainsKey("doc1"));
            Assert.True(index.invertedIndex[term].Documents.ContainsKey("doc2"));
            Assert.Equal(2, index.invertedIndex[term].DocumentFrequency);
            Assert.Equal(3, index.invertedIndex[term].TotalTermFrequency);
            Assert.Equal(2, index.invertedIndex[term].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex[term].Documents[docID2].TermFrequency);
        }

        // AddTerm method should increment the document frequency for a term that already
        // exists in the inverted index
        [Fact]
        public void AddTerm_ShouldIncrementDocumentFrequency()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            int position = 1;
            index.AddOrUpdateTerm(term, docId, position);
            Assert.Equal(1, index.GetDocumentFrequency(term));

            // Act
            index.AddOrUpdateTerm(term, "doc2", 2);

            // Assert
            Assert.Equal(2, index.GetDocumentFrequency(term));
        }

        // Test that AddTerm method works when a term is multiple times in the same document
        [Fact]
        public void AddTerm_ShouldAddTermMultipleTimesAndIncrementTermFrequency()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            int position = 1;
            index.AddOrUpdateTerm(term, docId, position);

            // Act
            index.AddOrUpdateTerm(term, docId, 2);

            // Assert
            Assert.True(index.invertedIndex.ContainsKey(term));
            Assert.True(index.invertedIndex[term].Documents.ContainsKey(docId));
            Assert.Equal(2, index.invertedIndex[term].Documents[docId].TermFrequency);
            Assert.Equal(new List<int> { 1, 2 }, index.invertedIndex[term].Documents[docId].Positions);
            Assert.Equal(1, index.GetDocumentFrequency(term));
        }
/*
        // Test if GetTermInfo method returns null for a term that does not exist in the inverted index
        [Fact]
        public void AddTerm_ShouldReturnNullTerm()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "nonexistentterm";

            // Act
            var result = index.GetTermInfo(term);

            // Assert
            Assert.Null(result);
        }

        // Test if GetTermInfo method returns the correct term info for a term that exists 
        // in the inverted index
        [Fact]
        public void AddTerm_ShouldReturnCorrectTermInfo()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            string docID2 = "doc2";
            int position = 1;
            int position2 = 2;
            index.AddTerm(term, docId, position);
            index.AddTerm(term, docID2, position2);
        
            // Act
            var result = index.GetTermInfo(term);

            // Assert
            Assert.True(result.ContainsKey(docId));
            Assert.True(result.ContainsKey(docID2));
            Assert.Equal(1, result[docId].TermFrequency);
            Assert.Equal(1, result[docID2].TermFrequency);
            Assert.Equal(1, result[docId].Positions[0]);
            Assert.Equal(2, result[docID2].Positions[0]);
        }
*/
        // Test GetDocumentFrequency method
        [Fact]
        public void GetDocumentFrequency_ShouldReturnCorrectDocumentFrequency()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            string docID2 = "doc2";
            int position = 1;
            index.AddOrUpdateTerm(term, docId, position);
            index.AddOrUpdateTerm(term, docID2, position);

            // Act
            var result = index.GetDocumentFrequency(term);

            // Assert
            Assert.Equal(2, result);
        }

        // Test GetDocumentFrequency method for a term that does not exist in the inverted index
        [Fact]
        public void GetDocumentFrequency_ShouldReturnZero()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "nonexistentterm";

            // Act
            var result = index.GetDocumentFrequency(term);

            // Assert
            Assert.Equal(0, result);
        }

        // Test IndexDocument method
        [Fact]
        public void IndexDocument_ShouldIndexDocument()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string content = "this is a test document";

            // Act
            index.IndexDocument(docId, content);

            // Assert
            Assert.True(index.invertedIndex.ContainsKey("this"));
            Assert.True(index.invertedIndex.ContainsKey("is"));
            Assert.True(index.invertedIndex.ContainsKey("a"));
            Assert.True(index.invertedIndex.ContainsKey("test"));
            Assert.True(index.invertedIndex.ContainsKey("document"));
            Assert.True(index.invertedIndex["this"].Documents.ContainsKey(docId));
            Assert.True(index.invertedIndex["is"].Documents.ContainsKey(docId));
            Assert.True(index.invertedIndex["a"].Documents.ContainsKey(docId));
            Assert.True(index.invertedIndex["test"].Documents.ContainsKey(docId));
            Assert.True(index.invertedIndex["document"].Documents.ContainsKey(docId));
            Assert.Equal(1, index.invertedIndex["this"].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex["is"].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex["a"].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex["test"].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex["document"].Documents[docId].TermFrequency);
            Assert.Equal(0, index.invertedIndex["this"].Documents[docId].Positions[0]);
            Assert.Equal(1, index.invertedIndex["is"].Documents[docId].Positions[0]);
            Assert.Equal(2, index.invertedIndex["a"].Documents[docId].Positions[0]);
            Assert.Equal(3, index.invertedIndex["test"].Documents[docId].Positions[0]);
            Assert.Equal(4, index.invertedIndex["document"].Documents[docId].Positions[0]);
        }
    
        // Test IndexDocument method for a document that contains multiple instances of the same term
        [Fact]
        public void IndexDocument_ShouldIndexDocumentWithMultipleInstancesOfTerm()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string content = "This is a test document";
            string content2 = "This is another test document with a sentence";

            // Act
            index.IndexDocument(docId, content);
            index.IndexDocument(docId, content2);

            // Assert
            Assert.Equal(2, index.invertedIndex["this"].Documents[docId].TermFrequency);
            Assert.Equal(2, index.invertedIndex["is"].Documents[docId].TermFrequency);
            Assert.Equal(2, index.invertedIndex["a"].Documents[docId].TermFrequency);
            Assert.Equal(2, index.invertedIndex["test"].Documents[docId].TermFrequency);
            Assert.Equal(2, index.invertedIndex["document"].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex["with"].Documents[docId].TermFrequency);
            Assert.Equal(1, index.invertedIndex["sentence"].Documents[docId].TermFrequency);
            Assert.NotEqual(10, index.invertedIndex["another"].Documents[docId].TermFrequency);
        }

        // Test GetTermData method for a term that exists in the inverted index
        [Fact]
        public void GetTermData_ShouldReturnTermData()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            int position = 1;
            index.AddOrUpdateTerm(term, docId, position);

            // Act
            var result = index.GetTermData(term);

            // Assert
            Assert.Contains(docId, result);
            Assert.Contains("\"TermFrequency\": 1,", result);
            //Assert.Contains("Positions\": [\n 1,\n ]", result);
            //Assert.True(result.ContainsKey(docId));
            //Assert.Equal(1, result[docId].TermFrequency);
            //Assert.Equal(1, result[docId].Positions[0]);
        }

        // Test GetDocumentIds method for a term that exists in the inverted index
        [Fact]
        public void GetDocumentIds_ShouldReturnDocumentIds()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId = "doc1";
            int position = 1;
            index.AddOrUpdateTerm(term, docId, position);

            // Act
            var result = index.GetDocumentIds(term);

            // Assert
            Assert.Contains(docId, result);
        }

        // Test GetDocumentIds method for a term that does not exist in the inverted index
        [Fact]
        public void GetDocumentIds_ShouldReturnEmptyList()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "nonexistentterm";

            // Act
            var result = index.GetDocumentIds(term);

            // Assert
            Assert.Empty(result);
        }

        // Test GetDocumentIds method for a term that exists in multiple documents
        [Fact]
        public void GetDocumentIds_ShouldReturnMultipleDocumentIds()
        {
            // Arrange
            var index = new InvertedIndex();
            string term = "term1";
            string docId1 = "doc1";
            string docId2 = "doc2";
            int position = 1;
            index.AddOrUpdateTerm(term, docId1, position);
            index.AddOrUpdateTerm(term, docId2, position);

            // Act
            var result = index.GetDocumentIds(term);

            // Assert
            Assert.Equal(new List<string> { docId1, docId2 }, result);
        }

        // Test DisplayInvertedIndex method
        [Fact]
        public void DisplayInvertedIndex_ShouldDisplayInvertedIndex()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string content = "This is a test document";
            //string content2 = "This is another test document with a sentence";
            index.IndexDocument(docId, content);
            //index.IndexDocument(docId, content2);

            // Capture the console output
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            index.DisplayIndex();
            var result = stringWriter.ToString();

            // Assert
            Assert.Contains("Term: this, Total Frequency: 1", result);
            Assert.Contains("Term: is, Total Frequency: 1", result);
            Assert.Contains("Term: a, Total Frequency: 1", result);
            Assert.Contains("Term: test, Total Frequency: 1", result);
            Assert.Contains("Term: document, Total Frequency: 1", result);
            Assert.Contains("DocId: doc1, TF: 1, Positions: 0", result);
        }

        // Test GetIndexData method for a non-empty inverted index
        [Fact]
        public void GetIndexData_ShouldReturnIndexData()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string content = "This is a test document";
            index.IndexDocument(docId, content);

            // Act
            var result = index.GetIndexData();

            // Assert
            Assert.NotEmpty(result);
        }

        // Test GetIndexData method for an empty inverted index in JSON format
        [Fact]
        public void GetIndexData_ShouldReturnEmptyIndexData()
        {
            // Arrange
            var index = new InvertedIndex();

            // Act
            var result = index.GetIndexData();

            // Assert
            Assert.Equal("{}", result);
        }


        // Test that GetIndexData method returns the correct JSON string
        [Fact]
        public void GetIndexData_ShouldReturnCorrectJSONString()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string content = "This is a test document";
            index.IndexDocument(docId, content);

            // Act
            var result = index.GetIndexData();

            // Assert
            Assert.Contains("this", result);
            Assert.Contains("is", result);
            Assert.Contains("a", result);
            Assert.Contains("test", result);
            Assert.Contains("document", result);
            Assert.Contains("doc1", result);
            Assert.Contains("TermFrequency", result);
            Assert.Contains("Positions", result);
        }

    }
}