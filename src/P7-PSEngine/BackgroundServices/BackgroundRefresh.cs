using P7_PSEngine.Model;
using P7_PSEngine.Services;

namespace P7_PSEngine.BackgroundServices;

public class BackgroundRefresh : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly SampleData _data;
    private readonly ILogger<BackgroundRefresh> _logger;
    private readonly ISearchService _searchService;

    public BackgroundRefresh(SampleData data, ILogger<BackgroundRefresh> logger, ISearchService searchService)
    {
        _data = data;
        _logger = logger;
        _searchService = searchService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BackgroundRefresh is starting.");
        //_timer = new Timer(AddIndexToCache, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        AddIndexToCache(null);
        _logger.LogInformation("Inserted data into the bag");
        return Task.CompletedTask;
    }

    private async void AddIndexToCache(object? state)
    {
        // Fetch document with their indexes
        IEnumerable<FileInformation> document = await _searchService.GetALlDocumentsWithIndex();
        foreach (var doc in document)
        {
            foreach (var index in doc.IndexInformations)
            {
                index.FileInformation = null;
            }
        }
        foreach (var doc in document)
        {
            _data.Data.Add(doc);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
