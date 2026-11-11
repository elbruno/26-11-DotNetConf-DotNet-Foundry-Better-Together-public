// Purpose: List an existing Foundry Toolbox's MCP metadata, never execute a tool.
// Requires a prepared TOOLBOX_ENDPOINT and an already-authorized Azure CLI identity.
// Contract: https://learn.microsoft.com/azure/foundry/agents/how-to/tools/toolbox
using System.Net.Http.Headers;
using Azure.Core;
using Azure.Identity;
using ModelContextProtocol.Client;

// Step 1: Connect to the central toolbox, not its individual tool servers.
var endpoint = new Uri(DemoEnvironment.Read("TOOLBOX_ENDPOINT"));
if (endpoint.Scheme != Uri.UriSchemeHttps
    || !endpoint.Host.EndsWith(".services.ai.azure.com", StringComparison.OrdinalIgnoreCase)
    || !endpoint.AbsolutePath.Contains("/toolboxes/", StringComparison.Ordinal)
    || !endpoint.AbsolutePath.EndsWith("/mcp", StringComparison.Ordinal)
    || endpoint.Query != "?api-version=v1")
    throw new InvalidOperationException("TOOLBOX_ENDPOINT must be a Foundry toolbox MCP HTTPS URL with ?api-version=v1.");

using var httpClient = new HttpClient(new ToolboxAuthenticationHandler());
await using var transport = new HttpClientTransport(new()
{
    Name = "Existing Foundry Toolbox",
    Endpoint = endpoint,
    TransportMode = HttpTransportMode.StreamableHttp,
    EnableStandaloneGetStream = false,
    MaxReconnectionAttempts = 0
}, httpClient);
await using var client = await McpClient.CreateAsync(transport);

// Step 2: Discovery only. Auth, HTTP and consent errors propagate to the terminal.
var tools = await client.ListToolsAsync();
Console.WriteLine($"Available tools: {tools.Count}");
foreach (var tool in tools)
{
    Console.WriteLine($"{tool.Name}: {tool.Description}");
    Console.WriteLine($"Input schema: {tool.JsonSchema}");
}

// Refresh the scoped bearer token per request. No browser consent or retry loop.
sealed class ToolboxAuthenticationHandler : DelegatingHandler
{
    private readonly AzureCliCredential credential = new();

    public ToolboxAuthenticationHandler()
        : base(new HttpClientHandler { AllowAutoRedirect = false })
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await credential.GetTokenAsync(
            new TokenRequestContext(["https://ai.azure.com/.default"]), cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        return await base.SendAsync(request, cancellationToken);
    }
}
