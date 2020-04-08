using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	public class ToDoKategorien4Dropdown
	{
		public int Nummer { get; set; }
		public string Bezeichnung { get; set; }

		public ToDoKategorien4Dropdown(ToDoKategorien kategorien)
		{
			this.Nummer = kategorien.Nummer;
			this.Bezeichnung = kategorien.Bezeichnung;
		}

		public override string ToString()
		{
			return this.Bezeichnung;
		}
	}
}