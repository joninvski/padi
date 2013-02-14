using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace MMG.Exec
{
    public class Login
    {
        /// <summary>
        /// Executa o comando Login
        /// </summary>
        /// <returns></returns>
        public static Cliente regista(ref bool aceite, string portoCliente, string portoServidor,
               string ipServidor, string nickName, string ipLocal)
        {
            //envia excepção em caso de erro
            ValidaLogin(ipServidor, nickName);

            //envia excepção em caso de erro
            int portoServidorInteiro = validaPorto(portoServidor);
            int portoClienteInteiro = validaPorto(portoCliente);
           
            Cliente _ligacao;
            
            //cria a ligação com o servidor
            _ligacao = new Cliente(ipServidor, portoServidorInteiro, portoClienteInteiro, nickName);


            //efectua Login no servidor
            aceite = _ligacao.RegistaCliente(ipLocal, "" + portoCliente, nickName);
            
            return _ligacao;
        }

        /// <summary>
        /// Verifica se o porto do cliente e do servidor estão escritos com valores numericos
        /// </summary>
        /// <param name="porto">string contendo o porto</param>
        /// <returns>Porto com valor inteiro</returns>
        private static int validaPorto(string porto)
        {
            try{
                return int.Parse(porto);
            }
            catch (Exception ex)
            {
                //para remover o warning do ex nao ser usado
                ex.Source = ex.Source;

                throw new DadosInvalidosException("Porto com valor Invalido");
            }
        }

        /// <summary>
        /// Verifica se o comando Login tem todos os campos 
        /// necessários preenchidos
        /// </summary>
        private static void ValidaLogin(string ipServidor, string nickName)
        {
            if (ipServidor == null || ipServidor == "")
            {
                throw new DadosInvalidosException("Necessita de definir o IP do Servidor\r\n");
            }

            if (nickName == null || nickName == "")
            {
                throw new DadosInvalidosException("Necessita de definir o NickName do Cliente\r\n");
            }
        }
    }
}
