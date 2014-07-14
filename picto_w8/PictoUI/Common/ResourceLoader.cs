using System;
using System.Collections.Generic;
using System.Dynamic;

namespace PictoUI.Common
{
    public class ResourceLoader //:DynamicObject
    {
        private readonly Windows.ApplicationModel.Resources.ResourceLoader _resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
        /*
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _resourceLoader.GetString(binder.Name);
            return result != null;
        }
        */
       public object DiscardButton { get { return _resourceLoader.GetString("DiscardButton"); } }
       public object DeleteButton { get { return _resourceLoader.GetString("DeleteButton"); } }
       public object PlayButton { get { return _resourceLoader.GetString("PlayButton"); } }

        public object HomeButton
        {
            get {return _resourceLoader.GetString("HomeButton"); } 
        }

        public object EditButton
        {
            get { return _resourceLoader.GetString("EditButton"); } 
        }

        public object AdminTittle
        {
            get { return _resourceLoader.GetString("AdminTittle"); }
        }

        public object CategoryTitle
        {
            get { return _resourceLoader.GetString("CategoryTittle"); }
        }

        public object AddButton 
        {
            get { return _resourceLoader.GetString("AddButton"); }
        }

        public object PictoTittle
        {
            get { return _resourceLoader.GetString("PictoTittle"); }
        }

        public object CategoryNameTittle
        {
            get { return _resourceLoader.GetString("CategoryNameTittle"); }
        }

        public object AcceptButton { get { return _resourceLoader.GetString("AcceptButton"); } }

        public object PictoNameTittle
        {
            get { return _resourceLoader.GetString("PictoNameTittle"); }
        }

        public object ImageTittle
        {
            get { return _resourceLoader.GetString("ImageTittle"); }
        }

        public object CancelButton
        {
            get { return _resourceLoader.GetString("CancelButton"); }
        }

        public string NameRequired
        {
            get { return _resourceLoader.GetString("NameRequired"); }
        }

        public string InvalidName
        {
            get { return _resourceLoader.GetString("NameInvalid"); }
        }

        public string UniqueName
        {
            get { return _resourceLoader.GetString("NameUnique"); }
        }

        public string ImageRequired
        {
            get { return _resourceLoader.GetString("ImageRequired"); }
        }

        public string SoundRequired
        {
            get { return _resourceLoader.GetString("SoundRequired"); }
        }

        public string PrivacityPolicyTittle
        {
            get { return _resourceLoader.GetString("PrivacityPolicyTittle"); }
        }

        public Uri PrivacityPolicyUri
        {
            get { return new Uri(_resourceLoader.GetString("PrivacityPolicyUri")); }
        }

        public string ContactUsTittle
        {
            get { return _resourceLoader.GetString("ContactUsTittle"); }
        }

        public Uri ContactUsUri
        {
            get { return new Uri(_resourceLoader.GetString("ContactUsUri")); }
            set { throw new NotImplementedException(); }
        }
    }
}
