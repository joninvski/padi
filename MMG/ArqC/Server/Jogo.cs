using System;
using System.Collections;
using System.Text;
using System.IO;
using MMG.Config;

namespace MMG.Exec
{
   [Serializable]
   public class Jogo
   {
      private string _idJogo;
      //Lista de jogadores que se encontram associados a este jogo
      private ArrayList _lstJogadores;

      //Mapa que se encontra associado a este jogo
      private MapDesc _mapDesc;

      //indica se o jogo terminou
      private bool _terminou;

      //Modo de jogo C, R1, R2
      private string _modoJogo;

      public Jogo(string idJogo, string mapName, string configName, string modoJogo, string idServidorResponsavel)
      {
         _idJogo = idJogo;
         _lstJogadores = new ArrayList();
         _mapDesc = new MapDesc(mapName, configName, idServidorResponsavel, idJogo);
         _terminou = false;
         _modoJogo = modoJogo;
      }

      /// <summary>
      /// Adiciona um jogador a lista de jogadores deste jogo 
      /// </summary>
      /// <param name="jogador"></param>
      public void AdicionaJogador(Jogador jogador)
      {
         _lstJogadores.Add(jogador);
         return;
      }

      /// <summary>
      /// Devolve um jogador do jogo
      /// </summary>
      /// <param name="idJogador">NickName do jogador</param>
      /// <returns>O jogador indicado</returns>
      public Jogador GetJogador(string idJogador)
      {
         return Jogador.getJogador(idJogador, _lstJogadores);
      }

      /// <summary>
      /// Devolve um jogo da lista de Jogos
      /// </summary>
      /// <param name="idJogo">O jogo procurado</param>
      /// <param name="listaJogos">A lista onde procurar</param>
      /// <returns>O jogo encontrado</returns>
      public static Jogo GetJogo(string idJogo, ArrayList listaJogos)
      {
         foreach (Jogo jogo in listaJogos)
         {
            //Caso já exista um jogo com esse ID
            if (jogo.IdJogo.Equals(idJogo))
            {
               return jogo;
            }
         }
         return null;
      }

      /// <summary>
      /// Realizar a accao do jogador abrir o cofre
      /// </summary>
      /// <param name="idJogador">Id do jogador</param>
      /// <param name="sala">Sala onde está o cofre</param>
      /// <returns>
      /// MapDesc.SalaTesouro caso se tenha tirado ouro do tesouro
      /// MapDesc.SalaVeneno caso se tenha tirado veneno do crofre
      /// MapDesc.SalaCofreVazio caso alguem ja se tenha antecipado
      /// </returns>
      public string JogadorAbreCofre(string idJogador, int sala)
      {
         //ve se a sala é premiada
         char cofre = _mapDesc.GetSala(sala).RoomType;
         Jogador jogador = Jogador.getJogador(idJogador, _lstJogadores);

         if (cofre.Equals(MapDesc.SALATESOURO))
         {
            jogador.AlteraPontuacao(_mapDesc.Config.PontTesouro);

            //Tira ouro do tesouro e caso tenha chegado ao fim retira o cofre
            _mapDesc.RetiraTesouro(sala);
            return Configuration.ACCAO_ABRIU_TESOURO;
         }

         else if (cofre.Equals(MapDesc.SALAVENENO))
         {
            jogador.AlteraPontuacao(_mapDesc.Config.PontVeneno);
            _mapDesc.RetiraGasSala(sala);
            return Configuration.ACCAO_ABRIU_VENENO;
         }

        //Caso alguem se tenha antecipado e já nao exista nem tesouro nem veneno
         else
         {
            Configuration.Debug(Configuration.TEXTO_SERVIDOR_ABERTURA_INVALIDA, Configuration.PRI_MED);
            return Configuration.ACCAO_ANTECIPOU_SE_CLIENTE;
         }
      }

      /// <summary>
      /// Retorna a sala de um jogo
      /// </summary>
      /// <param name="sala">A sala actual</param>
      /// <returns>A sala (room Desc) indicada</returns>
      public RoomDesc Sala(int sala)
      {
         return _mapDesc.GetSala(sala);
      }

