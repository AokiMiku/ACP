namespace ACP
{
	public class Core
	{
		#region Felder
		public Franchises Franchises { get; set; }
		public OrderBy CosplansOrderBy { get; set; }
		#endregion

		public Core()
		{ }

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
			this.Franchises = new Franchises
			{
				Where = "Nummer is not null"
			};
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
			cosplans.OrderBy = this.CosplansOrderBy.ToString().Replace("_", " ");
			cosplans.Read();

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

		public static void ResetCosplanNummern()
		{
			using (Cosplans cosplans = new Cosplans())
			{
				cosplans.Where = "Nummer is not null";
				cosplans.OrderBy = "Nummer asc";
				cosplans.Read();

				int newNummer = 1;

				while (!cosplans.EoF)
				{
					cosplans.Where = "Nummer = " + cosplans.Nummer;
					cosplans.Nummer = newNummer;

					cosplans.Save(ApS.Databases.SqlAction.Update);

					newNummer++;
					cosplans.Skip();
				}
				string stmt = "ALTER SEQUENCE GEN_COSPLANS_ID RESTART WITH " + --newNummer + ";";
				cosplans.FbSave.Execute(stmt);
			}
		}

		public enum OrderBy
		{
			Nummer_asc,
			Nummer_desc,
			Name_asc,
			Name_desc,
			Erledigt_asc,
			Erledigt_desc
		}
	}
}