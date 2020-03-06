namespace ACP.Datenbankversionen
{
	using ApS.Databases.Firebird;

	internal class Version_0_1_3 : UpdateDatabase
	{
		private readonly string CosplansAddErstelltAm = "";
		private readonly string CosplansAddAnzeigebild = "";

		public Version_0_1_3() : base()
		{
			base.AddStatements(this.CosplansAddErstelltAm);
			base.AddStatements(this.CosplansAddAnzeigebild);
		}
	}
}