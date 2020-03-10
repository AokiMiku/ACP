namespace ACP_GUI
{
	using System;
	using System.Windows;

	using ACP;
	using ApS;

    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Window
    {
		#region Constants
		private const string WindowName = "Einstellungen";
		#endregion

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

			if (this.letzteSortierungMerken.IsChecked != UserSettings.LetzteSortierungMerken)
			{
				UserSettings.LetzteSortierungMerken = this.letzteSortierungMerken.IsChecked.ToBoolean();
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
			MessageBox.Show(Constants.MessageBoxResetCosplanNummer, Constants.CaptionCosplanNummer, MessageBoxButton.OK);
			this.Close();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				UserSettings.SaveWidth(WindowName, this.Width.ToInt());
				UserSettings.SaveHeight(WindowName, this.Height.ToInt());
			}
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth(WindowName);
				this.Height = UserSettings.GetHeight(WindowName);
			}
			this.saveCancel.Speichern += SaveCancel_Speichern;
			this.saveCancel.Abbrechen += SaveCancel_Abbrechen;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.letztesFranchiseOeffnen.IsChecked = UserSettings.BeiProgrammstartLetztesFranchiseOeffnen;
			this.fensterGroesseMerken.IsChecked = UserSettings.FenstergroesseMerken;
			this.letzteSortierungMerken.IsChecked = UserSettings.LetzteSortierungMerken;
			this.updatesAktiv.IsChecked = UserSettings.Updates;
			SetUpdateSection();
			if (UserSettings.Updates)
			{
				if (UserSettings.UpdateAlleXTage > 0)
				{
					this.updateIntervall.Text = UserSettings.UpdateAlleXTage.ToString();
				}
			}
		}

		private void UpdateLaden_Click(object sender, RoutedEventArgs e)
		{
			ACP_GUI.Updater.CheckForUpdate();
		}

		private void UpdatesAktiv_Click(object sender, RoutedEventArgs e)
		{
			SetUpdateSection();
		}

		private void SetUpdateSection()
		{
			if (this.updatesAktiv.IsChecked == true)
			{
				this.updateIntervall.IsEnabled = true;
			}
			else
			{
				this.updateIntervall.IsEnabled = false;
			}
		}
	}
}