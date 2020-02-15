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

namespace ACP_GUI
{
	/// <summary>
	/// Interaktionslogik für AddFranchise.xaml
	/// </summary>
	public partial class AddFranchise : Window
	{
		private string name = "";

		public AddFranchise()
		{
			InitializeComponent();
		}

		private void SaveCancel_Speichern(object sender, RoutedEventArgs e)
		{
			this.name = this.franchiseName.Text;
			this.Close();
		}

		private void SaveCancel_Abbrechen(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		public static string ShowModal()
		{
			AddFranchise addFranchise = new AddFranchise();
			addFranchise.ShowDialog();

			return addFranchise.name;
		}
	}
}
