using System;
using System.Collections;
using System.Text;
using MMG.Config;

namespace MMG.Exec
{
   public interface ICliente
   {
      //Fica encarregue de receber as mensagens do cliente
      void MessageQueue(MensagemCliente mensagem);
   }

   public interface IServer
   {
      /// <summary>
      /// Fica encarregue de receber as mensagens do servidor
      /// (fila de mensagens de entrada)
      /// </summary>
      /// <param name="mensagem"></param>
      void MessageQueue(Mensagem mensagem);

      /// <summary>
      /// Regista um jogador
      /// </summary>
      /// <param name="ipCliente">Ip do cliente</param>
      /// <param name="portoCliente">Porto do cliente</param>
      /// <param name="nickName">NickName do cliente</param>
      /// <returns></returns>
      bool RegistaCliente(string ipCliente, string portoCliente, string nickName);

      /// <summary>
      /// Pede lista de jogos a decorrer no servidor
      /// </summary>
      /// <returns>ArrayList com String identificando os jogos</returns>
      ArrayList ListaJogos();

      /// <summary>
      /// Cria um novo jogo
      /// </summary>
      /// <param name="mapPath">Path para o ficheiro do mapa</param>
      /// <param name="configPath">Path para o ficheiro de confirguracao</param>
      /// <returns></returns
      bool CriaJogo(string idJogo, string mapPath, string configPath, string modoJogo);

      /// <summary>
      /// Junta o cliente a um jogo
      /// </summary>
      /// <param name="idJogo">id do jogo a que se vai ligar</param>
      /// <param name="nickJogador">nick do jogador</param>
      /// <returns>Jogo ao qual o cliente se ligou</returns>
      RoomDesc JuntarJogo(string idJogo, string nickJogador, ref int salasComGas);

      /// <summary>
      /// Quando o novo servidor avisa o servidor que quer entrar
      /// </summary>
      /// <param name="url">ip:porto do novoServidor</param>
      /// <returns>ArrayList com os servidores existentes</returns>
      ArrayList PedeListaServidores(ref int nivelSistema);

      /// <summary>
      /// Pede a lista dos clientes que existem no sistema
      /// </summary>
      /// <returns>Arraylist de strings do ids dos clientes que existem no sistema</returns>
      ArrayList PedeListaClientesExistentes();

      /// <summary>
      /// Pede a lista de jogos
      /// </summary>
      /// <returns>ArrayList com os Jogos dos jogos que existem no sistema MMG</returns>
      ArrayList PedeListaJogos();

      /// <summary>
      /// Acrescenta um servidor a lista de servidores
      /// </summary>
      /// <param name="urlNovoServidor"></param>
      /// <returns></returns>
      void AcrescentaServidor(string urlNovoServidor);

      /// <summary>
      /// Actualiza os jogos do servidor
      /// </summary>
      /// <param name="_lstJogos">Nova lista de jogos</param>
      void ActualizaJogos(ArrayList _lstJogos);

      /// <summary>
      /// Informa o sercidor que o cliente vai passar para modo offline
      /// </summary>
      /// <param name="idJogo"></param>
      /// <returns></returns>
      MapDesc ClienteVaiOffline(string idJogo);


      /// <summary>
      /// Pede o estado do sistema que o servidor conhece e desliga cliente
      /// </summary>
      /// <param name="idCliente"></param>
      /// <returns></returns>
      ArrayList ClienteVaiMudar(string idCliente);

      /// <summary>
      /// O cliente quer voltrar a entrar noutro servidor
      /// </summary>
      /// <param name="idCliente"></param>
      /// <returns></returns>
      bool ClienteVoltaEntrar(string idCliente, string ipCliente, string portoCliente, ArrayList lstRelogiosConhecidos);
   }
}
