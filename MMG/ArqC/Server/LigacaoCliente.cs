using System;
using System.Collections;
using System.Text;

namespace MMG.Exec
{
   class LigacaoCliente
   {
      private string _idCliente;
      private ICliente _canalComunicacao;
      private bool _pertenceEsteServidor;

      public LigacaoCliente(string idCliente, ICliente ligacao, bool pertenceEsteServidor)
      {
         _idCliente = idCliente;
         _canalComunicacao = ligacao;
         _pertenceEsteServidor = pertenceEsteServidor;
      }

      public static ICliente GetCanalComunicacao(string idCliente, ArrayList lstLigacoesClientes)
      {
         foreach (LigacaoCliente ligacaoCliente in lstLigacoesClientes)
         {
            if (ligacaoCliente._idCliente.Equals(idCliente))
            {
               return ligacaoCliente._canalComunicacao;
            }
         }
         return null;
      }

      public static LigacaoCliente GetLigacaoCliente(string idCliente, ArrayList lstLigacoesClientes)
      {
         foreach (LigacaoCliente ligacaoCliente in lstLigacoesClientes)
         {
            if (ligacaoCliente._idCliente.Equals(idCliente))
            {
               return ligacaoCliente;
            }
         }
         return null;
      }

      /// <summary>
      /// Verifica se um dado cliente é meu cliente
      /// </summary>
      /// <param name="idCliente"></param>
      /// <param name="lstLigacoesClientes"></param>
      /// <returns>True caso o cliente seja deste servidor False caso contrario</returns>
      public static bool EMeuCliente(string idCliente, ArrayList lstLigacoesClientes)
      {
         foreach (LigacaoCliente ligacao in lstLigacoesClientes)
         {
            if (ligacao._idCliente.Equals(idCliente))
            {
               return ligacao._pertenceEsteServidor;
            }
         }

         //So chega aqui caso o cliente nao exista nao devia acontecer
         System.Console.WriteLine("ERRO: O cliente: " + idCliente + " nao existe em todos os servidores");
         return false;
      }

      public static bool ClienteExiste(string idCliente, ArrayList lstLigacoesClientes)
      {
         foreach (LigacaoCliente ligacaoCliente in lstLigacoesClientes)
         {
            if (ligacaoCliente._idCliente.Equals(idCliente))
            {
               return true;
            }
         }
         return false;
      }

      public bool PertenceEsteServidor
      {
         get { return _pertenceEsteServidor; }
         set { _pertenceEsteServidor = value; }
      }

      public string IdCliente
      {
         get { return _idCliente; }
      }

      public ICliente CanalComunicacao
      {
         get { return _canalComunicacao; }
         set { _canalComunicacao = value; }
      }

      internal static void ClienteDeixouPertencerEsteServidor(string idCliente, ArrayList lstLigacoesClientes)
      {
         foreach (LigacaoCliente ligacaoCliente in lstLigacoesClientes)
         {
            if (ligacaoCliente._idCliente.Equals(idCliente))
            {
               ligacaoCliente._pertenceEsteServidor = false;
               return;
            }
         }
         return;
      }

      internal static void ClientePassouAPertencerEsteServidor(string idCliente, ArrayList lstLigacoesClientes)
      {
         foreach (LigacaoCliente ligacaoCliente in lstLigacoesClientes)
         {
            if (ligacaoCliente._idCliente.Equals(idCliente))
            {
               ligacaoCliente._pertenceEsteServidor = true;
               return;
            }
         }
         return;
      }
   }
}
