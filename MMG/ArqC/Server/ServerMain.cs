using System;
using System.Collections;
using System.Text;
using MMG.Config;
using System.IO;
using System.Net.Sockets;

namespace MMG.Exec
{
   class ServerMain
   {
      //Lista de jogos do Servidor (SO SEUS TIPO C + TODOS TIPO R1)
      public static ArrayList _lstJogos = new ArrayList();

      //Utilizada pela thread de envio pa ser mais rápido falar com os jogadores
      public static ArrayList _lstLigacoesAJogadores = new ArrayList();
      //Lista de servidores existentes
      private static ArrayList _lstServidores = new ArrayList();
      public static ArrayList _lstServidoresEmHold = new ArrayList();

      public static string _minhaIdentificacao;
      public static ArrayList _lstRelogiosVectoriais = new ArrayList();

      /// <summary>
      /// Valida argumentos, inicializa o servidor e le linha comandos
      /// </summary>
      /// <param name="args"></param>
      /// <returns></returns>
      static int Main(string[] args)
      {
         int meuPorto = -1;
         string meuIp = "";
         int portoServidorDestino = -1;
         string ipServidorDestino = "";
         bool ligacaoOutroServidor = false;

         if (ValidaEntradaSimples(args, ref meuPorto) == false)
         {
            return 1;
         }

         //Caso esteja a tentar-me ligar a outro servidor
         if (args.Length == 4)
         {
            ligacaoOutroServidor = true;
         }

         meuIp = args[0];
         _minhaIdentificacao = meuIp + ":" + meuPorto;

         //Caso esteja a ligar-me vou certificar-me que arg certos e porto é numero
         if (ligacaoOutroServidor == true)
         {
            if (ValidaEntradaLigacao(args, ref portoServidorDestino) == false)
            {
               return 1;
            }
            ipServidorDestino = args[2];
         }

         try
         {
            ComunicacaoServidor.Bind(meuPorto);
         }
         catch (Exception excep)
         {
            //So para tirar o warning de var nao usada
            excep.Equals(excep.Message);

            System.Console.WriteLine("Impossivel fazer bind do porto");
            return 1;
         }

         //Inicia a thread responsavel por enviar mensagens
         ComunicacaoServidor.LancaThreadEnvio();

         //Inicializa a thread responsavel pelos timeouts
         ComunicacaoServidor.LancaThreadTimeOuts();


         //Avisa que está tudo OK
         System.Console.WriteLine("Servidor iniciou OK. \n\rEncontra-se a correr no porto: {0}\r\n", meuPorto);

         //E vou ligar-me ao outro servidor
         if (ligacaoOutroServidor == true)
         {
            string identificadorServidorDestino = ipServidorDestino + ":" + portoServidorDestino;
            System.Console.WriteLine("Adicinei a minha lista de servidores {0}", identificadorServidorDestino);
            try
            {
               Servidor servidorDestino = new Servidor(identificadorServidorDestino);

               int nivelSistema = -2;
               //Regista o servidor no que já existe
               ArrayList listaIdsServidores = servidorDestino.PedeListaServidores(ref nivelSistema);
               //Como eu vou entrar o nivel do sistema sobe um
               nivelSistema++;
               System.Console.WriteLine("Nivel do sistema " + nivelSistema);

               //Cria ligacoes com todos os servidores (menos com quem falei)
               _lstServidores = Servidor.CriaLigacoesTodosServidores(listaIdsServidores);
               //So falta adicionar o gajo com quem falei
               _lstServidores.Add(servidorDestino);

               //Cria os relogios vectoriais de todos os servidores
               _lstRelogiosVectoriais = RelogioVectorial.CriaTodosRelogiosVectorias(listaIdsServidores, nivelSistema);
               _lstRelogiosVectoriais.Add(new RelogioVectorial(servidorDestino.Identificacao, nivelSistema));
               //e claro crio o meu relogio vectorial
               _lstRelogiosVectoriais.Add(new RelogioVectorial(_minhaIdentificacao, nivelSistema));

               /**** Neste momento ja tenho ligacoes com todos os servidores existentes *****/

               //Agora para todos eles vou-lhes dizer me inserirem na lista de servidores
               RegistarEmTodosServidores(_minhaIdentificacao);

               //Vou buscar os clientes que ja existem no sistema
               _lstLigacoesAJogadores = BuscaClientesDoSistema();

               //Agora vou buscar os varios jogos a um servidor qualquer (nao jogos tipo C)
               _lstJogos = BuscaJogosDoSistema();

               //Distribuo varias salas dos mapas igualmente pelos servidores
               _lstJogos = Jogo.DistribuiJogos(_lstJogos, _lstServidores, _minhaIdentificacao);

               //Agora toca de enviar os mapas "distribuidos" a todos os servidores
               EnviarNovosMapasDistribuidos(_lstJogos);

            }
            catch (System.Runtime.Remoting.RemotingException exception)
            {
               //So para tirar o warning
               exception.Equals("");

               System.Console.WriteLine("Nao foi possivel comunicar com o outro servidor ---> ip/porto errado?");
               return 1;
            }
			catch (SocketException exception)
			{
				//So para tirar o warning
				exception.Equals("");

				System.Console.WriteLine("Nao foi possivel comunicar com o outro servidor ---> ip/porto errado?");
				return 1;
			}


         }
         //Caso nao me esteja a ligar a ninguem
         else
         {
            _lstRelogiosVectoriais.Add(new RelogioVectorial(_minhaIdentificacao, Configuration.NIVEL_SISTEMA_INICIAL));
         }

         //Fica para sempre a ler a linha comandos
         LeLinhaComandos();
         return 0;
      }

