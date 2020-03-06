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
		public Franchises4Dropdown(int nummer, string name)
		{
			this.Nummer = nummer;
			this.Name = name;
		}
		public override string ToString()
		{
			return this.Name;
		}
	}
}