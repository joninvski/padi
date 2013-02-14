using System;
using System.Collections;
using System.Text;

namespace MMG.Exec
{
   [Serializable]
   public class RelogioVectorial
   {
      private string _idServidor;
      private int _valor;
      private int _nivel;

      public RelogioVectorial(string idServidor, int nivelSistema)
      {
         _idServidor = idServidor;
         _valor = 0;
         _nivel = nivelSistema;
      }

      public string IdServirvor
      {
         get { return _idServidor; }
      }

      public int Valor
      {
         get { return _valor; }
      }

      public int Nivel
      {
         get { return _nivel; }
      }

      public void Incrementa()
      {
         _valor++;

         Configuration.Debug("O relogio do " + _idServidor + " ja vai em " + _valor, Configuration.PRI_MIN);
      }

      public static RelogioVectorial GetRelogio(string idServidor, ArrayList listaRelogios)
      {
         foreach (RelogioVectorial rel in listaRelogios)
         {
            if (rel._idServidor == idServidor)
            {
               return rel;
            }
         }
         return null;
      }

      public static int GetValor(string idServidor, ArrayList listaRelogios){
         RelogioVectorial rel = GetRelogio(idServidor, listaRelogios);

         return rel._valor;
      }

      public static int GetNivel(string idServidor, ArrayList listaRelogios)
      {
         RelogioVectorial rel = GetRelogio(idServidor, listaRelogios);

         return rel._nivel;
      }

      public static void IncrementaServidor(string idServidor, ArrayList listaRelogios)
      {
         RelogioVectorial rel = GetRelogio(idServidor, listaRelogios);
         rel.Incrementa();
         return;
      }

      public static ArrayList CriaTodosRelogiosVectorias(ArrayList listaIdsServidores, int nivelActual)
      {
         ArrayList devolver = new ArrayList();
         foreach (String idServ in listaIdsServidores)
         {
            System.Console.WriteLine("Criei o relogio" + idServ);
            devolver.Add(new RelogioVectorial(idServ, nivelActual));
         }
         return devolver;
      }

      public static void IncrementaNivelTodosRelogios(ArrayList listaRelogiosVectoriais)
      {
         Configuration.Debug("Vou actualizar todos os relogios em um nivel", Configuration.PRI_MIN);
         foreach (RelogioVectorial rel in listaRelogiosVectoriais)
         {
            rel._nivel++;
            rel._valor = 0;
         }
         return;
      }

      public static string ToStringListaRegolios(ArrayList lstRelogiosVectoriais){
         string devolver = "";
         foreach (RelogioVectorial rel in lstRelogiosVectoriais)
         {
            devolver += rel.ToString() + "\n\r";
         }
         return devolver;
      }

      public override string ToString()
      {
         return "Servidor: " + _idServidor + " Nivel: " + _nivel + " Versao: " + _valor;
      }

      public static bool ListaMaisAtrasadaQue(ArrayList primeira, ArrayList seguinte)
      {
         foreach (RelogioVectorial relP in primeira)
         {
            RelogioVectorial relS = GetRelogio(relP.IdServirvor, seguinte);
            if (relS == null)
            {
               return false;
            }
            bool atrasado = relP.MaisAtrasadoQue(relS);
            if (atrasado == true)
            {
               return false;
            }
         }
         //Se percorrer todas sem nunca ter dado falso
         return true;
      }

      public bool MaisAtrasadoQue(RelogioVectorial b)
      {
         if (_nivel == b._nivel)
         {
            return _valor < b._valor;
         }
         else return _nivel < b._nivel;
      }
   }
}
