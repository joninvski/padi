using System;
using System.Collections;
using System.Text;
using MMG.Config;
using System.IO;

namespace MMG.Exec
{
   public class ServicosEntrada
   {

      #region Servicos para atender pedidos de outros servidores
      /// <summary>
      /// 
      /// </summary>
      /// <param name="urlNovoServidor"></param>
      /// <returns>Uma arraylist com strings dos servidores existentes</returns>
      public static ArrayList PedeListaServidores(ref int nivelSistema)
      {
         //Vou buscar os ids dos servidores que existem no sistema (menos eu)
         ArrayList listaIdsServidores = Servidor.DevolveIdentificacaoServidores(ServerMain.ServidoresVivos);
         nivelSistema = ServerMain.GetMeuNivelSistema();
         
         //devolvo a lista com os identificadores dos servidores do sistema (menos eu e com quem tou a falar)
         return listaIdsServidores;
      }

      public static void AcrescentaServidor(string urlNovoServidor)
      {
         //Adiciona com quem tou a falar no minha lista de servidores
         ServerMain.TodosServidores.Add(new Servidor(urlNovoServidor));
         RelogioVectorial.IncrementaNivelTodosRelogios(ServerMain._lstRelogiosVectoriais);
         ServerMain._lstRelogiosVectoriais.Add(new RelogioVectorial(urlNovoServidor, ServerMain.GetMeuNivelSistema()));
      }

