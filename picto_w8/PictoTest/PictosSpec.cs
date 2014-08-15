using System;
using System.Linq;
using Windows.ApplicationModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PictoUI.Common;
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
            pictos = new Pictos();
            new InitialDataService(pictos).LoadInitialData().Wait();
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
            var res = pictos.IsUniqueCategory("Sexuality", null);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void is_unique_should_return_true_if_the_only_one_coincidence_is_itself()
        {
            var res = pictos.IsUniqueCategory("Sexuality -16", "sexuality_-16");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void is_unique_return_false_for_a_existing_pictop_name_in_category()
        {
            var res = pictos.IsUniquePicto("sexuality_-16", "homosexual", null);

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void is_unique_return_true_if_the_only_one_coincidence_is_itself()
        {
            var res = pictos.IsUniquePicto("sexuality_-16", "homosexual", "homosexual");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void save_picto_should_add_new_picto_to_collection()
        {
            var category = pictos.GetCategories().Result.First();
            var file = Base64Converter.FromStorageFile(Package.Current.InstalledLocation.GetFileAsync("assets\\logo.jpg").AsTask().Result).Result;

            pictos.SavePicto(category, new Picto{Text="juarrrr", Image=file, Sound = ""}).Wait();

            var categoryCheck = new Pictos().GetCategory(category.Key).Result;
            Assert.AreEqual(127, categoryCheck.Children.Count);
            Assert.AreEqual(127, category.Children.Count);
        }

        [TestMethod]
        public void save_picto_should_update_existing_picto()
        {
            var parent = pictos.GetCategories().Result.Single(c=>c.Key=="abstract");
            var picto = parent.Children.Single(p=>p.Key=="1");
            var modifidiedPicto = new Picto
            {
                Key = picto.Key,
                Text = "test",
                Image = picto.Image,
                Sound = picto.Sound
            };

            pictos.SavePicto(parent, modifidiedPicto).Wait();

            var pictoCheck = new Pictos().GetCategories().Result.Single(c => c.Key == "abstract").Children.Single(p => p.Key == "1");
            Assert.AreEqual("test", parent.Children.Single(p => p.Key == "1").Text);            
            Assert.AreEqual("test",pictoCheck.Text);
        }

        [TestMethod]
        public void save_picto_should_update_existing_picto_and_keep_children()
        {
            var parent = pictos.GetCategories().Result.Single(c => c.Key == "abstract");
            var children = parent.Children.Count;
            var modifidiedPicto = new Picto
            {
                Key = parent.Key,
                Text = "test",
                Image = parent.Image,
                Sound = parent.Sound
            };

            pictos.SavePicto(null, modifidiedPicto).Wait();

            var pictoCheck = new Pictos().GetCategories().Result.Single(c => c.Key == "abstract");
            Assert.AreEqual("test", pictos.GetCategories().Result.Single(c => c.Key == "abstract").Text);
            Assert.AreEqual("test", pictoCheck.Text);
            Assert.AreEqual(children, pictos.GetCategories().Result.Single(c => c.Key == "abstract").Children.Count);
            Assert.AreEqual(children, pictoCheck.Children.Count);
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
