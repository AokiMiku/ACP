using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	public class Franchises4Dropdown
	{
		public int Nummer { get; set; }
		public string Name { get; set; }
		public int Anzahl { get; set; }

		public Franchises4Dropdown(int nummer, string name, int anzahl)
		{
			this.Nummer = nummer;
			this.Name = name;
			this.Anzahl = anzahl;
		}
		public override string ToString()
		{
			return this.Name + " (" + this.Anzahl + ")";
		}
	}
}