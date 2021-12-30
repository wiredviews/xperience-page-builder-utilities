using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Base;
using CMS.DocumentEngine;
using FluentAssertions;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using XperienceCommunity.PageBuilderUtilities;
using Xunit;

namespace XperienceCommunity.PageBuilderTagHelpers.Tests
{
    public class XperiencePageBuilderContextTests
    {
        [Fact]
        public void IsPreviewMode_Will_Be_False_If_There_Is_No_HttpContext()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();

            accessor.HttpContext.ReturnsNull();

            var context = new XperiencePageBuilderContext(accessor);

            context.IsPreviewMode.Should().BeFalse();
        }

        [Fact]
        public void IsPreviewMode_Will_Be_False_If_There_Is_No_KenticoFeatures_FeatureSet()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", null }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsPreviewMode.Should().BeFalse();
        }

        [Fact]
        public void IsPreviewMode_Will_Be_False_If_There_Is_No_PreviewFeature_FeatureSet()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPreviewFeature>().ReturnsNull();

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsPreviewMode.Should().BeFalse();
        }

        [Fact]
        public void IsPreviewMode_Will_Be_False_If_PreviewFeature_Is_Disabled()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var previewFeature = Substitute.For<IPreviewFeature>();
            previewFeature.Enabled.Returns(false);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPreviewFeature>().Returns(previewFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsPreviewMode.Should().BeFalse();
        }

        [Fact]
        public void IsPreviewMode_Will_Be_True_If_PreviewFeature_Is_Enabled()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var previewFeature = Substitute.For<IPreviewFeature>();
            previewFeature.Enabled.Returns(true);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPreviewFeature>().Returns(previewFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsPreviewMode.Should().BeTrue();
        }

        [Fact]
        public void IsEditMode_Will_Be_False_If_There_Is_No_HttpContext()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();

            accessor.HttpContext.ReturnsNull();

            var context = new XperiencePageBuilderContext(accessor);

            context.IsEditMode.Should().BeFalse();
        }

        [Fact]
        public void IsEditMode_Will_Be_False_If_PageBuilderFeature_Is_Not_In_EditMode()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var pageBuilderFeature = Substitute.For<IPageBuilderFeature>();
            pageBuilderFeature.EditMode.Returns(false);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPageBuilderFeature>().Returns(pageBuilderFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsEditMode.Should().BeFalse();
        }

        [Fact]
        public void IsEditMode_Will_Be_True_If_PageBuidlerFeature_EditMode_Is_True()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var pageBuilderFeature = Substitute.For<IPageBuilderFeature>();
            pageBuilderFeature.EditMode.Returns(true);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPageBuilderFeature>().Returns(pageBuilderFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsEditMode.Should().BeTrue();
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        public void IsLivePreviewMode_Will_Be_True_If_EditMode_Is_False_And_PreviewMode_Is_True(
            bool previewMode, bool editMode, bool livePreviewMode)
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var pageBuilderFeature = Substitute.For<IPageBuilderFeature>();
            pageBuilderFeature.EditMode.Returns(editMode);
            var previewFeature = Substitute.For<IPreviewFeature>();
            previewFeature.Enabled.Returns(previewMode);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPageBuilderFeature>().Returns(pageBuilderFeature);
            kenticoFeature.GetFeature<IPreviewFeature>().Returns(previewFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsLivePreviewMode.Should().Be(livePreviewMode);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void IsLiveMode_Will_Be_True_If_PreviewMode_Is_False(
            bool previewMode, bool liveMode)
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var previewFeature = Substitute.For<IPreviewFeature>();
            previewFeature.Enabled.Returns(previewMode);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPreviewFeature>().Returns(previewFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.IsLiveMode.Should().Be(liveMode);
        }

        [Fact]
        public void Mode_Returns_The_Current_Mode()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var pageBuilderFeature = Substitute.For<IPageBuilderFeature>();
            pageBuilderFeature.EditMode.Returns(true);
            var previewFeature = Substitute.For<IPreviewFeature>();
            previewFeature.Enabled.Returns(true);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPageBuilderFeature>().Returns(pageBuilderFeature);
            kenticoFeature.GetFeature<IPreviewFeature>().Returns(previewFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.Mode.Should().Be(PageBuilderMode.Edit);
        }

        [Fact]
        public void ModeName_Returns_The_Current_Mode_As_A_String()
        {
            var accessor = Substitute.For<IHttpContextAccessor>();
            var pageBuilderFeature = Substitute.For<IPageBuilderFeature>();
            pageBuilderFeature.EditMode.Returns(true);
            var previewFeature = Substitute.For<IPreviewFeature>();
            previewFeature.Enabled.Returns(true);
            var kenticoFeature = Substitute.For<IFeatureSet>();
            kenticoFeature.GetFeature<IPageBuilderFeature>().Returns(pageBuilderFeature);
            kenticoFeature.GetFeature<IPreviewFeature>().Returns(previewFeature);

            var http = Substitute.For<HttpContext>();
            http.Items = new Dictionary<object, object>
            {
                { "Kentico.Features", kenticoFeature }
            };

            accessor.HttpContext.Returns(http);

            var context = new XperiencePageBuilderContext(accessor);

            context.ModeName().Should().Be(nameof(PageBuilderMode.Edit));
        }
    }
}
