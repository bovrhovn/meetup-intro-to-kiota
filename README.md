# Meetup intro to Kiota

Demo repository about [Microsoft Kiota](https://learn.microsoft.com/en-us/openapi/kiota/overview) for .NET meetup in
Vilnius. Kiota is a command line tool for generating an API client to call any OpenAPI-described API you're interested
in.

<!-- TOC -->

* [Meetup intro to Kiota](#meetup-intro-to-kiota)
    * [Prerequisites](#prerequisites)
    * [Scripts](#scripts)
    * [Code and usage](#code-and-usage)
    * [Flow for the session](#flow-for-the-session)
* [Links](#links)
* [Trademarks](#trademarks)

<!-- TOC -->

![Kiota Design Overview](https://learn.microsoft.com/en-us/openapi/kiota/images/designoverview.png)

The goal is to eliminate the need to take a dependency on a different API client library for every API that you need
to call. Kiota API clients provide a strongly typed experience with all the features you expect from a high quality API
SDK, but without having to learn a new library for every HTTP API.

## Prerequisites

1. [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
   installed - we do recommend an editor like [Visual Studio Code](https://code.visualstudio.com) to be able to write
   scripts and work with code.
2. git installed - instructions step by step [here](https://docs.github.com/en/get-started/quickstart/set-up-git)
3. [.NET](https://dot.net) installed to run the application if you want to run it
4. an editor (besides notepad) to see and work with code, yaml, scripts and more (for
   example [Visual Studio Code](https://code.visualstudio.com) or [Visual Studio](https://visualstudio.microsoft.com/)
   or [Jetbrains Rider](https://jetbrains.com/rider))
5. [OPTIONAL] GitHub CLI installed to work with GitHub - [how to install](https://cli.github.com/manual/installation)
6. [OPTIONAL] [Github GUI App](https://desktop.github.com/) for managing changes and work
   on [forked](https://docs.github.com/en/get-started/quickstart/fork-a-repo) repo
7. [OPTIONAL] [Windows Terminal](https://learn.microsoft.com/en-us/windows/terminal/install)

## Scripts

Scripts are available in [scripts folder](./scripts). The scripts are written
in [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/overview?view=powershell-7.2).

1. [Add-DirToSystemEnv.ps1](./scripts/Add-DirToSystemEnv.ps1) - adds a directory to the system environment variable
   PATH
2. [Compile-Containers.ps1](./scripts/Compile-Containers.ps1) - uses Azure CLI to compile containers with the help of
   Azure Registry Tasks - check also [containers folder](./containers) for dockerfile definition.
3. [Set-EnvVariables.ps1](./scripts/Set-EnvVariables.ps1) - Set environment variables from local.env file

## Code and usage

The demo code is structured in the following way:

1. [Kiota.MinimalApi](./KiotaExamples/Kiota.MinimalApi) - the minimal api application with sample data and OpenAPI
   definitions.
2. [Kiota.ClassicalHttpCall](./KiotaExamples/Kiota.ClassicHttpCall) - project to call minimal api with classical http
   client.
3. [Kiota.ApiCall](./KiotaExamples/Kiota.ApiCall) - project to call OpenAPI to see the data and programmatically show
   them.
4. [Kiota.QuickStart](./KiotaExamples/Kiota.QuickStart) - project to call sample OpenAPI to generate data from provided
   file.

[Docker files](./containers/Kiota-DemoApi) are available to build and run the application in containers. You can also
leverage helper script [Compile-Containers.ps1](./scripts/Compile-Containers.ps1) to build containers
using [Azure Container Registry task builders](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-build-task).

## Flow for the session

Flow for the session is defined [here](./scripts/flow.md).

# Links

- [Kiota homepage](https://learn.microsoft.com/en-us/openapi/kiota/)
- [Kiota GitHub sample repository](https://github.com/microsoft/kiota-samples.git)
- [Apis.json](https://apisjson.org/)
- [Spectre Console](https://github.com/spectresystems/spectre.console/)

# Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft trademarks
or logos is subject to and must
follow [Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks?oneroute=true).
Use of Microsoft trademarks or logos in
modified versions of this project must not cause confusion or imply Microsoft sponsorship. Any use of third-party
trademarks or logos are subject to those third-party's policies.