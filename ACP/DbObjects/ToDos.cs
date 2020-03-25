namespace ACP
{
	using ApS;
	using ApS.Databases;
	using System;

	public class ToDos : Business
	{
		public ToDos() : base("ToDos", "", "", "", false, SqlAction.Null)
		{
		}
		public ToDos(SqlAction eSqlAction) : base("ToDos", "", "", "", false, eSqlAction)
		{
		}
		public ToDos(string sWhere, string sOrder) : base("ToDos", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public ToDos(string sColumns, string sWhere, string sOrder) : base("ToDos", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public ToDos(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("ToDos", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public int Cosplan_Nr
		{
			get { return this.GetInt("Cosplan_Nr"); }
			set { this.Put("Cosplan_Nr", value); }
		}

		public int Kategorie_Nr
		{
			get { return this.GetInt("Kategorie_Nr"); }
			set { this.Put("Kategorie_Nr", value); }
		}

		public string Bezeichnung
		{
			get { return this.GetString("Bezeichnung"); }
			set { this.Put("Bezeichnung", value); }
		}

		public int ProzentErledigt
		{
			get { return this.GetInt("ProzentErledigt"); }
			set { this.Put("ProzentErledigt", value); }
		}

		public decimal Kosten
		{
			get { return this.GetDecimal("Kosten"); }
			set { this.Put("Kosten", value); }
		}

		public Time Zeit
		{
			get { return this.GetTime("Zeit"); }
			set { this.Put("Zeit", value); }
		}
	}
}