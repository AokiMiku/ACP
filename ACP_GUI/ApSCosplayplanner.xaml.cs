﻿namespace ACP_GUI
{
	using System;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	using ACP;
	using GUI_Bases;
	using ApS;
	using ApS.WPF;

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class ApSCosplayplanner : Base4Windows
	{
		private Core core;
		private int? selectedCosplan;

		public ApSCosplayplanner() : base(null)
		{
			this.WindowName = "MainWindow";
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.addFranchise.ImageSource = ResourceConstants.AddIcon;
			this.delFranchise.ImageSource = ResourceConstants.DelIcon;
			this.addCosplan.ImageSource = ResourceConstants.AddIcon;
			this.Einstellungen.ImageSource = ResourceConstants.WheelIcon;

			this.version.Text = ApS.Version.StringAppVersion;

			if (UserSettings.Updates && UserSettings.LetztesUpdateAm.AddDays(UserSettings.UpdateAlleXTage) <= DateTime.Now.Date)
			{
				ACP_GUI.Updater.CheckForUpdate();
			}

			ACP.Updater updater = new ACP.Updater();
			updater.UpdateDatabase();

			this.core = new Core();

			if (!UserSettings.Settings.GetSetting("ErstelltAm_Fix").ToBoolean())
			{
				using (Cosplans c = this.core.GetCosplans())
				{
					c.Where = "ErstelltAm = '17.11.1858'";
					c.ErstelltAm = DateTime.Today;
					c.Save(ApS.Databases.SqlAction.Update);
					UserSettings.Settings.SetSetting("ErstelltAm_Fix", true);
				}
			}

			this.ActualizeFanchises();
			if (UserSettings.BeiProgrammstartLetztesFranchiseOeffnen && UserSettings.ZuletztGeoffnetesFranchise != 0)
			{
				this.SelectFranchise(UserSettings.ZuletztGeoffnetesFranchise);
				this.ActualizeData();
			}
			this.SetSortingIcons();
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
				int franchise = ((Franchises4Dropdown)this.franchises.SelectedItem).Nummer;
				this.core.SaveCosplan(((TextBox)sender).Text, franchise);

				this.data.Children.RemoveAt(0);

				this.ActualizeFanchises();
				this.SelectFranchise(franchise);

				this.ActualizeData();
			}
			else if (e.Key == Key.Escape)
			{
				this.data.Children.RemoveAt(0);
			}
		}

		private void Einstellungen_Click(object sender, RoutedEventArgs e)
		{
			Einstellungen einstellungen = new Einstellungen(this);
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
			this.colNummer.ImageVisibility = Visibility.Hidden;
			this.colName.ImageVisibility = Visibility.Hidden;
			//this.colErledigtIcon.Visibility = Visibility.Hidden;

			switch (this.core.CosplansOrderBy)
			{
				case Core.OrderBy.Nummer_asc:
					this.colNummer.ImageVisibility = Visibility.Visible;
					this.colNummer.ImageSource = ResourceConstants.Arrow_DownIcon;
					break;
				case Core.OrderBy.Nummer_desc:
					this.colNummer.ImageVisibility = Visibility.Visible;
					this.colNummer.ImageSource = ResourceConstants.Arrow_UpIcon;
					break;
				case Core.OrderBy.Name_asc:
					this.colName.ImageVisibility = Visibility.Visible;
					this.colName.ImageSource = ResourceConstants.Arrow_DownIcon;
					break;
				case Core.OrderBy.Name_desc:
					this.colName.ImageVisibility = Visibility.Visible;
					this.colName.ImageSource = ResourceConstants.Arrow_UpIcon;
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

		private void SelectFranchise(int nummer)
		{
			foreach (var franchise in this.franchises.Items)
			{
				Franchises4Dropdown f = (Franchises4Dropdown)franchise;
				if (f.Nummer == nummer)
				{
					this.franchises.SelectedItem = franchise;
					break;
				}
			}
		}

		private void ActualizeFanchises()
		{
			this.franchises.Items.Clear();

			this.core.GetFranchises();

			while (!this.core.Franchises.EoF)
			{
				int count = 0;
				using (Cosplans cosplans = new Cosplans())
				{
					cosplans.Where = "Franchise_Nr = " + this.core.Franchises.Nummer;
					cosplans.Read();

					count = cosplans.RecordCount;
				}
				this.franchises.Items.Add(new Franchises4Dropdown(this.core.Franchises.Nummer, this.core.Franchises.Name, count));
				this.core.Franchises.Skip();
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
				((Franchises4Dropdown)this.franchises.SelectedItem).Anzahl = cosplans.RecordCount;
			}
			
			while (!cosplans.EoF)
			{
				Grid grid = new Grid()
				{
					Background = Brushes.Transparent,
					Height = 50
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

			TextBlock labelNummer = new TextBlock
			{
				Text = cosplans.Nummer.ToString(),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
			};
			Grid.SetColumn(labelNummer, 0);
			grid.Children.Add(labelNummer);

			TextBlock labelName = new TextBlock
			{
				Text = cosplans.Name,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
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
				this.selectedCosplan = ((TextBlock)grid.Children[0]).Text.ToInt();

				if (!Cosplan.Show(this, this.core, this.selectedCosplan.ToInt()))
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