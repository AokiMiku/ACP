namespace GUI_Bases
{
	using System;
	using System.Windows;

	using ACP;
	using ApS;

	public class Base4Windows : Window
	{
		protected string WindowName = "";

		public Base4Windows()
		{
			this.Initialized += Window_Initialized;
			this.Closing += Window_Closing;
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth(WindowName);
				this.Height = UserSettings.GetHeight(WindowName);
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				UserSettings.SaveWidth(WindowName, this.Width.ToInt());
				UserSettings.SaveHeight(WindowName, this.Height.ToInt());
			}
		}
	}
}