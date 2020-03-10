using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	using ApS.Databases;

    public class Cosplans : Business
    {
		public Cosplans() : base("Cosplans", "", "", "", false, SqlAction.Null)
		{
		}
		public Cosplans(SqlAction eSqlAction) : base("Cosplans", "", "", "", false, eSqlAction)
		{
		}
		public Cosplans(string sWhere, string sOrder) : base("Cosplans", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Cosplans(string sColumns, string sWhere, string sOrder) : base("Cosplans", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Cosplans(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("Cosplans", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public int Franchise_Nr
		{
			get { return this.GetInt("Franchise_Nr"); }
			set { this.Put("Franchise_Nr", value); }
		}

		public int Bild_Nr
		{
			get { return this.GetInt("Bild_Nr"); }
			set { this.Put("Bild_Nr", value); }
		}

		public string Name
		{
			get { return this.GetString("Name"); }
			set { this.Put("Name", value); }
		}

		public DateTime ErstelltAm
		{
			get { return this.GetDateTime("ErstelltAm"); }
			set { this.Put("ErstelltAm", value); }
		}

		public bool Erledigt
		{
			get { return this.GetBool("Erledigt"); }
			set { this.Put("Erledigt", value); }
		}

		public DateTime ErledigtAm
		{
			get { return this.GetDateTime("ErledigtAm"); }
			set { this.Put("ErledigtAm", value); }
		}
	}
}