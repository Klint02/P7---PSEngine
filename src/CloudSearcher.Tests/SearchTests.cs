using System;
using System.Collections.Generic;
using Xunit;
using CloudSearcher;

namespace CloudSearcher.Tests
{
    public class SearchTest
    {
        // Test to check if search query is processed correctly
        [Fact]
        public void TestProcessSearchQuery()
        {
            // Arrange
            var invertedIndex = new InvertedIndex();
            var booleanSearch = new BooleanSearch(invertedIndex);
            string searchTerm = "hello world";
            List<string> expectedSearchTerms = new List<string> { "hello", "world" };

            // Act
            List<string> searchTerms = booleanSearch.ProcessSearchQuery(searchTerm);

            // Assert
            Assert.Equal(expectedSearchTerms, searchTerms);
        }

        // Test to check if CalculateTotalResults method returns correct number of results
        // First test case: searchTerm is present in one document
        [Fact]
        public void TestCalculateTotalResults()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId = "doc1";
            string content = "hello hello hello hello hello";
            index.IndexDocument(docId, content);            
            var booleanSearch = new BooleanSearch(index);
            string searchTerm = "hello";
            int expectedTotalResults = 5;

            // Act
            int totalResults = booleanSearch.CalculateTotalResults(searchTerm);

            // Assert
            Assert.Equal(expectedTotalResults, totalResults);
        }

        // Test to check if CalculateTotalResults method returns correct number of results
        // Second test case: searchTerm is present in multiple documents
        [Fact]
        public void TestCalculateTotalResultsMultipleDocs()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId1 = "doc1";
            string content1 = "hello hello hello hello hello";
            index.IndexDocument(docId1, content1);
            string docId2 = "doc2";
            string content2 = "hello hello hello";
            index.IndexDocument(docId2, content2);
            var booleanSearch = new BooleanSearch(index);
            string searchTerm = "hello";
            int expectedTotalResults = 8;

            // Act
            int totalResults = booleanSearch.CalculateTotalResults(searchTerm);

            // Assert
            Assert.Equal(expectedTotalResults, totalResults);
        }

        // Test to check if CalculateTotalResults method returns 0 if searchTerm is not present in the index
        [Fact]
        public void TestCalculateTotalResultsNoResults()
        {
            // Arrange
            var index = new InvertedIndex();
            var booleanSearch = new BooleanSearch(index);
            string searchTerm = "hello";
            int expectedTotalResults = 0;

            // Act
            int totalResults = booleanSearch.CalculateTotalResults(searchTerm);

            // Assert
            Assert.Equal(expectedTotalResults, totalResults);
        }

        // Test to check if BoolSearch method returns correct search results
        [Fact]
        public void TestBoolSearch()
        {
            // Arrange
            var index = new InvertedIndex();
            string docId1 = "doc1";
            string content1 = "hello world";
            index.IndexDocument(docId1, content1);
            string docId2 = "doc2";
            string content2 = "hello";
            index.IndexDocument(docId2, content2);
            var booleanSearch = new BooleanSearch(index);
            string searchTerm = "hello";
            SearchResult expectedSearchResults = new SearchResult();
            expectedSearchResults.SearchTerm = "hello";
            expectedSearchResults.TotalResults = 2;
            expectedSearchResults.SearchResults.Add(new Dictionary<string, string> { { "DocID", "doc1" }, { "Filename", "hello world" } });
            expectedSearchResults.SearchResults.Add(new Dictionary<string, string> { { "DocID", "doc2" }, { "Filename", "hello" } });

            // Act
            SearchResult searchResults = booleanSearch.BoolSearch(searchTerm);

            // Assert
            Assert.Equal(expectedSearchResults.SearchTerm, searchResults.SearchTerm);
            Assert.Equal(expectedSearchResults.TotalResults, searchResults.TotalResults);
            Assert.Equal(expectedSearchResults.SearchResults, searchResults.SearchResults);
        }
    }
}