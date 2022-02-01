using System.Collections.Generic;
using System.Text;
using CapstoneWeb.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CapstoneTest.WebTests.Pages
{
    [TestClass]
    public class TestIndex
    {
        [TestMethod]
        public void GetIndex_WithNoUserId()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string>());
            var httpContext = new DefaultHttpContext
            {
                Session = session.Object
            };
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            var page = new IndexModel
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };
            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("login", redirect.PageName);
        }

        [TestMethod]
        public void GetIndex_WithUserId()
        {
            var outBytes = Encoding.UTF8.GetBytes("0");
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            session.Setup(s => s.TryGetValue("userId", out outBytes)).Returns(true);
            var page = TestPageBuilder.BuildPage<IndexModel>(session.Object);
            var result = page.OnGet();
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.AreEqual(0, page.UserId);
        }

        [TestMethod]
        public void PostLogout()
        {
            var session = new Mock<ISession>();
            session.SetupGet(s => s.Keys).Returns(new List<string> {"userId"});
            var page = TestPageBuilder.BuildPage<IndexModel>(session.Object);
            var result = page.OnPostLogout();
            session.Verify(s => s.Remove("userId"));
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirect = (RedirectToPageResult) result;
            Assert.AreEqual("index", redirect.PageName);
        }
    }
}