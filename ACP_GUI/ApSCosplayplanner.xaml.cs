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
	using ApS;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class ApSCosplayplanner : Window
	{
		private Core core;
		private int? selectedCosplan;
		private SolidColorBrush CosplanHover = Brushes.Lavender;
		private SolidColorBrush CosplanClick = Brushes.MediumPurple;

		public ApSCosplayplanner()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.core = new Core
			{
				CosplansOrderBy = Core.OrderBy.Nummer_asc
			};
			this.ActualizeFanchises();
		}

		private void AddFranchise_Click(object sender, RoutedEventArgs e)
		{
			this.franchises.Visibility = Visibility.Collapsed;
			this.newFranchise.Visibility = Visibility.Visible;
			this.newFranchise.Focus();
			//this.core.SaveFranchise(AddFranchise.ShowModal());
		}

		private void NewFranchise_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				this.core.SaveFranchise(this.newFranchise.Text);
				this.newFranchise.Text = "";

				this.franchises.Visibility = Visibility.Visible;
				this.newFranchise.Visibility = Visibility.Collapsed;

				this.ActualizeFanchises();
				this.franchises.SelectedIndex = this.franchises.Items.Count - 1;
			}
			else if (e.Key == Key.Escape)
			{
				this.franchises.Visibility = Visibility.Visible;
				this.newFranchise.Visibility = Visibility.Collapsed;
			}
		}

		private void DelFranchise_Click(object sender, RoutedEventArgs e)
		{
			if (this.franchises.SelectedItem != null 
				&& MessageBox.Show("Wollen Sie das ausgewählte Franchise wirklich löschen? Alle zugehörigen Cospläne werden dabei unwiderruflich gelöscht.", "Löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				this.core.DeleteFranchise(((Franchises4Dropdown)this.franchises.SelectedItem).Nummer);
				this.ActualizeFanchises();
				this.ActualizeData();
			}
		}

		private void AddCosplan_Click(object sender, RoutedEventArgs e)
		{
			if (this.franchises.SelectedItem == null)
			{
				return;
			}

			Grid grid = new Grid();
			for (int i = 0; i < 9; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
			}

			TextBox	textName = new TextBox
			{
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Center
			};
			textName.KeyDown += TextName_KeyDown;
			Grid.SetColumn(textName, 1);
			Grid.SetColumnSpan(textName, 6);
			grid.Children.Add(textName);
			
			grid.Height = 50;

			this.data.Children.Insert(0, grid);
			textName.Focus();
		}

		private void TextName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				this.core.SaveCosplan(((TextBox)((Grid)this.data.Children[0]).Children[0]).Text, ((Franchises4Dropdown)this.franchises.SelectedItem).Nummer);

				this.data.Children.RemoveAt(0);

				this.ActualizeData();
			}
			else if (e.Key == Key.Escape)
			{
				this.data.Children.RemoveAt(0);
			}
		}

		private void DelCosplan_Click(object sender, RoutedEventArgs e)
		{
			if (this.selectedCosplan != null
				&& MessageBox.Show("Wollen Sie das ausgewählte Cosplan wirklich löschen?", "Löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				this.core.DeleteCosplan((int)this.selectedCosplan);
				this.ActualizeData();
			}
		}

		private void Einstellungen_Click(object sender, RoutedEventArgs e)
		{
			Einstellungen einstellungen = new Einstellungen();
			einstellungen.ShowDialog();

			if (einstellungen.ResetNummern)
			{
				this.ActualizeData();
			}
		}

		private void ColNummer_Click(object sender, RoutedEventArgs e)
		{
			this.colNameIcon.Visibility = Visibility.Hidden;
			this.colErledigtIcon.Visibility = Visibility.Hidden;

			if (this.core.CosplansOrderBy == Core.OrderBy.Nummer_asc)
			{
				this.core.CosplansOrderBy = Core.OrderBy.Nummer_desc;
				this.colNummerIcon.Visibility = Visibility.Visible;
				this.colNummerIcon.Source = new BitmapImage(new Uri(@"\Resources\Icons\arrow_up.ico", UriKind.Relative));
			}
			else
			{
				this.core.CosplansOrderBy = Core.OrderBy.Nummer_asc;
				this.colNummerIcon.Visibility = Visibility.Visible;
				this.colNummerIcon.Source = new BitmapImage(new Uri(@"\Resources\Icons\arrow_down.ico", UriKind.Relative));
			}
			ActualizeData();
		}

		private void ColName_Click(object sender, RoutedEventArgs e)
		{
			this.colNummerIcon.Visibility = Visibility.Hidden;
			this.colErledigtIcon.Visibility = Visibility.Hidden;

			if (this.core.CosplansOrderBy == Core.OrderBy.Name_asc)
			{
				this.core.CosplansOrderBy = Core.OrderBy.Name_desc;
				this.colNameIcon.Visibility = Visibility.Visible;
				this.colNameIcon.Source = new BitmapImage(new Uri(@"\Resources\Icons\arrow_up.ico", UriKind.Relative));
			}
			else
			{
				this.core.CosplansOrderBy = Core.OrderBy.Name_asc;
				this.colNameIcon.Visibility = Visibility.Visible;
				this.colNameIcon.Source = new BitmapImage(new Uri(@"\Resources\Icons\arrow_down.ico", UriKind.Relative));
			}
			ActualizeData();
		}

		private void ColErledigt_Click(object sender, RoutedEventArgs e)
		{
			this.colNameIcon.Visibility = Visibility.Hidden;
			this.colNummerIcon.Visibility = Visibility.Hidden;

			if (this.core.CosplansOrderBy == Core.OrderBy.Erledigt_asc)
			{
				this.core.CosplansOrderBy = Core.OrderBy.Erledigt_desc;
				this.colErledigtIcon.Visibility = Visibility.Visible;
				this.colErledigtIcon.Source = new BitmapImage(new Uri(@"\Resources\Icons\arrow_up.ico", UriKind.Relative));
			}
			else
			{
				this.core.CosplansOrderBy = Core.OrderBy.Erledigt_asc;
				this.colErledigtIcon.Visibility = Visibility.Visible;
				this.colErledigtIcon.Source = new BitmapImage(new Uri(@"\Resources\Icons\arrow_down.ico", UriKind.Relative));
			}
			ActualizeData();
		}

		private void Franchises_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.ActualizeData();
		}

		private void ActualizeFanchises()
		{
			this.franchises.Items.Clear();

			this.core.GetFranchises();

			while (!this.core.Franchises.EoF)
			{
				this.franchises.Items.Add(new Franchises4Dropdown(this.core.Franchises.Nummer, this.core.Franchises.Name));
				this.core.Franchises.Skip();
			}
		}

		private void ActualizeData()
		{
			this.data.Children.Clear();
			if (this.franchises.SelectedItem == null)
			{
				return;
			}

			Cosplans cosplansV = this.core.GetCosplans(((Franchises4Dropdown)this.franchises.SelectedItem).Nummer);
			
			while (!cosplansV.EoF)
			{
				Grid grid = new Grid()
				{
					Background = Brushes.Transparent
				};
				grid.MouseDown += Grid_MouseDown;
				grid.MouseEnter += Grid_MouseEnter;
				grid.MouseLeave += Grid_MouseLeave;

				for (int i = 0; i < 9; i++)
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
				Grid.SetColumnSpan(labelName, 6);
				grid.Children.Add(labelName);

				CheckBox checkBoxErledigt = new CheckBox
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
				Grid.SetColumn(checkBoxErledigt, 7);
				Grid.SetColumnSpan(checkBoxErledigt, 1);
				grid.Children.Add(checkBoxErledigt);

				Button buttonBilder = new Button
				{
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					Width = 40,
					Background = Brushes.Transparent,
					BorderThickness = new Thickness(0)
				};
				Image imageBilder = new Image
				{
					Source = new BitmapImage(new Uri(@"\Resources\Icons\picture.ico", UriKind.Relative))
				};
				buttonBilder.Content = imageBilder;
				Grid.SetColumn(buttonBilder, 8);
				grid.Children.Add(buttonBilder);
				grid.Height = 50;

				this.data.Children.Add(grid);
				cosplansV.Skip();
			}
		}

		private void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;
			if (grid.Background != CosplanClick)
			{
				grid.Background = Brushes.Transparent;
			}
		}

		private void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;
			if (grid.Background != CosplanClick)
			{
				grid.Background = CosplanHover;
			}
		}

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Grid grid = (Grid)sender;

			if (this.selectedCosplan != null)
			{
				foreach (UIElement item in this.data.Children)
				{
					Grid child = (Grid)item;
					if (child.Background == CosplanClick)
					{
						child.Background = Brushes.Transparent;
						break;
					}
				}
			}

			if (grid.Background == CosplanHover)
			{
				grid.Background = CosplanClick;
				this.selectedCosplan = ((Label)grid.Children[0]).Content.ToInt();
			}
			else
			{
				grid.Background = CosplanHover;
				this.selectedCosplan = null;
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				UserSettings.SaveWidth("MainWindow", this.Width.ToInt());
				UserSettings.SaveHeight("MainWindow", this.Height.ToInt());
			}
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth("MainWindow");
				this.Height = UserSettings.GetHeight("MainWindow");
			}
		}
	}
}