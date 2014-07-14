﻿using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace PictoUI.Services
{
    public class DialogService : IDialogService
    {
        private readonly ResourceLoader _resourceLoader = new ResourceLoader();

        public async Task ShowMessageAsync(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        public async Task ShowResourceMessageAsync(string key)
        {
            var dialog = new MessageDialog(_resourceLoader.GetString(key));
            await dialog.ShowAsync();
        }
    }
}
