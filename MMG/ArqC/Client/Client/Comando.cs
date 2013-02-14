using System;
using System.Collections;
using System.Text;
using MMG.Config;

namespace MMG.Exec
{
    class Comando
    {
        string _accao;
        Cliente _ligacao;
        RoomDesc _sala;
        string _idMapa;

        public Comando(string accao, Cliente ligacao, RoomDesc sala, string idMapa)
        {
            _accao = accao;
            _ligacao = ligacao;
            _sala = sala;
            _idMapa = idMapa;
        }

        public void Executa()
        {
            MensagemCliente mensagem = null;

            if (_accao == MapDesc.NORTE || _accao == MapDesc.SUL || _accao == MapDesc.ESTE || _accao == MapDesc.OESTE)
            {
                int salaDestino = IndicaSalaDestino(_accao, _sala);
                mensagem = MensagemCliente.JogadaMovimento(_ligacao.NickName, _idMapa, _accao, salaDestino);
            }
            else if (_accao == Mensagem.ABRETESOURO)
            {
                mensagem = MensagemCliente.JogadaAbrirTesouro(_ligacao.NickName, _idMapa, _accao, _sala.Num);
            }

            //envia a mensagem
            _ligacao.OutMessage(mensagem);

            return;
        }

        /// <summary>
        /// Dada uma sala e uma direccao, indica qual o numero da proxima sala
        /// </summary>
        /// <param name="accao">Direccao a tomar</param>
        /// <param name="Sala">Sala Actual</param>
        /// <returns>Numero da proxima sala</returns>
        public static int IndicaSalaDestino(string accao, RoomDesc Sala)
        {
            if (accao.Equals(MapDesc.NORTE))
            {
                return Sala.North;
            }

            if (accao.Equals(MapDesc.SUL))
            {
                return Sala.South;
            }

            if (accao.Equals(MapDesc.ESTE))
            {
                return Sala.East;
            }

            if (accao.Equals(MapDesc.OESTE))
            {
                return Sala.West;
            }

            return -1;
        }
    }
}
