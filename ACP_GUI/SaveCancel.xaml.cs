namespace ACP_GUI
{
	using System;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Interaktionslogik für SaveCancel.xaml
	/// </summary>
	public partial class SaveCancel : UserControl
	{
		public event EventHandler<RoutedEventArgs> Speichern;
		public event EventHandler<RoutedEventArgs> Abbrechen;

		public SaveCancel()
		{
			InitializeComponent();
		}

		private void speichern_Click(object sender, RoutedEventArgs e)
		{
			this.Speichern?.Invoke(sender, e);
		}

		private void abbrechen_Click(object sender, RoutedEventArgs e)
		{
			this.Abbrechen?.Invoke(sender, e);
		}
	}
}
