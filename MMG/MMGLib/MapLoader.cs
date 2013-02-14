using System;
using System.IO;
using System.Collections;

namespace MMG.Config {

	enum Direction {North, South, East, West};

	/// <summary>
	/// Loads a MMG map from a configuration file.
	/// </summary>
	public class MapLoader
	{
		public static RoomDesc[] LoadMap(string path, string idServidorResponsavel) {

			int lines, columns;
			DetectMapBoundaries(path,out lines,out columns);

			ArrayList rooms = new ArrayList();
			int[,] map = new int [lines,columns];
            LoadMapToMatrix(path, map, rooms, idServidorResponsavel);

			foreach (RoomDesc room in rooms) {
				room.North = FindRoom(room,map,Direction.North);
				room.South = FindRoom(room,map,Direction.South);
				room.East = FindRoom(room,map,Direction.East);
				room.West = FindRoom(room,map,Direction.West);
			}
			return (RoomDesc[])rooms.ToArray(typeof(RoomDesc));
		}


		// Auxiliary methods


		private static void DetectMapBoundaries(string path, out int numl, out int numc) {
			numl = 0;
			numc = 0;
			using (StreamReader sr = File.OpenText(path)) {
				string s = "";
				while ((s = sr.ReadLine()) != null) {
					numc = (s.Length > numc)?s.Length:numc;
					numl++;
				}
			}
		}


        private static void LoadMapToMatrix(string path, int[,] map, ArrayList rooms, string idServidorResponsavel)
        {
			int roomnum = 1;
			int line = 0;
			int lines = map.GetLength(0), columns = map.GetLength(1);

			map.Initialize();
			using (StreamReader sr = File.OpenText(path)) {
				string s = "";
				while ((s = sr.ReadLine()) != null) {
					Console.WriteLine(s);
					for (int col = 0; col < s.Length; col++) {
						char c = Char.ToLower(s[col]);
						if (c == 'x' || c == 't' || c == 'v') {
							rooms.Add(new RoomDesc(roomnum,line,col,c, idServidorResponsavel));
							map[line,col] = roomnum++;
						} else if (c == '|' || c == '-') {
							map[line,col] = -1;
						} else if (c != ' '){
							throw new ParseErrorException("Error parsing char '" + c + 
										"' at line " + line + " column " + col + ".");
						}
					}
					line++;
				}
			}
		}


		private static int FindRoom(RoomDesc room, int[,] map, Direction dir) {

			if (map[room.Line,room.Column] != room.Num) {
				throw new Exception("Inconsistent map.");
			}

			int l, c, lmax = map.GetLength(0) - 1, cmax = map.GetLength(1) - 1;

			switch(dir) {

				case Direction.North:
					l = room.Line - 1;
					while (l >= 0 && map[l,room.Column] == -1) {
						l--;
					}
					if (l != room.Line -1 && l >= 0 && map[l,room.Column] > 0) {
						return map[l,room.Column];
					}
					break;
				case Direction.South:
					l = room.Line + 1;
					while (l <= lmax && map[l,room.Column] == -1) {
						l++;
					}
					if (l != room.Line + 1 && l <= lmax && map[l,room.Column] > 0) {
						return map[l,room.Column];
					}
					break;
				case Direction.East:
					c = room.Column + 1;
					while (c <= cmax && map[room.Line,c] == -1) {
						c++;
					}
					if (c != room.Column + 1 && c <= cmax && map[room.Line,c] > 0) {
						return map[room.Line,c];
					}
					break;
				case Direction.West:
					c = room.Column - 1;
					while (c >= 0 && map[room.Line,c] == -1) {
						c--;
					}
					if (c != room.Column - 1 && c >= 0 && map[room.Line,c] > 0) {
						return map[room.Line,c];
					}
					break;
			}
			return -1;
		}
	}
}