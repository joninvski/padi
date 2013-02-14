using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using MMG.Config;

namespace MMG.Exec
{
   class ComunicacaoServidor : MarshalByRefObject, IServer
   {
      static ArrayList _lstMsg = new ArrayList();
      static ArrayList _outbox = new ArrayList();
      static private Thread _threadEnvio;
      static private Thread _threadTimeOut;
      static private ArrayList _lstRespostasMensagensPendentes = new ArrayList();
      static private ComunicacaoServidor _comunicacaoDesteServidor;
      //Lock para só se tratar de uma mensagem de cada vez
      public static bool lockGeral = false;

      //Tem que ter um construtor vazio
      public ComunicacaoServidor()
      {
         _comunicacaoDesteServidor = this;
      }

      #region ICliente Members
      /// <summary>
      /// Regista o cliente neste Servidor
      /// </summary>
      /// <param name="ipCliente">ip do Cliente</param>
      /// <param name="portoCliente">Porto do cliente</param>
      /// <param name="nickName">NickName do cliente</param>
      /// <returns>false caso o registar nao seja aceite (nick repetido)</returns>
      public bool RegistaCliente(string ipCliente, string portoCliente, string nickName)
      {
         try
         {
            DoLockGeral();
            return ServicosEntrada.Regista(ipCliente, portoCliente, nickName);
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// Quando um cliente após ter saido de um servidor, pede para entrar neste
      /// </summary>
      /// <param name="idCliente">nickname do cliente</param>
      /// <param name="ipCliente">ip do cliente</param>
      /// <param name="portoCliente">porto do cliente</param>
      /// <param name="lstRelogiosConhecidos">Lista dos relogios vectoriais que o servidor do qual o cliente saíu tinha</param>
      /// <returns>True caso o servidor possa garantir a propriedade RYW, false caso contrario</returns>
      public bool ClienteVoltaEntrar(string idCliente, string ipCliente, string portoCliente, ArrayList lstRelogiosConhecidos)
      {
         try
         {
            DoLockGeral();
            return ServicosEntrada.ClienteVoltaEntrar(idCliente, ipCliente, portoCliente, lstRelogiosConhecidos);
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// Cria um novo jogo no servidor
      /// </summary>
      /// <param name="idJogo">identificação do jogo</param>
      /// <param name="mapPath">path para o ficheiro do mapa</param>
      /// <param name="configPath">path para o ficheiro de configuracao</param>
      /// <returns>False caso nao tenha sido possível criar jogo. 
      /// True caso contrario</returns>
      public bool CriaJogo(string idJogo, string mapPath, string configPath, string modoJogo)
      {
         try
         {
            DoLockGeral();
            return ServicosEntrada.CriaJogo(idJogo, mapPath, configPath, modoJogo);
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// Permite a um cliente juntar-se a um jogo
      /// </summary>
      /// <param name="idJogo">id do jogo a que o cliente se quer juntar</param>
      /// <param name="nickJogador">nick do jogarod</param>
      /// <returns>A sala inicial do jogo</returns>
      public RoomDesc JuntarJogo(string idJogo, string nickJogador, ref int salasComGas)
      {
         try
         {
            DoLockGeral();
            return ServicosEntrada.JuntarJogo(idJogo, nickJogador, ref salasComGas);
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// Devolve os Mapas Desc dos vários jogos (Metodo a ser usado pelo cliente)
      /// </summary>
      /// <returns>ArrayLista com os MapDesc dos varios jogos </returns>
      public ArrayList ListaJogos()
      {
         try
         {
            DoLockGeral();
            return ServicosEntrada.ListaJogos();
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// Avisa o servidor que o cliente vai entrar em modo offline
      /// </summary>
      /// <param name="idJogo">id do jogo em que o cliente se encontra</param>
      /// <returns>O mapa do jogo</returns>
      public MapDesc ClienteVaiOffline(string idJogo)
      {
         try
         {
            DoLockGeral();
            return ServicosEntrada.ClienteVaiOffline(idJogo);
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// Realizada quando o servidor avisa que vai mudar de servidor
      /// </summary>
      /// <param name="idCliente">Nickname do cliente que vai mudar</param>
      /// <returns>ArrayList contendo os relogios vectoriais actuais do servidor</returns>
      public ArrayList ClienteVaiMudar(string idCliente)
      {
         try
         {
            DoLockGeral();
            Configuration.Debug("O cliente " + idCliente + " vai sair e pediu-me o estado do sistema", Configuration.PRI_MED);
            return ServicosEntrada.PedeEstadoSistema(idCliente);
         }
         finally
         {
            DoUnlockGeral();
         }
      }
      #endregion

      #region IServer Members
      /// <summary>
      /// 
      /// </summary>
      /// <param name="urlNovoServidor"></param>
      /// <returns></returns>
      public ArrayList PedeListaServidores(ref int nivelSistema)
      {
         try
         {
            DoLockGeral();
            System.Console.WriteLine("Pediram-me lista de servidores.");
            return ServicosEntrada.PedeListaServidores(ref nivelSistema);
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="urlNovoServidor"></param>
      /// <returns>ArrayList de strings contendo a identificacao dos clientes</returns>
      public void AcrescentaServidor(string urlNovoServidor)
      {
         try
         {
            DoLockGeral();
            System.Console.WriteLine("O servidor " + urlNovoServidor + " deseja que eu tome conhecimento dele.");
            ServicosEntrada.AcrescentaServidor(urlNovoServidor);
            return;
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      public ArrayList PedeListaJogos()
      {

         try
         {
            DoLockGeral();
            System.Console.WriteLine("Pediram-me a lista de Jogos");
            return ServicosEntrada.RetornaListaJogos();
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      public ArrayList PedeListaClientesExistentes()
      {

         try
         {
            DoLockGeral();
            ArrayList idClientesSistema = new ArrayList();

            foreach (LigacaoCliente cliente in ServerMain._lstLigacoesAJogadores)
            {
               idClientesSistema.Add(cliente.IdCliente);
            }
            return idClientesSistema;
         }
         finally
         {
            DoUnlockGeral();
         }
      }

      public void ActualizaJogos(ArrayList _lstJogos)
      {

         try
         {
            DoLockGeral();
            ServicosEntrada.ActualizaJogos(_lstJogos);
            return;
         }
         finally
         {
            DoUnlockGeral();
         }
      }
      #endregion

      /// <summary>
      /// Fila de mensagem de entrada, e por aqui que devem receber as mensagem para o servidor
      /// </summary>
      /// <param name="tipoMsg"> define o tipo de mensagem k se pretende receber
      /// </param>
      /// <param name="obj"> varia conforme o tipo de mensagem
      ///     "criaJogo" - {nomeMapa, nomeConfig}
      /// </param>
      /// <returns> varia conforme a mensagem
      ///     "criaJogo" - {idJogo}
      /// </returns>
      public void MessageQueue(Mensagem mensagem)
      {
         //Se for uma mensagem vinda de um cliente
         if (mensagem.TipoMensagemCliente() == true)
         {
            try
            {
               DoLockGeral();
               #region Mensagens vindas do cliente
               MensagemCliente msgArranjada = (MensagemCliente)mensagem;
               //Se for uma tentativa de abrir tesouro
               if (mensagem.TipoIgual(Mensagem.ABRETESOURO))
               {
                  TrataAberturaTesouro(msgArranjada.Nick, msgArranjada.Sala, msgArranjada.IdJogo);
                  return;
               }

               //se for um movimento
               if (mensagem.TipoIgual(Mensagem.MOVIMENTO))
               {
                  MensagemCliente mensagemRetorno = ServicosEntrada.MovimentoJogador(msgArranjada.Nick, msgArranjada.Sala, msgArranjada.IdJogo);
                  MeteMensagemParaEnviar(mensagemRetorno);
                  return;
               }
               #endregion
            }
            finally
            {
               DoUnlockGeral();
            }
         }


         //Se for uma mensagem vinda de um servidor
         if (mensagem.TipoMensagemServidor() == true)
         {
            #region Mensagens vindas de um servidor
            Configuration.Debug("Recebi uma mensagem do servidor " + mensagem._idOriginadorMensagem, Configuration.PRI_MIN);

            //Vamos ver se esse servidor nao devia estar morto
            if (Servidor.ServidorEstaVivo(mensagem._idOriginadorMensagem, ServerMain.TodosServidores) == false)
            {
               Configuration.Debug("Recebi uma mensagem de um servidor ja declarado como morto", Configuration.PRI_MAX);
               MensagemServidor msg = MensagemServidor.MensagemAvisoFosteDeclaradoMorto(ServerMain._minhaIdentificacao, mensagem._idOriginadorMensagem);
               MeteMensagemParaEnviar(msg);
               return;
            }

            MensagemServidor msgArranjada = (MensagemServidor)mensagem;
            string idJogo = msgArranjada.IdJogo;
            string idCliente = msgArranjada.IdCliente;
            int pontuacaoAntiga = msgArranjada.PontuacaoAntiga;
            int pontuacaoNova = msgArranjada.PontuacaoNova;
            bool jogoTerminou = msgArranjada.JogoTerminou;

            //Se for uma mensagem de retorno
            if (msgArranjada.EMsgDeRetorno() == true)
            {
               string guid = msgArranjada.guidUnico;
               //Vou ver se é retorno para mim
               lock (_lstRespostasMensagensPendentes)
               {
                  if (MensagemServidor.ListaContemMensagem(guid, _lstRespostasMensagensPendentes) == true)
                  {
                     Configuration.Debug("Recebi o retorno de uma msg que enviei.", Configuration.PRI_MIN);
                     MensagemServidor.RemoveMensagemPorGuid(guid, _lstRespostasMensagensPendentes);
                  }
               }
            }

            if (mensagem.TipoIgual(Mensagem.UPDATEESTADOGLOBAL))
            {
               Configuration.Debug("Vou realizar um update do meu estado ", Configuration.PRI_MED);
               RoomDesc novaSala = msgArranjada.NovaSala;
               string resultadoAccaoCliente = msgArranjada.ResultadoAccaoCliente;
               ServicosEntrada.RealizaUpdateDoEstado(idJogo, novaSala, idCliente, pontuacaoAntiga, pontuacaoNova, jogoTerminou, resultadoAccaoCliente);
               RelogioVectorial.IncrementaServidor(mensagem._idOriginadorMensagem, ServerMain._lstRelogiosVectoriais);
               return;
            }

            if (mensagem.TipoIgual(Mensagem.TENTATIVAABRIRTESOURO))
            {
               try
               {
                  DoLockGeral();

                  Configuration.Debug("Outro Servidor pediu-me para abrir uma sala de minha responsabilidade", Configuration.PRI_MED);

                  string guidUnico = msgArranjada.guidUnico;
                  ServicosEntrada.RealizaTentativaAbrirSala(idCliente, msgArranjada.NumSala, idJogo, guidUnico);

                  //Incremento o meu relogio vectorial
                  RelogioVectorial.IncrementaServidor(ServerMain._minhaIdentificacao, ServerMain._lstRelogiosVectoriais);
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.ENTRANOVOJOGADORNUMJOGO))
            {
               try
               {
                  DoLockGeral();
                  Configuration.Debug("Existe um novo cliente se juntou a um jogo", Configuration.PRI_MED);
                  ServicosEntrada.EntraNovoJogadorNumJogo(msgArranjada.IdJogo, msgArranjada.IdCliente);
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.REGISTANOVOJOGADORNOSISTEMA))
            {
               try
               {
                  DoLockGeral();

                  Configuration.Debug("Existe um novo cliente que se registou no sistema", Configuration.PRI_MED);
                  ServicosEntrada.RegistaNovoJogadorNoSistema(msgArranjada.IdCliente);
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.ADICIONANOVOJOGOSISTEMA))
            {
               try
               {
                  DoLockGeral();

                  Configuration.Debug("Existe um novo jogo no sistema", Configuration.PRI_MED);
                  ServicosEntrada.InsereNovoJogoSistema(msgArranjada.NovoJogo);
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.AVISOSERVIDORVAISAIR))
            {
               try
               {
                  DoLockGeral();
                  Configuration.Debug("Um servidor avisou-me que vai sair do sistema", Configuration.PRI_MED);
                  ServicosEntrada.AvisoServidorVaiSair(msgArranjada._idOriginadorMensagem);
                  RelogioVectorial.IncrementaNivelTodosRelogios(ServerMain._lstRelogiosVectoriais);
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.REMOVESERVIDORSISTEMA))
            {
               try
               {
                  DoLockGeral();

                  Configuration.Debug("Vou remover um servidor do meu sistema", Configuration.PRI_MED);
                  ServicosEntrada.RemoveServidorSistema(msgArranjada._idOriginadorMensagem, msgArranjada.NovoEstado);
                  RelogioVectorial.IncrementaNivelTodosRelogios(ServerMain._lstRelogiosVectoriais);
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.MORREU_OUTRO_SERVIDOR))
            {
               Configuration.Debug("Fui informado que o servidor " + msgArranjada.idServidorQueMorreu + " morreu", Configuration.PRI_MED);
               ServicosEntrada.MorreuOutroServidor(msgArranjada.idServidorQueMorreu, msgArranjada.ListaJogos);
               RelogioVectorial.IncrementaNivelTodosRelogios(ServerMain._lstRelogiosVectoriais);
               //Este lock é realizado quando recebo um sinal de STOP e só o "unlocko" quando me vem o novo estado
               DoUnlockGeral();
               return;
            }

            if (mensagem.TipoIgual(Mensagem.FOSTE_CONSIDERADO_MORTO))
            {
               try
               {
                  DoLockGeral();

                  Configuration.Debug("Argh...Os outros servidores consideraram-me morto!", Configuration.PRI_MED);
                  ServicosEntrada.FuiConsideradoMorto();
                  return;
               }
               finally
               {
                  DoUnlockGeral();
               }
            }

            if (mensagem.TipoIgual(Mensagem.STOP))
            {
               //Quando recebo esta mensagem faço stop ao meu sistema e só desligo quando receber o novo estado
               DoLockGeral();
               Configuration.Debug("Recebi uma mensagem de STOP do: " + mensagem._idOriginadorMensagem, Configuration.PRI_MED);
               ServicosEntrada.TrataStop(mensagem._idOriginadorMensagem, msgArranjada.idServidorQueMorreu, msgArranjada.IdCliente, msgArranjada.IdJogo, msgArranjada.NumSala);
               return;
            }

            if (mensagem.TipoIgual(Mensagem.REPLY_STOP))
            {
               Configuration.Debug("Recebi uma mensagem de REPLY-STOP do: " + mensagem._idOriginadorMensagem, Configuration.PRI_MED);
               bool pronto = ServicosEntrada.TrataReplyStop(mensagem._idOriginadorMensagem, msgArranjada.idServidorQueMorreu);
               if (pronto)
               {
                  //Este unlockGeral recebe o IdCLiente que vai servir de flag para depois fazer unlock
                  ComunicacaoServidor.DoUnlockGeral();
                  //E vou finalmente responder ao cliente
                  RefazPedidoAberturaDoCliente(msgArranjada.IdCliente, msgArranjada.IdJogo, msgArranjada.NumSala);
               }
               return;
            }

            return;
            #endregion
         }
         Configuration.Debug("ERRO:Recebi uma mensagem que nem 'e de um cliente nem servidor Tipo --> " + mensagem.Tipo, Configuration.PRI_MAX);
      }

      #region THREADS
      /// <summary>
      /// Thread responsavel por enviar as mensagens de saida do servidor 
      /// para os clientes
      /// </summary>
      static private void ThreadEnviar()
      {
         while (true)
         {
            //Percorre a lista de mensagens para enviar
            foreach (Mensagem mensagem in _outbox.ToArray())
            {
               //Se for uma mensagem para ir para um cliente
               if (mensagem.TipoMensagemCliente() == true)
               {
                  try
                  {
                     //procura Ligacao com o cliente respectivo
                     ICliente ligacao = LigacaoCliente.GetCanalComunicacao(mensagem._idOriginadorMensagem, ServerMain._lstLigacoesAJogadores);

                     //envia mensagem para o cliente e apaga a mensagem da lista
                     ligacao.MessageQueue((MensagemCliente)mensagem);
                     _outbox.Remove(mensagem);
                  }
                  catch (System.Runtime.Remoting.RemotingException exception)
                  {
                     //para tirar o warning
                     exception.GetType();

                     string msgErro = "Cliente Incontactável, vou tentar mais tarde";
                     Console.WriteLine(msgErro);
                  }
                  catch (SocketException exception)
                  {
                     //para tirar o warning
                     exception.GetType();

                     string msgErro = "Cliente Incontactável, vou tentar mais tarde";
                     Console.WriteLine(msgErro);
                  }
               }

               //Se for uma mensagem para enviar para outro servidor
               if (mensagem.TipoMensagemServidor() == true)
               {
                  Configuration.Debug("Vou enviar a mensagem para do tipo: " + mensagem.Tipo + " para " + mensagem._idDestinatarioMensagem, Configuration.PRI_MIN);

                  //Se eu tiver a atrasar mensagens para o servidor
                  if (ServerMain._lstServidoresEmHold.Contains(mensagem._idDestinatarioMensagem))
                  {
                     Configuration.Debug("Vou atrasar uma mensagem para: " + mensagem._idDestinatarioMensagem, Configuration.PRI_MIN);
                  }

                  //Se puder mandar a msg
                  else
                  {
                     enviaMensagemParaServidor((MensagemServidor)mensagem);
                     //Caso o sistema deva morrer a seguir
                     if (mensagem.TipoIgual(Mensagem.AVISOSERVIDORVAISAIR))
                     {
                        Configuration.Debug("Mori", Configuration.PRI_MED);
                        System.Environment.Exit(0);
                     }
                     _outbox.Remove(mensagem);
                  }
               }
            }
            //adormece um terco de segundo
            Thread.Sleep(Configuration.TEMPO_THREAD_ENVIAR_SERVIDOR);
         }
      }

      /// <summary>
      /// Thread responsavel por detectar os timeouts
      /// </summary>
      static private void ThreadTimeOut()
      {
         while (true)
         {
            ArrayList minhaLista = new ArrayList();
            lock (_lstRespostasMensagensPendentes)
            {
               foreach (MensagemServidor pend in _lstRespostasMensagensPendentes)
               {
                  minhaLista.Add(pend);
               }
            }
            Thread.Sleep(Configuration.TEMPO_THREAD_TIMEOUT);
            foreach (MensagemServidor minha in minhaLista)
            {
               if (MensagemServidor.ListaContemMensagem(minha.guidUnico, _lstRespostasMensagensPendentes) == true)
               {
                  string idDestinatario = minha._idDestinatarioMensagem;
                  Configuration.Debug("Detectei Timeout de " + idDestinatario, Configuration.PRI_MAX);
                  MensagemServidor.RemoveMensagemPorGuid(minha.guidUnico, _lstRespostasMensagensPendentes);
                  TrataDescobertaMorteServidor(idDestinatario, minha.IdCliente, minha.IdJogo, minha.NumSala);
               }
            }
         }
      }
      #endregion

   
      /// <summary>
      /// Funcao a ser chamada quando se descobre que um servidor morreu
      /// </summary>
      /// <param name="idServidorMorto">Identificacao do servidor que morreu</param>
      /// <param name="listaServidores">Lista de servidores (ainda com o servidor que morreu</param>
      /// <param name="listaJogos">Lista de jogos (ainda como se o servidor nao tivesso morrido</param>
      public static void TrataDescobertaMorteServidor(string idServidorMorto, string idCliente, string idJogo, int numSala)
      {
         //Vou remover o servidor
         Servidor.MeteServidorComoMorto(idServidorMorto, ServerMain.TodosServidores);

         //Caso eu seja o ultimo servidor vivo
         if (ServerMain.ServidoresVivos.Count == 0)
         {
            //Vou rearranjar os jogos
            ArrayList listaServidoresVivos = ServerMain.ServidoresVivos;
            ServerMain._lstJogos = Jogo.DistribuiJogos(ServerMain._lstJogos, listaServidoresVivos, ServerMain._minhaIdentificacao);

            //E refaço o pedido para mim
            RefazPedidoAberturaDoCliente(idCliente, idJogo, numSala);

            return;
         }

         //Caso contrario (ainda hajam mais servidores vivos para além de mim)
         /**** Vou mandar pedir a toda a gente para fazer STOP e me responder ao STOP *****/
         //Digo que o servidor tá na lista de servidores que espero um STOP-REPLY
         Servidor.MeteQueEsperoReplyStop(ServerMain.ServidoresVivos);

         //Criar e mandar mensagem
         MensagemServidor msgStop = MensagemServidor.FazSTOP(ServerMain._minhaIdentificacao, idServidorMorto, idCliente, idJogo, numSala);
         ComunicacaoServidor.MeteMensagemParaEnviar(msgStop);

         //Actualizo o nivel dos relogios vectoriais (pois sei que a estrutura do sistema vai mudar)
         RelogioVectorial.IncrementaNivelTodosRelogios(ServerMain._lstRelogiosVectoriais);

         //E faço lock do sistema enquando nao obtiver os reply-STOP de toda a gente
         ComunicacaoServidor.DoLockGeral();
      }
      #region FuncoesAuxiliares
      /// <summary>
      /// Realiza a repeticao de um pedido de abertura para este servidor
      /// Caso o idCliente == null nao faz nada
      /// </summary>
      /// <param name="idCliente">id do cliente que realizou a mensagem</param>
      /// <param name="idJogo">id do jogo em que se realiza a abertura</param>
      /// <param name="numSala">id da sala em que se realiza o pedido</param>
      private static void RefazPedidoAberturaDoCliente(string idCliente, string idJogo, int numSala)
      {
         if (idCliente != null)
         {
            TrataAberturaTesouro(idCliente, numSala, idJogo);
         }
      }

      /// <summary>
      /// Realiza a abertura do tesouro
      /// </summary>
      /// <param name="idCliente">id do cliente que abriu a sala</param>
      /// <param name="sala">Numero da sala em que o tesouro é aberto</param>
      /// <param name="idJogo">Id do jogo em que se abre o cofre</param>
      private static void TrataAberturaTesouro(string idCliente, int sala, string idJogo)
      {
         //Devolve null quando nao sou responsavel da sala
         MensagemCliente mensagemRetorno = ServicosEntrada.AbreTesouro(idCliente, sala, idJogo);

         //Pq quando nao sou responsavel da sala so respondo ao cliente mto mais tarde
         if (mensagemRetorno == null)
         {
            return;
         }
         //Senao digo ja que existe uma mensagem para enviar
         MeteMensagemParaEnviar(mensagemRetorno);

         //E incremento o contador do meu estado
         RelogioVectorial.IncrementaServidor(ServerMain._minhaIdentificacao, ServerMain._lstRelogiosVectoriais);
      }

      /// <summary>
      /// Metodo para fazer unlock geral do servidor
      /// </summary>
      private static void DoUnlockGeral()
      {
         lockGeral = false;
      }


      /// <summary>
      /// Metodo para fazer lock geral do servidor
      /// </summary>
      private static void DoLockGeral()
      {
         while (true)
         {
            if (lockGeral == true)
            {
               Thread.Sleep(Configuration.TEMPO_ESPERA_LOCK);
            }
            else
            {
               lockGeral = true;
               return;
            }
         }
      }

      /// <summary>
      /// Cria o Channel TCP
      /// </summary>
      /// <param name="portoEntrada">Porto a que o Servidor irá fazer bind</param>
      static public void Bind(int portoEntrada)
      {
         //inicializacao do servidor
         TcpChannel channel = new TcpChannel(portoEntrada);
         ChannelServices.RegisterChannel(channel);

         RemotingConfiguration.RegisterWellKnownServiceType(
             typeof(ComunicacaoServidor),
             "MMGServer",
             WellKnownObjectMode.Singleton);
         //FIM da inicializacao do porto
      }

      /// <summary>
      /// Envia de facto a mensagem para o servidor (trata do problema do control-c
      /// </summary>
      /// <param name="mensagem">Mensagem a ser enviada para o outro servidor</param>
      private static void enviaMensagemParaServidor(MensagemServidor mensagem)
      {
         Servidor servidorDestino = Servidor.GetServidor(mensagem._idDestinatarioMensagem, ServerMain.TodosServidores);
         try
         {
            //envia mensagem para o cliente
            servidorDestino.Ligacao.MessageQueue(mensagem);

            //Se nao der erro
            //Se for uma mensagem da qual espero retorno (pode haver timeout)
            if (mensagem.EMsgComRetorno() == true)
            {
               Configuration.Debug("Vou meter para enviar uma mensagem do tipo:" + mensagem.Tipo + " para: " + mensagem._idDestinatarioMensagem, Configuration.PRI_MIN);
               //Só as mensagens do tipo de servidor é que podem ter retorno
               MensagemServidor msg = (MensagemServidor)mensagem;
               lock (_lstRespostasMensagensPendentes)
               {
                  _lstRespostasMensagensPendentes.Add(msg);
               }
            }

         }
         catch (System.Runtime.Remoting.RemotingException exception)
         {
            if (servidorDestino.Vivo == false)
            {
               Configuration.Debug("Comunicaçao morreu a meio porque o outro servidor fechou-se a meio. Era o esperado.", Configuration.PRI_MAX);
               return;
            }
            //para tirar o warning
            exception.GetType();
            string msgErro = "Servidor morto com control-C, vou remover da lista de servidores, redistribuir todos jogos e avisar outros";
            Configuration.Debug("Servidor morto: " + mensagem._idDestinatarioMensagem + " tipo: " + mensagem.Tipo, Configuration.PRI_MAX);
            Configuration.Debug(msgErro, Configuration.PRI_MAX);

            TrataDescobertaMorteServidor(servidorDestino.Identificacao, mensagem.IdCliente, mensagem.IdJogo, mensagem.NumSala);
         }
         catch (System.Net.Sockets.SocketException exception)
         {
            if (servidorDestino.Vivo == false)
            {
               Configuration.Debug("Comunicaçao morreu a meio porque o outro servidor fechou-se a meio. Era o esperado.", Configuration.PRI_MAX);
               return;
            }
            //para tirar o warning
            exception.GetType();
            string msgErro = "Servidor morto com control-C, vou remover da lista de servidores, redistribuir todos jogos e avisar outros";
            Configuration.Debug("Servidor morto: " + mensagem._idDestinatarioMensagem + " tipo: " + mensagem.Tipo, Configuration.PRI_MAX);
            Configuration.Debug(msgErro, Configuration.PRI_MAX);

            TrataDescobertaMorteServidor(servidorDestino.Identificacao, mensagem.IdCliente, mensagem.IdJogo, mensagem.NumSala);
         }
      }

      static public void LancaThreadEnvio()
      {
         //Lança Threads de envio de mensagens
         _threadEnvio = new Thread(new ThreadStart(ThreadEnviar));
         _threadEnvio.Start();
      }

      static public void LancaThreadTimeOuts()
      {
         //Lança Threads de envio de mensagens
         _threadTimeOut = new Thread(new ThreadStart(ThreadTimeOut));
         _threadTimeOut.Start();
      }

      /// <summary>
      ///Unico ponto para enviar mensagens assincronas para a lista de mensagens a enviar
      ///Desdobra mensagens que estejam indicadas como broadcast
      /// </summary>
      /// <param name="mensagem">Mensagem a meter na lista para enviar</param>
      static public void MeteMensagemParaEnviar(Mensagem mensagem)
      {
         ArrayList servidoresVivos = ServerMain.ServidoresVivos;
         //E a mensagem for para enviar em broadcast 
         if (mensagem._idDestinatarioMensagem.Equals(Mensagem.IPBROADCAST))
         {
            //preciso de multiplicar mensagem para enviar pa todos os servidores
            Configuration.Debug(Configuration.DEBUG_VOU_REALIZAR_BROADCAST, Configuration.PRI_MIN);
            foreach (Servidor servidor in servidoresVivos)
            {
               Mensagem clone = mensagem.DuplicaMsg();
               clone._idDestinatarioMensagem = servidor.Identificacao;
               lock (_outbox)
               {
                  _outbox.Add(clone);
               }
            }
         }

       //Se nao for uma mensagem em broadcast
         else
         {
            lock (_outbox)
            {
               _outbox.Add(mensagem);
            }
         }
      }
      #endregion
   }
}

