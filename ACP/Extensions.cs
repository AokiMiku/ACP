namespace ACP
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	
	public static class Extensions
	{
		#region Constants
		private const string ExMessageOrderByNotFound = "String.ToOrderBy::order by not found";
		private const string ExMessageOrderByStringEmpty = "String.ToOrderBy::string is empty";
		#endregion

		public static Core.OrderBy ToOrderBy(this string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				switch (s)
				{
					case "Nummer_asc":
						return Core.OrderBy.Nummer_asc;
					case "Nummer_desc":
						return Core.OrderBy.Nummer_desc;
					case "Name_asc":
						return Core.OrderBy.Name_asc;
					case "Name_desc":
						return Core.OrderBy.Name_desc;
					case "Erledigt_asc":
						return Core.OrderBy.Erledigt_asc;
					case "Erledigt_desc":
						return Core.OrderBy.Erledigt_desc;
					default:
						throw new Exception(ExMessageOrderByNotFound);
				}
			}
			else
			{
				throw new Exception(ExMessageOrderByStringEmpty);
			}
		}
	}
}