﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SandboxApp.Pages.LoadingWidgetExamples
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingWidgetPage : ContentPage
    {
        public LoadingWidgetPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadingWidget.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            loadingWidget.Stop();
        }

        private void OnGotoSandboxPage_BtnClicked(object sender, EventArgs e)
            => this.Navigation.PushAsync(new LoadingWidgetTestPage());
    }
}