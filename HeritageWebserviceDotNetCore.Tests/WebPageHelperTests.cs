using HeritageWebserviceDotNetCore.Reptile;
using NUnit.Framework;

namespace Tests
{
    public class WebPageHelperTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetUrlTestWithSlash()
        {
            var subName = WebpageHelper.GetSubUrl(@"http://www.baidu.com");
            var subNameMultiSlash = WebpageHelper.GetSubUrl(@"http://www.baidu.com/newsdetail/10204214.html");
            Assert.AreEqual("www.baidu.com",subName);
            Assert.AreEqual("10204214.html", subNameMultiSlash);
            Assert.Pass();
        }

        [Test]
        public void GetUrlTestWithoutSlash()
        {
            var subName = "asdsads.txt";
            Assert.AreEqual(subName, WebpageHelper.GetSubUrl(subName));
        }
    }
}