using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	using ApS.Databases;

    public class Bilder : Business
    {
		public Bilder() : base("Bilder", "", "", "", false, SqlAction.Null)
		{
		}
		public Bilder(SqlAction eSqlAction) : base("Bilder", "", "", "", false, eSqlAction)
		{
		}
		public Bilder(string sWhere, string sOrder) : base("Bilder", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Bilder(string sColumns, string sWhere, string sOrder) : base("Bilder", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Bilder(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("Bilder", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public int Cosplan_Nr
		{
			get { return this.GetInt("Cosplan_Nr"); }
			set { this.Put("Cosplan_Nr", value); }
		}

		public byte[] Bild
		{
			get { return this.GetByteArray("Bild"); }
			set { this.Put("Bild", value); }
		}
	}
}