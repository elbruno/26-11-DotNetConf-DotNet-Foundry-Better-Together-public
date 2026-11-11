// Purpose: Send the same short prompt to GPT, Grok and Kimi through IChatClient.
// Requires existing compatible project Responses deployments and Azure CLI access.
// Expected result: three real responses, not identical wording or universal support.
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Extensions.AI;

// Step 1: Bind the three actual deployments during preparation, not through CLI switches.
var project = new AIProjectClient(
    new Uri(DemoEnvironment.Read("FOUNDRY_PROJECT_ENDPOINT")), new AzureCliCredential());
(string Label, string Deployment)[] deployments =
[
    ("GPT", DemoEnvironment.Read("GPT_DEPLOYMENT")),
    ("Grok", DemoEnvironment.Read("GROK_DEPLOYMENT")),
    ("Kimi", DemoEnvironment.Read("KIMI_DEPLOYMENT"))
];
const string prompt = "In two short sentences, explain how .NET and Microsoft Foundry work together.";
Console.WriteLine($"Question: {prompt}");

// Step 2: The adapter owns the service protocol; application code only sees IChatClient.
foreach (var (label, deployment) in deployments)
{
    Console.WriteLine($"\nConfigured model label: {label}");
    Console.WriteLine($"Deployment: {deployment}");
#pragma warning disable OPENAI001 // The pinned Foundry Responses adapter is a preview API.
    using IChatClient client = project.GetProjectOpenAIClient()
        .GetProjectResponsesClient().AsIChatClient(deployment);
#pragma warning restore OPENAI001

    // Step 3: Keep the same request shape and application-facing call for every model.
    ChatResponse response = await client.GetResponseAsync(prompt);
    Console.WriteLine($"Model ID (service): {(string.IsNullOrWhiteSpace(response.ModelId) ? "[not returned]" : response.ModelId)}");
    Console.WriteLine($"Response:\n{response.Text}");
}
