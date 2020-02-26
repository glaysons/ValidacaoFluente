using System;
using System.Linq.Expressions;

namespace ValidacaoFluente
{
	public static class ValidadorExtensions
	{

		public static IValidadorValor<T, K> Campo<T, K>(this IValidador<T> sender, Expression<Func<T, K>> campo)
		{
			if (sender is Internals.IValidadorRaiz<T> validador)
				return validador.AdicionarValidador(new Internals.ValidadorCampo<T, K>(validador, campo));
			throw new Exceptions.ValidadorInvalidoException();
		}

		public static IValidadorCondicional<T> Se<T>(this IValidador<T> sender, Func<T, bool> condicao)
		{
			if (sender is Internals.IValidadorRaiz<T> validador)
				return validador.AdicionarValidador(new Internals.ValidadorCondicional<T, bool>(validador, condicao, resultadoEsperado: true));
			throw new Exceptions.ValidadorInvalidoException();
		}

		public static IValidadorCampoEnum<T, K> Quando<T, K>(this IValidador<T> sender, Func<T, K> opcoes) where K : struct, Enum
		{
			if (sender is Internals.IValidadorRaiz<T> validador)
				return validador.AdicionarValidador(new Internals.ValidadorEnum<T, K>(validador, opcoes));
			throw new Exceptions.ValidadorInvalidoException();
		}

		public static IValidadorCampoEnum<T, K> Quando<T, K>(this IValidador<T> sender, Func<T, Nullable<K>> opcoes) where K : struct, Enum
		{
			if (sender is Internals.IValidadorRaiz<T> validador)
				return validador.AdicionarValidador(new Internals.ValidadorEnum<T, K>(validador, opcoes));
			throw new Exceptions.ValidadorInvalidoException();
		}

	}

}
