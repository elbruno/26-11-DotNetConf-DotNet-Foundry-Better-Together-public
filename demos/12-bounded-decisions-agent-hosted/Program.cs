// Purpose: Host the custom native Decisions AIAgent through Foundry Responses v2.
// Startup configures only the Decisions endpoint and uses DefaultAzureCredential.
// Starting this project deploys nothing and makes no inference request.
using System.Reflection;
using Azure.AI.AgentServer.Core;
using Azure.Identity;
using ElBruno.AI.Decisions.Foundry;
using HostedDecisionAgent;
using Microsoft.Agents.AI.Foundry.Hosting;
using Microsoft.Extensions.Configuration;

// Step 1: Load explicit production-ready user secrets, then let environment values override them.
var configuration = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    .AddEnvironmentVariables()
    .Build();
var endpoint = new Uri(ReadRequiredSetting("Decision:Endpoint"));
var deployment = ReadRequiredSetting("Decision:Deployment");

// Step 2: Build the native Decisions client with the hosting identity.
using var decisions = new FoundryDecisionClient(new FoundryDecisionOptions
{
    Endpoint = endpoint,
    Model = deployment,
    Credential = new DefaultAzureCredential(),
    Timeout = TimeSpan.FromSeconds(45)
});

// Step 3: Register the custom AIAgent with the Foundry Responses protocol.
var builder = AgentHost.CreateBuilder();
builder.WebApplicationBuilder.AddFoundryResponses(new SupportRoutingAgent(decisions));
builder.RegisterProtocol("responses", endpoints => endpoints.MapFoundryResponses());
var app = builder.Build();
app.Run();

string ReadRequiredSetting(string name)
{
    var value = configuration[name];
    if (value is null)
        throw new InvalidOperationException($"Set {name} in user secrets or the environment.");
    return !string.IsNullOrWhiteSpace(value)
        ? value.Trim()
        : throw new InvalidOperationException($"Setting {name} cannot be blank.");
}
