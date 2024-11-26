using System.Collections.Concurrent;

namespace P7_PSEngine.BackgroundServices;

public class SampleData
{
    public ConcurrentBag<string> Data { get; set; } = new();
}
