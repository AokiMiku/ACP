namespace ACP
{
	public class Core
	{
		#region Felder
		public Franchises Franchises { get; set; }
		public OrderBy CosplansOrderBy { get; set; }
		#endregion

		public Core()
		{
			try
			{
				this.CosplansOrderBy = UserSettings.LetzteSortierung;
			}
			catch (System.Exception ex)
			{
				ApS.Services.WriteErrorLog(ex.Message);
				this.CosplansOrderBy = OrderBy.Nummer_asc;
			}
		}

		~Core()
		{
			UserSettings.LetzteSortierung = this.CosplansOrderBy;
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
		public void SaveCosplan(string name = "", int franchise_nr = 0, int? nummer = null, bool erledigt = false)
		{
			Cosplans cosplans = new Cosplans();
			if (!string.IsNullOrEmpty(name))
			{
				cosplans.Name = name;
			}
			if (franchise_nr > 0)
			{
				cosplans.Franchise_Nr = franchise_nr;
			}

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
			if (franchise_nr == null || franchise_nr == 0)
			{
				cosplans.Where = "Nummer is not null";
			}
			else
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

		#region ToDos
		public void SaveToDo(string bezeichnung, int cosplan_nr, int prozentErledigt, decimal kosten, ApS.Time zeit, int nummer = 0)
		{
			using (ToDos toDos = new ToDos())
			{
				if (nummer == 0)
				{
					toDos.Bezeichnung = bezeichnung;
					toDos.Cosplan_Nr = cosplan_nr;
					toDos.ProzentErledigt = prozentErledigt;
					toDos.Kosten = kosten;
					toDos.Zeit = zeit;

					toDos.Save(ApS.Databases.SqlAction.Insert);
				}
				else
				{
					toDos.Where = "Nummer = " + nummer;
					toDos.Read();

					toDos.Bezeichnung = bezeichnung;
					toDos.ProzentErledigt = prozentErledigt;
					toDos.Kosten = kosten;
					toDos.Zeit = zeit;

					toDos.Save(ApS.Databases.SqlAction.Update);
				}
			}
		}

		public ToDos GetToDos(int cosplan_nr)
		{
			ToDos toDos = new ToDos
			{
				Where = "Cosplan_Nr = " + cosplan_nr,
				OrderBy = "Kategorie_Nr asc"
			};
			toDos.Read();

			return toDos;
		}

		public void DeleteTodo(int nummer)
		{
			using (ToDos toDos = new ToDos())
			{
				toDos.Where = "Nummer = " + nummer;
				toDos.Read();

				if (!toDos.EoF)
				{
					toDos.Delete();
				}
			}
		}

		public string GetKategorie(int nummer)
		{
			ToDoKategorien kategorien = new ToDoKategorien
			{
				Where = "Nummer = " + nummer
			};

			kategorien.Read();

			if (!kategorien.EoF)
			{
				return kategorien.Bezeichnung;
			}
			else
			{
				return "";
			}
		}
		#endregion

		public static void ResetCosplanNummern()
		{
			using (Cosplans cosplans = new Cosplans())
			{
				// read highest number
				cosplans.Where = "Nummer is not null";
				cosplans.OrderBy = "Nummer desc";
				cosplans.AnzahlTop = 1;

				cosplans.Read();
				int newNummer = cosplans.Nummer + 1;

				cosplans.OrderBy = "Franchise_Nr asc, Nummer asc";
				cosplans.Read();

				while (!cosplans.EoF)
				{
					cosplans.Where = "Nummer = " + cosplans.Nummer;
					cosplans.Nummer = newNummer++;

					cosplans.Save(ApS.Databases.SqlAction.Update);

					cosplans.Skip();
				}
				// now reset all 
				cosplans.Where = "Nummer is not null";
				cosplans.Read();
				newNummer = 1;
				while (!cosplans.EoF)
				{
					cosplans.Where = "Nummer = " + cosplans.Nummer;
					cosplans.Nummer = newNummer++;

					cosplans.Save(ApS.Databases.SqlAction.Update);

					cosplans.Skip();
				}

				string stmt = "ALTER SEQUENCE GEN_COSPLANS_ID RESTART WITH " + --newNummer + ";";
				cosplans.Execute(stmt);
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