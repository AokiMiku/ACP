namespace ACP_GUI
{
	using System;
	using System.IO;
	using System.Drawing;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	using GUI_Bases;
	using ApS.WPF;

	/// <summary>
	/// Interaktionslogik für Bild.xaml
	/// </summary>
	public partial class Bild : Base4Windows
	{
		public ImageSource ImageSource;

		public Bild(WPFBase parent) : base(parent)
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
			Stream bildStream = ((BitmapImage)this.ImageSource).StreamSource;
			using (Image img = Image.FromStream(bildStream, false, false))
			{
				this.Height = img.Height;
				this.Width = img.Width;
			}
			this.ResizeMode = ResizeMode.NoResize;
			this.CenterRelativeToParent();
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