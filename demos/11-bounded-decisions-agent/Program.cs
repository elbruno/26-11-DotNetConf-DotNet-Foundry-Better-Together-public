// Purpose: Run a real MAF agent whose implementation uses native Decisions.
// Requires Decision endpoint and deployment settings plus local Azure CLI authentication.
// Expected result: the agent returns one bounded support-route recommendation.
using Azure.Identity;
using BoundedDecisionsAgent;
using ElBruno.AI.Decisions.Foundry;
using Microsoft.Agents.AI;
using System.Text.Json;

// Step 1: Create the native Decisions client used behind the MAF boundary.
using var decisions = new FoundryDecisionClient(new FoundryDecisionOptions
{
    Endpoint = new Uri(DemoEnvironment.Read("Decision__Endpoint")),
    Model = DemoEnvironment.Read("Decision__Deployment"),
    Credential = new AzureCliCredential(),
    Timeout = TimeSpan.FromSeconds(45)
});

// Step 2: Invoke the custom AIAgent through the normal MAF RunAsync API.
AIAgent agent = new SupportRoutingAgent(decisions);
const string ticket = "I was charged twice for my subscription.";
Console.WriteLine($"Question: {ticket}");
var response = await agent.RunAsync(ticket);

// Step 3: Print the JSON produced by the agent, without a chat-model wrapper.
using var result = JsonDocument.Parse(response.Messages.Single().Text!);
Console.WriteLine(JsonSerializer.Serialize(result.RootElement, new JsonSerializerOptions { WriteIndented = true }));
