// Purpose: Provide three deterministic, read-only functions for the hosted tool agent.
// No service, credentials or setup is needed by these functions.
// AddNumbers reports its real execution and emits a correlated tool activity.
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Extensions.AI;

public static class DemoTools
{
    public const string TelemetrySource = "BetterTogether.HostedTools";
    private static readonly ActivitySource Activities = new(TelemetrySource);

    public static IList<AITool> Create() =>
    [
        AIFunctionFactory.Create(GetDotNetFact, name: "GetDotNetFact"),
        AIFunctionFactory.Create(AddNumbers, name: "AddNumbers"),
        AIFunctionFactory.Create(CountWords, name: "CountWords")
    ];

    [Description("Get the demo's short, fixed fact about the .NET garbage collector.")]
    public static string GetDotNetFact() =>
        "The .NET garbage collector automatically reclaims memory occupied by unreachable objects.";

    [Description("Add two integers and return their exact sum.")]
    public static int AddNumbers(
        [Description("The first integer.")] int a,
        [Description("The second integer.")] int b)
    {
        using var activity = Activities.StartActivity("tool.AddNumbers");
        activity?.SetTag("tool.name", "AddNumbers");
        activity?.SetTag("demo.input.a", a);
        activity?.SetTag("demo.input.b", b);
        int result = checked(a + b);
        activity?.SetTag("demo.result", result);
        Console.WriteLine($"Tool executed: AddNumbers(a={a}, b={b}) -> {result}");
        return result;
    }

    [Description("Count the whitespace-separated words in the supplied text.")]
    public static int CountWords(
        [Description("Text whose whitespace-separated words are counted.")] string text) =>
        text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
}
