using P7_PSEngine.Model;
using System.Collections.Concurrent;

namespace P7_PSEngine.BackgroundServices;

public class SampleData
{
    public ConcurrentBag<DocumentInformation> Data { get; set; } = new();
}
