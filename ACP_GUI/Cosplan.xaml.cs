namespace ACP_GUI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;

	using ACP;
	using ApS;

	/// <summary>
	/// Interaktionslogik für Cosplan.xaml
	/// </summary>
	public partial class Cosplan : Window
	{
		#region Constants
		private const string WindowName = "Cosplan";
		#endregion

		private Cosplans cosplan = null;

		public Cosplan()
		{
			InitializeComponent();
		}

		public static void Show(int cosplan)
		{
			Cosplan c = new Cosplan
			{
				cosplan = new Cosplans()
				{
					Where = "Nummer = " + cosplan
				}
			};
			c.cosplan.Read();
			c.ShowDialog();
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
				UserSettings.SaveHeight(WindowName, this.Height.ToInt());
				UserSettings.SaveWidth(WindowName, this.Width.ToInt());
			}
		}

		private void Erledigt_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Erledigt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.erledigt.IsChecked = !this.erledigt.IsChecked;
			this.Erledigt_Click(sender, e);
		}
		private void Delete_Click(object sender, RoutedEventArgs e)
		{

		}

		private void AddBild_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DelBild_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseEnter(sender);
		}

		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseLeave(sender);
		}
	}
}