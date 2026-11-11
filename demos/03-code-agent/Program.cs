// Purpose: Say hello with a code-defined Microsoft Agent Framework agent.
// Requires a configured Foundry model and authorized Azure CLI identity.
// Expected result: a short real model response; there are no tools or deployment.
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;

// Step 1: The application owns the agent; the configured model is remote.
var project = new AIProjectClient(
    new Uri(DemoEnvironment.Read("FOUNDRY_PROJECT_ENDPOINT")), new AzureCliCredential());
AIAgent agent = project.AsAIAgent(
    model: DemoEnvironment.Read("GPT_DEPLOYMENT"),
    name: "BetterTogetherHello",
    instructions: "You are a friendly .NET assistant. Reply in one short sentence.");

// Step 2: Demo 04 hosts this same greeting agent instead of running this console loop.
const string question = "Say hello to .NET Conf and explain your role briefly.";
Console.WriteLine($"Question: {question}");
Console.WriteLine(await agent.RunAsync(question));
