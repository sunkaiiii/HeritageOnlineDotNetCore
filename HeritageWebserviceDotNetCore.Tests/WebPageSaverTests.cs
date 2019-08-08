using System;
using System.IO;
using HeritageWebserviceDotNetCore.Reptile;
using NUnit.Framework;

namespace Tests
{
    public class WebPageSaverTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryToSaveHtmlFile()
        {
            var url = "http://billie66.github.io/TLCL/book/chap01.html";
            WebPageSaver.SaveHtml(url);
            Assert.IsTrue(File.Exists(WebpageHelper.GetSubUrl(url)));
        }
    }
}
