using System;

namespace MMG.Config
{
	/// <summary>
	/// Description of an MMG map's room
	/// </summary>
    [Serializable]
	public class RoomDesc
	{
		private int _line;
		private int _column;
		private int _num;
		private char _roomtype;
		private int[] _doors;
        private int _ouroNoTesouro;
        private bool _cofreAberto;
        private string _idServidorResponsavel;

		public RoomDesc(int num, int line, int column, char roomtype, string idServidorResponsavel) {
			_num = num;
			_line = line;
			_column = column;
			_doors = new int[4];
			_doors.Initialize();
			_roomtype = roomtype;
            _ouroNoTesouro = 0;
            _cofreAberto = false;
            _idServidorResponsavel = idServidorResponsavel;
		}

		public int Line {
			get {return _line;}
		}

		public int Column {
			get {return _column;}
		}

		public int Num {
			get {return _num;}
		}

		public char RoomType {
			get {return _roomtype;}
            set { _roomtype = value; }
		}

		public int[] Doors {
			get {return _doors;}
		}

		public int North {
			get {return _doors[0];}
			set {_doors[0] = value;}
		}

		public int South {
			get {return _doors[1];}
			set {_doors[1] = value;}
		}

		public int East {
			get {return _doors[2];}
			set {_doors[2] = value;}
		}

		public int West {
			get {return _doors[3];}
			set {_doors[3] = value;}
		}

        public int OuroNoTesouro {
			get {return _ouroNoTesouro;}
			set {_ouroNoTesouro= value;}
		}

        public bool CofreAberto
        {
            get { return _cofreAberto; }
            set { _cofreAberto = value; }
        }

        public override string ToString()
        {
            string devolver = "Room " + Num + "( " +_line + ", " + _column + ")";
            devolver += " (" + _roomtype + "): " + "\r\n";
            
            devolver += "N= " +  North + "\r\n";
            devolver += "E= " + East + "\r\n";
            devolver += "S= " + South + "\r\n";
            devolver += "W= " + West + "\r\n";
            devolver += "TipoSala= " + RoomType + "\r\n";
            devolver += "OuroRestante= " + OuroNoTesouro + "\r\n";
            devolver += "CofreAberto= " + CofreAberto + "\r\n";
            devolver += "Responsavel da sala: " + IdServidorResponsavel + "\r\n";
            return devolver;
        }

		public void Dump() {
			Console.WriteLine("Room {0,2} ({5}): N={1,2} S={2,2} E={3,2} W={4,2}", 
				_num, _doors[0], _doors[1], _doors[2], _doors[3],_roomtype);
		}

        public string IdServidorResponsavel
        {
            get { return _idServidorResponsavel; }
            set { _idServidorResponsavel = value; }
        }
	}
}