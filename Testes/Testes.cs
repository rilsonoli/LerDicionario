using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilitario;
using LerDicionario;

namespace Testes
{
    [TestClass]
    public class Testes
    {
        [TestMethod]
        public void ValidaServico()
        {
            JSONHelper.GetJSONString(String.Format("http://testes.ti.lemaf.ufla.br/api/Dicionario" + "/{0}", 0));
        }

        [TestMethod]
        public void ValidaEntradaMaisPalavras()
        {

            string[] entrada = { "", "" };

            var posicao = new BuscarPosicao(entrada);

            var validacao = posicao.ValdiarParametros(entrada);

            if (validacao)
                throw new ArgumentOutOfRangeException("true");

        }

        [TestMethod]
        public void ValidaEntradaNula()
        {
            string[] entrada = null;

            var posicao = new BuscarPosicao(entrada);            

            var validacao = posicao.ValdiarParametros(entrada);

            if (validacao)
                throw new ArgumentOutOfRangeException("true");
        }


        [TestMethod]
        public void ValidaEntradaValida()
        {
            string[] entrada = {"Casa"};

            var posicao = new BuscarPosicao(entrada);

            var validacao = posicao.ValdiarParametros(entrada);

            if (!validacao)
                throw new ArgumentOutOfRangeException("false");
        }
    }
}
