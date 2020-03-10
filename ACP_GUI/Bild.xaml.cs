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

namespace ACP_GUI
{
    /// <summary>
    /// Interaktionslogik für Bild.xaml
    /// </summary>
    public partial class Bild : Window
	{
		public ImageSource ImageSource;

		public Bild()
        {
            InitializeComponent();

			this.ShowInTaskbar = false;
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