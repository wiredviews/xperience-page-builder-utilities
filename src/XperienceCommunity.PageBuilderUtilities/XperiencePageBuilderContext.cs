using System;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace XperienceCommunity.PageBuilderUtilities
{
    public class XperiencePageBuilderContext : IPageBuilderContext
    {
        private readonly IHttpContextAccessor accessor;

        public XperiencePageBuilderContext(IHttpContextAccessor accessor) =>
            this.accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));

        /// <inheritdoc />
        public bool IsPreviewMode => accessor.HttpContext?.Kentico()?.Preview()?.Enabled ?? false;

        /// <inheritdoc />
        public bool IsLivePreviewMode => IsPreviewMode && !IsEditMode;

        /// <inheritdoc />
        public bool IsEditMode => accessor.HttpContext?.Kentico()?.PageBuilder()?.EditMode ?? false;

        /// <inheritdoc />
        public bool IsLiveMode => !IsPreviewMode;

        /// <inheritdoc />
        public PageBuilderMode Mode =>
            IsLiveMode
                ? PageBuilderMode.Live
                : IsLivePreviewMode
                    ? PageBuilderMode.LivePreview
                    : PageBuilderMode.Edit;

        /// <inheritdoc />
        public string ModeName() => Enum.GetName(typeof(PageBuilderMode), Mode) ?? "";
    }
}
