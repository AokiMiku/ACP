namespace ACP
{
	using System;

	using ApS;
	using ApS.Databases.Firebird;

	public class Updater : ApS.Update.Updater
	{
		public void UpdateDatabase()
		{
			UpdateDatabase("V0_1_3");
			UpdateDatabase("V0_3_0");
		}

		private void UpdateDatabase(Versionen vers)
		{
			this.Error = false;
			UpdateDatabase v = null;
			switch (vers)
			{
				case Versionen.V0_1_3:
					v = new Datenbankversionen.Version_0_1_3();
					break;
				case Versionen.V0_3_0:
					v = new Datenbankversionen.Version_0_3_0();
					break;
				default:
					break;
			}

			if (v != null)
			{
				v.ErrorOccured += VersionenErrorOccured;
				v.RunStatements();
				v.ErrorOccured -= VersionenErrorOccured;
				if (!this.Error)
				{
					UserSettings.Settings.SetSetting(vers.ToString() + "_Updated", true);
				}
			}
		}

		private void UpdateDatabase(string vers)
		{
			if (!UserSettings.Settings.GetSetting(vers + "_Updated").ToBoolean())
			{
				foreach (Versionen item in (Versionen[])Enum.GetValues(typeof(Versionen)))
				{
					if (item.ToString() == vers)
					{
						UpdateDatabase(item);
					}
				}
			}
		}

		private void VersionenErrorOccured(object sender, ErrorInUpdate e)
		{
			this.Error = true;
			Services.WriteErrorLog(e.ErrorMessage);
		}

		public enum Versionen
		{
			V0_1_3,
			V0_3_0
		}
	}
}