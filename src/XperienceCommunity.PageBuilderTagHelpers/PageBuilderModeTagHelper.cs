﻿using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;

namespace XperienceCommunity.PageBuilderUtilities
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;page-builder-mode&gt; elements that conditionally renders
    /// content based on the current value of <see cref="IPageBuilderContext.Mode"/>.
    /// If the current Page Builder 'mode' is not listed in the specified <see cref="Include"/>, 
    /// or if it is in <see cref="Exclude"/>, the content will not be rendered.
    /// </summary>
    /// <remarks>
    /// Sourced from https://github.com/dotnet/aspnetcore/blob/v5.0.1/src/Mvc/Mvc.TagHelpers/src/EnvironmentTagHelper.cs
    /// </remarks>
    [HtmlTargetElement("page-builder-mode")]
    public class PageBuilderModeTagHelper : TagHelper
    {
        private static readonly char[] nameSeparator = new[] { ',' };

        private readonly IPageBuilderContext pageBuilderContext;

        public PageBuilderModeTagHelper(IPageBuilderContext pageBuilderContext) =>
            this.pageBuilderContext = pageBuilderContext ?? throw new ArgumentNullException(nameof(pageBuilderContext));

        /// <inheritdoc />
        public override int Order => -999;

        /// <summary>
        /// A comma separated list of page builder modes (<see cref="PageBuilderMode" />) in which the content should be rendered.
        /// If the current mode is also in the <see cref="Exclude"/> list, the content will not be rendered.
        /// </summary>
        public string Include { get; set; } = "";

        /// <summary>
        /// A comma separated list of page builder modes (<see cref="PageBuilderMode" />) in which the content will not be rendered.
        /// </summary>
        public string Exclude { get; set; } = "";

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            // Always strip the outer tag name as we never want <page-builder-mode> to render
            output.TagName = null;

            if (string.IsNullOrWhiteSpace(Include) && string.IsNullOrWhiteSpace(Exclude))
            {
                // No names specified, do nothing
                return;
            }

            string currentPageBuilderModeName = pageBuilderContext.ModeName();

            if (string.IsNullOrEmpty(currentPageBuilderModeName))
            {
                // No current page builder mode name, do nothing
                return;
            }

            if (Exclude != null)
            {
                var tokenizer = new StringTokenizer(Exclude, nameSeparator);
                foreach (var item in tokenizer)
                {
                    var mode = item.Trim();
                    if (mode.HasValue && mode.Length > 0)
                    {
                        if (mode.Equals(currentPageBuilderModeName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Matching page builder mode name found, suppress output
                            output.SuppressOutput();
                            return;
                        }
                    }
                }
            }

            bool hasModes = false;

            if (Include != null)
            {
                var tokenizer = new StringTokenizer(Include, nameSeparator);
                foreach (var item in tokenizer)
                {
                    var mode = item.Trim();
                    if (mode.HasValue && mode.Length > 0)
                    {
                        hasModes = true;
                        if (mode.Equals(currentPageBuilderModeName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Matching page builder mode name found, do nothing
                            return;
                        }
                    }
                }
            }

            if (hasModes)
            {
                // This instance had at least one non-empty mode (include) specified but none of these
                // modes matched the current mode. Suppress the output in this case.
                output.SuppressOutput();
            }
        }
    }
}
