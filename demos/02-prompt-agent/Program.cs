// Purpose: Call the exact prompt-agent version prepared in the Foundry portal.
// Requires its project, name, version and an authorized Azure CLI identity.
// Expected result: a remote agent response; this program does not publish custom code.
using Azure.AI.Projects;
using Azure.AI.Extensions.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI.Foundry;

// Step 1: Connect to the existing project using the presenter identity.
var project = new AIProjectClient(
    new Uri(DemoEnvironment.Read("FOUNDRY_PROJECT_ENDPOINT")), new AzureCliCredential());

// Step 2: Reference the portal-created agent version, including its configured tools.
var agent = project.AsAIAgent(new AgentReference(
    DemoEnvironment.Read("FOUNDRY_AGENT_NAME"),
    DemoEnvironment.Read("FOUNDRY_AGENT_VERSION")));

// Step 3: Ask once. Inspect tool execution and citations in the portal, not just this text.
const string question = "What is the current weather in Toronto and Seattle? Include observation times and cite sources.";
Console.WriteLine($"Question: {question}");
Console.WriteLine(await agent.RunAsync(question));
