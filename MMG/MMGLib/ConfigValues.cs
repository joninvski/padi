using System;

namespace MMG.Config
{
	/// <summary>
	/// MMG general configuration values.
	/// </summary>
    [Serializable]
	public class ConfigValues {
	
		private int _pontTesouro;
		private int _pontAbrir;
		private int _pontVeneno;
		private int _pontMaxima;
		private int _salaInicial;

		public ConfigValues() {
			_pontTesouro = 0;
			_pontAbrir = 0;
			_pontVeneno = 0;
			_pontMaxima = 0;
			_salaInicial = 0;
		}

		public int PontTesouro {
			get {return _pontTesouro;}
			set {_pontTesouro = value;}
		}
		
		public int PontAbrir {
			get {return _pontAbrir;}
			set {_pontAbrir = value;}
		}
		
		public int PontVeneno {
			get {return _pontVeneno;}
			set {_pontVeneno = value;}
		}
		
		public int PontMaxima {
			get {return _pontMaxima;}
			set {_pontMaxima = value;}
		}

		public int SalaInicial {
			get {return _salaInicial;}
			set {_salaInicial = value;}
		}

		public void Dump() {
			Console.WriteLine("PontTesouro =" + PontTesouro);
			Console.WriteLine("PontAbrir   =" + PontAbrir);
			Console.WriteLine("PontVeneno  =" + PontVeneno);
			Console.WriteLine("PontMaxima  =" + PontMaxima);
			Console.WriteLine("SalaInicial =" + SalaInicial);
		}
	}
}