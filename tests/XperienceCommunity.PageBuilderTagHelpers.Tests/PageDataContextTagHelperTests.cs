using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using FluentAssertions;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;
using Xunit;

namespace XperienceCommunity.PageBuilderTagHelpers.Tests
{
    public class PageDataContextTagHelperTests
    {
        [Fact]
        public void Process_Will_Render_No_Content_When_There_Is_No_PageDataContext_And_Initialized_Is_True()
        {
            var retriever = Substitute.For<IPageDataContextRetriever>();

            retriever.TryRetrieve<TreeNode>(out Arg.Any<IPageDataContext<TreeNode>>())
                .Returns(false);

            var tagHelper = new PageDataContextTagHelper(retriever)
            {
                Initialized = true
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-data-context",
                new TagHelperAttributeList { },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-data-context",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(new HtmlString("<h1>Hello</h1>"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            tagHelper.Process(tagHelperContext, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeNull();
            tagHelperOutput.IsContentModified.Should().BeTrue();

            tagHelperOutput.PreElement.GetContent().Should().BeEmpty();
            tagHelperOutput.PreContent.GetContent().Should().BeEmpty();
            tagHelperOutput.Content.GetContent().Should().BeEmpty();
            tagHelperOutput.PostContent.GetContent().Should().BeEmpty();
            tagHelperOutput.PostElement.GetContent().Should().BeEmpty();
        }

        [Fact]
        public void Process_Will_Render_Content_When_There_Is_No_PageDataContext_And_Initialized_Is_False()
        {
            var retriever = Substitute.For<IPageDataContextRetriever>();

            retriever.TryRetrieve<TreeNode>(out Arg.Any<IPageDataContext<TreeNode>>())
                .Returns(false);

            var tagHelper = new PageDataContextTagHelper(retriever)
            {
                Initialized = false
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-data-context",
                new TagHelperAttributeList { },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-data-context",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(new HtmlString("<h1>Hello</h1>"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            tagHelper.Process(tagHelperContext, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeNull();
            tagHelperOutput.IsContentModified.Should().BeFalse();
        }

        [Fact]
        public void Process_Will_Render_No_Content_When_There_Is_PageDataContext_And_Initialized_Is_False()
        {
            var retriever = Substitute.For<IPageDataContextRetriever>();
            var context = Substitute.For<IPageDataContext<TreeNode>>();

            retriever.TryRetrieve<TreeNode>(out Arg.Any<IPageDataContext<TreeNode>>())
                .Returns(x =>
                {
                    x[0] = context;

                    return true;
                });

            var tagHelper = new PageDataContextTagHelper(retriever)
            {
                Initialized = false
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-data-context",
                new TagHelperAttributeList { },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-data-context",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(new HtmlString("<h1>Hello</h1>"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            tagHelper.Process(tagHelperContext, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeNull();
            tagHelperOutput.IsContentModified.Should().BeTrue();

            tagHelperOutput.PreElement.GetContent().Should().BeEmpty();
            tagHelperOutput.PreContent.GetContent().Should().BeEmpty();
            tagHelperOutput.Content.GetContent().Should().BeEmpty();
            tagHelperOutput.PostContent.GetContent().Should().BeEmpty();
            tagHelperOutput.PostElement.GetContent().Should().BeEmpty();
        }

        [Fact]
        public void Process_Will_Render_Content_When_There_Is_PageDataContext_And_Initialized_Is_True()
        {
            var retriever = Substitute.For<IPageDataContextRetriever>();
            var context = Substitute.For<IPageDataContext<TreeNode>>();

            retriever.TryRetrieve<TreeNode>(out Arg.Any<IPageDataContext<TreeNode>>())
                .Returns(x =>
                {
                    x[0] = context;

                    return true;
                });

            var tagHelper = new PageDataContextTagHelper(retriever)
            {
                Initialized = true
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-data-context",
                new TagHelperAttributeList { },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-data-context",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(new HtmlString("<h1>Hello</h1>"));
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            tagHelper.Process(tagHelperContext, tagHelperOutput);

            tagHelperOutput.TagName.Should().BeNull();
            tagHelperOutput.IsContentModified.Should().BeFalse();
        }
    }
}
