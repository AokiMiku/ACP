namespace GUI_Bases
{
	using System;
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;
	using System.Windows.Controls;
	using System.Windows.Media;

	using ACP;
	using System.Windows;

	public static class Layout
	{
		public static List<DockPanel> Buttons = new List<DockPanel>();
		public static List<Control> WindowBackgrounds = new List<Control>();
		
		private static SolidColorBrush buttonHover = null;
		private static SolidColorBrush buttonBackground = null;
		private static SolidColorBrush buttonForeground = null;
		private static SolidColorBrush selectedBackground = null;
		private static SolidColorBrush windowBackground = null;
		private static SolidColorBrush windowForeground = null;

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
				UserSettings.ButtonHover = (buttonHover = value).ToString();
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
				UserSettings.ButtonBackground = (buttonBackground = value).ToString();
			}
		}

		public static SolidColorBrush ButtonForeground
		{
			get
			{
				if (buttonForeground != null)
				{
					return buttonForeground;
				}

				try
				{
					return buttonForeground = UserSettings.ButtonForeground.ToSolidColorBrush();
				}
				catch (NotSupportedException)
				{
					return buttonForeground = UserSettings.ButtonForegroundDefault.ToSolidColorBrush();
				}
			}
			set
			{
				UserSettings.ButtonForeground = (buttonForeground = value).ToString();
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
				UserSettings.SelectedBackground = (selectedBackground = value).ToString();
			}
		}

		public static SolidColorBrush WindowBackground
		{
			get
			{
				if (windowBackground != null)
				{
					return windowBackground;
				}

				try
				{
					return windowBackground = UserSettings.WindowBackground.ToSolidColorBrush();
				}
				catch (NotSupportedException)
				{
					return windowBackground = UserSettings.WindowBackgroundDefault.ToSolidColorBrush();
				}
			}
			set
			{
				UserSettings.WindowBackground = (windowBackground = value).ToString();
			}
		}

		public static SolidColorBrush WindowForeground
		{
			get
			{
				if (windowForeground != null)
				{
					return windowForeground;
				}

				try
				{
					return windowForeground = UserSettings.WindowForeground.ToSolidColorBrush();
				}
				catch (NotSupportedException)
				{
					return windowForeground = UserSettings.WindowForegroundDefault.ToSolidColorBrush();
				}
			}
			set
			{
				UserSettings.WindowForeground = (windowForeground = value).ToString();
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

		public static void SetButtonBackgroundColors()
		{
			foreach (DockPanel button in Buttons)
			{
				if (button != null && button.IsLoaded)
				{
					button.Background = ButtonBackground;
				}
			}
		}

		public static void SetButtonForegroundColors()
		{
			foreach (DockPanel button in Buttons)
			{
				if (button != null && button.IsLoaded)
				{
					var texts = button.FindVisualChildren<TextBlock>();
					if (texts.Count() == 1)
					{
						TextBlock text = texts.First();
						if (text != null)
						{
							text.Foreground = ButtonForeground;
						}
					}
				}
			}
		}

		public static void SetWindowBackgroundColors()
		{
			foreach (Control item in WindowBackgrounds)
			{
				if (item != null && item.IsLoaded)
				{
					item.Background = WindowBackground;
				}
			}
		}

		public static void SetWindowForegroundColors()
		{
			foreach (Control item in WindowBackgrounds)
			{
				foreach (TextBlock textBlock in item.FindVisualChildren<TextBlock>())
				{
					textBlock.Foreground = WindowForeground;
				}
				foreach (CheckBox checkBox in item.FindVisualChildren<CheckBox>())
				{
					checkBox.Foreground = WindowForeground;
				}
			}
		}
	}
}