using CloudSearcher;

public class SearchController
{

    private readonly IndexService _indexService;

    public SearchController(IndexService indexService)
    {
        _indexService = indexService;
    }

    public SearchResult Search(string searchTerm)
    {
        var invertedIndex = _indexService.GetInvertedIndex();
        Console.WriteLine(invertedIndex.tester);
        var test = invertedIndex.GetIndexData();
        //            Console.WriteLine("Display index: ");
        //            Console.WriteLine(test);
        var booleanSearch = new BooleanSearch(invertedIndex);
        //            Console.WriteLine("Search term: " + searchTerm);
        //            Console.WriteLine("Inverted index: ");
        invertedIndex.DisplayIndex();

        return booleanSearch.BSearch(searchTerm);
    }

}