      public static ArrayList RetornaListaJogos()
      {
         ArrayList jogosDistribuidos = new ArrayList();
         foreach (Jogo jogo in ServerMain._lstJogos)
         {
            if (jogo.ModoJogo.Equals(Configuration.MODO_C) == false)
            {
               jogosDistribuidos.Add(jogo);
            }
         }
         return jogosDistribuidos;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="lstJogos">A lst de Jogos nao contem os meus jogos centralizados</param>
      internal static void ActualizaJogos(ArrayList lstJogos)
      {
         Configuration.Debug("Querem-me actualizar a lista de jogos dos jogos", Configuration.PRI_MED);

         //Guardo os jogos que ja tenho
         ArrayList backup = (ArrayList)ServerMain._lstJogos.Clone();

         //Digo que a lista que veio é a minha lista de jogos (faltam meus jogos do tipo C)
         ServerMain._lstJogos = lstJogos;

         //Agora vou adicionar os meus jogos do tipo C
         foreach (Jogo jogo in backup)
         {
            if (jogo.ModoJogo.Equals(Configuration.MODO_C))
            {
               ServerMain._lstJogos.Add(jogo);
            }
         }
         return;
      }

      public static void RealizaUpdateDoEstado(string idJogo, RoomDesc sala, string idCliente, int pontuacaoAntiga, int pontuacaoNova, bool jogoTerminou, string resultadoAccaoCliente)
      {
         Configuration.Debug("Vou fazer update da sala:" + sala.Num + " do jogo:" + idJogo + " devido ao jogador:" + idCliente, Configuration.PRI_MIN);

         //Significa que o estado do sistema global mudou. Preciso de actualizar a sala
         Jogo jogo = Jogo.GetJogo(idJogo, ServerMain._lstJogos);

         if (jogo == null)
         {
            Configuration.Debug("Descartei msg referente a 1jogo que eu ainda nem sei que foi criado.", Configuration.PRI_MAX);
            return;
         }

         if (LigacaoCliente.ClienteExiste(idCliente, ServerMain._lstLigacoesAJogadores) == false)
         {
            Configuration.Debug("Esta mensagem é devido a um cliente que eu ainda nem conheco mas nao há problema.", Configuration.PRI_MAX);
         }

         //Preciso de actualizar a sala
         jogo.Mapa.ActualizaSala(sala);
         //E pontuacao do cliente
         jogo.ActualizaPontuacao(idCliente, pontuacaoNova);
         //E se o jogo acabou ou nao
         jogo.Terminou = jogoTerminou;

         //Se este update corresponde a um pedido de um cliente meu entao vou avisa-lo
         if (LigacaoCliente.EMeuCliente(idCliente, ServerMain._lstLigacoesAJogadores) == true)
         {
            MensagemCliente mensagem;

            if (jogoTerminou == false)
            {
               mensagem = MensagemCliente.RespondeAbertura(idCliente, pontuacaoAntiga, pontuacaoNova, resultadoAccaoCliente);
            }
            else
            {
               ArrayList top10 = jogo.ElaboraListaTop10();
               mensagem = MensagemCliente.RespondeJogoFim(idCliente, pontuacaoNova, top10);
            }

            ComunicacaoServidor.MeteMensagemParaEnviar(mensagem);
         }
         return;
      }

      public static void EntraNovoJogadorNumJogo(string idJogo, string idCliente)
      {
         Jogo jogo = Jogo.GetJogo(idJogo, ServerMain._lstJogos);
         jogo.AdicionaJogador(new Jogador(idCliente));
         return;
      }

      internal static void RegistaNovoJogadorNoSistema(string idCliente)
      {
         ServerMain._lstLigacoesAJogadores.Add(new LigacaoCliente(idCliente, null, false));
      }

      internal static void InsereNovoJogoSistema(Jogo jogo)
      {
         ServerMain._lstJogos.Add(jogo);
      }

      internal static void RemoveServidorSistema(string idServidor, ArrayList lstJogosDoNovoEstado)
      {
         //Apago o servidor da minha lista de servidores
         Servidor.MeteServidorComoMorto(idServidor, ServerMain.TodosServidores);

         //E vou actualizar a lista de jogos
         ActualizaJogos(lstJogosDoNovoEstado);

         return;
      }

      internal static void AvisoServidorVaiSair(string idServidor)
      {
         ComunicacaoServidor.TrataDescobertaMorteServidor(idServidor, null, null, -1); 
      }

      internal static void MorreuOutroServidor(string idServidorQueMorreu, ArrayList lstJogos)
      {
         Servidor.MeteServidorComoMorto(idServidorQueMorreu, ServerMain.TodosServidores);

         //Substituo a lst de jogos
         ServerMain._lstJogos = lstJogos;
      }

      internal static void FuiConsideradoMorto()
      {
         Configuration.Debug("GoodBye Cruel World", Configuration.PRI_MIN);
         System.Environment.Exit(0);
      }


      internal static void TrataStop(string idServidorPediuStop, string idServidorMorto, string idCliente, string idJogo, int numSala)
      {
         //Se recebi uma mensagem de stop simplesmento tenho que lhe responder com um reply stop
         MensagemServidor msg = MensagemServidor.FazReplyStop(ServerMain._minhaIdentificacao, idServidorPediuStop, idServidorMorto, idCliente, idJogo, numSala);
         ComunicacaoServidor.MeteMensagemParaEnviar(msg);
      }

      internal static bool TrataReplyStop(string servidorQueRespondeuAStop, string idServidorMorto)
      {
         bool pronto = false;

         //Digo que este servidor ja respondeu
         Servidor.ServidorRespondeuAReply(servidorQueRespondeuAStop, ServerMain.ServidoresVivos);

         //Caso ja todos os servidores ja tenham respondido ao STOP
         if (Servidor.JaTodosOsServidoresResponderam(ServerMain.ServidoresVivos))
         {
            pronto = true;
            Configuration.Debug("Já todos os servidores responderam STOP-REPLY", Configuration.PRI_MED);

            //Vou rearranjar os jogos
            ArrayList listaServidoresVivos = ServerMain.ServidoresVivos;
            ServerMain._lstJogos = Jogo.DistribuiJogos(ServerMain._lstJogos, listaServidoresVivos, ServerMain._minhaIdentificacao);

            //Vou mandar a nova situacao para toda a gente
            MensagemServidor msg = MensagemServidor.MorreuOutroServidor(ServerMain._minhaIdentificacao, idServidorMorto, ServerMain._lstJogos);
            ComunicacaoServidor.MeteMensagemParaEnviar(msg);
           }
         return pronto;
      }

      public static void RealizaTentativaAbrirSala(string idCliente, int numSala, string jogoId, string guid)
      {
         Jogo jogo = Jogo.GetJogo(jogoId, ServerMain._lstJogos);
         MensagemCliente dados = jogo.PedidoDeAbrirTesouro(idCliente, numSala);
         EnviaNovoEstadoTodosServidores(idCliente, jogoId, numSala, dados._pontuacaoAntiga,
             dados._pontuacaoNova, dados._jogoTerminou, dados.ResultadoAccaoCliente, guid);
         return;
      }
      #endregion

      #region Servicos para atender pedidos do Cliente

      /// <summary>
      /// Responde a um pedido de movimento por parte do cliente
      /// </summary>
      /// <param name="idCliente">Identificacao do cliente</param>
      /// <param name="numSala">Numero da sala actual do cliente</param>
      /// <param name="IdJogo">Identificador do Jogo</param>
      /// <returns>Mensagem para ser enviada como resposta ao cliente</returns>
      public static MensagemCliente MovimentoJogador(string idCliente, int numSala, string IdJogo)
      {
         Jogo jogo = Jogo.GetJogo(IdJogo, ServerMain._lstJogos);

         //Casp o jogo ja tenha terminado
         if (jogo.Terminou == true)
         {
            Jogador jogador = jogo.GetJogador(idCliente);
            ArrayList top10 = jogo.ElaboraListaTop10();
            return MensagemCliente.RespondeJogoFim(idCliente, jogador.Pontuacao, top10);
         }

        //Se o jogo ainda nao terminou
         else
         {
            return jogo.MovimentaJogador(idCliente, numSala);
         }
      }

      /// <summary>
      /// Responde a um pedido de abertura do cofre por parte do cliente
      /// </summary>
      /// <param name="idCliente">Identificacao do cliente</param>
      /// <param name="numSala">Numero da sala actual do cliente</param>
      /// <param name="IdJogo">Identificador do Jogo</param>
      /// <returns>Mensagem para ser enviada como resposta ao cliente</returns>
      public static MensagemCliente AbreTesouro(string idCliente, int numSala, string idJogo)
      {
         Jogo jogo = Jogo.GetJogo(idJogo, ServerMain._lstJogos);
         MensagemCliente msgRetornoCli = null;

         //Casp o jogo ja tenha terminado
         if (jogo.Terminou == true)
         {
            Jogador jogador = jogo.GetJogador(idCliente);
            ArrayList top10 = jogo.ElaboraListaTop10();
            return MensagemCliente.RespondeJogoFim(idCliente, jogador.Pontuacao, top10);
         }

         //Se for um jogo do modo centralizado
         if (jogo.ModoJogo.Equals(Configuration.MODO_C))
         {
            return msgRetornoCli = jogo.PedidoDeAbrirTesouro(idCliente, numSala);
         }

        //Se for um jogo R1 ou R2
         else
         {
            //Descobre entao quem é o responsavel pela sala
            string responsavelSala = jogo.GetResponsavelSala(numSala);

            //Caso o responsavel da sala seja eu
            if (responsavelSala == ServerMain._minhaIdentificacao)
            {
               Configuration.Debug(Configuration.DEBUG_EU_SERVIDOR_RESPONSAVEL_ABERTURA, Configuration.PRI_MED);

               //Mensagem que vou mandar para o cliente
               msgRetornoCli = jogo.PedidoDeAbrirTesouro(idCliente, numSala);

               //Actualiza estado varios Servidores
               EnviaNovoEstadoTodosServidores(idCliente, idJogo, numSala, msgRetornoCli._pontuacaoAntiga,
                   msgRetornoCli._pontuacaoNova, msgRetornoCli._jogoTerminou,
                   msgRetornoCli.ResultadoAccaoCliente, MensagemServidor.criaCarimboMensagem());

               //Mando uma mensagem de retorno para o cliente
               return msgRetornoCli;
            }

           //Se eu nao for o responsavel da sala
            else
            {
               Configuration.Debug(Configuration.DEBUG_NAO_SOU_EU_RESPONSAVEL, Configuration.PRI_MED);

               //Se nao for eu o responsavel vou buscar o responsavel
               Servidor servidorResponsavel = Servidor.GetServidor(responsavelSala, ServerMain.ServidoresVivos);

               //Digo ao servidor responsavel para tentar abrir o cofre (nao recebo resposta, 'e assincrono)
               MensagemServidor msg;
               msg = MensagemServidor.PedidoTentativaAbrirTesouro(idCliente, idJogo, ServerMain._minhaIdentificacao,
                   servidorResponsavel.Identificacao, numSala, MensagemServidor.criaCarimboMensagem());
               ComunicacaoServidor.MeteMensagemParaEnviar(msg);
               return null;
            }
         }
      }

      public static bool Regista(string ipCliente, string portoCliente, string nickName)
      {
         //verifica se o nick já se registou
         if (LigacaoCliente.ClienteExiste(nickName, ServerMain._lstLigacoesAJogadores) == true)
         {
            //Se já existir recusamos Login
            return false;
         }

         ICliente cli = CriaComunicacaoCliente(ipCliente, portoCliente);

         //adiciona o cliente à lista de ligacoes de clientes
         ServerMain._lstLigacoesAJogadores.Add(new LigacaoCliente(nickName, cli, true));

         //Agora digo a todos os outros servidores que existe um novo jogador registado
         MensagemServidor msg = MensagemServidor.RegistaNovoJogadorNoSistema(nickName, ServerMain._minhaIdentificacao, Mensagem.IPBROADCAST);
         ComunicacaoServidor.MeteMensagemParaEnviar(msg);

         //escreve no servidor
         Console.WriteLine("Registou-se um novo cliente: {0}:{1} -- {2}", ipCliente, portoCliente, nickName);

         return true;
      }

      public static bool CriaJogo(string idJogo, string mapPath, string configPath, string modoJogo)
      {
         //Vai ver se o jogo já existe
         Jogo antigo = Jogo.GetJogo(idJogo, ServerMain._lstJogos);

         //Caso já exista
         if (antigo != null)
         {
            return false;
         }

         try
         {
            //Caso esteja tudo okey cria e adiciona o jogo
            Jogo jogo = new Jogo(idJogo, mapPath, configPath, modoJogo, ServerMain._minhaIdentificacao);

            //Se nao for um jogo centralizado
            if (jogo.ModoJogo.Equals(Configuration.MODO_C) == false)
            {
               Configuration.Debug(Configuration.DEBUG_CRIAR_JOGO_REPARTIDO, Configuration.PRI_MIN);

               //Distribuo a responsabilidade das salas
               jogo.DistribuiJogoPorServidores(ServerMain.ServidoresVivos, ServerMain._minhaIdentificacao);

               //Distribui o jogo por todos os servidores
               MensagemServidor msg = MensagemServidor.AdicionaNovoJogoSistema(ServerMain._minhaIdentificacao, Mensagem.IPBROADCAST, jogo);
               ComunicacaoServidor.MeteMensagemParaEnviar(msg);
            }

            //E agora vou enviar o jogo para todos os outros servidores
            ServerMain._lstJogos.Add(jogo);
            Configuration.Debug("Foi criado o jogo: " + idJogo, Configuration.PRI_MED);
         }

         catch (FileNotFoundException ex)
         {
            Configuration.Debug("Ficheiro de mapa ou configuracao invalido {0}" + ex.Message, Configuration.PRI_MAX);
            return false;
         }

         return true;
      }

      public static RoomDesc JuntarJogo(string idJogo, string nickJogador, ref int salasComGas)
      {
         //Avisa na consola
         Configuration.Debug("O cliente " + nickJogador + " quer-se juntar ao jogo " + idJogo, Configuration.PRI_MED);

         //Busca o jogo pretendido
         Jogo jogo = Jogo.GetJogo(idJogo, ServerMain._lstJogos);

         //Cria o jogador
         Jogador jogador = new Jogador(nickJogador);

         //adicionar este jogador ao jogo
         jogo.AdicionaJogador(jogador);

         //vai buscar a sala inicial
         int inicial = jogo.Mapa.Config.SalaInicial;

         //Avisa todos os Outros Servidores que existe mais um jogador
         MensagemServidor msg = MensagemServidor.EntraNovoJogadorNumJogo(nickJogador, ServerMain._minhaIdentificacao, Mensagem.IPBROADCAST, idJogo);
         ComunicacaoServidor.MeteMensagemParaEnviar(msg);

         if (inicial == 0)
         {
            Random a = new Random();
            //Este menos um foi pk o random pode ser zero
            inicial = a.Next(jogo.Mapa.NumeroDeSalas() - 1);
            //Compenso o -1 que pus antes
            inicial++;
         }

         //calcula o numero de salas com gas á volta
         salasComGas = jogo.Mapa.ContaSalasVenenoRodear(inicial);

         //Retorna a sala inicial
         return jogo.Mapa.GetSala(inicial);
      }

      public static ArrayList ListaJogos()
      {
         ArrayList devolver = new ArrayList();
         foreach (Jogo jogo in ServerMain._lstJogos)
         {
            devolver.Add(jogo.IdJogo);
         }
         return devolver;
      }

      public static MapDesc ClienteVaiOffline(string idJogo)
      {
         Configuration.Debug("Um cliente deseja ir offline no jogo: " + idJogo, Configuration.PRI_MED);
         return Jogo.GetJogo(idJogo, ServerMain._lstJogos).Mapa;
      }

      internal static ArrayList PedeEstadoSistema(string idCliente)
      {
         LigacaoCliente.ClienteDeixouPertencerEsteServidor(idCliente, ServerMain._lstLigacoesAJogadores);
         return ServerMain._lstRelogiosVectoriais;
      }

      /// <summary>
      /// Funcao para quando um cliente se muda de servidor
      /// </summary>
      /// <param name="idCliente"></param>
      /// <returns>False caso o servidor ainda nao esteja pronto a aceitar o cliente</returns>
      internal static bool ClienteVoltaEntrar(string idCliente, string ipCliente, string portoCliente, ArrayList lstRelogiosConhecidos)
      {
         Configuration.Debug("O cliente " + idCliente + " que veio de outro servidor quer-se ligar a mim", Configuration.PRI_MAX);
         Configuration.Debug("Seu relogio vectorial é: \n\r" + RelogioVectorial.ToStringListaRegolios(lstRelogiosConhecidos), Configuration.PRI_MED);
         //Se a minha vista é mais atrasada que o cliente
         bool servidorPronto = RelogioVectorial.ListaMaisAtrasadaQue(ServerMain._lstRelogiosVectoriais, lstRelogiosConhecidos);

         if (servidorPronto)
         {
            Configuration.Debug("O estado do cliente é compativel com o meu para RYW", Configuration.PRI_MIN);
            LigacaoCliente ligacao = LigacaoCliente.GetLigacaoCliente(idCliente, ServerMain._lstLigacoesAJogadores);
            ligacao.PertenceEsteServidor = true;
            ligacao.CanalComunicacao = CriaComunicacaoCliente(ipCliente, portoCliente);
         }
         return servidorPronto;
      }
     
#endregion

      private static void EnviaNovoEstadoTodosServidores(string idCliente, string idJogo, int numSala, int pontuacaoAntiga, int pontuacaoNova, bool jogoTerminou, string resultadoAccaoCliente, string guid)
      {
         //Primeiro vou buscar os dados que vou enviar para os varios servidores
         Jogo jogo = Jogo.GetJogo(idJogo, ServerMain._lstJogos);
         RoomDesc novaSala = jogo.Mapa.GetSala(numSala);
         //Envio Mensagem para toda a gente com o update da sala

         MensagemServidor msg;
         msg = MensagemServidor.UpdateEstadoGlobalSistema(idCliente, jogo.IdJogo, ServerMain._minhaIdentificacao,
             Mensagem.IPBROADCAST, novaSala, pontuacaoAntiga, pontuacaoNova, jogoTerminou, resultadoAccaoCliente, guid);
         ComunicacaoServidor.MeteMensagemParaEnviar(msg);

         return;
      }

      private static ICliente CriaComunicacaoCliente(string ipCliente, string portoCliente)
      {
         //caso contrario cria o cliente
         string url = "tcp://" + ipCliente + ":" + portoCliente + "/Cliente";
         ICliente cli = (ICliente)Activator.GetObject(
             typeof(ICliente),
             url);
         return cli;
      }
   }
}
