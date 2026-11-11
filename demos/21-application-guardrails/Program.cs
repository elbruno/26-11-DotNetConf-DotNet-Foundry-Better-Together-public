// Purpose: Demonstrate deterministic application guardrails without calling a service.
// No credentials are required. These checks are not a Foundry content-safety service.
// Expected result: a normal ticket is accepted; a model-proposed refund is blocked.

// Step 1: Bound the input before sending it to any agent.
const string ticket = "I was billed twice for my subscription.";
if (string.IsNullOrWhiteSpace(ticket) || ticket.Length > 4096)
    throw new InvalidOperationException("The ticket must contain 1 to 4096 characters.");
Console.WriteLine($"Accepted input: {ticket}");

// Step 2: Treat a proposed action as untrusted, even if a model sounds confident.
const string proposedAction = "issue-refund";
string[] permittedActions = ["recommend-billing", "recommend-support", "manual-review"];
if (!permittedActions.Contains(proposedAction, StringComparer.Ordinal))
    Console.WriteLine($"Blocked action: {proposedAction}. No account change was performed.");
else
    Console.WriteLine($"Permitted recommendation: {proposedAction}. Human confirmation is still required.");
