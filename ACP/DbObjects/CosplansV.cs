using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	using ApS.Databases;

    public class CosplansV : Business
    {
		public CosplansV() : base("CosplansV", "", "", "", false, SqlAction.Null)
		{
		}
		public CosplansV(SqlAction eSqlAction) : base("CosplansV", "", "", "", false, eSqlAction)
		{
		}
		public CosplansV(string sWhere, string sOrder) : base("CosplansV", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public CosplansV(string sColumns, string sWhere, string sOrder) : base("CosplansV", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public CosplansV(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("CosplansV", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public string Franchise
		{
			get { return this.GetString("Franchise"); }
		}

		public string Cosplan
		{
			get { return this.GetString("Cosplan"); }
		}

		public bool Erledigt
		{
			get { return this.GetBool("Erledigt"); }
		}
	}
}