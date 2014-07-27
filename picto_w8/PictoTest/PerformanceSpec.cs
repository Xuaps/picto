using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PictoUI.Model;

namespace PictoTest
{
    [TestClass]
    public class PerformanceSpec
    {
        [TestInitialize]
        public void Initialize()
        {
            StorageFile databaseFile = Package.Current.InstalledLocation.GetFileAsync("assets\\pictos.db").AsTask().Result;
            databaseFile.CopyAsync(ApplicationData.Current.LocalFolder).AsTask().Wait();
        }

        [TestMethod]
        public void load_pictos()
        {
            var pictos = new Pictos();
            var watch = Stopwatch.StartNew();
            pictos.Initialize().Wait();
            watch.Stop();
           
            Assert.IsTrue(watch.ElapsedMilliseconds<3500, watch.ElapsedMilliseconds+" - Excesive load time");
        }
    }
}
