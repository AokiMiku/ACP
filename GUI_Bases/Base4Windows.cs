namespace GUI_Bases
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using ACP;
	using ApS;
	using ApS.WPF;

	public class Base4Windows : WPFBase
	{

		public Base4Windows()
		{
			Layout.WindowBackgrounds.Add(this);
		}

		protected override void SetFenstergroessen()
		{
			if (this.Fenstergroessen && UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth(WindowName);
				this.Height = UserSettings.GetHeight(WindowName);
			}
		}

		protected override void SetColors()
		{
			Layout.SetButtonBackgroundColors();
			Layout.SetButtonForegroundColors();
			Layout.SetWindowBackgroundColors();
			Layout.SetWindowForegroundColors();
		}

		protected override void SaveFenstergroessen()
		{
			if (this.Fenstergroessen && UserSettings.FenstergroesseMerken)
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