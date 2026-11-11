# .NET + Foundry: Better Together

These materials accompany the [.NET Conf 2026 session](https://dotnet.microsoft.com/en-us/conf/2026/agenda/brk007). See the [official conference agenda](https://dotnet.microsoft.com/en-us/conf/2026/agenda).

## Demos

- [01-model-comparison](demos/01-model-comparison/): Compare configured GPT, Grok, and Kimi deployments through one `IChatClient` call site.
- [02-prompt-agent](demos/02-prompt-agent/): Call an existing, versioned Foundry prompt agent.
- [03-code-agent](demos/03-code-agent/): Run a code-defined, tool-free .NET agent locally.
- [04-hosted-agent](demos/04-hosted-agent/): Host the tool-free agent through the Foundry Responses protocol.
- [05-hosted-agent-tools](demos/05-hosted-agent-tools/): Host an agent with deterministic tools and tool telemetry.
- [10-bounded-decisions](demos/10-bounded-decisions/): Request a bounded support route directly from a native Decisions model.
- [11-bounded-decisions-agent](demos/11-bounded-decisions-agent/): Expose a native Decisions call through a custom .NET agent.
- [12-bounded-decisions-agent-hosted](demos/12-bounded-decisions-agent-hosted/): Host the Decisions agent through the Foundry Responses protocol.
- [20-toolbox-discovery](demos/20-toolbox-discovery/): Discover tools and schemas from an existing Foundry Toolbox without calling a tool.
- [21-application-guardrails](demos/21-application-guardrails/): Demonstrate offline input validation and action allowlisting.
- [22-tool-evaluation](demos/22-tool-evaluation/): Evaluate a deterministic tool against a small offline dataset.

## Run the demos

Requires .NET SDK 11.0.100. From the repository root, enter `demos/` and run a project directly. The `Prepare-Demos.ps1` helper mentioned in some error text is not included in this snapshot.

```powershell
Set-Location .\demos
$project = ".\01-model-comparison\ModelComparison.csproj"
dotnet user-secrets set "FOUNDRY_PROJECT_ENDPOINT" "<your Foundry project endpoint>" --project $project
dotnet user-secrets set "GPT_DEPLOYMENT" "<your GPT deployment name>" --project $project
dotnet user-secrets set "GROK_DEPLOYMENT" "<your Grok deployment name>" --project $project
dotnet user-secrets set "KIMI_DEPLOYMENT" "<your Kimi deployment name>" --project $project
dotnet run --project $project
```

Every project has a per-project User Secrets ID set in `Directory.Build.props` or its project file. You can use the environment-variable names below instead of User Secrets. `DemoEnvironment.Read` gives environment variables precedence over User Secrets; hosted projects load User Secrets and then environment variables. These settings identify endpoints, deployments, agents, or toolboxes. They do not authenticate you, and no credentials or Foundry resources are included.

| Demo | Required configuration | Identity and service requirement |
| --- | --- | --- |
| `01-model-comparison` | `FOUNDRY_PROJECT_ENDPOINT`, `GPT_DEPLOYMENT`, `GROK_DEPLOYMENT`, `KIMI_DEPLOYMENT` | `AzureCliCredential`; an existing project with all three deployments. Sends requests to each deployment. |
| `02-prompt-agent` | `FOUNDRY_PROJECT_ENDPOINT`, `FOUNDRY_AGENT_NAME`, `FOUNDRY_AGENT_VERSION` | `AzureCliCredential`; an existing, versioned Foundry agent. Runs that remote agent. |
| `03-code-agent` | `FOUNDRY_PROJECT_ENDPOINT`, `GPT_DEPLOYMENT` | `AzureCliCredential`; an existing model deployment. The agent code runs locally but calls the remote model. |
| `04-hosted-agent` | `AZURE_AI_PROJECT_ENDPOINT` or `FOUNDRY_PROJECT_ENDPOINT`; `AZURE_AI_MODEL_DEPLOYMENT_NAME` or `GPT_DEPLOYMENT` | `DefaultAzureCredential`; an existing model deployment. Starts a local Responses host. |
| `05-hosted-agent-tools` | `AZURE_AI_PROJECT_ENDPOINT` or `FOUNDRY_PROJECT_ENDPOINT`; `AZURE_AI_MODEL_DEPLOYMENT_NAME` or `GPT_DEPLOYMENT` | `DefaultAzureCredential`; an existing model deployment. Starts a local Responses host with deterministic tools. |
| `10-bounded-decisions` | Environment: `Decision__Endpoint`, `Decision__Deployment`; User Secrets: `Decision:Endpoint`, `Decision:Deployment` | `AzureCliCredential`; an existing Decisions endpoint and deployment. |
| `11-bounded-decisions-agent` | Environment: `Decision__Endpoint`, `Decision__Deployment`; User Secrets: `Decision:Endpoint`, `Decision:Deployment` | `AzureCliCredential`; an existing Decisions endpoint and deployment. |
| `12-bounded-decisions-agent-hosted` | Environment: `Decision__Endpoint`, `Decision__Deployment`; User Secrets: `Decision:Endpoint`, `Decision:Deployment` | `DefaultAzureCredential`; an existing Decisions endpoint and deployment. Starts a local Responses host. |
| `20-toolbox-discovery` | `TOOLBOX_ENDPOINT` | `AzureCliCredential`; an existing Foundry Toolbox MCP HTTPS endpoint on `*.services.ai.azure.com`, with a `/toolboxes/` path and `/mcp?api-version=v1` suffix. Lists metadata only; it does not call a tool. |

Before running a demo that explicitly uses `AzureCliCredential`, sign in with `az login` as an identity authorized for the selected project, deployment, or Toolbox. The hosted demos use `DefaultAzureCredential`, not `AzureCliCredential`; configure a credential source supported by that chain for local use and the appropriate identity for any deployment. The hosted projects start local servers and do not deploy resources. Deployed hosted .NET 11 runtime behavior has not been verified.

`21-application-guardrails` and `22-tool-evaluation` run locally without settings, credentials, or Foundry/model service calls. `dotnet run` may still need NuGet access to restore packages:

```powershell
dotnet run --project .\21-application-guardrails\ApplicationGuardrails.csproj
dotnet run --project .\22-tool-evaluation\ToolEvaluation.csproj
```

## Slides

[View the final v07 session slides (PDF)](dotnet-foundry-better-together-v07.pdf).

The slides reuse artwork from the official .NET Conf 2026 template, Copyright (c) 2026 .NET Conf. Its MIT notice and scope are in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md). The .NET logo identifies the platform and is used subject to the [official .NET Brand Guidelines](https://github.com/dotnet/brand). Microsoft Learn sources are cited on slide 7.
