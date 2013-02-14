using System;
using System.Collections;
using System.Text;
using MMG.Config;

namespace MMG.Exec
{
   [Serializable]
   public class MapDesc
   {
      public const string NORTE = "Norte";
      public const string ESTE = "Este";
      public const string SUL = "Sul";
      public const string OESTE = "Oeste";
      public const string ABRIR = "Abrir";

      public const char SALATESOURO = 't';
      public const char SALAAROMAVENENO = 'z';
      public const char COFREVAZIO = 'y';
      public const char SALAVENENO = 'v';
      public const char SALAVAZIA = 'x';

      private RoomDesc[] _mapa;
      private ConfigValues _config;
      //o jogo a que este jogo se encontra associado
      private string _idJogoAssociado;

      public MapDesc(string mapName, string configName, string idServidorResponsavel, string idJogoAssociado)
      {
         _mapa = MapLoader.LoadMap(mapName, idServidorResponsavel);
         _config = ConfigLoader.LoadConfig(configName);
         _idJogoAssociado = idJogoAssociado;
         InicializaOuroTesouroSalas();
      }

      /// <summary>
      ///Retorna o numero da sala pedido
      ///Realizada para evitar confusoes com o array que tem -1
      /// </summary>
      /// <param name="numeroSala">Numero da sala pretendida</param>
      /// <returns>A sala (RoomDesc) indicada</returns>
      public RoomDesc GetSala(int numeroSala)
      {
         return _mapa[numeroSala - 1];
      }

      /// <summary>
      /// Conta o numero de salas com veneno que rodeiam a indicada
      /// </summary>
      /// <param name="numSala">Numero da Sala do centro</param>
      /// <returns>Numero de salas com veneno 'a volta</returns>
      public int ContaSalasVenenoRodear(int numSala)
      {
         int salasComGas = 0;
         RoomDesc salaPresente = GetSala(numSala);

         if (salaPresente.North != -1)
         {
            RoomDesc salaNorte = GetSala(salaPresente.North);
            if (salaNorte.RoomType.Equals(MapDesc.SALAVENENO))
               salasComGas++;
         }

         if (salaPresente.South != -1)
         {
            RoomDesc salaSul = GetSala(salaPresente.South);
            if (salaSul.RoomType.Equals(MapDesc.SALAVENENO))
               salasComGas++;
         }
         if (salaPresente.East != -1)
         {
            RoomDesc salaEste = GetSala(salaPresente.East);
            if (salaEste.RoomType.Equals(MapDesc.SALAVENENO))
               salasComGas++;
         }

         if (salaPresente.West != -1)
         {
            RoomDesc salaOeste = GetSala(salaPresente.West);
            if (salaOeste.RoomType.Equals(MapDesc.SALAVENENO))
               salasComGas++;
         }

         return salasComGas;
      }

      /// <summary>
      /// Percorre as várias salas do mapa e caso tenham tesouro,
      /// enche-o com o valor configurado 
      /// </summary>
      private void InicializaOuroTesouroSalas()
      {
         int tesouro = _config.PontTesouro;

         foreach (RoomDesc sala in _mapa)
         {
            if (sala.RoomType.Equals(MapDesc.SALATESOURO))
            {
               sala.OuroNoTesouro = tesouro;
            }
         }
      }

      public void RetiraGasSala(int numSala)
      {
         RoomDesc sala = GetSala(numSala);
         sala.RoomType = MapDesc.SALAAROMAVENENO;

         return;
      }

      public void RetiraTesouro(int numSala)
      {
         RoomDesc sala = GetSala(numSala);

         sala.OuroNoTesouro -= _config.PontAbrir;
         sala.CofreAberto = true;

         //Caso o cofre fique sem ouro
         if (sala.OuroNoTesouro <= 0)
         {
            sala.RoomType = MapDesc.COFREVAZIO;
         }
      }

      public int NumeroDeSalas()
      {
         return _mapa.Length;
      }

      public ConfigValues Config
      {
         get
         {
            return _config;
         }
      }

      public RoomDesc[] GetTodasSalasMapa()
      {
         return _mapa;
      }

      public string IdJogoAssociado
      {
         get { return _idJogoAssociado; }
         set { _idJogoAssociado = value; }
      }

      public void ActualizaSala(RoomDesc sala)
      {
         _mapa[sala.Num - 1] = sala;

         return;
      }
   }
}
