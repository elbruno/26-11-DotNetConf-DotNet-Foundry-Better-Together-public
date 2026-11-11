// Purpose: Define the stable JSON result sent by the hosted Decisions agent.
namespace HostedDecisionAgent;

public sealed record RoutingDecision(
    string Team,
    double Confidence,
    IReadOnlyDictionary<string, double> Probabilities,
    RoutingUsage? Usage,
    string Answer);

public sealed record RoutingUsage(
    long? InputTokens,
    long? OutputTokens,
    long? TotalTokens,
    long? ReasoningTokens,
    long? CachedTokens);
