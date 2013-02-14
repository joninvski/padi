using System;
using System.Collections;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using MMG;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using MMG.Config;
using MMG.Exec;


namespace MMG.Exec
{
   ///<summary>Classe representa as conexões do cliente com o servidor.
   ///</summary>
   ///<remarks>
   ///Responsavel por mandar/receber msg do canal de comunicação e comunicar com a form
   ///</remarks>
   public class Cliente : MarshalByRefObject, ICliente
   {
      private IServer _servidor = null;
      private string _nickName;
      private Thread _threadEnvio;
      private static TcpChannel _channel;
      private ArrayList _outbox = new ArrayList();

      //Tem que existir um construtor vazio
      public Cliente() { }

      /// <summary>
      /// Constructor - Cria porto para comunicar com o servidor e estabelece ligação
      /// </summary>
      /// <param name="ipServidor"> ip do servidor a quem se vai ligar</param>
      /// <param name="portoServidor"> porto do servidor a quem se vai ligar </param>
      /// <param name="portoCliente"> porto do cliente (meu) </param>
      /// <param name="nickName">NickName do cliente (meu)</param>
      public Cliente(string ipServidor, int portoServidor, int portoCliente, string nickName)
      {
         NickName = nickName;

         //Manda excepção caso nao consiga criar porto
         TcpChannel channel = CriaPorto(portoCliente);

         //Criar o meu objecto
         RemotingConfiguration.RegisterWellKnownServiceType(
             typeof(Cliente),
             "Cliente",
             WellKnownObjectMode.Singleton);
         //meu objecto criado

         //Manda excepção caso nao consiga
         LigaServidor(ipServidor, portoServidor);

         //Cria thread para enviar mensagens
         _threadEnvio = new Thread(new ThreadStart(ThreadEnviar));
         _threadEnvio.Start();
      }


      /// <summary>
      /// Regista (Login) um cliente no Servidor
      /// </summary>
      /// <param name="ipCliente">Ip do cliente</param>
      /// <param name="portoCliente">Porto do cliente</param>
      /// <param name="nickName">NickName do Cliente</param>
      /// <returns>False Caso o Login nao tenha sido aceite</returns>
      public bool RegistaCliente(string ipCliente, string portoCliente, string nickName)
      {
         return Servidor.RegistaCliente(ipCliente, portoCliente, nickName);
      }

      /// <summary>
      /// Função sincrona que cria um novo jogo no servidor
      /// </summary>
      /// <param name="idJogo"></param>
      /// <param name="mapPath"></param>
      /// <param name="configPath"></param>
      /// <returns></returns>
      public bool CriaJogo(string idJogo, string mapPath, string configPath, string modoJogo)
      {
         return Servidor.CriaJogo(idJogo, mapPath, configPath, modoJogo);
      }

      /// <summary>
      /// Junta o cliente a um jogo
      /// </summary>
      /// <param name="idJogo">Id do Jogo</param>
      /// <returns>Jogo ao qual o cliente se juntou</returns>
      public RoomDesc JuntarJogo(string idJogo, ref int salasComGas)
      {
         return Servidor.JuntarJogo(idJogo, _nickName, ref salasComGas);
      }

      /// <summary>
      /// Função Sincrona que pede a lista de idJogos ao servidor
      /// </summary>
      public ArrayList PedeListaJogos()
      {
         return Servidor.ListaJogos();
      }

      internal MapDesc GoOffline(string idJogo)
      {
         return _servidor.ClienteVaiOffline(idJogo);
      }

      internal bool VoltarEntrarServidor(string ipCliente, string portoCliente, ArrayList lstRelogios)
      {
         return _servidor.ClienteVoltaEntrar(_nickName, ipCliente, portoCliente, lstRelogios);
      }

      internal ArrayList MudarServidor()
      {
         return _servidor.ClienteVaiMudar(_nickName);
      }

      /// <summary>
      /// Fila de mensagem de entrada, e por aqui que devem receber as mensagem do
      /// </summary>
      /// <param name="tipoMsg"> define o tipo de mensagem k se pretende receber
      /// 
      /// </param>
      /// <param name="obj"> varia conforme o tipo de mensagem
      /// </param>
      public void MessageQueue(MensagemCliente mensagem)
      {
         //Recebe mensagens no cliente
         if (mensagem._jogoTerminou == true)
         {
            MMGCliente.TrataFimJogo(mensagem._pontuacaoNova, mensagem._top10);
         }

         if (mensagem.TipoIgual(Mensagem.RESPOSTAMOVIMENTO))
         {
            MMGCliente.TrataRespostaMovimento(mensagem._novaSala, mensagem._numSalasAdjComGas, 0);
            return;
         }
         else if (mensagem.TipoIgual(Mensagem.RESPOSTAABERTURA))
         {
            MMGCliente.TrataRespostaAbertura(mensagem._pontuacaoAntiga, mensagem._pontuacaoNova, mensagem._top10, mensagem.ResultadoAccaoCliente);
            return;
         }

         else if (mensagem.TipoIgual(Mensagem.SISTEMA_EM_ACTUALIZACAO))
         {
            MMGCliente.TrataActualizacaoSistema();
            return;
         }
         return;
      }

