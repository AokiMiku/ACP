namespace ACP_GUI
{
	using System;
	using System.Windows.Controls;
	using System.Windows.Media;

	using ACP;

	public static class Layout
	{
		private static SolidColorBrush buttonHover = null;
		private static SolidColorBrush buttonBackground = null;
		private static SolidColorBrush selectedBackground = null;

		public static SolidColorBrush ButtonHover
		{
			get
			{
				if (buttonHover != null)
				{
					return buttonHover;
				}

				try
				{
					return buttonHover = UserSettings.ButtonHover.ToSolidColorBrush();
				}
				catch (NotSupportedException)
				{
					return buttonHover = UserSettings.ButtonHoverDefault.ToSolidColorBrush();
				}
			}
			set
			{
				UserSettings.ButtonHover = value.ToString();
				buttonHover = value;
			}
		}

		public static SolidColorBrush ButtonBackground
		{
			get
			{
				if (buttonBackground != null)
				{
					return buttonBackground;
				}

				try
				{
					return buttonBackground = UserSettings.ButtonBackground.ToSolidColorBrush();
				}
				catch (NotSupportedException)
				{
					return buttonBackground = UserSettings.ButtonBackgroundDefault.ToSolidColorBrush();
				}
			}
			set
			{
				UserSettings.ButtonBackground = value.ToString();
				buttonBackground = value;
			}
		}

		public static SolidColorBrush SelectedBackground
		{
			get
			{
				if (selectedBackground != null)
				{
					return selectedBackground;
				}

				try
				{
					return selectedBackground = UserSettings.SelectedBackground.ToSolidColorBrush();
				}
				catch (NotSupportedException)
				{
					return selectedBackground = UserSettings.SelectedBackgroundDefault.ToSolidColorBrush();
				}
			}
			set
			{
				UserSettings.SelectedBackground = value.ToString();
				selectedBackground = value;
			}
		}

		public static void Button_MouseEnter(object sender)
		{
			Panel button = (Panel)sender;
			button.Background = ButtonHover;
		}

		public static void Button_MouseLeave(object sender)
		{
			Panel button = (Panel)sender;
			button.Background = ButtonBackground;
		}
	}
}