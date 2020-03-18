namespace ACP_GUI
{
	using System;
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
	}
}