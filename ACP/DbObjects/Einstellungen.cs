namespace ACP
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class Einstellungen : ApS.Databases.Firebird.Einstellungen
	{
		public Einstellungen() : base("ACP.FDB")
		{

		}
	}
}