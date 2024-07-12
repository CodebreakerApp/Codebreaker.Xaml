# GitHub Actions

## Publish to Azure DevOps

[Azure DevOps feed for daily builds](https://pkgs.dev.azure.com/cnilearn/codebreakerpackages/_packaging/codebreaker/nuget/v3/index.json)

Daily build version of NuGet packages (*CNinnovation.Codebreaker.ViewModels*) are published to Azure DevOps with every merge to the main branch.

For publishing, an Azure DevOps PAT is required.
Permissions: Packaging, Read & Write

The PAT is configured with the environment *DevOpsArtifacts" actions secret *DEVOPS_ARTIFACT_PAT*.

Valid until 2025/01/12

## Publish to NuGet

Preview and release versions are published to the NuGet server.

[CNInnovation.Codebreaker.ViewModels](https://www.nuget.org/packages/CNInnovation.Codebreaker.ViewModels/)
