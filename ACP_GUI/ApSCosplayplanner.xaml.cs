namespace ACP_GUI
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	using ACP;
	using ApS;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class ApSCosplayplanner : Window
	{
		#region Constants
		private const string WindowName = "MainWindow";
		#endregion

		private Core core;
		private int? selectedCosplan;

		public ApSCosplayplanner()
		{
			InitializeComponent();
		}

		private void Window_Initialized(object sender, EventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				this.Width = UserSettings.GetWidth(WindowName);
				this.Height = UserSettings.GetHeight(WindowName);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.version.Text = ApS.Version.StringAppVersion;

			if (UserSettings.Updates && UserSettings.LetztesUpdateAm.AddDays(UserSettings.UpdateAlleXTage) <= DateTime.Now.Date)
			{
				ACP_GUI.Updater.CheckForUpdate();
			}

			ACP.Updater updater = new ACP.Updater();
			updater.UpdateDatabase();

			this.core = new Core();
			this.ActualizeFanchises();
			this.SetSortingIcons();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (UserSettings.FenstergroesseMerken)
			{
				UserSettings.SaveWidth(WindowName, this.Width.ToInt());
				UserSettings.SaveHeight(WindowName, this.Height.ToInt());
			}
		}

		private void AddFranchise_Click(object sender, RoutedEventArgs e)
		{
			this.franchises.Visibility = Visibility.Collapsed;
			this.newFranchise.Visibility = Visibility.Visible;
			this.newFranchise.Focus();
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
				&& MessageBox.Show(Constants.MessageBoxDeleteFranchise, Constants.CaptionDelete, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
				this.core.SaveCosplan(((TextBox)sender).Text, ((Franchises4Dropdown)this.franchises.SelectedItem).Nummer);

				this.data.Children.RemoveAt(0);

				this.ActualizeData();
			}
			else if (e.Key == Key.Escape)
			{
				this.data.Children.RemoveAt(0);
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
			if (this.core.CosplansOrderBy == Core.OrderBy.Nummer_asc)
			{
				this.core.CosplansOrderBy = Core.OrderBy.Nummer_desc;
			}
			else
			{
				this.core.CosplansOrderBy = Core.OrderBy.Nummer_asc;
			}
			
			SetSortingIcons();
			ActualizeData();
		}

		private void ColName_Click(object sender, RoutedEventArgs e)
		{
			if (this.core.CosplansOrderBy == Core.OrderBy.Name_asc)
			{
				this.core.CosplansOrderBy = Core.OrderBy.Name_desc;
			}
			else
			{
				this.core.CosplansOrderBy = Core.OrderBy.Name_asc;
			}

			SetSortingIcons();
			ActualizeData();
		}

		private void SetSortingIcons()
		{
			this.colNummerIcon.Visibility = Visibility.Hidden;
			this.colNameIcon.Visibility = Visibility.Hidden;
			//this.colErledigtIcon.Visibility = Visibility.Hidden;

			switch (this.core.CosplansOrderBy)
			{
				case Core.OrderBy.Nummer_asc:
					this.colNummerIcon.Visibility = Visibility.Visible;
					this.colNummerIcon.Source = new BitmapImage(new Uri(Constants.Arrow_DownIcon, UriKind.Relative));
					break;
				case Core.OrderBy.Nummer_desc:
					this.colNummerIcon.Visibility = Visibility.Visible;
					this.colNummerIcon.Source = new BitmapImage(new Uri(Constants.Arrow_UpIcon, UriKind.Relative));
					break;
				case Core.OrderBy.Name_asc:
					this.colNameIcon.Visibility = Visibility.Visible;
					this.colNameIcon.Source = new BitmapImage(new Uri(Constants.Arrow_DownIcon, UriKind.Relative));
					break;
				case Core.OrderBy.Name_desc:
					this.colNameIcon.Visibility = Visibility.Visible;
					this.colNameIcon.Source = new BitmapImage(new Uri(Constants.Arrow_UpIcon, UriKind.Relative));
					break;
				//case Core.OrderBy.Erledigt_asc:
				//	this.colErledigtIcon.Visibility = Visibility.Visible;
				//	this.colErledigtIcon.Source = new BitmapImage(new Uri(Arrow_DownIcon, UriKind.Relative));
				//	break;
				//case Core.OrderBy.Erledigt_desc:
				//	this.colErledigtIcon.Visibility = Visibility.Visible;
				//	this.colErledigtIcon.Source = new BitmapImage(new Uri(Arrow_UpIcon, UriKind.Relative));
				//	break;
				default:
					break;
			}
		}

		private void Franchises_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.franchises.SelectedItem != null)
			{
				UserSettings.ZuletztGeoffnetesFranchise = ((Franchises4Dropdown)this.franchises.SelectedItem).Nummer;
			}
			else
			{
				UserSettings.ZuletztGeoffnetesFranchise = 0;
			}
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

			if (UserSettings.BeiProgrammstartLetztesFranchiseOeffnen && UserSettings.ZuletztGeoffnetesFranchise != 0)
			{
				foreach (var franchise in this.franchises.Items)
				{
					Franchises4Dropdown f = (Franchises4Dropdown)franchise;
					if (f.Nummer == UserSettings.ZuletztGeoffnetesFranchise)
					{
						this.franchises.SelectedItem = franchise;
						break;
					}
				}
			}
		}

		private void ActualizeData()
		{
			this.data.Children.Clear();
			bool alleCosplans = false;
			if (this.franchises.IsEnabled == true)
			{
				if (this.franchises.SelectedItem == null)
				{
					return;
				}
			}
			else
			{
				alleCosplans = true;
			}
			
			Cosplans cosplans;
			if (alleCosplans)
			{
				cosplans = this.core.GetCosplans(0);
			}
			else
			{
				cosplans = this.core.GetCosplans(((Franchises4Dropdown)this.franchises.SelectedItem).Nummer);
			}
			
			while (!cosplans.EoF)
			{
				Grid grid = new Grid()
				{
					Background = Brushes.Transparent
				};
				grid.MouseUp += Grid_MouseUp;
				grid.MouseEnter += Grid_MouseEnter;
				grid.MouseLeave += Grid_MouseLeave;

				for (int i = 0; i < 9; i++)
				{
					grid.ColumnDefinitions.Add(new ColumnDefinition());
				}

				this.ActualizeSingleRow(grid, cosplans);

				this.data.Children.Add(grid);
				cosplans.Skip();
			}
		}

		private void ActualizeSingleRow(Grid grid, Cosplans cosplans)
		{
			grid.Children.Clear();

			Label labelNummer = new Label
			{
				Content = cosplans.Nummer,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			Grid.SetColumn(labelNummer, 0);
			grid.Children.Add(labelNummer);

			Label labelName = new Label
			{
				Content = cosplans.Name,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			Grid.SetColumn(labelName, 1);
			Grid.SetColumnSpan(labelName, 8);
			grid.Children.Add(labelName);

			//CheckBox checkBoxErledigt = new CheckBox
			//{
			//	HorizontalAlignment = HorizontalAlignment.Center,
			//	VerticalAlignment = VerticalAlignment.Center
			//};
			//if (cosplans.Erledigt)
			//{
			//	checkBoxErledigt.IsChecked = true;
			//}
			//checkBoxErledigt.Click += CheckBoxErledigt_Click;
			//Grid.SetColumn(checkBoxErledigt, 7);
			//Grid.SetColumnSpan(checkBoxErledigt, 1);
			//grid.Children.Add(checkBoxErledigt);

			//Button buttonBilder = new Button
			//{
			//	HorizontalAlignment = HorizontalAlignment.Center,
			//	VerticalAlignment = VerticalAlignment.Center,
			//	Width = 40,
			//	Background = Brushes.Transparent,
			//	BorderThickness = new Thickness(0)
			//};
			//buttonBilder.Click += ButtonBilder_Click;
			//Image imageBilder = new Image
			//{
			//	Source = new BitmapImage(new Uri(PictureIcon, UriKind.Relative))
			//};
			//buttonBilder.Content = imageBilder;
			//Grid.SetColumn(buttonBilder, 8);
			//grid.Children.Add(buttonBilder);
			grid.Height = 50;
		}

		private void CheckBoxErledigt_Click(object sender, RoutedEventArgs e)
		{
			CheckBox checkBox = (CheckBox)sender;
			Grid grid = (Grid)checkBox.Parent;
			this.core.SaveCosplan(nummer: ((Label)grid.Children[0]).Content.ToInt(), erledigt: checkBox.IsChecked.ToBoolean());
		}

		private void Grid_MouseLeave(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;
			if (grid.Background != Layout.SelectedBackground)
			{
				grid.Background = Brushes.Transparent;
			}
		}

		private void Grid_MouseEnter(object sender, MouseEventArgs e)
		{
			Grid grid = (Grid)sender;
			if (grid.Background != Layout.SelectedBackground)
			{
				grid.Background = Layout.ButtonHover;
			}
		}

		private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Grid grid = (Grid)sender;

			if (this.selectedCosplan != null)
			{
				foreach (UIElement item in this.data.Children)
				{
					Grid child = (Grid)item;
					if (child.Background == Layout.SelectedBackground)
					{
						child.Background = Brushes.Transparent;
						break;
					}
				}
			}

			if (grid.Background == Layout.ButtonHover)
			{
				grid.Background = Layout.SelectedBackground;
				this.selectedCosplan = ((Label)grid.Children[0]).Content.ToInt();

				if (!Cosplan.Show(this.core, this.selectedCosplan.ToInt()))
				{
					this.ActualizeData();
				}
				else
				{
					this.Grid_MouseUp(sender, e);
				}
			}
			else
			{
				grid.Background = Layout.ButtonHover;
				this.selectedCosplan = null;
			}
		}

		private void FranchisesEnabled_Click(object sender, RoutedEventArgs e)
		{
			if (this.franchisesEnabled.IsChecked == true)
			{
				this.franchises.IsEnabled = true;
			}
			else
			{
				this.franchises.IsEnabled = false;
			}

			this.ActualizeData();
		}

		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseEnter(sender);
		}

		private void Button_MouseLeave(object sender, MouseEventArgs e)
		{
			Layout.Button_MouseLeave(sender);
		}
	}
}