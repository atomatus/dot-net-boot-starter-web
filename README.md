# Atomatus BootStarter Web 🌐

[![GitHub issues](https://img.shields.io/github/issues/atomatus/dot-net-boot-starter-web?style=flat-square&color=%232EA043&label=help%20wanted)](https://github.com/atomatus/dot-net-boot-starter-web)

[![NuGet version (Com.Atomatus.BootStarter)](https://img.shields.io/nuget/v/Com.Atomatus.BootStarter.Web.svg?style=flat-square)](https://www.nuget.org/packages/Com.Atomatus.BootStarter.Web/)

# Bootstarter Web API Project

The ***Bootstarter Web API*** project is a versatile framework for building robust and efficient RESTful web APIs in C# using ASP.NET Core. It provides a set of powerful tools and abstractions that simplify common development tasks, allowing developers to focus on building and maintaining their API endpoints.

## Key Features and Advantages

### 1. ControllerCrudBase and ControllerCrudBaseAsync

The `ControllerCrudBase` and `ControllerCrudBaseAsync` classes are the core components for creating CRUD (Create, Read, Update, Delete) operations in your API. These classes offer the following benefits:

- Simplified API endpoints for CRUD operations.
- Built-in support for handling database interactions.
- Extensible to accommodate custom business logic.
- Enforces good design practices for separation of concerns.

#### Example Usage:

```csharp
// Create a controller for a specific data model
public class MyDataController : ControllerCrudBase<MyDataService, MyDataModel>
{
    public MyDataController(MyDataService service) : base(service) { }
}
```
### 1.1 ControllerCrud - Generic Framework for CRUD Controllers
The `ControllerCrud` class is an abstract class that acts as a base controller for `CRUD` operations on model entities in an ASP.NET Core application. It contains methods that correspond to `CRUD` operations and provides a generic framework for creating specific controllers for model entity types. The class is highly configurable and allows you to easily create specific controllers for your model entities.

#### Class Usage
To use the `ControllerCrud` class, follow these steps:

- **Create a Derived Controller:** Create a controller class derived from `ControllerCrud` and specify the type of entity model (`TModel`) you want to manipulate. For example:

     ```csharp
     public class UserController : ControllerCrud<UserModel>
     {
         // Implement custom action methods here if needed.
     }
     ```
- **Implement Custom Action Methods:** If needed, you can implement custom action methods in your derived controller to add specific functionality to your application.

- **Dependency Injection:** You can inject a service instance that implements the IServiceCrud<TModel> interface into your controller's constructor. This allows the controller to access service methods to perform CRUD operations on the entity model. For example:

     ```csharp
     public interface IUserModelService : IServiceCrud<UserModel>
     {
         // services...
     }
     ```

     ```csharp
     public UserController(IUserModelService userService)
         : base(userService)
     {
         // The service is injected and can be used for CRUD operations.
     }
     ```
- **Routes and Attributes:** The action methods in the derived class are decorated with attributes that specify the routes for standard `CRUD` operations (`Create`, `Read`, `Update` and `Delete`). For example, the Create method is decorated with `[HttpPost]` and the Get method is decorated with `[HttpGet]`.

- **Customization:** You can further customize attributes such as naming routes, setting constraints, etc. as needed to meet your application requirements.

- **Access to CRUD Operations:** You can access CRUD operations through the action methods inherited from the `ControllerCrud` class. For example, the Create method can be used to create a new model entity.

```csharp
[HttpPost]
public IActionResult Create([FromBody] UserModel userModel)
{
     // Calls the create service operation to add a new model.
     var result = CreateAction(userModel);
     return result;
}
```
- **Routing and URLs:** Standard `CRUD` operations are accessed via URLs such as `/api/controllerName` for creation (`POST`), `/api/controllerName/id` for reading (` GET`), `/api/controllerName/id` for update (`PUT`), and `/api/controllerName/id` for delete (`DELETE`), where `controllerName` is the name of your derived controller.

This is a high-level overview of how you can use the ControllerCrud class to create CRUD controllers in an ASP.NET Core application. You can create controllers for different entity models and customize CRUD operations as needed to meet your application requirements.
### 2. ControllerMapperBase
The `ControllerMapperBase` class simplifies the conversion of DTO objects to and from model objects. It automates the process of copying property values between objects and is particularly useful for handling data transformations.


#### Example Usage:

```csharp
// Inherit from ControllerMapperBase for your data model and service
public class MyDataController : ControllerMapperBase<MyDataService, MyDataModel>
{
    // ...
}
```

### 3. IDocumentPatcher
The IDocumentPatcher interface enables partial updates for objects by copying shared properties based on name and type between the source and target objects. It is commonly used for handling PATCH requests in RESTful APIs.

#### Example Usage:

```csharp
// Implement the IDocumentPatcher interface in your DTO or model classes
public class MyDataDto : IDocumentPatcher
{
    // ...
}
```

```csharp
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Com.Atomatus.Bootstarter.Web
{
    // ... (Other code above)

    /// <summary>
    /// Example usage of IDocumentPatcher and IDocumentPatcherExtensions.
    /// </summary>
    public class IDocumentPatcherExample
    {
        public void ApplyPatchExample()
        {
            // Create an instance of your source object (DTO or model)
            var source = new MyDataDto
            {
                Id = 1,
                Name = "Updated Name",
                Description = null, // Null value should not be copied
                OtherProperty = "Some other value",
            };

            // Create an instance of your target object (DTO or model)
            var target = new MyDataDto
            {
                Id = 1,
                Name = "Original Name",
                Description = "Original Description",
                OtherProperty = "Original Other Property",
            };

            // Apply the patch using the ApplyTo extension method
            source.ApplyTo(target);

            // Now, target should have updated values where source is not null
            Console.WriteLine($"Updated Name: {target.Name}");
            Console.WriteLine($"Description: {target.Description}"); // Should still be the original value
            Console.WriteLine($"Other Property: {target.OtherProperty}"); // Should still be the original value
        }
    }
}
```

### 4. AddSwagger and UseSwagger
The AddSwagger and UseSwagger extension methods simplify the integration of Swagger API documentation into your ASP.NET Core application. They allow you to generate interactive API documentation with ease, making it easier for developers to understand and test your APIs.

#### Example Usage:

```csharp
// ConfigureServices method in Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Add Swagger documentation
    services.AddSwagger();

    // ...
}

// Configure method in Startup.cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Use Swagger middleware
    app.UseSwagger(env);

    // ...
}

```

### 5. AddVersioning
The AddVersioning extension method adds API versioning support to your ASP.NET Core application. This allows you to manage different versions of your API endpoints, ensuring backward compatibility and a smooth API evolution process.

#### Example Usage:

```csharp
// ConfigureServices method in Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Add API versioning
    services.AddVersioning();

    // ...
}
```
### Getting Started
To get started with the Bootstarter Web API project, follow these steps:

- **Install Package using NuGet:**

     - **Open Visual Studio:** Start Visual Studio and open your existing project or create a new project depending on your needs.

     - **Access NuGet Package Manager:**

         - **For a specific project:** Right-click the project in Solution Explorer and choose "Manage NuGet Packages".

         - **For entire solution:** If you want to install the package in all projects in the solution, right-click the solution in Solution Explorer and choose "Manage NuGet Packages for Solution".

     - **Search for Package `Atomatus.Bootstarter.Web`:** In the NuGet Package Manager, go to the "Browse" tab and type "Atomatus.Bootstarter.Web" in the search box.

     - **Install Package:** Click the "Install" button to start the installation process.

     - **Accept Terms of Use (if required):** NuGet may ask you to accept the terms of use for the package. Make sure you read and accept the terms if necessary.

     - **Verify Installation:** After successful installation, you will see output messages indicating that the package was installed successfully. You can also check the "Dependencies" folder in Solution Explorer to confirm that the package has been added to the project.

- **Customize the provided classes** such as ControllerCrudBase, ControllerCrudBaseAsync, and ControllerMapperBase to match your data models and business logic.

- **Implement your API endpoints** and actions using the classes provided by the Bootstarter framework.

- **Configure Swagger** documentation and API versioning in your Startup.cs file using the provided extension methods.

- **Build and run your ASP.NET Core** application to start using your Web API with all the advantages of the Bootstarter framework.

### Conclusion

Overall, the project aimed to enhance developer productivity by providing a set of well-structured and reusable components for building ASP.NET Core web applications. These tools and extensions help developers follow best practices, reduce boilerplate code, and create more maintainable and scalable APIs. Whether you're building a small-scale web service or a large-scale RESTful API, these resources can significantly streamline your development process.

----
© Atomatus - All Rights Reserveds.