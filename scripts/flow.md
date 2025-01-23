## Flow for the session

1. Introduction to the session about Microsoft Kiota (1 minute)
   - Briefly introduce the session and what will be covered.
   - Mention that the session will include a demonstration of how to use Microsoft Kiota to generate API clients from OpenAPI descriptions.

2. OpenAPI overview (8-10 mins):
   - Demonstrating how to add support in the app for OpenAPI (Kiota.MinimalApi)
   - demonstrating how to read "classical" way of calling HTTP client API (Kiota.ClassicalHttpCall) 
   - demonstrating how to read OpenAPI way of calling HTTP client API via stream (Kiota.OpenApiCall)

3. Microsoft Kiota Overview (15 mins):
   - Explaining how Kiota works and differences from other API client generators 
   - Command line tool demonstration (calls below)
   - Demonstrating how to generate clients data code (Kiota.QuickStart, Kiota.SimpleApiCall)
   - Demonstrating how to use the generated clients
     - Authentication Example (Kiota.AuthenticationExample)
     - using automation, serialization and reflection with Kiota (Kiota.ApiCall)

4. Q & A (4- 5 mins)

## How Kiota Works

![Kiota Design Overview](https://learn.microsoft.com/en-us/openapi/kiota/images/designoverview.png)

1. **OpenAPI Descriptions**: Kiota uses OpenAPI descriptions to generate API clients. OpenAPI is a standard format for defining APIs, which ensures that Kiota can work with any API that provides an OpenAPI description.
2. **Command Line Tool**: Kiota is operated through the command line, making it a versatile tool for developers who prefer scripting and automation.
3. **Strongly Typed Clients**: The generated clients are strongly typed, meaning they provide a robust and type-safe way to interact with APIs. This helps catch errors at compile time rather than at runtime.
4. **Language Support**: Kiota supports multiple programming languages, including C#, Go, Java, PHP, Python, Ruby, and TypeScript.
5. **Minimal Dependencies**: Kiota minimizes external dependencies, making the generated clients lightweight and easy to integrate into existing projects.

## Differences from Other API Client Generators

1. **Unified Approach**: Unlike some other tools that require different SDKs for different APIs, Kiota aims to provide a unified approach. You don't need to learn a new library for each API; Kiota generates a consistent client interface for any OpenAPI-described API.
2. **Customization**: Kiota allows for customization of the generated code, enabling developers to tailor the client to their specific needs.
3. **IDE Integration**: The generated clients support IDE features like autocomplete, which aids in API resource discovery and improves the developer experience.
4. **OpenAPI and JSON Schema**: Kiota builds on the Microsoft.OpenAPI.NET library to ensure comprehensive support for OpenAPI and JSON Schema features.
5. **Focus on OpenAPI**: While other tools like Postman or Insomnia are more focused on testing and interacting with APIs, Kiota is specifically designed for generating client code from OpenAPI descriptions.

## Command line demonstration

Api usage is defined [here](https://learn.microsoft.com/en-us/openapi/kiota/using).

``` powershell
# search api on Github (show index and explanation)
kiota search github
kiota search Kiota

# showcase languages supported to used them in app 
kiota info

# show what you need from dependencies to use the generated code
kiota info -l CSharp

# search api
kiota show -d https://localhost:5010/openapi/v1.json -k "categories"

# generate code for c#
Set-Location c:\Work\
New-Item -ItemType Directory -Path ./FolderForGeneratedCode
kiota generate -l CSharp -c SampleApiClient -n SampleApiClient.RestApiCalls -d https://localhost:5010/openapi/v1.json -o ./FolderForGeneratedCode

```

