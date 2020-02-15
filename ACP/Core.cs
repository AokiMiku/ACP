using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACP
{
	using FirebirdSql.Data.FirebirdClient;
	using ApS;

	public class Core
	{
		#region Felder
		public Franchises Franchises { get; set; }
		#endregion

		public Core()
		{
			FbConnectionStringBuilder fbConnString = new FbConnectionStringBuilder
			{
				ServerType = FbServerType.Embedded,
				UserID = "sysdba",
				Password = " ",

				Database = Services.GetAppDir() + @"\Datenbank\ACP.fdb"
			};
			ApS.Databases.Settings.ConnectionString = fbConnString.ToString();
		}

		#region Franchises
		public void SaveFranchise(string name)
		{
			Franchises franchises = new Franchises
			{
				Name = name
			};

			franchises.Save(ApS.Databases.SqlAction.Insert);
		}

		public Franchises GetFranchises()
		{
			this.Franchises = new Franchises();
			this.Franchises.Where = "Nummer is not null";
			this.Franchises.Read();
			return this.Franchises;
		}

		public Franchises GetFranchise(int nummer)
		{
			Franchises franchises = new Franchises
			{
				Where = "Nummer = " + nummer
			};

			franchises.Read();
			return franchises;
		}

		public void DeleteFranchise(int nummer)
		{
			Franchises franchises = new Franchises
			{
				Where = "Nummer = " + nummer
			};

			franchises.Read();
			if (!franchises.EoF)
			{
				franchises.Delete();
			}
		}
		#endregion

		#region Cosplans
		public void SaveCosplan(string name, int franchise_nr, int? nummer = null, bool erledigt = false)
		{
			Cosplans cosplans = new Cosplans
			{
				Name = name,
				Franchise_Nr = franchise_nr
			};

			if (nummer == null)
			{
				cosplans.Erledigt = false;
				cosplans.Save(ApS.Databases.SqlAction.Insert);
			}
			else
			{
				cosplans.Where = "Nummer = " + nummer;
				cosplans.Erledigt = erledigt;
				cosplans.Save(ApS.Databases.SqlAction.Update);
			}
		}

		public Cosplans GetCosplans(int? franchise_nr = null)
		{
			Cosplans cosplans = new Cosplans();
			if (franchise_nr != null)
			{
				cosplans.Where = "Franchise_Nr = " + franchise_nr;
			}
			cosplans.Read();
			cosplans.GoTop();

			return cosplans;
		}

		public void DeleteCosplan(int nummer)
		{
			Cosplans cosplans = new Cosplans
			{
				Where = "Nummer = " + nummer
			};

			cosplans.Read();
			if (!cosplans.EoF)
			{
				cosplans.Delete();
			}
		}
		#endregion

		#region Bilder
		public void SaveBild(int cosplan_nr, byte[] bild)
		{
			Bilder bilder = new Bilder
			{
				Cosplan_Nr = cosplan_nr,
				Bild = bild
			};

			bilder.Save(ApS.Databases.SqlAction.Insert);
		}

		public Bilder GetBilder(int cosplan_nr)
		{
			Bilder bilder = new Bilder
			{
				Where = "Cosplan_Nr = " + cosplan_nr
			};

			bilder.Read();
			return bilder;
		}

		public void DeleteBild(int nummer)
		{
			Bilder bilder = new Bilder
			{
				Where = "Nummer = " + nummer
			};

			bilder.Read();
			if (!bilder.EoF)
			{
				bilder.Delete();
			}
		}
		#endregion
	}
}