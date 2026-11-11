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

Requires .NET SDK 11.0.100. Run commands from `demos/` so `global.json` selects the SDK, for example:

```powershell
dotnet run --project .\01-model-comparison\ModelComparison.csproj
```

Service-connected demos require compatible, preconfigured Microsoft Foundry resources and credentials; none are included. Hosted projects are source examples only: this bundle does not include deployment tooling, and hosted .NET 11 runtime behavior has not been verified.

## Slides

[View the final v07 session slides (PDF)](dotnet-foundry-better-together-v07.pdf).

The slides reuse artwork from the official .NET Conf 2026 template, Copyright (c) 2026 .NET Conf. Its MIT notice and scope are in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md). The .NET logo identifies the platform and is used subject to the [official .NET Brand Guidelines](https://github.com/dotnet/brand). Microsoft Learn sources are cited on slide 7.
