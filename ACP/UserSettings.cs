using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	using ApS;

	public class UserSettings
	{
		#region Allgemein
		public static bool BeiProgrammstartLetztesFranchiseOeffnen
		{
			get { return Core.UserSettings.GetSetting("BeiProgrammstartLetztesFranchiseOeffnen").ToBoolean(); }
			set { Core.UserSettings.SetSetting("BeiProgrammstartLetztesFranchiseOeffnen", value); }
		}


		#endregion
	}
}