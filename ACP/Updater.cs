namespace ACP
{
	using ApS;
	using ApS.Databases.Firebird;

	public class Updater : ApS.Update.Updater
	{
		public void UpdateDatabase()
		{
			if (!UserSettings.Settings.GetSetting("V0_1_3_Updated").ToBoolean())
			{
				this.UpdateDatabase(Versionen.V0_1_3);
			}
		}

		private void UpdateDatabase(Versionen vers)
		{
			this.Error = false;
			switch (vers)
			{
				case Versionen.V0_1_3:
					Datenbankversionen.Version_0_1_3 v013 = new Datenbankversionen.Version_0_1_3();
					v013.ErrorOccured += VersionenErrorOccured;
					v013.RunStatements();
					v013.ErrorOccured -= VersionenErrorOccured;
					if (!this.Error)
					{
						UserSettings.Settings.SetSetting("V0_1_3_Updated", true);
					}
					break;
				default:
					break;
			}
		}

		private void VersionenErrorOccured(object sender, ErrorInUpdate e)
		{
			this.Error = true;
			Services.WriteErrorLog(e.ErrorMessage);
		}

		public enum Versionen
		{
			V0_1_3
		}
	}
}