using Rg.Plugins.Popup.Pages;
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
	public partial class LogFaultPopUpView : PopupPage
    {
		public LogFaultPopUpView ()
		{
			InitializeComponent ();

            BindingContext = new MainVM();
        }
	}
}