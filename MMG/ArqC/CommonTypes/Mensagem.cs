using System;
using System.Collections;
using System.Text;

namespace MMG.Exec
{
   [Serializable]
   public abstract class Mensagem
   {
      public const string RESPOSTAABERTURA = "RespostaAbertura";
      public const string RESPOSTAMOVIMENTO = "RespostaMovimento";
      public const string RESPOSTATERMINOUJOGO = "TerminouJogo";
      public const string ABRETESOURO = "Abre";
      public const string MOVIMENTO = "Movimento";
      public const string UPDATEESTADOGLOBAL = "UpdateEstadoGlobal";
      public const string MORREU_OUTRO_SERVIDOR = "MorreuOutroServidor";

      public const string TENTATIVAABRIRTESOURO = "TentativaAbrirTesouro";
      public const string ENTRANOVOJOGADORNUMJOGO = "EntraNovoJogador";
      public const string REGISTANOVOJOGADORNOSISTEMA = "RegistaNovoJogador";
      public const string ADICIONANOVOJOGOSISTEMA = "AdicionaNovoJogoSistema";

      public const string FOSTE_CONSIDERADO_MORTO = "FosteConsideradoMorto";

      public const string STOP = "STOP";
      public const string REPLY_STOP = "STOP_REPLY";

      //O servidor avisa que vai sair
      public const string AVISOSERVIDORVAISAIR = "AvisoServidorVaiSair";
      //O servidor avisa que outro servidor saiu
      public const string REMOVESERVIDORSISTEMA = "RemoveServidorSistema";
      public const string SISTEMA_EM_ACTUALIZACAO = "SistemaEmActualizacao";
         
      public const string IPBROADCAST = "0.0.0.0";

      //Quem é que iniciou um par de mensagens
      public string _idOriginadorMensagem;
      public string _idDestinatarioMensagem;
      public string _tipoMensagem;

      //Representa o numero de vezes que a mensagem foi tentada enviar
      private int _numeroTentativas;

      public Mensagem() { }

      public Mensagem(string idOriginadorMensagem, string idDestinatarioMensagem, string tipoMensagem)
      {
         _idOriginadorMensagem = idOriginadorMensagem;
         _idDestinatarioMensagem = idDestinatarioMensagem;
         _tipoMensagem = tipoMensagem;
         _numeroTentativas = 0;
      }

      public string Tipo
      {
         get { return _tipoMensagem; }
         set { _tipoMensagem = value; }
      }

      public bool TipoIgual(string tipoMensagem)
      {
         return _tipoMensagem.Equals(tipoMensagem);
      }

      /// <summary>
      /// Verifica se a mensagem é para ser enviada para um cliente
      /// </summary>
      /// <returns></returns>
      public bool TipoMensagemCliente()
      {
         bool devolver = false;

         if (TipoIgual(Mensagem.ABRETESOURO))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.MOVIMENTO))
         {
            devolver = true;
         }

         if (TipoIgual(Mensagem.RESPOSTAABERTURA))
         {
            devolver = true;
         }

         if (TipoIgual(Mensagem.RESPOSTAMOVIMENTO))
         {
            devolver = true;
         }

         if (TipoIgual(Mensagem.RESPOSTATERMINOUJOGO))
         {
            devolver = true;
         }

         if (TipoIgual(Mensagem.SISTEMA_EM_ACTUALIZACAO))
         {
            devolver = true;
         }

         return devolver;
      }

      public bool TipoMensagemServidor()
      {
         bool devolver = false;
         if (TipoIgual(Mensagem.TENTATIVAABRIRTESOURO))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.UPDATEESTADOGLOBAL))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.ENTRANOVOJOGADORNUMJOGO))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.REGISTANOVOJOGADORNOSISTEMA))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.ADICIONANOVOJOGOSISTEMA))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.REMOVESERVIDORSISTEMA))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.AVISOSERVIDORVAISAIR))
         {
            devolver = true;
         }

         if (TipoIgual(Mensagem.MORREU_OUTRO_SERVIDOR))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.FOSTE_CONSIDERADO_MORTO))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.STOP))
         {
            devolver = true;
         }
         if (TipoIgual(Mensagem.REPLY_STOP))
         {
            devolver = true;
         }

         return devolver;
      }

      public void IncrementaTentativas()
      {
         _numeroTentativas++;
      }

      //Indica o numero de tentativas que foram feitas para enviar a mensagem
      public int NumeroTentativas
      {
         get { return _numeroTentativas; }
         set { _numeroTentativas = value; }
      }

      public Mensagem DuplicaMsg()
      {
         return (Mensagem)this.MemberwiseClone();
      }

      public bool EMsgComRetorno()
      {
         if (TipoIgual(Mensagem.TENTATIVAABRIRTESOURO))
         {
            return true;
         }
         return false;
      }

      public bool EMsgDeRetorno()
      {
         if (TipoIgual(Mensagem.UPDATEESTADOGLOBAL))
         {
            return true;
         }
         return false;
      }

   }
}
