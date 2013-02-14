using System;
using System.Collections;
using System.Text;
using MMG.Config;

namespace MMG.Exec
{
   [Serializable]
   public class MensagemServidor : Mensagem
   {
      int _numSala;

      //Id do cliente que tentou abrir a sala
      string _idCliente;

      RoomDesc _novaSala;
      string _idJogo;

      int _pontuacaoNova;
      int _pontuacaoAntiga;

      bool _jogoTerminou;

      Jogo _novoJogo;

      ArrayList _novoEstado;
      string _resultadoAccaoCliente;
      string _idUnico;

      private ArrayList _listaJogos;
      private string _idServidorQueMorreu;

      private MensagemServidor(string idCliente, string idOriginadorMensagem, string idDestinatarioMensagem, string tipoMensagem)
      {
         _idCliente = idCliente;
         _idOriginadorMensagem = idOriginadorMensagem;
         _idDestinatarioMensagem = idDestinatarioMensagem;
         _tipoMensagem = tipoMensagem;
         _jogoTerminou = false;
         NumeroTentativas = 0;
      }

      public static MensagemServidor PedidoTentativaAbrirTesouro(string idCliente, string idJogo, string idOriginadorMensagem, string idServidorDestino, int numSala, string guid)
      {
         MensagemServidor mensagem = new MensagemServidor(idCliente, idOriginadorMensagem, idServidorDestino, TENTATIVAABRIRTESOURO);
         mensagem._idJogo = idJogo;
         mensagem._numSala = numSala;
         mensagem._idUnico = guid;

         return mensagem;
      }



      public static MensagemServidor UpdateEstadoGlobalSistema(string idCliente, string idJogo, string idOrigem, string destino, RoomDesc novaSala, int pontuacaoAntiga, int pontuacaoNova, bool jogoTerminou, string resultadoAccaoCliente, string guid)
      {
         MensagemServidor mensagem = new MensagemServidor(idCliente, idOrigem, destino, Mensagem.UPDATEESTADOGLOBAL);
         mensagem._novaSala = novaSala;
         mensagem._idJogo = idJogo;
         mensagem._pontuacaoAntiga = pontuacaoAntiga;
         mensagem._pontuacaoNova = pontuacaoNova;
         mensagem._jogoTerminou = jogoTerminou;
         mensagem._resultadoAccaoCliente = resultadoAccaoCliente;
         mensagem._idUnico = guid;

         return mensagem;
      }

      public static MensagemServidor EntraNovoJogadorNumJogo(string idCliente, string idOrigem, string idDestino, string idJogo)
      {
         MensagemServidor mensagem = new MensagemServidor(idCliente, idOrigem, idDestino, Mensagem.ENTRANOVOJOGADORNUMJOGO);
         mensagem._idJogo = idJogo;
         return mensagem;
      }

      public static MensagemServidor RegistaNovoJogadorNoSistema(string idCliente, string idOrigem, string idDestino)
      {
         MensagemServidor mensagem = new MensagemServidor(idCliente, idOrigem, idDestino, Mensagem.REGISTANOVOJOGADORNOSISTEMA);
         return mensagem;
      }

      public static MensagemServidor AdicionaNovoJogoSistema(string idOrigem, string idDestino, Jogo jogo)
      {
         string idCliente = "Nao 'e importante saber quem criou o jogo";
         MensagemServidor mensagem = new MensagemServidor(idCliente, idOrigem, idDestino, Mensagem.ADICIONANOVOJOGOSISTEMA);
         mensagem._novoJogo = jogo;
         return mensagem;
      }

      public static MensagemServidor AvisaServidorVaiSair(string idOrigem, string idDestino)
      {
         MensagemServidor mensagem = new MensagemServidor("Nao vem de um cliente", idOrigem, idDestino, Mensagem.AVISOSERVIDORVAISAIR);
         return mensagem;
      }


      public static MensagemServidor RemoveServidorDoSistema(string idOrigem, string idDestino, string idServidorARemover, ArrayList lstJogos)
      {
         MensagemServidor mensagem = new MensagemServidor("Nao vem de um cliente", idOrigem, idDestino, Mensagem.REMOVESERVIDORSISTEMA);
         mensagem._novoEstado = lstJogos;

         return mensagem;
      }

