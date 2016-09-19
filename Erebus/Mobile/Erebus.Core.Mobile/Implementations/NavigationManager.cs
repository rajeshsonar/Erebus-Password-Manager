﻿using Erebus.Core.Mobile.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Core.Mobile.Implementations
{
    public class NavigationManager : INavigationManager
    {
        private Application Application;
        private IPresenterFactory PresenterFactory;

        public NavigationManager(Application application, IPresenterFactory presenterFactory)
        {
            this.Application = application;
            this.PresenterFactory = presenterFactory;
        }

        public async Task NavigateAsync<TPresenter>() where TPresenter : IPresenter
        {
            var presenter = this.PresenterFactory.Create<TPresenter>();
            await Application.MainPage.Navigation.PushAsync(presenter.GetView() as Page);
        }

        public async Task NavigateByPopingCurrent<TPresenter>() where TPresenter : IPresenter
        {
            var presenter = this.PresenterFactory.Create<TPresenter>();
            await this.Application.MainPage.Navigation.PopAsync();
            await this.Application.MainPage.Navigation.PushAsync(presenter.GetView() as Page);
        }
    }
}