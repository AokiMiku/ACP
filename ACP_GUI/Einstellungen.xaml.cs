namespace ACP_GUI
{
	using System;
	using System.Windows;
	using System.Windows.Media;

	using ACP;
	using ApS;
	using GUI_Bases;

    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Base4Windows
    {
		public bool ResetNummern = false;

		private SolidColorBrush ButtonHover;
		private SolidColorBrush ButtonBackground;
		private SolidColorBrush ButtonForeground;
		private SolidColorBrush SelectedBackground;
		private SolidColorBrush WindowBackground;
		private SolidColorBrush WindowForeground;

        public Einstellungen()
        {
            InitializeComponent();
			this.WindowName = "Einstellungen";

			Layout.WindowBackgrounds.Add(this.Tabs);
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

			if (this.ButtonHover != null && this.ButtonHover != UserSettings.ButtonHover.ToSolidColorBrush())
			{
				UserSettings.ButtonHover = (Layout.ButtonHover = this.ButtonHover).ToString();
			}

			if (this.ButtonBackground != null && this.ButtonBackground != UserSettings.ButtonBackground.ToSolidColorBrush())
			{
				UserSettings.ButtonBackground = (Layout.ButtonBackground = this.ButtonBackground).ToString();
				Layout.SetButtonBackgroundColors();
			}

			if (this.ButtonForeground != null && this.ButtonForeground != UserSettings.ButtonForeground.ToSolidColorBrush())
			{
				UserSettings.ButtonForeground = (Layout.ButtonForeground = this.ButtonForeground).ToString();
				Layout.SetButtonForegroundColors();
			}

			if (this.SelectedBackground != null && this.SelectedBackground != UserSettings.SelectedBackground.ToSolidColorBrush())
			{
				UserSettings.SelectedBackground = (Layout.SelectedBackground = this.SelectedBackground).ToString();
			}

			if (this.WindowBackground != null && this.WindowBackground != UserSettings.WindowBackground.ToSolidColorBrush())
			{
				UserSettings.WindowBackground = (Layout.WindowBackground = this.WindowBackground).ToString();
				Layout.SetWindowBackgroundColors();
			}

			if (this.WindowForeground != null && this.WindowForeground != UserSettings.WindowForeground.ToSolidColorBrush())
			{
				UserSettings.WindowForeground = (Layout.WindowForeground = this.WindowForeground).ToString();
				Layout.SetWindowForegroundColors();
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

		private void Window_Initialized(object sender, EventArgs e)
		{
			this.saveCancel.Speichern += SaveCancel_Speichern;
			this.saveCancel.Abbrechen += SaveCancel_Abbrechen;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//Allgemein
			this.letztesFranchiseOeffnen.IsChecked = UserSettings.BeiProgrammstartLetztesFranchiseOeffnen;
			this.fensterGroesseMerken.IsChecked = UserSettings.FenstergroesseMerken;
			this.letzteSortierungMerken.IsChecked = UserSettings.LetzteSortierungMerken;

			//Farben
			this.ButtonHoverColor.SelectedColor = UserSettings.ButtonHover.ToSolidColorBrush().Color;
			this.ButtonBackgroundColor.SelectedColor = UserSettings.ButtonBackground.ToSolidColorBrush().Color;
			this.ButtonForegroundColor.SelectedColor = UserSettings.ButtonForeground.ToSolidColorBrush().Color;
			this.SelectedItemColor.SelectedColor = UserSettings.SelectedBackground.ToSolidColorBrush().Color;
			this.WindowBackgroundColor.SelectedColor = UserSettings.WindowBackground.ToSolidColorBrush().Color;
			this.WindowForegroundColor.SelectedColor = UserSettings.WindowForeground.ToSolidColorBrush().Color;
			this.ButtonHoverColor.SelectedColorChanged += ButtonHoverColor_SelectedColorChanged;
			this.ButtonBackgroundColor.SelectedColorChanged += ButtonBackgroundColor_SelectedColorChanged;
			this.ButtonForegroundColor.SelectedColorChanged += ButtonForegroundColor_SelectedColorChanged;
			this.SelectedItemColor.SelectedColorChanged += SelectedItemColor_SelectedColorChanged;
			this.WindowBackgroundColor.SelectedColorChanged += WindowBackgroundColor_SelectedColorChanged;
			this.WindowForegroundColor.SelectedColorChanged += WindowForegroundColor_SelectedColorChanged;

			//Updates
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

		private void Base4Windows_Closed(object sender, EventArgs e)
		{
			Layout.WindowBackgrounds.Remove(this.Tabs);
		}

		private void ButtonHoverColor_SelectedColorChanged(Color obj)
		{
			this.ButtonHover = new SolidColorBrush(obj);
		}

		private void ButtonBackgroundColor_SelectedColorChanged(Color obj)
		{
			this.ButtonBackground = new SolidColorBrush(obj);
		}

		private void ButtonForegroundColor_SelectedColorChanged(Color obj)
		{
			this.ButtonForeground = new SolidColorBrush(obj);
		}

		private void SelectedItemColor_SelectedColorChanged(Color obj)
		{
			this.SelectedBackground = new SolidColorBrush(obj);
		}

		private void WindowBackgroundColor_SelectedColorChanged(Color obj)
		{
			this.WindowBackground = new SolidColorBrush(obj);
		}

		private void WindowForegroundColor_SelectedColorChanged(Color obj)
		{
			this.WindowForeground = new SolidColorBrush(obj);
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