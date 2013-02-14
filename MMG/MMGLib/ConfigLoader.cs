using System;
using System.IO;
using System.Collections;

namespace MMG.Config
{
	/// <summary>
	/// Loads the initialization values from a configuration file.
	/// </summary>
	public class ConfigLoader
	{
		public static ConfigValues LoadConfig(string path) {
			Hashtable properties = new Hashtable();

			using (StreamReader sr = File.OpenText(path)) {
				string s = "";
				while ((s = sr.ReadLine()) != null) {
					string[] prop = s.Split('=');
					try {
						if (prop != null && prop[0] != null && prop[1] != null) {
							properties.Add(prop[0],Int32.Parse((string)prop[1]));
						}
					} catch (FormatException) {
						throw new ParseErrorException("Error converting value '" + prop[1] + "'");
					}
				}
			}

			try {
				ConfigValues config = new ConfigValues();
				config.PontAbrir = (int) properties["PontAbrir"];
				config.PontMaxima = (int) properties["PontMaxima"];
				config.PontTesouro = (int) properties["PontTesouro"];
				config.PontVeneno = (int) properties["PontVeneno"];
				config.SalaInicial = (int) properties["SalaInicial"];
				return config;
			} catch (NullReferenceException) {
				throw new ConfigErrorException("Initialization property not defined.");
			}
		}
	}
}