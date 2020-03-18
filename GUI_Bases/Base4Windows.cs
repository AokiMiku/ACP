namespace GUI_Bases
{
	using System;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using ACP;
	using ApS;

	public class Base4Windows : Window
	{
		protected string WindowName = "";

		public Base4Windows()
		{
			this.Initialized += Window_Initialized;
			this.Closing += Window_Closing;
			this.Closed += Base4Windows_Closed;
			this.Loaded += Base4Windows_Loaded;
			Layout.WindowBackgrounds.Add(this);
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth(WindowName);
				this.Height = UserSettings.GetHeight(WindowName);
			}
		}

		private void Base4Windows_Loaded(object sender, RoutedEventArgs e)
		{
			Layout.SetButtonBackgroundColors();
			Layout.SetButtonForegroundColors();
			Layout.SetWindowBackgroundColors();
			Layout.SetWindowForegroundColors();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				UserSettings.SaveWidth(WindowName, this.Width.ToInt());
				UserSettings.SaveHeight(WindowName, this.Height.ToInt());
			}
		}

		private void Base4Windows_Closed(object sender, EventArgs e)
		{
			Layout.WindowBackgrounds.Remove(this);
		}
	}
}