# Xperience Page Builder Mode Tag Helper

[![NuGet Package](https://img.shields.io/nuget/v/WiredViews.Xperience.PageBuilderModeTagHelper.svg)](https://www.nuget.org/packages/WiredViews.Xperience.PageBuilderModeTagHelper)

This library provides an [ASP.NET Core Tag Helper](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1) for [Kentico Xperience 13.0](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/reference-xperience-tag-helpers) that adds to the existing set.

## Dependencies

This package is compatible with ASP.NET Core 3.1 -> ASP.NET Core 5 and is designed to be used with the Xperience 13.0 Content Delivery (MVC) application [running on ASP.NET Core](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core).

## How to Use?

1. First, install the NuGet package in your ASP.NET Core project

    ```bash
    dotnet add package WiredViews.Xperience.PageBuilderModeTagHelper
    ```

1. Add the required types to the DI container in your `Startup.cs` file

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPageBuilderContext, XperiencePageBuilderContext>)();
    }
    ```

1. Include the tag builder namespace in the `~/Views/_ViewImports.cshtml`

    ```html
    @addTagHelper *, XperienceCommunity.PageBuilderModeTagHelper
    ```

1. Use the tag helper in your Razor views

    ```html
    <page-builder-mode modes="Live">
        <!-- will only be displayed on the live site -->
        <h1>Hello!</h1>
    </page-builder-mode>

    <page-builder-mode exclude="Live">
        <!-- will be displayed in Edit and LivePreview modes -->
        <h1>Hello!</h1>
    </page-builder-mode>

    <page-builder-mode include="LivePreview, Edit">
        <!-- will be displayed in Edit and LivePreview modes -->
        <h1>Hello!</h1>
    </page-builder-mode>
    ```

1. You can also use the `IPageBuilderContext` as a constructor dependency elsewhere in your application to more easily determine the state of the current request, without having to use `IHttpContextAccessor` and all the Kentico Xperience extension methods:

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

## References

### ASP.NET Core

- [Using Tag Helpers](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1)
- [Authoring Tag Helpers](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-5.0)
- [ASP.NET Core - Environment Tag Helper (source)](https://github.com/dotnet/aspnetcore/blob/v5.0.1/src/Mvc/Mvc.TagHelpers/src/EnvironmentTagHelper.cs)

### Kentico Xperience

- [Using Xperience 13.0 Built-in Tag Helpers](https://docs.xperience.io/developing-websites/developing-xperience-applications-using-asp-net-core/reference-xperience-tag-helpers)
