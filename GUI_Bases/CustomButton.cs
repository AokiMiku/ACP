namespace GUI_Bases
{
	using System.Windows;
	using System.Windows.Input;

	public class CustomButton : ApS.WPF.BaseButton
	{
		public CustomButton() : base()
		{
			this.MouseEnter += Container_MouseEnter;
			this.MouseLeave += Container_MouseLeave;
			this.Loaded += CustomButton_Loaded;
			this.Unloaded += CustomButton_Unloaded;

			this.Background = Layout.ButtonBackground;
			this.ForeGround = Layout.ButtonForeground;
		}

		private void CustomButton_Loaded(object sender, RoutedEventArgs e)
		{
			Layout.Buttons.Add(this);
		}

		private void CustomButton_Unloaded(object sender, RoutedEventArgs e)
		{
			Layout.Buttons.Remove(this);
		}

		private void Container_MouseEnter(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseEnter(this);
		}

		private void Container_MouseLeave(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseLeave(this);
		}
	}
}