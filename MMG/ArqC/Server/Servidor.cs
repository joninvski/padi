using System;
using System.Collections;
using System.Text;

namespace MMG.Exec
{
   class Servidor
   {
      string _identificacao;
      public IServer _ligacao;
      private bool _consideradoVivo;
      private bool _esperoReplyStop;

      public Servidor(string identificacaoServidor)
      {
         string url = "tcp://" + identificacaoServidor + "/MMGServer";
         _ligacao = (IServer)Activator.GetObject(
             typeof(IServer),
             url);

         _identificacao = identificacaoServidor;
         _consideradoVivo = true;

         //escreve no servidor
         Console.WriteLine("Criei ligacao ao servidor {0}\n\r", identificacaoServidor);
      }


      public static Servidor GetServidor(string identificadorServidor, ArrayList lstServidores)
      {
         foreach (Servidor servidor in lstServidores)
         {
            if (servidor._identificacao.Equals(identificadorServidor))
            {
               return servidor;
            }
         }
         return null;
      }

      public static string ImprimeListaServidores(ArrayList lstServidores)
      {
         string devolver = "";
         if (lstServidores.Count == 0)
         {
            return "Nao ha Servidores";
         }
         foreach (Servidor servidor in lstServidores)
         {
            devolver += servidor.ToString();
         }
         return devolver;
      }

      internal static ArrayList DevolveIdentificacaoServidores(ArrayList lstServidores)
      {
         ArrayList devolver = new ArrayList();
         foreach (Servidor servidor in lstServidores)
         {
            devolver.Add(servidor._identificacao);
         }
         return devolver;
      }


      public override string ToString()
      {
         return _identificacao + "\n\r";
      }


      internal ArrayList PedeListaServidores(ref int nivelSistema)
      {
         return _ligacao.PedeListaServidores(ref nivelSistema);
      }

      static public ArrayList CriaLigacoesTodosServidores(ArrayList listaIdsServidores)
      {
         ArrayList servidores = new ArrayList();
         foreach (String idServidor in listaIdsServidores)
         {
            Configuration.Debug("Liguei-me a: " + idServidor, Configuration.PRI_MED);
            servidores.Add(new Servidor(idServidor));
         }
         return servidores;
      }

      static public void AcrescentaServidor(string idDoMeuServidor, ArrayList lstServidores)
      {
         foreach (Servidor servidor in lstServidores)
         {
            servidor._ligacao.AcrescentaServidor(idDoMeuServidor);
         }
         return;
      }

      internal ArrayList PedeListaJogos()
      {
         return _ligacao.PedeListaJogos();
      }

      public string Identificacao
      {
         get { return _identificacao; }
      }


      public IServer Ligacao
      {
         get { return _ligacao; }
      }

      internal void ActualizaListaJogos(ArrayList _lstMapas)
      {
         _ligacao.ActualizaJogos(_lstMapas);
      }

      internal ArrayList PedeListaClientesDoSistema()
      {
         return _ligacao.PedeListaClientesExistentes();
      }

      public static Boolean ServidorExiste(string servID, ArrayList lstServidores)
      {
         if (GetServidor(servID, lstServidores) == null)
         {
            return false;
         }
         //else
         return true;
      }

      public static Boolean ServidorEstaVivo(string servID, ArrayList lstServidores)
      {
         Servidor serv = GetServidor(servID, lstServidores);

         return serv.Vivo;
      }


      public static void MeteServidorComoMorto(string servID, ArrayList lstServidores)
      {
         Servidor serv = GetServidor(servID, lstServidores);
         serv._consideradoVivo = false;
      }


      public static ArrayList GetServidoresVivos(ArrayList lstServidores)
      {
         ArrayList devolver = new ArrayList();

         foreach (Servidor serv in lstServidores)
         {
            if (serv.Vivo == true)
            {
               devolver.Add(serv);
            }
         }

         return devolver;
      }

      public bool Vivo
      {
         get { return _consideradoVivo; }
      }


      public static void ServidorRespondeuAReply(string servidorQueRespondeuAStop, ArrayList lstServidores)
      {
         Servidor serv = GetServidor(servidorQueRespondeuAStop, lstServidores);
         serv._esperoReplyStop = false;
      }

      public static bool JaTodosOsServidoresResponderam(ArrayList lstServidores)
      {
         foreach (Servidor serv in lstServidores)
         {
            if (serv._esperoReplyStop == true)
            {
               return false;
            }
         }
         return true;
      }

      internal static void MeteQueEsperoReplyStop(ArrayList lstServidores)
      {
         foreach (Servidor serv in lstServidores)
         {
            serv._esperoReplyStop = true;
         }
      }
   }
}
