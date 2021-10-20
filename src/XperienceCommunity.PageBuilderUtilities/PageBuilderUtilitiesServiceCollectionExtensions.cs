using XperienceCommunity.PageBuilderUtilities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PageBuilderContextServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Page Builder Context, which can be accessed by <see cref="IPageBuilderContext" />
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPageBuilderContext(this IServiceCollection services) =>
            services.AddSingleton<IPageBuilderContext, XperiencePageBuilderContext>();
    }
}
