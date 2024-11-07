using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CloudFileIndexer;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class IndexController 
{
    
        private readonly InvertedIndex _invertedIndex;

        public IndexController(InvertedIndex invertedIndex)
        {
            _invertedIndex = invertedIndex;
        }