      /// <summary>
      /// Devolve uma lista com ids dos 10 melhores Jogadores deste jogo
      /// </summary>
      /// <returns>ArrayList com os nomes dos 10 jogadores (ArrayList de strings)</returns>
      public ArrayList ElaboraListaTop10()
      {
         _lstJogadores.Sort(new ComparadorJogadores());
         int numJogadores = _lstJogadores.Count;


         ArrayList top10 = new ArrayList();

         //Para ser no maximo so' 10 jogadores
         int max = numJogadores;
         if (max > 10)
            max = 10;

         for (int i = 0; i < max; i++)
         {
            Jogador jogador = (Jogador)_lstJogadores[i];
            top10.Add(jogador.NickName);
         }

         return top10;
      }

      /// <summary>
      /// Realiza a accao do jogador abrir o tesouro
      /// </summary>
      /// <param name="idCliente">id do cliente que abre o tesouro</param>
      /// <param name="numSala">numero da sala onde esta o tesouro</param>
      /// <returns>Mensagem de resposta que deverá ser dada ao cliente</returns>
      internal MensagemCliente PedidoDeAbrirTesouro(string idCliente, int numSala)
      {
         //vai buscar o jogador
         Jogador jogador = GetJogador(idCliente);
         int pontuacaoAntiga = jogador.Pontuacao;

         //verifica se o jogo ja terminou
         if (Terminou == true)
         {
            //Mensagem de retorno a dizer que o jogo ja' terminou
            return MensagemCliente.RespondeJogoFim(idCliente, pontuacaoAntiga, ElaboraListaTop10());
         }

         //Realiza a accao do jogador abrir o cofre (actualiza pontuacao)
         string conteudoSala = JogadorAbreCofre(idCliente, numSala);

         //Verificar que se o jogo ja terminou
         int pontMaxima = Mapa.Config.PontMaxima;

         if (jogador.Pontuacao >= pontMaxima)
         {
            //Avisa que o jogo já terminou
            _terminou = true;
            return MensagemCliente.RespondeJogoFim(idCliente, jogador.Pontuacao, ElaboraListaTop10());
         }

         //Caso o jogo ainda nao tenha terminado
         MensagemCliente mensagemRetorno = MensagemCliente.RespondeAbertura(idCliente, pontuacaoAntiga, jogador.Pontuacao, conteudoSala);

         return mensagemRetorno;
      }

      /// <summary>
      /// Realiza a movimentacao do jogador
      /// </summary>
      /// <param name="idCliente">id do cliente que se movimenta</param>
      /// <param name="numSala">numero da sala para onde o cliente se VAI deslocar</param>
      /// <returns>Mensagem de resposta que deverá ser dada ao cliente</returns>
      internal MensagemCliente MovimentaJogador(string idCliente, int numSala)
      {
         //vai buscar o jogador pa dps ir ver a pontuacao
         Jogador jogador = GetJogador(idCliente);

         //verifica se o jogo ja terminou
         if (Terminou == true)
         {
            //Mensagem de retorno a dizer que o jogo ja' terminou
            return MensagemCliente.RespondeJogoFim(idCliente, jogador.Pontuacao, ElaboraListaTop10());
         }

         //Ve a proxima sala do jogador
         RoomDesc novaSala = Sala(numSala);

         //devolve numero de salas adjacentes com gas
         int salasComGas = Mapa.ContaSalasVenenoRodear(novaSala.Num);

         //manda mensagem com nova posicao 
         MensagemCliente mensagemRetorno = MensagemCliente.RespostaMovimentoJogador(idCliente, novaSala, salasComGas, jogador.Pontuacao);
         return mensagemRetorno;
      }

      public static ArrayList DevolveJogosPartilhados(ArrayList lstJogos)
      {
         ArrayList devolver = new ArrayList();

         foreach (Jogo jogo in lstJogos)
         {
            if (jogo.ModoJogo == Configuration.MODO_R1 || jogo.ModoJogo == Configuration.MODO_R2)
            {
               devolver.Add(jogo);
            }
         }
         return devolver;
      }

      public static string ImprimeListaJogos(ArrayList lstJogos)
      {
         if (lstJogos.Count == 0)
         {
            return "Nao ha jogos";
         }

         string devolver = "";
         foreach (Jogo jogo in lstJogos)
         {
            devolver += jogo.ToString();
         }
         return devolver;
      }

