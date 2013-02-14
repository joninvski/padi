using System;
using System.Collections;
using System.Text;
using MMG.Config;

namespace MMG.Exec
{
   [Serializable]
   public class MensagemCliente : Mensagem
   {
      /**************************************
       * Nos dois casos
       * ************************************/
      private string _nick;

      /*************************************
       * Para enviar do cliente para o servidor
       * 
       * ***********************************/
      private string _idJogo;
      //para onde vai o jogador ou onde se abrir cofre
      private int _salaDestino;

      /*************************************
       * Para enviar do servidor para o cliente
       * 
       * ***********************************/
      //quando o servidor recebe a nova sala
      public RoomDesc _novaSala;
      public int _numSalasAdjComGas;
      public int _pontuacaoNova;
      public string _direccao;

      //Quando se abre um cofre
      public int _pontuacaoAntiga;

      //Quando um jogo acaba
      public ArrayList _top10;
      public bool _jogoTerminou;

      private string _resultadoAccaoCliente;

      /// <summary>
      /// Construtor da classe MensagemCliente
      /// </summary>
      /// <param name="idOriginadorMensagem">Quem enviou a mensagem</param>
      /// <param name="tipoMensagem">Qual o tipo da mensagem</param>
      /// <param name="idDestinatarioMensagem">Quem é o destinatario da mensagem</param>
      private MensagemCliente(string idOriginadorMensagem, string tipoMensagem, string idDestinatarioMensagem)
      {
         _idOriginadorMensagem = idOriginadorMensagem;
         _idDestinatarioMensagem = idDestinatarioMensagem;
         _tipoMensagem = tipoMensagem;

         NumeroTentativas = 0;
      }

      public static MensagemCliente JogadaMovimento(string nickName, string idJogo, string accao, int salaDestino)
      {
         MensagemCliente mensagem = new MensagemCliente(nickName, Mensagem.MOVIMENTO, "O servidor do cliente: " + nickName);

         mensagem._nick = nickName;
         mensagem._direccao = accao;
         mensagem._idJogo = idJogo;
         mensagem._salaDestino = salaDestino;

         return mensagem;
      }

      public static MensagemCliente JogadaAbrirTesouro(string nickName, string idJogo, string accao, int salaDestino)
      {
         MensagemCliente mensagem = new MensagemCliente(nickName, Mensagem.ABRETESOURO, "O servidor do cliente: " + nickName);

         mensagem._idDestinatarioMensagem = nickName;
         mensagem._nick = nickName;
         mensagem._direccao = accao;
         mensagem._idJogo = idJogo;
         mensagem._salaDestino = salaDestino;

         return mensagem;
      }

      public static MensagemCliente RespostaMovimentoJogador(string idCliente, RoomDesc novaSala, int NumSalasAdjComGas, int pontuacaoNova)
      {
         MensagemCliente mensagem = new MensagemCliente(idCliente, Mensagem.RESPOSTAMOVIMENTO, idCliente);

         mensagem._nick = idCliente;
         mensagem._novaSala = novaSala;
         mensagem._numSalasAdjComGas = NumSalasAdjComGas;
         mensagem._pontuacaoNova = pontuacaoNova;

         return mensagem;
      }


      public static MensagemCliente RespondeAbertura(string idCliente, int pontuacaoAntiga, int pontuacaoNova, string resultadoAccaoCliente)
      {
         MensagemCliente mensagem = new MensagemCliente(idCliente, Mensagem.RESPOSTAABERTURA, idCliente);

         mensagem._nick = idCliente;
         mensagem._pontuacaoNova = pontuacaoNova;
         mensagem._pontuacaoAntiga = pontuacaoAntiga;
         mensagem._resultadoAccaoCliente = resultadoAccaoCliente;

         return mensagem;
      }

      public static MensagemCliente SistemaEmActualizacao(string idCliente)
      {
         MensagemCliente mensagem = new MensagemCliente(idCliente, Mensagem.SISTEMA_EM_ACTUALIZACAO, idCliente);

         return mensagem;
      }

      public override string ToString()
      {
         string retorno = "Mensagem: \r\n";

         retorno += "nick: " + _nick + "\r\n";
         retorno += "accao: " + Tipo + "\r\n";
         retorno += "idMapa: " + _idJogo + "\r\n";
         retorno += "SalaDestino: " + _salaDestino + "\r\n";

         return retorno;
      }

      public static MensagemCliente RespondeJogoFim(string idCliente, int pontuacaoFinal, ArrayList Top10)
      {
         MensagemCliente mensagem = new MensagemCliente(idCliente, Mensagem.RESPOSTATERMINOUJOGO, idCliente);

         mensagem._nick = idCliente;
         mensagem._pontuacaoNova = pontuacaoFinal;
         mensagem._top10 = Top10;
         mensagem._jogoTerminou = true;
         return mensagem;
      }

      public string IdJogo
      {
         get { return _idJogo; }
         set { _idJogo = value; }
      }

      public string Nick
      {
         get { return _nick; }
         set { _nick = value; }
      }


      public int Sala
      {
         get { return _salaDestino; }
         set { _salaDestino = value; }
      }

      public string ResultadoAccaoCliente
      {
         get { return _resultadoAccaoCliente; }
         set { _resultadoAccaoCliente = value; }
      }
   }
}
