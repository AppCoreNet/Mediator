AppCore .NET Mediator
-----------------

![Nuget](https://img.shields.io/nuget/v/AppCoreNet.Mediator.Abstractions)
![MyGet](https://img.shields.io/myget/appcorenet/vpre/AppCoreNet.Mediator.Abstractions?label=myget)

This repository includes projects containing abstractions and implementations of the mediator framework.

All artifacts are licensed under the [MIT license](LICENSE). You are free to use them in open-source or commercial projects as long
as you keep the copyright notice intact when redistributing or otherwise reusing our artifacts.

## Packages

Latest development packages can be found on [MyGet](https://www.myget.org/gallery/appcorenet).

| Package                                            | Description                                          |
|----------------------------------------------------|------------------------------------------------------|
| `AppCoreNet.Mediator`                              | Provides mediator framework default implementations. |
| `AppCoreNet.Mediator.Abstractions`                 | Provides the public API of the mediator framework.   |
| `AppCoreNet.Mediator.Authentication`               | Adds support for request authentication.             |
| `AppCoreNet.Mediator.Authentication.Abstractions`  | Provides the public API for request authentication.  |

## Usage

This section explains how to use the AppCore .NET Mediator framework in your project.

### Installation / Getting Started

To get started with AppCore .NET Mediator, you'll need to install the necessary NuGet packages. The primary package to get started is `AppCoreNet.Mediator`. If you are building abstractions or extensions that only depend on the mediator's contracts, you might only need `AppCoreNet.Mediator.Abstractions`.

**Using .NET CLI:**
```bash
dotnet add package AppCoreNet.Mediator
```
or for just the abstractions:
```bash
dotnet add package AppCoreNet.Mediator.Abstractions
```

**Using Package Manager Console (Visual Studio):**
```powershell
Install-Package AppCoreNet.Mediator
```
or for just the abstractions:
```powershell
Install-Package AppCoreNet.Mediator.Abstractions
```

### Registration

To use the mediator, you first need to register it in your application's service container. You can do this using the `AddMediator()` extension method on `IServiceCollection`.

**Example:**

```csharp
using Microsoft.Extensions.DependencyInjection;
using AppCoreNet.Extensions.DependencyInjection; // Required for AddMediator()

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ... other service registrations
        services.AddMediator(builder =>
        {
            // Optional: Configure mediator pipelines, handlers, etc.
            // Example: Register all handlers from an assembly
            builder.AddHandlersFrom(typeof(Startup).Assembly);

            // Example: Register all pre-request handlers
            // builder.AddPreRequestHandlersFrom(typeof(Startup).Assembly);
            
            // Example: Register all post-request handlers
            // builder.AddPostRequestHandlersFrom(typeof(Startup).Assembly);

            // Example: Register all notification handlers
            // builder.AddNotificationHandlersFrom(typeof(Startup).Assembly);
        });
        // ...
    }
}
```

### Commands (Requests)

Commands (or Requests) are used to perform actions and typically have a single handler.

#### Defining a Command

A command is a class that implements the `IRequest<TResponse>` interface, where `TResponse` is the type of the response the command will return. If a command does not return a value, you can use `VoidResponse`.

**Example:**

```csharp
using AppCoreNet.Mediator;

// Command that returns a string response
public class MyCommand : IRequest<string>
{
    public string InputData { get; }

    public MyCommand(string inputData)
    {
        InputData = inputData;
    }
}

// Command that does not return a value
public class MyParameterlessCommand : IRequest<VoidResponse>
{
}
```

#### Defining a Command Handler

A command handler is a class that implements the `IRequestHandler<TRequest, TResponse>` interface.

**Example:**

```csharp
using AppCoreNet.Mediator;
using System.Threading;
using System.Threading.Tasks;

public class MyCommandHandler : IRequestHandler<MyCommand, string>
{
    public Task<string> HandleAsync(MyCommand request, CancellationToken cancellationToken)
    {
        // Process the command
        string result = $"Processed: {request.InputData}";
        return Task.FromResult(result);
    }
}

public class MyParameterlessCommandHandler : IRequestHandler<MyParameterlessCommand, VoidResponse>
{
    public Task<VoidResponse> HandleAsync(MyParameterlessCommand request, CancellationToken cancellationToken)
    {
        // Process the command
        System.Console.WriteLine("MyParameterlessCommand handled");
        return VoidResponse.Task;
    }
}
```
*Note: Command handlers are automatically discovered and registered if they are in an assembly registered via `builder.AddHandlersFrom(...)` during mediator setup. Otherwise, they need to be registered manually in the DI container (e.g., `services.AddTransient<IRequestHandler<MyCommand, string>, MyCommandHandler>();`).*

#### Sending a Command

To send a command, inject `IMediator` and use the `ProcessAsync` method.

**Example:**

```csharp
using AppCoreNet.Mediator;
using System.Threading.Tasks;

public class MyService
{
    private readonly IMediator _mediator;

    public MyService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DoSomethingAsync(string data)
    {
        var command = new MyCommand(data);
        string response = await _mediator.ProcessAsync(command);
        // Use the response
        System.Console.WriteLine(response);

        // Send a command with no response
        await _mediator.ProcessAsync(new MyParameterlessCommand());
    }
}
```

### Notifications

Notifications are used to inform other parts of the application about an event that has occurred. A notification can have multiple handlers.

#### Defining a Notification

A notification is a class that implements the `INotification` interface.

**Example:**

```csharp
using AppCoreNet.Mediator;

public class MyNotification : INotification
{
    public string Message { get; }

    public MyNotification(string message)
    {
        Message = message;
    }
}
```

#### Defining a Notification Handler

A notification handler is a class that implements the `INotificationHandler<TNotification>` interface.

**Example:**

```csharp
using AppCoreNet.Mediator;
using System.Threading;
using System.Threading.Tasks;

public class MyNotificationHandler1 : INotificationHandler<MyNotification>
{
    public Task HandleAsync(MyNotification notification, CancellationToken cancellationToken)
    {
        // Handle the notification
        System.Console.WriteLine($"Handler 1 received: {notification.Message}");
        return Task.CompletedTask;
    }
}

public class MyNotificationHandler2 : INotificationHandler<MyNotification>
{
    public Task HandleAsync(MyNotification notification, CancellationToken cancellationToken)
    {
        // Handle the notification
        System.Console.WriteLine($"Handler 2 received: {notification.Message}");
        return Task.CompletedTask;
    }
}
```
*Note: Notification handlers are automatically discovered and registered if they are in an assembly registered via `builder.AddNotificationHandlersFrom(...)` during mediator setup. Otherwise, they need to be registered manually in the DI container (e.g., `services.AddTransient<INotificationHandler<MyNotification>, MyNotificationHandler1>();`).*

#### Publishing a Notification

To publish a notification, inject `IMediator` and use the `PublishAsync` method.

**Example:**

```csharp
using AppCoreNet.Mediator;
using System.Threading.Tasks;

public class MyOtherService
{
    private readonly IMediator _mediator;

    public MyOtherService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task NotifySomethingAsync(string eventMessage)
    {
        var notification = new MyNotification(eventMessage);
        await _mediator.PublishAsync(notification);
    }
}
```

## Contributing

Contributions, whether you file an issue, fix some bug or implement a new feature, are highly appreciated. The whole user community
will benefit from them.

Please refer to the [Contribution guide](CONTRIBUTING.md).
