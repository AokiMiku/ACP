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
		private int? selectedToDo;

		public Cosplan()
		{
			InitializeComponent();
			this.WindowName = "Cosplan";

			Layout.Buttons.Add(this.addBild);
			Layout.Buttons.Add(this.delBild);
			Layout.Buttons.Add(this.addTodo);
			Layout.Buttons.Add(this.delTodo);
			Layout.Buttons.Add(this.delete);
		}

		~Cosplan()
		{
			Layout.Buttons.Remove(this.addBild);
			Layout.Buttons.Remove(this.delBild);
			Layout.Buttons.Remove(this.addTodo);
			Layout.Buttons.Remove(this.delTodo);
			Layout.Buttons.Remove(this.delete);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cosplan"></param>
		/// <returns>False if Cosplan got deleted. Else True.</returns>
		public static bool Show(WPFBase parent, Core core, int cosplan)
		{
			Cosplan c = new Cosplan
			{
				core = core,
				cosplan = new Cosplans()
				{
					Where = "Nummer = " + cosplan
				}
			};

			c.SetPositionToParent(parent);
			c.cosplan.Read();
			c.ShowDialog();

			return !c.geloescht;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.addBild.FindVisualChildren<Image>().First().Source = ResourceConstants.AddIcon;
			this.delBild.FindVisualChildren<Image>().First().Source = ResourceConstants.DelIcon;
			this.addTodo.FindVisualChildren<Image>().First().Source = ResourceConstants.AddIcon;
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
			Bild showBild = new Bild
			{
				ImageSource = bild.Source
			};

			showBild.SetPositionToParent(this);
			showBild.ShowDialog();
		}
		#endregion

		#region ToDos
		private void AddTodo_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void DelTodo_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void LoadToDos()
		{
			this.todos.Children.Clear();

			using (ToDos toDos = this.core.GetToDos(this.cosplan.Nummer))
			{
				//hier "Tabellenkoepfe" fuer Kategorien einfuegen 
				while (!toDos.EoF)
				{
					Grid grid = new Grid()
					{
						Background = Brushes.Transparent,
						Height = 50
					};
					grid.MouseEnter += Grid_MouseEnter;
					grid.MouseLeave += Grid_MouseLeave;

					for (int i = 0; i < 2; i++)
					{
						grid.ColumnDefinitions.Add(new ColumnDefinition());
					}

					this.ActualizeSingleToDoRow(grid, toDos);

					this.todos.Children.Add(grid);
					toDos.Skip();
				}
			}
		}

		private void ActualizeSingleToDoRow(Grid grid, ToDos toDos)
		{
			grid.Children.Clear();

			TextBlock labelNummer = new TextBlock
			{
				Text = toDos.Nummer.ToString(),
				Visibility = Visibility.Collapsed
			};
			grid.Children.Add(labelNummer);

			TextBlock labelBezeichnung = new TextBlock
			{
				Text = toDos.Bezeichnung,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
			};
			Grid.SetColumn(labelBezeichnung, 0);
			grid.Children.Add(labelBezeichnung);

			ComboBox comboProzentErledigt = new ComboBox
			{
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Center,
				Foreground = Layout.WindowForeground
			};
			for (int i = 0; i <= 100; i = i + 5)
			{
				comboProzentErledigt.Items.Add(new ComboBoxItem { Content = i + "%" });
			}

			Grid.SetColumn(comboProzentErledigt, 1);
			grid.Children.Add(comboProzentErledigt);
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