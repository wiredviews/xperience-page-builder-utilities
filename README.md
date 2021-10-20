# Xperience Page Builder Tag Helpers

[![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.PageBuilderUtilities.svg)](https://www.nuget.org/packages/XperienceCommunity.PageBuilderUtilities)
[![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.PageBuilderTagHelpers.svg)](https://www.nuget.org/packages/XperienceCommunity.PageBuilderTagHelpers)

This library provides an [ASP.NET Core Tag Helper](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1) for [Kentico Xperience 13.0](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/reference-xperience-tag-helpers) to help toggle HTML in Razor views based on the Page Builder 'mode' of the request to the ASP.NET Core application.

## Dependencies

This package is compatible with ASP.NET Core 3.1 -> ASP.NET Core 5 and is designed to be used with the Xperience 13.0 Content Delivery (MVC) application [running on ASP.NET Core](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core).

## How to Use?

1. First, install the NuGet package in your ASP.NET Core project

    ```bash
    dotnet add package XperienceCommunity.PageBuilderTagHelpers
    ```

1. Add the required types to the DI container in your `Startup.cs` file

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddPageBuilderContext();
    }
    ```

    - _Note_: This extension method comes from the `XperienceCommunity.PageBuilderUtilities` package which is a dependency of `XperienceCommunity.PageBuilderTagHelpers`

1. Include the tag builder namespace in the `~/Views/_ViewImports.cshtml`

    ```html
    @addTagHelper *, XperienceCommunity.PageBuilderTagHelpers
    ```

1. Use the tag helper in your Razor views

    ```html
    <page-builder-mode exclude="Live">
        <!-- will be displayed in Edit and LivePreview modes -->
        <h1>Hello!</h1>
    </page-builder-mode>

    <page-builder-mode include="LivePreview, Edit">
        <!-- will be displayed in Edit and LivePreview modes -->
        <h1>Hello!</h1>
    </page-builder-mode>
    ```

1. You can also use the `IPageBuilderContext` from `XperienceCommunity.PageBuilderUtilities` as a constructor dependency elsewhere in your application to more easily determine the state of the current request, without having to use `IHttpContextAccessor` and all the Kentico Xperience extension methods. This is both easier to mock in a unit test and easier to read in your code:

    ```csharp
    public class ProductsController
    {
        private readonly IPageBuilderContext context;

        public ProductsController(IPageBuilderContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ActionResult Index()
        {
            if (context.IsEditMode)
            {
                // ...
            }
            
            if (context.IsLivePreviewMode)
            {
                // ...
            }
            
            if (context.IsLiveMode)
            {
                // ...
            }

            if (context.IsPreviewMode)
            {
                // ...
            }
        }
    }
    ```

    - _Note_: If you only want to use the `IPageBuilderContext`, you can install its package directly:

        ```bash
        dotnet add package XperienceCommunity.PageBuilderUtilities
        ```

## References

### ASP.NET Core

- [Using Tag Helpers](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1)
- [Authoring Tag Helpers](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-5.0)
- [ASP.NET Core - Environment Tag Helper (source)](https://github.com/dotnet/aspnetcore/blob/v5.0.1/src/Mvc/Mvc.TagHelpers/src/EnvironmentTagHelper.cs)

### Kentico Xperience

- [Using Xperience 13.0 Built-in Tag Helpers](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/reference-xperience-tag-helpers)
