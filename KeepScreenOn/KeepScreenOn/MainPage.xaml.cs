using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KeepScreenOn
{
	public partial class MainPage : ContentPage
	{
		private const string ScreenLockCopy = "ScreenLock status:";

		public MainPage()
		{
			InitializeComponent();
			ScreeLockStatus.Text = GetScreenLockText(ScreenLock.IsActive);
		}

		private string GetScreenLockText(bool isActive)
		{
			return ScreenLockCopy + isActive;
		}

		private void ToggleWithEssentials_Clicked(object sender, EventArgs e)
		{
			ScreenLock.RequestActive();
			ScreeLockStatus.Text = GetScreenLockText(ScreenLock.IsActive);
		}

		private void ToggleWithNativeCommand_Clicked(object sender, EventArgs e)
		{
			DependencyService.Get<IKeepScreenWake>().KeepScreenOn();
			ScreeLockStatus.Text = GetScreenLockText(DependencyService.Get<IKeepScreenWake>().IsActive());
		}
	}
}
