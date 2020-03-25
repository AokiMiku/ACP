namespace ACP
{
	using ApS.Databases;

    public class Franchises : Business
    {
		public Franchises() : base("Franchises", "", "", "", false, SqlAction.Null)
		{
		}
		public Franchises(SqlAction eSqlAction) : base("Franchises", "", "", "", false, eSqlAction)
		{
		}
		public Franchises(string sWhere, string sOrder) : base("Franchises", "", sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Franchises(string sColumns, string sWhere, string sOrder) : base("Franchises", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}
		public Franchises(string sColumns, string sWhere, string sOrder, string sWhereStandard) : base("Franchises", sColumns, sWhere, sOrder, true, SqlAction.Null)
		{
		}

		public string Name
		{
			get { return this.GetString("Name"); }
			set { this.Put("Name", value); }
		}
	}
}