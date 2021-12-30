using System;
using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace XperienceCommunity.PageBuilderTagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;page-data-context&gt; elements that conditionally renders
    /// content based on the existance of the <see cref="IPageDataContext{TreeNode}" /> retrieved from 
    /// <see cref="IPageDataContextRetriever"/> and the value of the <see cref="Initialized" /> attribute of the Tag Helper.
    /// </summary>
    [HtmlTargetElement("page-data-context")]
    public class PageDataContextTagHelper : TagHelper
    {
        private readonly IPageDataContextRetriever retriever;

        public PageDataContextTagHelper(IPageDataContextRetriever retriever)
        {
            this.retriever = retriever ?? throw new ArgumentNullException(nameof(retriever));
        }

        /// <inheritdoc />
        public override int Order => 0;

        /// <summary>
        /// Determines whether or not the inner content of the Tag Helper is displayed.
        /// If the <see cref="IPageDataContext{TreeNode}" /> is populated for the current request and <see cref="Initialized" />
        /// is true or the <see cref="IPageDataContext{TreeNode}" /> is not populated and <see cref="Initialized" /> is false, the
        /// content will be displayed. Otherwise the content will not be displayed.
        /// </summary>
        public bool Initialized { get; set; } = true;

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

            // Always strip the outer tag name as we never want <page-data-context> to render
            output.TagName = null;

            if (retriever.TryRetrieve<TreeNode>(out var data))
            {
                if (data is object && Initialized)
                {
                    return;
                }

                output.SuppressOutput();
            }

            if (!Initialized)
            {
                return;
            }

            output.SuppressOutput();
        }
    }
}
