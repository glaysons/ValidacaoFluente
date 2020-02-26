using System;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorLogico<T> : ValidadorValor<T, bool>
	{

		private readonly Func<T, bool> _condicao;

		public override bool Valor => _condicao(Validador.Objeto);

		public ValidadorLogico(IValidadorRaiz<T> validador, Func<T, bool> condicao)
			: base(validador)
		{
			_condicao = condicao;
		}

	}
}
