using XperienceCommunity.PageBuilderUtilities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PageBuilderContextServiceCollectionExtensions
    {
        public static IServiceCollection AddPageBuilderContext(this IServiceCollection services) =>
            services.AddSingleton<IPageBuilderContext, XperiencePageBuilderContext>();
    }
}
