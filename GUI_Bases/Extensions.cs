namespace GUI_Bases
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Media;

	public static class Extensions
	{
		public static SolidColorBrush ToSolidColorBrush(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				throw new NotSupportedException();
			}
			return (SolidColorBrush)new BrushConverter().ConvertFromString(s);
		}

		/// <summary>
		/// Copied from https://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="depObj"></param>
		/// <returns></returns>
		public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child != null && child is T)
					{
						yield return (T)child;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}
	}
}