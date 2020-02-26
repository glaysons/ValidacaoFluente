using System;

namespace ValidacaoFluente
{
	public static class ValidadorCondicionalExtensions
	{

		public static void ForVerdadeiro<T>(this IValidadorCondicional<T> sender, string mensagem)
		{
			if (!(sender is Internals.ValidadorCondicional<T, bool> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.DefinirResultadoEsperado(resultado: true, mensagem: mensagem);
		}

		public static void ForVerdadeiro<T>(this IValidadorCondicional<T> sender, params string[] mensagem)
		{
			if (!(sender is Internals.ValidadorCondicional<T, bool> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.DefinirResultadoEsperado(resultado: true, mensagem: string.Concat(mensagem));
		}

		public static void ForVerdadeiro<T>(this IValidadorCondicional<T> sender, Func<T, string> consultarMensagem)
		{
			if (!(sender is Internals.ValidadorCondicional<T, bool> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.DefinirResultadoEsperado(resultado: true, mensagem: consultarMensagem(validador.Validador.Objeto));
		}

		public static void ForFalso<T>(this IValidadorCondicional<T> sender, string mensagem)
		{
			if (!(sender is Internals.ValidadorCondicional<T, bool> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.DefinirResultadoEsperado(resultado: false, mensagem: mensagem);
		}

		public static void ForFalso<T>(this IValidadorCondicional<T> sender, params string[] mensagem)
		{
			if (!(sender is Internals.ValidadorCondicional<T, bool> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.DefinirResultadoEsperado(resultado: false, mensagem: string.Concat(mensagem));
		}

		public static void ForFalso<T>(this IValidadorCondicional<T> sender, Func<T, string> consultarMensagem)
		{
			if (!(sender is Internals.ValidadorCondicional<T, bool> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.DefinirResultadoEsperado(resultado: false, mensagem: consultarMensagem(validador.Validador.Objeto));
		}

	}
}
