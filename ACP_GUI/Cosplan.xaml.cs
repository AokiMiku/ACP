namespace ACP_GUI
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using Microsoft.Win32;

	using ACP;
	using GUI_Bases;
	using ApS;
	using ApS.WPF;

	/// <summary>
	/// Interaktionslogik für Cosplan.xaml
	/// </summary>
	public partial class Cosplan : Base4Windows
	{
		private Cosplans cosplan = null;
		private Core core = null;
		private bool geloescht = false;
		private int bilderRow = 0;
		private int bilderCountInLastRow = 0;

		public Cosplan(WPFBase parent) : base(parent)
		{
			this.WindowName = "Cosplan";
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cosplan"></param>
		/// <returns>False if Cosplan got deleted. Else True.</returns>
		public static bool Show(WPFBase parent, Core core, int cosplan)
		{
			Cosplan c = new Cosplan(parent)
			{
				core = core,
				cosplan = new Cosplans()
				{
					Where = "Nummer = " + cosplan
				}
			};
			
			c.cosplan.Read();
			c.ShowDialog();

			return !c.geloescht;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.addBild.ImageSource = ResourceConstants.AddIcon;
			this.delBild.ImageSource = ResourceConstants.DelIcon;
			this.addTodo.ImageSource = ResourceConstants.AddIcon;
			this.TodoTab.FindVisualChildren<Image>().First().Source = ResourceConstants.ToDoIcon;
			this.BilderTab.FindVisualChildren<Image>().First().Source = ResourceConstants.PictureIcon;

			this.cosplanName.Text = this.cosplan.Name;

			if (this.cosplan.ErstelltAm != Settings.NullDate)
			{
				this.erstelltDatum.Text = this.cosplan.ErstelltAm.ToShortDateString();
			}
			else
			{
				this.cosplan.ErstelltAm = DateTime.Today;
				this.erstelltDatum.Text = DateTime.Today.ToShortDateString();

				this.cosplan.Save(ApS.Databases.SqlAction.Update);
				this.cosplan.Read();
			}

			this.SetErledigt(this.cosplan.Erledigt);

			this.LoadBilder();
			this.LoadToDos();
		}

		private void Erledigt_Click(object sender, RoutedEventArgs e)
		{
			this.cosplan.Erledigt = this.erledigt.IsChecked.ToBoolean();
			if (this.erledigt.IsChecked.ToBoolean())
			{
				this.cosplan.ErledigtAm = DateTime.Today;
			}
			else
			{
				this.cosplan.ErledigtAm = Settings.NullDate;
			}
			this.cosplan.Save(ApS.Databases.SqlAction.Update);
			this.cosplan.Read();
			this.SetErledigt(this.cosplan.Erledigt);
		}

		private void Erledigt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.erledigt.IsChecked = !this.erledigt.IsChecked;
			this.Erledigt_Click(sender, e);
		}

		private void SetErledigt(bool erledigt)
		{
			this.erledigt.IsChecked = erledigt;
			if (erledigt)
			{
				this.erledigtDatum.Text = this.cosplan.ErledigtAm.ToShortDateString();
				this.erledigtDatum.Visibility = Visibility.Visible;
			}
			else
			{
				this.erledigtDatum.Visibility = Visibility.Collapsed;
			}
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show(Constants.MessageBoxDeleteCosplan, Constants.CaptionDelete, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				this.core.DeleteCosplan(this.cosplan.Nummer);
				this.geloescht = true;
				this.Close();
			}
		}

		#region Bilder
		private void AddBild_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog selectBild = new OpenFileDialog
			{
				Title = Constants.OpenFileDialogBilder,
				Multiselect = true,
				Filter = "Bilder (*.png, *.jpeg, *.jpg)|*.png;*.jpeg;*.jpg"
			};

			if (selectBild.ShowDialog(this).ToBoolean())
			{
				foreach (string file in selectBild.FileNames)
				{
					using (Bilder bilder = new Bilder())
					{
						bilder.Cosplan_Nr = this.cosplan.Nummer;
						bilder.Save(ApS.Databases.SqlAction.Insert);

						bilder.AnzahlTop = 1;
						bilder.Where = "Nummer is not null";
						bilder.OrderBy = "Nummer desc";
						bilder.Read();

						bilder.Where = "Nummer = " + bilder.Nummer;
						bilder.Bild = File.ReadAllBytes(file);
						bilder.Save(ApS.Databases.SqlAction.Update);
					}
				}

				this.LoadBilder();
			}
		}

		private void DelBild_Click(object sender, RoutedEventArgs e)
		{

		}

		private void LoadBilder()
		{
			this.bilderCountInLastRow = 0;
			this.bilderRow = 0;
			this.bilder.Children.Clear();

			DockPanel bildRow = new DockPanel();
			Border rowBorder = new Border
			{
				BorderBrush = Brushes.Black,
				BorderThickness = new Thickness(2, 3, 2, 3)
			};
			rowBorder.Child = bildRow;
			this.bilder.Children.Add(rowBorder);

			using (Bilder bilder = this.core.GetBilder(this.cosplan.Nummer))
			{
				while (!bilder.EoF)
				{
					Image bild = new Image()
					{
						Source = ByteToImageConverter.ConvertByteArrayToBitMapImage(bilder.Bild)
					};
					bild.MouseUp += Bild_MouseUp;

					DockPanel bildContainer = new DockPanel
					{
						Background = Brushes.Gray,
						Margin = new Thickness(3),
						Width = 200
					};
					//bildContainer.MouseUp += BildContainer_MouseUp;
					bildContainer.Children.Add(bild);

					if (this.bilderCountInLastRow >= 3)
					{
						this.bilderCountInLastRow = 1;
						this.bilderRow++;
						bildRow = new DockPanel();
						rowBorder = new Border
						{
							BorderBrush = Brushes.Black,
							BorderThickness = new Thickness(2, 3, 2, 3)
						};
						rowBorder.Child = bildRow;
						this.bilder.Children.Add(rowBorder);
					}
					else
					{
						this.bilderCountInLastRow++;
					}

					Border bildBorder = new Border
					{
						BorderBrush = Brushes.Black,
						BorderThickness = new Thickness(1, 0, 1, 0)
					};
					bildBorder.Child = bildContainer;
					((DockPanel)((Border)this.bilder.Children[this.bilderRow]).Child).Children.Add(bildBorder);

					bilder.Skip();
				}
			}
		}

		private void Bild_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (!(sender is Image))
			{
				return;
			}

			Image bild = (Image)sender;
			Bild showBild = new Bild(this)
			{
				ImageSource = bild.Source
			};
			showBild.ShowDialog();
		}
		#endregion

		#region ToDos
		private void AddTodo_Click(object sender, MouseButtonEventArgs e)
		{
			if (EditToDo.Show(this.core, this, this.cosplan.Nummer))
			{
				this.LoadToDos();
			}
		}

		private void DelTodo_Click(object sender, MouseButtonEventArgs e)
		{
			CustomButton button = (CustomButton)sender;
			if (MessageBox.Show(Constants.MessageBoxDeleteToDo, Constants.CaptionDelete, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				this.core.DeleteTodo(((ToDos4Grid)((GridExtended)button.Parent).DataObject).Nummer);
				this.LoadToDos();
			}
		}

		private void LoadToDos()
		{
			this.todos.Children.Clear();

			using (ToDos toDos = this.core.GetToDos(this.cosplan.Nummer))
			{
				int kategorie = 0;
				int columnCount = 9;
				//hier "Tabellenkoepfe" fuer Kategorien einfuegen 
				while (!toDos.EoF)
				{
					if (kategorie != toDos.Kategorie_Nr)
					{
						kategorie = toDos.Kategorie_Nr;

						Grid header = new Grid
						{
							Background = Brushes.Transparent,
							Height = 20
						};

						for (int i = 0; i <= columnCount; i++)
						{
							header.ColumnDefinitions.Add(new ColumnDefinition());
						}
						this.CreateTableHeader(header, kategorie);
						this.todos.Children.Add(header);
					}
					GridExtended grid = new GridExtended
					{
						Background = Brushes.Transparent,
						Height = 50
					};
					grid.MouseEnter += Grid_MouseEnter;
					grid.MouseLeave += Grid_MouseLeave;
					grid.MouseUp += Grid_MouseUp;
					grid.EditCompleted += Grid_EditCompleted;

					for (int i = 0; i <= columnCount; i++)
					{
						grid.ColumnDefinitions.Add(new ColumnDefinition());
					}

					this.ActualizeSingleToDoRow(grid, toDos);

					this.todos.Children.Add(grid);
					toDos.Skip();
				}
			}
		}

		private void CreateTableHeader(Grid grid, int kategorie)
		{
			if (kategorie <= 0)
			{
				return;
			}

			grid.Children.Clear();

			TextBlock labelBezeichnung = new TextBlock
			{
				Text = this.core.GetKategorie(kategorie) + ":",
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
			};
			Grid.SetColumn(labelBezeichnung, 0);
			Grid.SetColumnSpan(labelBezeichnung, 5);
			grid.Children.Add(labelBezeichnung);

			if (labelBezeichnung.Text == "Kaufen:")
			{
				TextBlock labelKosten = new TextBlock
				{
					Text = "Kosten",
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Center,
					Foreground = Layout.WindowForeground
				};
				Grid.SetColumn(labelKosten, 5);
				Grid.SetColumnSpan(labelKosten, 2);
				grid.Children.Add(labelKosten);
			}
			else if (labelBezeichnung.Text == "Machen:")
			{
				TextBlock labelZeit = new TextBlock
				{
					Text = "Zeit",
					HorizontalAlignment = HorizontalAlignment.Left,
					VerticalAlignment = VerticalAlignment.Center,
					Foreground = Layout.WindowForeground
				};
				Grid.SetColumn(labelZeit, 5);
				Grid.SetColumnSpan(labelZeit, 2);
				grid.Children.Add(labelZeit);
			}

			TextBlock labelErledigt = new TextBlock
			{
				Text = "Erledigt",
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
			};
			Grid.SetColumn(labelErledigt, 7);
			Grid.SetColumnSpan(labelErledigt, 2);
			grid.Children.Add(labelErledigt);
		}

		private void ActualizeSingleToDoRow(GridExtended grid, ToDos toDos)
		{
			grid.Children.Clear();
			grid.DataObject = new ToDos4Grid(toDos);

			TextBlock labelNummer = new TextBlock
			{
				Text = toDos.Nummer.ToString(),
				Visibility = Visibility.Collapsed
			};
			grid.Add2Children(labelNummer, true);

			TextBlock labelBezeichnung = new TextBlock
			{
				Text = toDos.Bezeichnung,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
			};
			Grid.SetColumn(labelBezeichnung, 0);
			Grid.SetColumnSpan(labelBezeichnung, 5);
			grid.Add2Children(labelBezeichnung);

			if (this.core.GetKategorie(toDos.Kategorie_Nr) == "Kaufen")
			{
				TextBlock labelKosten = new TextBlock
				{
					Text = toDos.Kosten.ToString("#.##") + " €",
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Center,
					Foreground = Layout.WindowForeground
				};

				Grid.SetColumn(labelKosten, 5);
				Grid.SetColumnSpan(labelKosten, 2);
				grid.Add2Children(labelKosten);
			}
			else if (this.core.GetKategorie(toDos.Kategorie_Nr) == "Machen")
			{
				//TODO: create custom user control to handle custom type Time properly
				TextBlock labelZeit = new TextBlock
				{
					Text = toDos.Zeit.ToString(),
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Center,
					Foreground = Layout.WindowForeground
				};

				Grid.SetColumn(labelZeit, 5);
				Grid.SetColumnSpan(labelZeit, 2);
				grid.Add2Children(labelZeit);

				ComboBox comboProzentErledigt = new ComboBox
				{
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Center,
					Foreground = Layout.WindowForeground
				};
				for (int i = 0; i <= 100; i = i + 5)
				{
					comboProzentErledigt.Items.Add(new ComboBoxItem { Content = i + "%" });
				}

				Grid.SetColumn(comboProzentErledigt, 7);
				Grid.SetColumnSpan(comboProzentErledigt, 2);
				grid.Add2Children(comboProzentErledigt);
			}

			CustomButton buttonDelToDo = new CustomButton
			{
				Text = "",
				ImageSource = ResourceConstants.DelIcon,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};
			buttonDelToDo.Click += DelTodo_Click;
			Grid.SetColumn(buttonDelToDo, 9);
			grid.Children.Add(buttonDelToDo);
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
			((GridExtended)sender).Edit = true;
		}

		private void Grid_EditCompleted(object sender, EventArgs e)
		{
			GridExtended grid = ((GridExtended)sender);

			if (grid.Edit)
			{
				string bezeichnung = grid.FindVisualChildren<TextBox>().ElementAt(0).Text;
				int erledigt = 0;
				decimal kosten = 0;
				Time zeit = null;

				ToDos4Grid toDos4Grid = (ToDos4Grid)grid.DataObject;
				if (this.core.GetKategorie(toDos4Grid.Kategorie_Nr) == "Kaufen")
				{
					kosten = grid.FindVisualChildren<TextBox>().ElementAt(1).Text.Replace(" €", "").ToDecimal();
				}
				else if (this.core.GetKategorie(toDos4Grid.Kategorie_Nr) == "Machen")
				{
					zeit = new Time(grid.FindVisualChildren<TextBox>().ElementAt(1).Text);
				}

				this.core.SaveToDo(bezeichnung, toDos4Grid.Cosplan_Nr, erledigt, kosten, zeit, toDos4Grid.Nummer);
			}
		}
		#endregion

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