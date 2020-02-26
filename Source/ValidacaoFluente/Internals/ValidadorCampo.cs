using System;
using System.Linq.Expressions;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorCampo<T, K> : ValidadorValor<T, K>
	{

		private readonly Expression<Func<T, K>> _expressaoCampo;

		internal ValidadorCampo(IValidadorRaiz<T> validador, Expression<Func<T, K>> expressaoCampo)
			: base(validador)
		{
			_expressaoCampo = expressaoCampo;
		}

		internal string Nome
		{
			get
			{
				var propriedade = ReflectionUtils.ConsultarPropriedade(_expressaoCampo);
				if (propriedade == null)
					throw new Exceptions.ExpressaoComNomeDePropriedadeInvalidaException();
				return propriedade.Name;
			}
		}

		internal string Titulo
		{
			get
			{
				var propriedade = ReflectionUtils.ConsultarPropriedade(_expressaoCampo);
				if (propriedade == null)
					throw new Exceptions.ExpressaoComNomeDePropriedadeInvalidaException();
				return ReflectionUtils.ConsultarTituloDaPropriedade(propriedade);
			}
		}

		public override K Valor
		{
			get
			{
				var funcao = _expressaoCampo.Compile() as Func<T, K>;
				return funcao(Validador.Objeto);
			}
		}

		protected override Mensagem CriarNovaMensagemDeValidacao(RegraParaValidacao validacao)
		{
			return new Mensagem(Nome, ConsultarMensagemErroPadronizada(validacao));
		}

		private string ConsultarMensagemErroPadronizada(RegraParaValidacao validacao)
		{
			var mensagem = ConsultarMensagemDeErro(validacao);
			if (mensagem?.IndexOf("[?]") > -1)
				return mensagem.Replace("[?]", Titulo);
			return mensagem;
		}

	}
}
