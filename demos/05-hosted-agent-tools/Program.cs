// Purpose: Host a MAF agent with deterministic tools and export real tool spans.
// Requires Foundry model access through the hosting identity; startup deploys nothing.
// Inspect the actual invocation's tool spans in Foundry, not a narrated model answer.
using System.Reflection;
using Azure.AI.AgentServer.Core;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.AI;
using OpenTelemetry.Trace;

// Step 1: Keep this package independent of the sibling console demos.
var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .AddEnvironmentVariables()
    .Build();
var endpoint = new Uri(ReadSetting("AZURE_AI_PROJECT_ENDPOINT", "FOUNDRY_PROJECT_ENDPOINT"));
var deployment = ReadSetting("AZURE_AI_MODEL_DEPLOYMENT_NAME", "GPT_DEPLOYMENT");

// Step 2: Instrument the model client before MAF builds its function-call loop.
AIAgent agent = new AIProjectClient(endpoint, new DefaultAzureCredential())
    .AsAIAgent(
        model: deployment,
        name: "BetterTogetherTools",
        instructions: "Use the named tool for requests about integer sums, word counts or the demo .NET fact. Keep answers brief.",
        tools: DemoTools.Create(),
        clientFactory: client => client.AsBuilder()
            .UseOpenTelemetry(sourceName: DemoTools.TelemetrySource)
            .Build());

// Step 3: The host owns the function loop and exposes the Responses protocol.
var builder = AgentHost.CreateBuilder();
builder.ConfigureTracing(tracing => tracing.AddSource(DemoTools.TelemetrySource));
builder.WebApplicationBuilder.AddFoundryResponses(agent);
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