      /// <summary>
      /// Fila de mensagem para enviar para o servidor
      /// </summary>
      /// <param name="tipoMsg"> define o tipo de mensagem k se pretende enviar</param>
      /// <param name="obj"> varia conforme o tipo de mensagem</param>
      public void OutMessage(MensagemCliente mensagem)
      {
         //Guarda na lista de mensagens para enviar
         lock (_outbox)
         {
            _outbox.Add(mensagem);
         }
      }


      /// <summary>
      /// Destroi o porto a que o cliente previamente fez bin
      /// (util quando o nickname é registado)
      /// </summary>
      public static void DestroiPorto()
      {
         ChannelServices.UnregisterChannel(_channel);
         return;
      }

      /// <summary>
      /// Cria o porto do cliente
      /// </summary>
      /// <param name="portoCliente">porto que o cliente pretende obter</param>
      /// <returns> TcpChannel do porto </returns>
      /// <exception cref="LigacaoException"> Caso seja impossivel criar o porto
      /// </exception>
      private TcpChannel CriaPorto(int portoCliente)
      {
         //Cria o porto do cliente
         try
         {
            _channel = new TcpChannel(portoCliente);
            ChannelServices.RegisterChannel(_channel);
            return _channel;
         }

         catch (Exception)
         {
            throw new LigacaoException("Impossivel criar o porto do cliente...");
         }
      }

      /// <summary>
      /// Liga o cliente ao servidor, somente pa ser usada uma vez ao inicio
      /// </summary>
      /// <param name="ipServidor">ip do servidor a quem se vai ligar</param>
      /// <param name="portoServidor"> porto do servidor a quem se vai ligar</param>
      /// <exception cref="LigacaoException"> Caso seja impossivel ligar-se 
      /// ao servidor </exception>
      private void LigaServidor(string ipServidor, int portoServidor)
      {
         //realiza a ligação ao servidor
         _servidor = (IServer)Activator.GetObject(
                         typeof(IServer),
                         "tcp://" + ipServidor + ":" + portoServidor + "/MMGServer");


         if (_servidor == null)
         {
            throw new LigacaoException("Impossivel encontrar o servidor...");
         }
      }

      /// <summary>
      /// Cria a ligacao a um servidor 
      /// </summary>
      /// <param name="idServidor">Identificacao do servidor a quem me vou ligar (ip:porto)</param>
      private void LigaServidor(string idServidor)
      {
         //realiza a ligação ao servidor
         try
         {
            _servidor = (IServer)Activator.GetObject(
                         typeof(IServer),
                         "tcp://" + idServidor + "/MMGServer");
         }
         catch (System.Runtime.Remoting.RemotingException excep)
         {
            //So para tirar o warning
            excep.GetType();
            throw new LigacaoException("Impossivel encontrar o servidor...");
         }
		 catch (SocketException excep)
		 {
			 //So para tirar o warning
			 excep.GetType();
			 throw new LigacaoException("Impossivel encontrar o servidor...");
		 }


         if (_servidor == null)
         {
            throw new LigacaoException("Impossivel encontrar o servidor...");
         }
      }


      /// <summary>
      /// Thread encarregue de ler a lista de mensagens para enviar e envia-las
      /// </summary>
      private void ThreadEnviar()
      {
         while (true)
         {
            //adormece um segundo
            Thread.Sleep(Configuration.TEMPO_THREAD_ENVIAR_CLIENTE);
            if (MMGCliente._modoOffline)
            {
               continue;
            }

            lock (_outbox)
            {
               //Percorre a lista de mensagens para enviar
               foreach (MensagemCliente mensagem in _outbox.ToArray())
               {
                  try
                  {
                     this.Servidor.MessageQueue(mensagem);
                     _outbox.Remove(mensagem);
                  }
                  catch (System.Runtime.Remoting.RemotingException exception)
                  {
                     //para tirar o warning
                     exception.GetType();
                     string msgErro = "Servidor Incontactável, vou tentar mais tarde. " + exception;
                     //escreve Log da form
                     MMGCliente.EscreveLog(msgErro);
                  }
				  catch (SocketException exception)
				  {
					  //para tirar o warning
					  exception.GetType();
					  string msgErro = "Servidor Incontactável, vou tentar mais tarde. " + exception;
					  //escreve Log da form
					  MMGCliente.EscreveLog(msgErro);
				  }

               }
            }
         }
      }

      /// <summary>
      /// Ligacao ao Servidor
      /// </summary>
      private IServer Servidor
      {
         get { return _servidor; }
      }

      public void MudarComunicacaoServidor(string idServidor)
      {
         LigaServidor(idServidor);
      }

      /// <summary>
      /// NickName do cliente
      /// </summary>
      public string NickName
      {
         get { return _nickName; }
         set { _nickName = value; }
      }
   }
}