      private static void EnviarNovosMapasDistribuidos(ArrayList _lstJogos)
      {
         foreach (Servidor servidor in _lstServidores)
         {
            servidor.ActualizaListaJogos(_lstJogos);
         }
      }

      private static ArrayList BuscaClientesDoSistema()
      {
         //Para escolher um servidor aleatorio ao qual vai pedir os jogos
         int servidorRandom = new Random().Next(_lstServidores.Count);
         ArrayList devolver = new ArrayList();

         //Pede os jogos
         Servidor servidor = (Servidor)_lstServidores[servidorRandom];
         ArrayList listaIdsCliente = servidor.PedeListaClientesDoSistema();

         foreach (String idCliente in listaIdsCliente)
         {
            devolver.Add(new LigacaoCliente(idCliente, null, false));
         }
         return devolver;
      }

      private static ArrayList BuscaJogosDoSistema()
      {
         //Para escolher um servidor aleatorio ao qual vai pedir os jogos
         int servidorRandom = new Random().Next(_lstServidores.Count);

         //Pede os jogos
         Servidor servidor = (Servidor)_lstServidores[servidorRandom];
         return servidor.PedeListaJogos();
      }

      private static void RegistarEmTodosServidores(string idDoMeuServidor)
      {
         Servidor.AcrescentaServidor(idDoMeuServidor, _lstServidores);
      }

      /// <summary>
      /// Le a linha de comandos do servidor e executa comandos adequados
      /// </summary>
      private static void LeLinhaComandos()
      {
         string comando;

         while (true)
         {
            System.Console.WriteLine("\n\rOpções de Menu");
            System.Console.WriteLine("[S] - Listar Servidores");
            System.Console.WriteLine("print para imprimir");
            System.Console.WriteLine("[Q] - Para Sair");
            comando = System.Console.ReadLine();

            if (comando.ToLower().Equals("s"))
            {
               System.Console.WriteLine(Servidor.ImprimeListaServidores(_lstServidores));
               continue;
            }

            if (comando.ToLower().Equals("q"))
            {
               System.Console.WriteLine("Saindo graciosamente.");
               RemoveMeuServidorSistema(_minhaIdentificacao);
               return;
            }

            //A partir daqui é para se tiver vários argumentos
            char[] separador = { ' ' };
            string[] arrumado = comando.Split(separador);

            if (arrumado[0].Equals("print"))
            {
               if (arrumado.Length != 3)
               {
                  System.Console.WriteLine(Configuration.DEBUG_N_ARGUMENTOS_INVALIDOS, Configuration.PRI_MAX);
                  continue;
               }
               Jogo jogo = Jogo.GetJogo(arrumado[1], _lstJogos);

               if (jogo == null)
               {
                  System.Console.WriteLine("Jogo inexistente");
                  continue;
               }

               try
               {
                  int numSala = Int32.Parse(arrumado[2]);
                  try
                  {
                     System.Console.WriteLine(jogo.Sala(numSala));
                     continue;
                  }
                  catch (IndexOutOfRangeException ex)
                  {
                     ex.Source = ex.Source;
                     System.Console.WriteLine("Essa sala nao existe");
                     continue;
                  }
               }

               catch (FormatException excep)
               {
                  excep.Source = excep.Source;
                  Jogador jogador = jogo.GetJogador(arrumado[2]);
                  if (jogador == null)
                  {
                     System.Console.WriteLine("Esse jogador nao se encontra neste jogo");
                     continue;
                  }

                  System.Console.WriteLine(jogador);
                  continue;
               }
            }
            if (arrumado[0].ToLower().Equals("hold"))
            {
               if (arrumado.Length != 2)
               {
                  System.Console.WriteLine(Configuration.DEBUG_N_ARGUMENTOS_INVALIDOS, Configuration.PRI_MAX);
                  continue;
               }
               string servID = arrumado[1];

               if (Servidor.ServidorExiste(servID, _lstServidores) == false)
               {
                  Configuration.Debug("ID de servidor invalido", Configuration.PRI_MAX);
                  continue;
               }

               //Caso esteja tudo OK
               _lstServidoresEmHold.Add(servID);
               continue;
            }

            if (arrumado[0].ToLower().Equals("resume"))
            {
               if (arrumado.Length != 2)
               {
                  System.Console.WriteLine(Configuration.DEBUG_N_ARGUMENTOS_INVALIDOS, Configuration.PRI_MAX);
                  continue;
               }
               string servID = arrumado[1];
               if (Servidor.ServidorExiste(servID, _lstServidores) == false)
               {
                  Configuration.Debug("ID de servidor invalido", Configuration.PRI_MAX);
                  continue;
               }

               if (_lstServidoresEmHold.Contains(servID) == false)
               {
                  Configuration.Debug("Esse servidor nao estava em hold", Configuration.PRI_MAX);
                  continue;
               }

               //Caso esteja tudo OK
               _lstServidoresEmHold.Remove(servID);
               continue;
            }
         }
      }

