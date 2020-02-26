using System;
using System.Collections.Generic;
using System.Linq;

namespace ValidacaoFluente.Internals
{
	internal abstract class ValidadorValor<T, K> : IValidadorValor<T, K>, IItemValidacao
	{

		private readonly IList<RegraParaValidacao> _validacoes;

		internal IValidadorRaiz<T> Validador { get; }

		public virtual K Valor { get; }

		internal ValidadorValor(IValidadorRaiz<T> validador)
		{
			Validador = validador;
			_validacoes = new List<RegraParaValidacao>();
		}

		internal void PersonalizarMensagemDaUltimaValidacao(string mensagem)
		{
			var regra = _validacoes.LastOrDefault();
			if (regra != null)
				regra.ConsultarMensagem = c => mensagem;
		}

		internal void PersonalizarMensagemDaUltimaValidacao(string[] mensagem)
		{
			var regra = _validacoes.LastOrDefault();
			if (regra != null)
				regra.ConsultarMensagem = c => string.Concat(mensagem);
		}

		internal void PersonalizarMensagemDaUltimaValidacao(Func<Constantes, string> consultarMensagem)
		{
			var regra = _validacoes.LastOrDefault();
			if (regra != null)
				regra.ConsultarMensagem = consultarMensagem;
		}

		internal void AdicionarValidacao(Func<bool> validacao, Func<object> consultarValor = null, Func<Constantes, string> consultarMensagem = null, IItemValidacao validador = null)
		{
			_validacoes.Add(new RegraParaValidacao()
			{
				Valido = validacao, 
				ConsultarValor = consultarValor, 
				ConsultarMensagem = consultarMensagem,
				Validador = validador
			});
		}

		public virtual IEnumerable<Mensagem> ConsultarMensagensDeValidacao()
		{
			foreach (var validacao in _validacoes)
			{
				if (!validacao.Valido())
					if (validacao.Validador == null)
						yield return CriarNovaMensagemDeValidacao(validacao);
					else
						foreach (var mensagem in validacao.Validador.ConsultarMensagensDeValidacao())
							yield return mensagem;
			}
		}

		protected virtual Mensagem CriarNovaMensagemDeValidacao(RegraParaValidacao validacao)
		{
			return new Mensagem(string.Empty, ConsultarMensagemDeErro(validacao));
		}

		protected string ConsultarMensagemDeErro(RegraParaValidacao validacao)
		{
			var mensagem = validacao.ConsultarMensagem(Validador.Constantes);
			if (mensagem?.IndexOf("[#]") > -1)
				return mensagem.Replace("[#]", Convert.ToString((validacao.ConsultarValor == null) ? Valor : validacao.ConsultarValor() ?? Valor));
			return mensagem;
		}
	}
}
