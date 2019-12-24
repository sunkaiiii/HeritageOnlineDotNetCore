using System;
using System.IO;
using HeritageWebserviceDotNetCore.Reptile;
using NUnit.Framework;

namespace HeritageWebserviceRetileDotNetCore.Tests
{
    public class WebImageSaverTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetCompressImageName()
        {
            var imagePath = "/Users/sunkai/Documents/GitHub/HeritageOnlineDotNetCore/HeritageWebserviceDotNetCore/img/420715940a814eaebe774e478bc4b662.jpg";
            var exptectedResult = "/Users/sunkai/Documents/GitHub/HeritageOnlineDotNetCore/HeritageWebserviceDotNetCore/img/420715940a814eaebe774e478bc4b662_compress.jpg";
            Assert.AreEqual(exptectedResult, WebImageSaver.Instance.GetComressImageName(imagePath));
        }


        [Test]
        public void TestImageCompress()
        {
            var imagePath = "/Users/sunkai/Documents/GitHub/HeritageOnlineDotNetCore/HeritageWebService/img/s5bf789c69c6cb.jpg";
            WebImageSaver.Instance.CompressImage(imagePath);
            Assert.IsTrue(File.Exists(WebImageSaver.Instance.GetComressImageName(imagePath)));
        }

        [Test]
        public void TestCompressImageWithGuetzli()
        {
            var imagePath = "/Users/sunkai/Documents/GitHub/HeritageOnlineDotNetCore/HeritageWebService/img/s5bf789c69c6cb.jpg";
            WebImageSaver.Instance.CompressImageWithGuetzli(imagePath);
            Assert.IsTrue(File.Exists(imagePath));
        }

    }
}
