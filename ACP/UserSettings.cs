namespace ACP
{
	using ApS;
	using System;

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

		public static int ZuletztGeoffnetesFranchise
		{
			get { return Settings.GetSetting("ZuletztGeoffnetesFranchise").ToInt(); }
			set { Settings.SetSetting("ZuletztGeoffnetesFranchise", value); }
		}

		public static bool FenstergroesseMerken
		{
			get { return Settings.GetSetting("FenstergroesseMerken").ToBoolean(); }
			set { Settings.SetSetting("FenstergroesseMerken", value); }
		}

		public static bool LetzteSortierungMerken
		{
			get { return Settings.GetSetting("LetzteSortierungMerken").ToBoolean(); }
			set { Settings.SetSetting("LetzteSortierungMerken", value); }
		}

		public static Core.OrderBy LetzteSortierung
		{
			get { return Settings.GetSetting("LetzteSortierung").ToOrderBy(); }
			set { Settings.SetSetting("LetzteSortierung", value.ToString()); }
		}
		#endregion

		#region Farben
		public static string ButtonHover
		{
			get { return Settings.GetSetting("ButtonHover"); }
			set { Settings.SetSetting("ButtonHover", value); }
		}

		public static string ButtonHoverDefault
		{
			get { return "Lavender"; }
		}

		public static string ButtonBackground
		{
			get { return Settings.GetSetting("ButtonBackground"); }
			set { Settings.SetSetting("ButtonBackground", value); }
		}

		public static string ButtonBackgroundDefault
		{
			get { return "Transparent"; }
		}

		public static string SelectedBackground
		{
			get { return Settings.GetSetting("SelectedBackground"); }
			set { Settings.SetSetting("SelectedBackground", value); }
		}

		public static string SelectedBackgroundDefault
		{
			get { return "MediumPurple"; }
		}
		#endregion

		#region Updates
		public static bool Updates
		{
			get { return Settings.GetSetting("Updates").ToBoolean(); }
			set { Settings.SetSetting("Updates", value); }
		}

		public static DateTime LetztesUpdateAm
		{
			get { return Settings.GetSetting("LetztesUpdateAm").ToDateTime(); }
			set { Settings.SetSetting("LetztesUpdateAm", value); }
		}

		public static int UpdateAlleXTage
		{
			get { return Settings.GetSetting("UpdateAlleXTage").ToInt(); }
			set { Settings.SetSetting("UpdateAlleXTage", value); }
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