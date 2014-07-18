using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PictoUI.Model;
using PictoUI.Services;

namespace PictoTest
{
    [TestClass]
    public class PictosSpec
    {
        private Pictos pictos;

        [TestInitialize]
        public void Initialize()
        {
            new InitialDataService().LoadInitialData().Wait();
            pictos = new Pictos();
        }

        [TestMethod]
        public void get_categories_should_return_all_pictos_whitout_parent()
        {
            var categories = pictos.GetCategories().Result;

            Assert.AreEqual(37, categories.Count);
            Assert.AreEqual(126, categories.First().Children.Count);
        }

        [TestMethod]
        public void get_category_should_return_a_picto_with_childrens()
        {
            var category = pictos.GetCategory("body").Result;
           
            Assert.AreEqual("Body", category.Text);
            Assert.AreEqual(37, category.Children.Count);
        }

        [TestMethod]
        public void is_unique_should_return_true_for_a_new_category_name()
        {
            var res = pictos.IsUnique("Sexuality");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void is_unique_return_false_for_a_existing_pictop_name_in_category()
        {
            var res = pictos.IsUnique("sexuality_-16", "homosexual");

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void save_picto_should_add_new_picto_to_collection()
        {
            var category = pictos.GetCategories().Result.First();
            var file = Package.Current.InstalledLocation.GetFileAsync("assets\\logo.jpg").AsTask().Result;
            
            pictos.SavePicto(category, "juarrrr", file, null).Wait();

            var categoryCheck = new Pictos().GetCategory(category.Key).Result;
            Assert.AreEqual(127, categoryCheck.Children.Count);
            Assert.AreEqual(127, category.Children.Count);
        }

        [TestMethod]
        public void delete_picto_should_delete_a_picto_from_collection()
        {
            var category = pictos.GetCategories().Result.First();
            pictos.DeletePicto(category, category.Children.First()).Wait();

            var categoryCheck = new Pictos().GetCategory(category.Key).Result;
            Assert.AreEqual(125, categoryCheck.Children.Count());
            Assert.AreEqual(125, category.Children.Count());
        }

        [TestMethod]
        public void delete_category_should_delete_a_category_from_collection()
        {
            var category = pictos.GetCategories().Result.First();
            pictos.DeleteCategory(category).Wait();

            var pictosCheck = new Pictos();
            Assert.ThrowsException<AggregateException>(() => pictos.GetCategory(category.Key).Result);
            Assert.ThrowsException<AggregateException>(() => pictosCheck.GetCategory(category.Key).Result);
            Assert.AreEqual(36, pictosCheck.GetCategories().Result.Count);
        }
    }
}
