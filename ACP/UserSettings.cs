using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	using ApS;

	public static class UserSettings
	{
		public static Einstellungen Settings { get; set; }

		static UserSettings()
		{
			Settings = new Einstellungen();
		}
		#region Allgemein
		public static bool BeiProgrammstartLetztesFranchiseOeffnen
		{
			get { return Settings.GetSetting("BeiProgrammstartLetztesFranchiseOeffnen").ToBoolean(); }
			set { Settings.SetSetting("BeiProgrammstartLetztesFranchiseOeffnen", value); }
		}

		public static bool FenstergroesseMerken
		{
			get { return Settings.GetSetting("FenstergroesseMerken").ToBoolean(); }
			set { Settings.SetSetting("FenstergroesseMerken", value); }
		}
		#endregion

		#region Fenstergroessen
		public static void SaveWidth(string windowName, int width)
		{
			Settings.SetSetting(windowName + "_Width", width);
		}

		public static void SaveHeight(string windowName, int height)
		{
			Settings.SetSetting(windowName + "_Height", height);
		}

		public static int GetWidth(string windowName)
		{
			return Settings.GetSetting(windowName + "_Width").ToInt();
		}

		public static int GetHeight(string windowName)
		{
			return Settings.GetSetting(windowName + "_Height").ToInt();
		}
		#endregion
	}
}