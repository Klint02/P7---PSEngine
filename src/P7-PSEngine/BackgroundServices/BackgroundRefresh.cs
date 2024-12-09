using P7_PSEngine.Services;

namespace P7_PSEngine.BackgroundServices;

public class BackgroundRefresh : IHostedService, IDisposable
{
    //TODO (djb) Make a queue for new user cloud services
    //TODO (djb) Setup background services to work 
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
        //ScheduleTask();
        //_timer = new Timer(test1, null, delay, Timeout.InfiniteTimeSpan);
        return Task.CompletedTask;
    }

    private void ScheduleTask()
    {
        var now = DateTime.Now;
        var nextRunTime = now.Date + _executionTime;
        if (nextRunTime <= now)
        {
            nextRunTime = nextRunTime.AddDays(1);
            Console.WriteLine("added to next day");
        }
        var delay = nextRunTime - now;
        _timer = new Timer(test1, null, delay, Timeout.InfiniteTimeSpan);
    }

    private void test1(object? state)
    {
        Console.WriteLine($"test1 : time {DateTime.Now.ToLongTimeString()}");

        // Schedule the next run
        ScheduleTask();
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

    //private TimeSpan getScheduledParsedTime()
    //{
    //    string[] formats = { @"hh\:mm\:ss", "hh\\:mm" };
    //    string jobStartTime = "10:31";
    //    TimeSpan.TryParseExact(jobStartTime, formats, CultureInfo.InvariantCulture, out TimeSpan ScheduledTimespan);
    //    return ScheduledTimespan;
    //}

    //private TimeSpan getJobRunDelay()
    //{
    //    TimeSpan scheduledParsedTime = getScheduledParsedTime();
    //    TimeSpan curentTimeOftheDay = TimeSpan.Parse(DateTime.Now.TimeOfDay.ToString("hh\\:mm"));
    //    TimeSpan delayTime = scheduledParsedTime >= curentTimeOftheDay
    //        ? scheduledParsedTime - curentTimeOftheDay     // Initial Run, when ETA is within 24 hours
    //        : new TimeSpan(24, 0, 0) - curentTimeOftheDay + scheduledParsedTime;   // For every other subsequent runs
    //    return delayTime;
    //}

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
