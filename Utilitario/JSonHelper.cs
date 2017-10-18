using System;
using System.IO;
using System.Net;
using System.Text;

namespace Utilitario
{
    public static class JSONHelper
    {

        /// <summary>
        /// Recupera o valor da requisição pela URL
        /// </summary>
        /// <param name="aUrl"></param>
        /// <returns></returns>
        public static string GetJSONString(string aUrl)
        {
            try
            {
                // Cria a requisição pela URL
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(aUrl);

                // Realiza o response 
                WebResponse response = request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    
                    // Lê o valor do reader
                    return reader.ReadToEnd(); ;
                }
            }
            catch (Exception)
            {
                throw;
            }                       
        }

        /// <summary>
        /// Método que valida se a url é válida
        /// </summary>
        /// <param name="aUrl"></param>
        public static void ValidaURL(string aUrl)
        {
            try
            {
                // Cria a requisição pela URL
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(aUrl);

                // Realiza o response 
                WebResponse response = request.GetResponse();
            }
            catch (Exception)
            {
                throw new Exception(Contexto.Excecao001);
            }
        }

    }

}
