using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFormsGeoBackground.Interfaces;
using XFormsGeoBackground.ViewModels;

namespace XFormsGeoBackground
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel VM { get; }

        public MainPage()
        {
            InitializeComponent();

            BindingContext = VM = new MainPageViewModel();
        }
    }
}
