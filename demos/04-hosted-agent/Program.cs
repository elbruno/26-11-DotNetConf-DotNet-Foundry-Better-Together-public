// Purpose: Host Demo 03's same tool-free hello agent behind Foundry Responses v2.
// Starting this host deploys nothing. A response still needs a real Foundry model.
using System.Reflection;
using Azure.AI.AgentServer.Core;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry.Hosting;
using Microsoft.Extensions.Configuration;

// Step 1: Keep this package independent of the sibling console demos.
var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .AddEnvironmentVariables()
    .Build();
var endpoint = new Uri(ReadSetting("AZURE_AI_PROJECT_ENDPOINT", "FOUNDRY_PROJECT_ENDPOINT"));
var deployment = ReadSetting("AZURE_AI_MODEL_DEPLOYMENT_NAME", "GPT_DEPLOYMENT");

// Step 2: Keep Demo 03's name and instructions, with the hosting identity.
AIAgent agent = new AIProjectClient(endpoint, new DefaultAzureCredential())
    .AsAIAgent(
        model: deployment,
        name: "BetterTogetherHello",
        instructions: "You are a friendly .NET assistant. Reply in one short sentence.");

// Step 3: Replace the console RunAsync call with the Responses protocol host.
var builder = AgentHost.CreateBuilder();
builder.Services.AddFoundryResponses(agent);
builder.RegisterProtocol("responses", endpoints => endpoints.MapFoundryResponses());
var app = builder.Build();
app.Run();

string ReadSetting(string name, string legacyName)
{
    var value = configuration[name];
    if (value is null)
        value = configuration[legacyName];
    return !string.IsNullOrWhiteSpace(value)
        ? value.Trim()
        : throw new InvalidOperationException($"Set {name} in this project's user secrets or environment.");
}
