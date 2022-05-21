# Xperience Page Builder Utilities

## Packages

- [![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.PageBuilderUtilities.svg)](https://www.nuget.org/packages/XperienceCommunity.PageBuilderUtilities)

- [![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.PageBuilderTagHelpers.svg)](https://www.nuget.org/packages/XperienceCommunity.PageBuilderTagHelpers)

## Dependencies

These libraries are compatible with ASP.NET Core 3.1 -> ASP.NET Core 6 and are designed to be used with the Xperience 13.0 Content Delivery (MVC) application [running on ASP.NET Core](https://docs.xperience.io/x/BQ2RBg).

## Page Builder Utilities

This library provides an abstraction over the Kentico Xperience Page Builder [rendering mode](https://docs.xperience.io/x/QA2RBg#Creatingpageswitheditableareas-Checkingforrenderingcontext) so that developers can conditionally execute code based on the mode of a given HTTP request to their ASP.NET Core application.

### How to Use?

1. First, install the NuGet package in your ASP.NET Core project:

   ```bash
   dotnet add package XperienceCommunity.PageBuilderUtilities
   ```

1. Add the required types to the DI container in your project's `Startup.cs` file

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddPageBuilderContext();
   }
   ```

1. You can now use the `IPageBuilderContext` interface (available in the `XperienceCommunity.PageBuilderUtilities` namespace) as a constructor dependency anywhere in your application to more easily determine the state of the current request:

   ```csharp
   public class ProductsController
   {
       private readonly IPageBuilderContext context;

       public ProductsController(IPageBuilderContext context) =>
           this.context = context;

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

1. By not using `IHttpContextAccessor` and all the Kentico Xperience extension methods, your code is both easier to unit test and read.

1. You can inject this type into your Razor views, but the better option is to use ... 👇

## Page Builder Tag Helpers

This library provides an [ASP.NET Core Tag Helper](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1) for [Kentico Xperience 13.0](https://docs.xperience.io/x/bYHaBg)
to help toggle HTML in Razor views based on the Page Builder 'mode' of the request to the ASP.NET Core application.

### How to Use?

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

   - _Note_: This extension method comes from the `XperienceCommunity.PageBuilderUtilities` package, above, which is a dependency of `XperienceCommunity.PageBuilderTagHelpers`

1. Include the [tag helper assembly name](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-5.0#addtaghelper-makes-tag-helpers-available) in the `~/Views/_ViewImports.cshtml`

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

   <page-data-context>
     <!-- or <page-data-context initialized="true"> -->
     <!-- will be displayed only if the IPageDataContext is popualted -->
     <widget-zone />
   </page-data-context>

   <page-data-context initialized="false">
     <!-- will be displayed only if the IPageDataContext is not populated -->
     <div>widget placeholder!</div>
   </page-data-context>
   ```

## Contributing

To build this project, you must have v6.0.300 or higher
of the [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed.

If you've found a bug or have a feature request, please [open an issue](https://github.com/wiredviews/xperience-page-builder-utilities/issues/new) on GitHub.

If you'd like to make a contribution, you can create a [PR on GitHub](https://github.com/wiredviews/xperience-page-builder-utilities/compare).

## References

### Real World Examples

- [Kentico Xperience: MVC Widget Experiments Part 2 - Page Specific Marketing Tags with Widgets](https://dev.to/seangwright/kentico-xperience-mvc-widget-experiments-part-2-page-specific-marketing-tags-with-widgets-1j69)
- [Kentico Xperience: MVC Widget Experiments Part 3 - Unused Widgets Section](https://dev.to/seangwright/kentico-xperience-mvc-widget-experiments-part-3-unused-widgets-section-323j)

### ASP.NET Core

- [Using Tag Helpers](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-6.0)
- [Authoring Tag Helpers](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-6.0)
- [ASP.NET Core - Environment Tag Helper (source)](https://github.com/dotnet/aspnetcore/blob/v5.0.1/src/Mvc/Mvc.TagHelpers/src/EnvironmentTagHelper.cs)

### Kentico Xperience

- [Using Xperience 13.0 Built-in Tag Helpers](https://docs.xperience.io/x/bYHaBg)