      public override string ToString()
      {
         string retorno = "\n\rJogo: " + _idJogo + " Modo: " + _modoJogo + " \n\rJogadores:\n\r";

         foreach (Jogador jogador in _lstJogadores)
         {
            retorno += jogador.ToString();
         }

         retorno += "\n\r";

         return retorno;
      }

      /// <summary>
      /// Indica se o jogo ja terminou
      /// </summary>
      public bool Terminou
      {
         set { _terminou = value; }
         get { return _terminou; }
      }

      /// <summary>
      /// Lista de Jogadores que se encontram a jogar este jogo
      /// </summary>
      public ArrayList ListaJogadores
      {
         set { _lstJogadores = value; }
         get { return _lstJogadores; }
      }

      /// <summary>
      /// Identificador unico do jogo
      /// </summary>
      public string IdJogo
      {
         set { _idJogo = value; }
         get { return _idJogo; }
      }

      /// <summary>
      /// Mapa a que o jogo se encontra associado
      /// </summary>
      public MapDesc Mapa
      {
         set { _mapDesc = value; }
         get { return _mapDesc; }
      }

      public string ModoJogo
      {
         set { _modoJogo = value; }
         get { return _modoJogo; }
      }

      public static ArrayList DistribuiJogos(ArrayList lstJogos, ArrayList lstServidores, string idMeuServidor)
      {
         //Para cada jogo
         foreach (Jogo jogo in lstJogos)
         {
            Configuration.Debug("Vou distribuir " + jogo.Mapa.GetTodasSalasMapa().Length + "salas por " + (Servidor.GetServidoresVivos(lstServidores).Count + 1) + " servidores", Configuration.PRI_MED);
            jogo.DistribuiJogoPorServidores(lstServidores, idMeuServidor);
         }
         return lstJogos;
      }

      public void DistribuiJogoPorServidores(ArrayList lstServidores, string meuIdentificador)
      {
         ArrayList lstServidoresVivos = Servidor.GetServidoresVivos(lstServidores);

         int index = 0;
         int numeroServidores = lstServidoresVivos.Count;
         
         RoomDesc[] arraySala = Mapa.GetTodasSalasMapa();

         foreach (RoomDesc sala in arraySala)
         {

            //Se ainda nao chegou ao maximo
            if (index != numeroServidores)
            {
               Servidor servidor = (Servidor)lstServidoresVivos[index];
               sala.IdServidorResponsavel = servidor.Identificacao;
               index++;
            }
            //Caso já tenha dado a volta a todos os outro, fico eu responsavel
            else
            {
               sala.IdServidorResponsavel = meuIdentificador;
               index = 0;
            }

            //Imprime distribuicao das salas
            Configuration.Debug("Sala: " + sala.Num + "--> " + sala.IdServidorResponsavel, Configuration.PRI_MIN);
         }
         return;
      }

      /// <summary>
      /// </summary>
      /// <param name="numSala"></param>
      /// <returns></returns>
      public string GetResponsavelSala(int numSala)
      {
         return Mapa.GetSala(numSala).IdServidorResponsavel;
      }

      public static ArrayList GetTodosMapasDeListaJogos(ArrayList lstJogos)
      {
         ArrayList mapas = new ArrayList();
         foreach (Jogo jogo in lstJogos)
         {
            mapas.Add(jogo._mapDesc);
         }
         return mapas;
      }

      public static void ActualizaMapas(ArrayList lstMapas, ArrayList lstJogos)
      {
         foreach (MapDesc mapa in lstMapas)
         {
            Jogo jogoRespectivo = Jogo.GetJogo(mapa.IdJogoAssociado, lstJogos);
            jogoRespectivo._mapDesc = mapa;
         }
      }

      public void ActualizaPontuacao(string idCliente, int pontuacaoNova)
      {
         Jogador jogador = GetJogador(idCliente);
         jogador.Pontuacao = pontuacaoNova;
         return;
      }

      internal static ArrayList GetJogosDistribuidos(ArrayList todosJogos)
      {
         ArrayList devolver = new ArrayList();

         foreach (Jogo jogo in todosJogos)
         {
            if (jogo.ModoJogo.Equals(Configuration.MODO_C) == false)
            {
               devolver.Add(jogo);
            }
         }
         return devolver;
      }
   }
}