      public static MensagemServidor MorreuOutroServidor(string meuId, string idServidorQueMorreu, ArrayList listaJogos)
      {
         MensagemServidor mensagem = new MensagemServidor("Nao vem de um cliente", meuId, Mensagem.IPBROADCAST, Mensagem.MORREU_OUTRO_SERVIDOR);
         mensagem._idServidorQueMorreu = idServidorQueMorreu;
         mensagem._listaJogos = listaJogos;

         return mensagem;
      }

      public static MensagemServidor MensagemAvisoFosteDeclaradoMorto(string meuId, string idServidorQueMorreu)
      {
         MensagemServidor mensagem = new MensagemServidor("Nao vem de um cliente", meuId, idServidorQueMorreu, Mensagem.FOSTE_CONSIDERADO_MORTO);
         return mensagem;
      }

      
      public static MensagemServidor FazSTOP(string meuId, string servidorMortoCausouStop, string idCliente, string idJogo, int numSala)
      {
         MensagemServidor mensagem = new MensagemServidor(idCliente, meuId, Mensagem.IPBROADCAST, Mensagem.STOP);
         mensagem._idJogo = idJogo;
         mensagem._numSala = numSala;
         mensagem._idServidorQueMorreu = servidorMortoCausouStop;
         return mensagem;
      }

      public static MensagemServidor FazReplyStop(string meuId, string idDestinatario, string servidorMortoCausouStop, string idCliente, string idJogo, int numSala)
      {
         MensagemServidor mensagem = new MensagemServidor(idCliente, meuId, idDestinatario, Mensagem.REPLY_STOP);
         mensagem._idJogo = idJogo;
         mensagem._numSala = numSala;
         mensagem._idServidorQueMorreu = servidorMortoCausouStop;
         return mensagem;
      }

      public RoomDesc NovaSala
      {
         get { return _novaSala; }
      }

      public string IdJogo
      {
         get { return _idJogo; }
      }

      public string IdCliente
      {
         get { return _idCliente; }
      }

      public int NumSala
      {
         get { return _numSala; }
      }

      public int PontuacaoAntiga
      {
         get { return _pontuacaoAntiga; }
      }

      public int PontuacaoNova
      {
         get { return _pontuacaoNova; }
      }

      public bool JogoTerminou
      {
         get { return _jogoTerminou; }
      }

      public Jogo NovoJogo
      {
         get { return _novoJogo; }
      }

      public ArrayList NovoEstado
      {
         get { return _novoEstado; }
      }

      public string ResultadoAccaoCliente
      {
         get { return _resultadoAccaoCliente; }
         set { _resultadoAccaoCliente = value; }
      }

      public string guidUnico
      {
         get { return _idUnico; }
      }

      public ArrayList ListaJogos
      {
         get { return _listaJogos; }
      }

      public string idServidorQueMorreu
      {
          get { return _idServidorQueMorreu; }
      }

      public static string criaCarimboMensagem()
      {
         string retorno = ServerMain._minhaIdentificacao + "@";
         retorno = retorno + System.Guid.NewGuid().ToString();
         return retorno;
      }

      public static MensagemServidor GetMensagemServidor(string guid, ArrayList listaMensagensServidor)
      {
         foreach (MensagemServidor msg in listaMensagensServidor)
         {
            if (msg.guidUnico.Equals(guid))
            {
               return msg;
            }
         }
         return null;
      }
      

      public static bool ListaContemMensagem(string guid, ArrayList listaMensagensServidor)
      {
         MensagemServidor msg = GetMensagemServidor(guid, listaMensagensServidor);
         if(msg == null){
            return false;
         }
         return true;
      }

      public static void RemoveMensagemPorGuid(string guid, ArrayList listaMensagensServidor)
      {
         MensagemServidor msg = GetMensagemServidor(guid, listaMensagensServidor);
         listaMensagensServidor.Remove(msg);
      }
   }
}
