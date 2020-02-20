﻿namespace ACP_GUI
{
	using System.Windows;

	using ACP;

	/// <summary>
	/// Interaktionslogik für Updater.xaml
	/// </summary>
	public partial class Updater : Window
    {
		#region Constants
		private const string MessageBoxUpdateVerfuegbar = "Es ist eine neue Version verfügbar. Wollen Sie sie jetzt herunterladen und installieren?";
		private const string CaptionUpdateVerfuegbar = "Update erforderlich";
		#endregion

		private ACP.Updater updater;

		public Updater()
		{
			InitializeComponent();
		}

		public void Start(ACP.Updater updater)
		{
			this.updater = updater;
			this.updater.UpdateProgressChanged += Core_UpdateProgressChanged;
			this.updater.DownloadCompleted += Updater_DownloadCompleted;
			this.ShowDialog();
		}

		private void Updater_DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			this.updater.InstallUpdate();
			Application.Current.Shutdown();
		}

		private void Core_UpdateProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
		{
			this.progressUpdater.Value = e.ProgressPercentage;
		}

		private void ProgressUpdater_Loaded(object sender, RoutedEventArgs e)
		{
			this.updater.DownloadUpdateAsync();
		}

		public static void CheckForUpdate()
		{
			ACP.Updater up = new ACP.Updater();
			if (up.CheckForUpdate(UserSettings.LetztesUpdateAm, "AKV"))
			{
				if (MessageBox.Show(MessageBoxUpdateVerfuegbar, CaptionUpdateVerfuegbar, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					ACP_GUI.Updater upd = new ACP_GUI.Updater();
					upd.Start(up);
				}
			}
		}
	}
}
