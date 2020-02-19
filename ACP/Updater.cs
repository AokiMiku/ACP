namespace ACP
{
	using ApS.Databases.Firebird;

	public class Updater : ApS.Update.Updater
	{
		public void UpdateDatabase()
		{
			//if (!Core.CoreSettings.GetSetting("Version02Updated").ToBoolean())
			//{
			//	this.UpdateDatabase(Versionen.Version02);
			//}
		}

		private void UpdateDatabase(Versionen vers)
		{
			//switch (vers)
			//{
			//	case Versionen.Version02:
			//		Datenbankversionen.Version02 v02 = new Datenbankversionen.Version02();
			//		v02.ErrorOccured += VersionenErrorOccured;
			//		v02.RunStatements();
			//		v02.ErrorOccured -= VersionenErrorOccured;
			//		CoreSettings.SetSetting("Version02Updated", true);
			//		break;
			//	default:
			//		break;
			//}
		}

		private void VersionenErrorOccured(object sender, ErrorInUpdate e)
		{
			ApS.Services.WriteErrorLog(e.ErrorMessage);
		}

		public enum Versionen
		{
			
		}
	}
}