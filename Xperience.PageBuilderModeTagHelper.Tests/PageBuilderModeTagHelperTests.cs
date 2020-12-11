using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute;
using Xunit;

namespace Xperience.PageBuilderModeTagHelper.Tests
{
    public class PageBuilderModeTagHelperTests
    {
        [Theory]
        [InlineData("Live", "Live")]
        [InlineData("LivePreview", "LivePreview")]
        [InlineData("Edit", "Edit")]
        [InlineData("Live,Edit", "Live")]
        [InlineData("Live,Edit", "Edit")]
        [InlineData("Edit, LivePreview", "LivePreview")]
        public void Process_Will_Render_Child_Content_When_The_Included_Modes_Match_The_Context(
            string modes,
            string currentModeName)
        {
            var context = Substitute.For<IPageBuilderContext>();

            SetContextFromModeName(context, currentModeName);

            var tagHelper = new PageBuilderModeTagHelper(context)
            {
                Include = modes
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-builder-mode",
                new TagHelperAttributeList{ { "include", modes } },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-builder-mode",
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

        [Theory]
        [InlineData("", "Live")]
        [InlineData("", "LivePreview")]
        [InlineData("", "Edit")]
        [InlineData("   ", "Live")]
        public void Process_Will_Render_Child_Content_When_No_Included_Modes_Are_Provided(
            string modes,
            string currentModeName)
        {
            var context = Substitute.For<IPageBuilderContext>();

            SetContextFromModeName(context, currentModeName);

            var tagHelper = new PageBuilderModeTagHelper(context)
            {
                Include = modes
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-builder-mode",
                new TagHelperAttributeList{ { "include", modes } },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-builder-mode",
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

        [Theory]
        [InlineData("Edit", "ABC", "Edit")]
        [InlineData("Live", "DEF", "Live")]
        [InlineData("LivePreview", "1, 3, 4", "LivePreview")]
        [InlineData("Edit,LivePreview", "Live", "LivePreview")]
        public void Process_Will_Render_Child_Content_When_All_Properties_Are_Assigned(
            string include,
            string exclude,
            string currentModeName)
        {
            var context = Substitute.For<IPageBuilderContext>();

            SetContextFromModeName(context, currentModeName);

            var tagHelper = new PageBuilderModeTagHelper(context)
            {
                Include = include,
                Exclude = exclude
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-builder-mode",
                new TagHelperAttributeList{ { "include", include }, { "exclude", exclude } },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-builder-mode",
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

        [Theory]
        [InlineData("Edit", "Edit", "Edit")]
        [InlineData("", "Edit,LivePreview", "Edit")]
        [InlineData("LivePreview,Live", "Edit", "Edit")]
        public void Process_Will_Hide_Child_Content_When_Excluded_Modes_Are_Provided(
            string include,
            string exclude,
            string currentModeName)
        {
            var context = Substitute.For<IPageBuilderContext>();

            SetContextFromModeName(context, currentModeName);

            var tagHelper = new PageBuilderModeTagHelper(context)
            {
                Include = include,
                Exclude = exclude
            };

            var tagHelperContext = new TagHelperContext(
                tagName: "page-builder-mode",
                new TagHelperAttributeList{ { "include", include }, { "exclude", exclude } },
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput("page-builder-mode",
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

        private void SetContextFromModeName(IPageBuilderContext context, string modeName)
        {
            context.ModeName().Returns(modeName);

            if (modeName == "Live")
            {
                context.IsLiveMode.Returns(true);
                context.IsLivePreview.Returns(false);
                context.IsEditMode.Returns(false);
                context.Mode.Returns(PageBuilderMode.Live);

                return;
            }

            if (modeName == "LivePreview")
            {
                context.IsLiveMode.Returns(false);
                context.IsLivePreview.Returns(true);
                context.IsEditMode.Returns(false);
                context.Mode.Returns(PageBuilderMode.LivePreview);

                return;
            }

            if (modeName == "Edit")
            {
                context.IsLiveMode.Returns(false);
                context.IsLivePreview.Returns(false);
                context.IsEditMode.Returns(true);
                context.Mode.Returns(PageBuilderMode.Edit);

                return;
            }
        }
    }
}
