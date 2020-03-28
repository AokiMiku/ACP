namespace ACP
{
	using ApS;

	public class ToDos4Grid
	{
		public ToDos4Grid(ToDos copyFrom)
		{
			this.Nummer = copyFrom.Nummer;
			this.Cosplan_Nr = copyFrom.Cosplan_Nr;
			this.Kategorie_Nr = copyFrom.Kategorie_Nr;
			this.Bezeichnung = copyFrom.Bezeichnung;
			this.ProzentErledigt = copyFrom.ProzentErledigt;
			this.Kosten = copyFrom.Kosten;
			this.Zeit = copyFrom.Zeit;
		}

		public int Nummer;
		public int Cosplan_Nr;
		public int Kategorie_Nr;
		public string Bezeichnung;
		public int ProzentErledigt;
		public decimal Kosten;
		public Time Zeit;
	}
}