namespace ACP_GUI
{
	using System;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;

	using GUI_Bases;

	/// <summary>
	/// Interaktionslogik für Bild.xaml
	/// </summary>
	public partial class Bild : Base4Windows
	{
		public ImageSource ImageSource;

		public Bild()
        {
            InitializeComponent();

			this.ShowInTaskbar = false;
			this.Fenstergroessen = false;
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.ImageSource == null)
			{
				this.Close();
				return;
			}

			this.bild.Source = this.ImageSource;
		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.Close();
		}

		private void Bild_Loaded(object sender, RoutedEventArgs e)
		{
			this.ResizeMode = ResizeMode.NoResize;
		}

		protected override void OnDeactivated(EventArgs e)
		{
			base.OnDeactivated(e);
			try
			{
				this.Close();
			}
			catch
			{
			}
		}
	}
}