      private static void RemoveMeuServidorSistema(string _minhaIdentificacao)
      {
         int servidorRandom = new Random().Next(_lstServidores.Count);

         //Pede os jogos
         Servidor servidor = (Servidor)_lstServidores[servidorRandom];

         MensagemServidor msg = MensagemServidor.AvisaServidorVaiSair(_minhaIdentificacao, servidor.Identificacao);
         ComunicacaoServidor.MeteMensagemParaEnviar(msg);
      }

      /// <summary>
      /// Valida os argumentos de inicialização do servidor
      /// pela linha de comandos
      /// </summary>
      /// <param name="args"></param>
      /// <param name="portoEntrada"></param>
      /// <returns></returns>
      private static bool ValidaEntradaSimples(string[] args, ref int portoCliente)
      {
         //validacao dos parametros entrada
         if ((args.Length == 2 || args.Length == 4) == false)
         {
            Console.WriteLine("[ERRO] - Argumentos Invalidos\r\n \tUsage: Server <<ipServidor>> <<PortoServidor>> [<<ipServidorDestino>> <<portoServidorDestino>>]");
            return false;
         }

         try
         {
            portoCliente = int.Parse(args[1]);
         }
         catch (FormatException e)
         {
            Console.WriteLine("[ERRO] - Argumentos Invalidos\r\n \tUsage: Server [portoEntrada]\r\nDescricao do Erro: " + e);
            return false;
         }
         //FIM validacao da entrada

         return true;
      }

      /// <summary>
      /// Quando o servidor necessita de se ligar a outro servidor
      /// </summary>
      /// <param name="args">Arguementos vindos da linha de comandos</param>
      /// <param name="portoServidorDestino">Porto do servidor de destino</param>
      /// <returns>true caso esteja tudo OK. False caso contrario</returns>
      private static bool ValidaEntradaLigacao(string[] args, ref int portoServidorDestino)
      {
         try
         {
            portoServidorDestino = int.Parse(args[3]);
            return true;
         }
         catch (FormatException e)
         {
            Console.WriteLine("[ERRO] - Argumentos Invalidos\r\n \tUsage: Server [portoServidorDestino]\r\nDescricao do Erro: " + e);
            return false;
         }
      }

      public static ArrayList ServidoresVivos
      {
         get { return Servidor.GetServidoresVivos(_lstServidores); }
      }

      public static ArrayList TodosServidores
      {
         get { return _lstServidores; }
      }

      public static int GetMeuNivelSistema()
      {
         RelogioVectorial rel = RelogioVectorial.GetRelogio(_minhaIdentificacao, _lstRelogiosVectoriais);

         return rel.Nivel;
      }
   }
}
