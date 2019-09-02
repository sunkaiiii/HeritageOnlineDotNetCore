using HeritageWebserviceDotNetCore.Reptile;
using NUnit.Framework;

namespace Tests
{
    public class WebPageHelperTests
    {
        private const string MAIN_PAGE = "http://www.ihchina.cn";
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

        [Test]
        public void CorrectRequestStringTest()
        {
            string url1 = @"/asdsad/asdasdasd";
            string url2 = @"+23/dasvsa/fcasvsaf";
            string url3 = MAIN_PAGE + @"/asd/asd/asd";
            Assert.AreEqual(MAIN_PAGE + url1, WebpageHelper.CorrectRequestString(url1));
            Assert.AreEqual(MAIN_PAGE + @"/dasvsa/fcasvsaf", WebpageHelper.CorrectRequestString(url2));
            Assert.AreEqual(url3, WebpageHelper.CorrectRequestString(url3));
        }
    }
}
