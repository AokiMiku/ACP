namespace ACP_GUI
{
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
	using System.Windows.Shapes;

	using ACP;
	using ApS.WPF;
	using GUI_Bases;

	/// <summary>
	/// Interaction logic for EditToDo.xaml
	/// </summary>
	public partial class EditToDo : Base4Windows
	{
		private Core core = null;
		private int cosplan_nr;

		public bool Saved { get; private set; } = false;

		public EditToDo(WPFBase parent) : base(parent)
		{
			InitializeComponent();

			this.Fenstergroessen = false;
			this.WindowName = "EditToDo";

			this.saveCancel.Speichern += SaveCancel_Speichern;
			this.saveCancel.Abbrechen += SaveCancel_Abbrechen;
		}

		private void Base4Windows_Loaded(object sender, RoutedEventArgs e)
		{
			ToDoKategorien kat = this.core.GetKategorien();
			while (!kat.EoF)
			{
				this.kategorien.Items.Add(new ToDoKategorien4Dropdown(kat));
				kat.Skip();
			}
		}

		public static bool Show(Core core, WPFBase parent, int cosplan_nr)
		{
			EditToDo edit = new EditToDo(parent)
			{
				core = core,
				cosplan_nr = cosplan_nr
			};
			edit.ShowDialog();
			return edit.Saved;
		}

		private void SaveCancel_Speichern(object sender, RoutedEventArgs e)
		{
			ToDos toDos = new ToDos();
			toDos.Bezeichnung = this.bezeichnung.Text;
			toDos.Kategorie_Nr = ((ToDoKategorien4Dropdown)this.kategorien.SelectedItem).Nummer;
			toDos.Cosplan_Nr = this.cosplan_nr;
			toDos.Save(ApS.Databases.SqlAction.Insert);

			this.Saved = true;
			this.Close();
		}

		private void SaveCancel_Abbrechen(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}