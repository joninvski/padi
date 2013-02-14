using System;
using System.Text;
using System.Collections;

namespace MMG.Exec{

    [Serializable]
    public class Jogador
    {
        //Representa a ligacao ao cliente
        private string _nickName;
        int _pontuacao;

        public Jogador(string nickName)
        {
            _nickName = nickName;
            _pontuacao = 0;
        }

        public static Jogador getJogador(string nick, ArrayList listaJogadores)
        {
            foreach (Jogador jogador in listaJogadores)
            {
                if (jogador.NickName.Equals(nick))
                {
                    return jogador;
                }
            }

            //Caso nao tenha encontrado nenhum
            return null;
        }

        public static string ImprimeListaJogadores(ArrayList listaJogadores)
        {
            string retorno = "";
            foreach (Jogador jogador in listaJogadores)
            {
                retorno += jogador.ToString() + "\n\r";
            }
            return retorno;
        }

        public override string ToString()
        {
            return "NickName: " + NickName;
        }

        public void AlteraPontuacao(int valor)
        {
            _pontuacao += valor;
        }

        public int Pontuacao
        {
            get { return _pontuacao; }
            set { _pontuacao = value; }
        }


        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        private ICliente GetLigacaoCliente()
        {
            return LigacaoCliente.GetCanalComunicacao(_nickName, ServerMain._lstLigacoesAJogadores);
        }
    }

    /**********************************************
     * Classe para comparar jogadores
     * 
     * ********************************************/
    public class ComparadorJogadores : IComparer
    {

        public ComparadorJogadores() : base() { }

        int IComparer.Compare(object x, object y)
        {
            Jogador jogadorA = (Jogador)x;
            Jogador jogadorB = (Jogador)y;

            return jogadorB.Pontuacao.CompareTo(jogadorA.Pontuacao);
        }
    }
}
