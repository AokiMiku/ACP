﻿namespace ACP_GUI
{
	using System.Windows;

	/// <summary>
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			ApS.Version.MajorVersion = 0;
			ApS.Version.MinorVersion = 3;
			ApS.Version.PatchNumber = 0;
		}
	}
}