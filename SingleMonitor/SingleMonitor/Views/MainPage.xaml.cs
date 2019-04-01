using SingleMonitor.Models;
using SingleMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SingleMonitor.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        public static string faultAnimation = "Xamarin-Animation-circle-red-button.json";
        public static string solvedAnimation = "Xamarin-Animation-green-check-iii.json";
        public bool hasLoaded = false;
        public bool noFaultLoaded = false;
        
        public MainPage ()
		{
            StartFaultTracking();
            InitializeComponent ();
            BindingContext = new MainVM();
        }

        private void StartFaultTracking()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (Equipment.IsThereActiveFault())
                {
                    if(!hasLoaded)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ThereIsFaultDropIn();
                        });
                        hasLoaded = true;
                        noFaultLoaded = false;
                    }
                }
                else
                {
                    if (!noFaultLoaded)
                    {
                        NoFaultDropIn();
                        noFaultLoaded = true;
                        hasLoaded = false;
                    }
                }

                return true;
            });
        }


        private void CanIRun()
        {
            
        }

        private async void NoFaultDropIn()
        {
            timerRep.IsVisible = false;
            faultRestoreButtonRep.IsVisible = false;
            animationActiveFaultView.Animation = solvedAnimation;
            await animationActiveFaultView.TranslateTo(0, 80);
        }

        private async void ThereIsFaultDropIn()
        {
            animationActiveFaultView.Animation = faultAnimation;
            await animationActiveFaultView.TranslateTo(0, 30);
            faultLogButtonRep.IsVisible = false;
            await timerRep.TranslateTo(0, -50, 800);
            await timerRep.TranslateTo(0, 0, 2000, Easing.BounceOut);
        }
    }
}