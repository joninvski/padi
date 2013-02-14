using System;
using System.Collections;
using System.Text;

namespace MMG.Exec
{
   public class Configuration
   {
      public const string MODO_C = "C";
      public const string MODO_R1 = "R1";
      public const string MODO_R2 = "R2";
      public const char SEPARADOR_IDJOGO = ':';
      

      public const string ACCAO_ABRIU_TESOURO = "O cliente abriu um cofre com tesouro";
      public const string ACCAO_ABRIU_VENENO = "O cliente abriu um cofre com veneno";
      public const string ACCAO_ANTECIPOU_SE_CLIENTE = "Existiu um cliente que antecipou esta jogada";

      public const string DEFAULT_MAP_PATH = "C:\\Documents and Settings\\Joao Trindade\\Desktop\\padi\\ConfigFiles\\map.txt";
      public const string DEFAULT_CONFIG_PATH = "C:\\Documents and Settings\\Joao Trindade\\Desktop\\padi\\ConfigFiles\\config.txt";

      public const string PORTO_CLIENTE_DEFAULT = "8080";
      public const string PORTO_SERVIDOR_DEFAULT = "8086";

      public const string IP_CLIENTE_DEFAULT = "localhost";
      public const string IP_SERVIDOR_DEFAULT = "localhost";

      public const string NICKNAME_CLIENTE_DEFAULT = "Manel";

      public const string TEXTO_GANHOU_OURO = "HUUUURRAYYYY!!! I'M RICH!!! FILTHY RICH!!!";
      public const string TEXTO_RESPIROU_VENENO = "Cofff...cofff.....Veneno :-(";
      public const string TEXTO_ABRIR_COFRE_2_VEZES = "Já abriu este cofre. O mesmo jogador nao pode tirar 2 vezes.";
      public const string TEXTO_JOGADOR_ANTECIPOU_JOGADA = "Como jogo é distribuido, houve um jogador se antecipou a si. Cofre ja nao existe";
      public const string TEXTO_SALA_ONDE_EXISTIU_VENENO = "Existe um leve aroma a veneno no ar desta sala, mas já nao há perigo";
      public const string TEXTO_COFRE_VAZIO = "Esta sala já conteve um tesouro. Agora encontra-se completamente vazio";

      public const string TEXTO_COFRE_ABERTO = "O cofre nesta sala encontra-se aberto :D";

      public const string TEXTO_SERVIDOR_ABERTURA_INVALIDA = "Um cliente acabou de executar uma abertura invalida pois outro cliente antecipou-se";

      public const string DEBUG_EU_SERVIDOR_RESPONSAVEL_ABERTURA = "Cliente pediu-me para abrir tesouro e sou eu o responsavel da sala";
      public const string DEBUG_NAO_SOU_EU_RESPONSAVEL = "Cliente pediu-me para abrir tesouro e eu nao o responsavel da sala";
      public const string DEBUG_CRIAR_JOGO_REPARTIDO = "Vou criar um jogo repartido --- R1 ou R2";
      public const string DEBUG_VOU_REALIZAR_BROADCAST = "Nova msg para a outbox em broadcast. Vou desdobrar em varias msgs pa td a gente";

      public const string DEBUG_N_ARGUMENTOS_INVALIDOS = "Numero de argumentos errado";
      public const int TEMPO_THREAD_TIMEOUT = 5000;
      public const int TEMPO_THREAD_ENVIAR_SERVIDOR = 300;
      public const int TEMPO_THREAD_ENVIAR_CLIENTE = 300;
      public const int TEMPO_ESPERA_LOCK = 100;

      public const int NIVEL_SISTEMA_INICIAL = 0;

      public const int PRI_MAX = 3;
      public const int PRI_MED = 2;
      public const int PRI_MIN = 1;

      private const int DEBUG_LEVEL_ACTUAL = PRI_MED;


      static public void Debug(string texto, int prioridade)
      {
         if (prioridade >= DEBUG_LEVEL_ACTUAL)
         {
            System.Console.WriteLine(texto);
         }
      }
   }
}
