using P7_PSEngine.Services;

namespace P7_PSEngine.BackgroundServices;

public class BackgroundRefresh : IHostedService, IDisposable
{
    //TODO (djb) Make a queue for new user cloud services
    //TODO (djb) Setup background services to work - fetch files from cloudservices
    //TODO (djb) Map invertedIndex to our desire DTO
    //TODO (djb) Make sure there is a flow
    private Timer? _timer;
    private readonly TimeSpan _executionTime;
    private readonly SampleData _data;
    private readonly ILogger<BackgroundRefresh> _logger;
    private readonly ISearchService _searchService;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundRefresh(SampleData data, ILogger<BackgroundRefresh> logger, IServiceProvider serviceProvider)
    {
        _data = data;
        _logger = logger;
        _executionTime = new TimeSpan(10, 58, 0);
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("BackgroundRefresh is starting.");
        _logger.LogInformation("Inserting data into the bag");
        AddIndexToCache(null);
        //_timer = new Timer(test1, null, delay, Timeout.InfiniteTimeSpan);
        return Task.CompletedTask;
    }

    private async void AddIndexToCache(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var searchService = scope.ServiceProvider.GetService<ISearchService>();
        // Fetch document with their indexes
        //IEnumerable<FileInformation> document = await searchService!.GetALlDocumentsWithIndex();
        //foreach (var doc in document)
        //{
        //    foreach (var index in doc.WordInformations)
        //    {
        //        index.FileInformation = null;
        //    }
        //}
        //foreach (var doc in document)
        //{
        //    _data.Data.Add(doc);
        //}
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
