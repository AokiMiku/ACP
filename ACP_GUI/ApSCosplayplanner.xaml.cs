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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACP_GUI
{
	using ACP;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class ApSCosplayplanner : Window
	{
		private Core core;

		public ApSCosplayplanner()
		{
			InitializeComponent();
		}

		private void AddFranchise_Click(object sender, RoutedEventArgs e)
		{
			this.core.SaveFranchise(AddFranchise.ShowModal());
		}

		private void DelFranchise_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Einstellungen_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ColNummer_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ColErledigt_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ColName_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Franchises_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.ActualizeData();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.core = new Core();
			this.core.GetFranchises();
			while (!this.core.Franchises.EoF)
			{
				this.franchises.Items.Add(new Franchises4Dropdown(this.core.Franchises.Nummer, this.core.Franchises.Name));
				this.core.Franchises.Skip();
			}
			
		}

		private void ActualizeData()
		{
			if (this.franchises.SelectedItem == null)
			{
				return;
			}

			Cosplans cosplansV = this.core.GetCosplans(((Franchises4Dropdown)this.franchises.SelectedItem).Nummer);
			
			while (!cosplansV.EoF)
			{
				Grid grid = new Grid();
				for (int i = 0; i < 5; i++)
				{
					grid.ColumnDefinitions.Add(new ColumnDefinition());
				}

				Label labelNummer = new Label
				{
					Content = cosplansV.Nummer,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetColumn(labelNummer, 0);
				grid.Children.Add(labelNummer);

				Label labelName = new Label
				{
					Content = cosplansV.Name,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetColumn(labelName, 1);
				Grid.SetColumnSpan(labelName, 2);
				grid.Children.Add(labelName);

				CheckBox checkBoxErledigt = new CheckBox
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetColumn(checkBoxErledigt, 3);
				grid.Children.Add(checkBoxErledigt);

				Button buttonBilder = new Button
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					Width = 40
				};
				Image imageBilder = new Image
				{
					Source = new BitmapImage(new Uri(@"\Resources\Icons\picture.ico", UriKind.Relative))
				};
				buttonBilder.Content = imageBilder;
				Grid.SetColumn(buttonBilder, 4);
				grid.Children.Add(buttonBilder);
				grid.Height = 50;

				this.data.Children.Add(grid);
				cosplansV.Skip();
			}
		}

		private void AddCosplan_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DeleteCosplan_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}