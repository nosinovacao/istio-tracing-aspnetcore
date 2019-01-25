# Istio.Tracing.Progagation
[![Travis Build Status](https://travis-ci.org/nosinovacao/istio-tracing-aspnetcore.svg?branch=master)](https://travis-ci.org/nosinovacao/istio-tracing-aspnetcore)
[![Coverage Status](https://codecov.io/gh/nosinovacao/istio-tracing-aspnetcore/branch/master/graph/badge.svg)](https://codecov.io/gh/nosinovacao/istio-tracing-aspnetcore/branch/master/)

This package turns an Asp.Net Core service into a istio-enabled application by propagating the [ISTIO tracing headers](https://istio.io/docs/tasks/telemetry/distributed-tracing/#understanding-what-happened) from the incoming Asp.Net Core requests to the HttpClient outgoing requests using the HttpClient factory. 

At @nosinovacao we believe that developers shouldn't have to worry about cross-concerns, that's why this package only needs 2 lines of code to enable.

## Installing on ASP.NET Core
1. Install the NuGet package

```shell
    dotnet add package Istio.Tracing.Propagation.AspNetCore
```

2. Add all the services and middlewares to the `IWebHostBuilder` in `Program.cs`:
```csharp
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    // The following line enables Istio.Tracing.Propagation
    .PropagateIstioHeaders();
```

## Usage with HttpClientFactory
If you are using the HttpClientFactory extensions to create your HttpClient's you are ready to go and you can see your applications tracing and telemtry on ISTIO jaeger! 😊

### Usage without HttpClientFactory
Since you are not using the  `HttpClientFactory` integration you need to explicitely add the `CorrelationIdHeaderDelegatingHandler` to your HttpClients:

```csharp
public class SomeClass 
{
    public SomeClass(string connectionString, CorrelationIdHeaderDelegatingHandler correlationDelegatingHandler) {
        // We need to set the InnerHandler to the default HttpClient one.
        correlationDelegatingHandler.InnerHandler = new HttpClientHandler();
        // This ensures all outgoing requests will contain the Correlation Id header
        var client = new HttpClient(correlationDelegatingHandler);
    }
}
```

## Building and testing

To build and run unit tests execute the commands in the root of repository:
    
    dotnet build -c Release
    dotnet test -c Release

## Contributing
We really appreciate your interest in contributing for this project. 👍

All we ask is that you follow some simple guidelines, so please read the [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests.

Thank you, [contributors](https://github.com/nosinovacao/istio-tracing-aspnetcore/graphs/contributors)!

## License
Copyright © NOS Inovação.

This project is licensed under the BSD 3-Clause License - see the [LICENSE](LICENSE) file for details