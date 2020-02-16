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
	using ACP;
	using ApS;

    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Window
    {
		public bool ResetNummern = false;

        public Einstellungen()
        {
            InitializeComponent();
        }

		private void SaveCancel_Speichern(object sender, RoutedEventArgs e)
		{
			if (this.letztesFranchiseOeffnen.IsChecked != UserSettings.BeiProgrammstartLetztesFranchiseOeffnen)
			{
				UserSettings.BeiProgrammstartLetztesFranchiseOeffnen = this.letztesFranchiseOeffnen.IsChecked.ToBoolean();
			}

			if (this.fensterGroesseMerken.IsChecked != UserSettings.FenstergroesseMerken)
			{
				UserSettings.FenstergroesseMerken = this.fensterGroesseMerken.IsChecked.ToBoolean();
			}

			this.Close();
		}

		private void SaveCancel_Abbrechen(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ResetCosplanNummer_Click(object sender, RoutedEventArgs e)
		{
			this.ResetNummern = true;
			Core.ResetCosplanNummern();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				UserSettings.SaveWidth("Einstellungen", this.Width.ToInt());
				UserSettings.SaveHeight("Einstellungen", this.Height.ToInt());
			}
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth("Einstellungen");
				this.Height = UserSettings.GetHeight("Einstellungen");
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.letztesFranchiseOeffnen.IsChecked = UserSettings.BeiProgrammstartLetztesFranchiseOeffnen;
			this.fensterGroesseMerken.IsChecked = UserSettings.FenstergroesseMerken;
		}
	}
}