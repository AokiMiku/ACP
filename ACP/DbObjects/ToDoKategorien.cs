namespace ACP
{
	using ApS.Databases;

	public class ToDoKategorien : Business
	{
		public ToDoKategorien() : base("ToDoKategorien", "", "", "", false, SqlAction.Null)
		{
		}
		public ToDoKategorien(SqlAction eSqlAction) : base("ToDoKategorien", "", "", "", false, eSqlAction)
		{
		}
		public ToDoKategorien(string sWhere, string sOrder) : base("ToDoKategorien", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public ToDoKategorien(string sColumns, string sWhere, string sOrder) : base("ToDoKategorien", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public ToDoKategorien(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("ToDoKategorien", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public string Bezeichnung
		{
			get { return this.GetString("Bezeichnung"); }
			set { this.Put("Bezeichnung", value); }
		}
	}
}