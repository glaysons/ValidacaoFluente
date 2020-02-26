using System;
using System.Linq.Expressions;
using ValidacaoFluente.Internals;

namespace ValidacaoFluente
{
	public static class ValidadorValorMaiorOuIgualExtensions
	{

		public static IValidadorValor<T, K> MaiorOuIgual<T, K>(this IValidadorValor<T, K> sender, Expression<Func<T, K>> campo)
		{
			if (!(sender is ValidadorCampo<T, K> validador))
				throw new Exceptions.ValidadorInvalidoException();

			var outroCampo = new ValidadorCampo<T, K>(validador.Validador, campo);

			validador.AdicionarValidacao(() => (
					(validador.Valor == null) ||
					(outroCampo.Valor == null) ||
					((validador.Valor is IComparable comparador) && (comparador.CompareTo(outroCampo.Valor) >= 0))
				),
				() => outroCampo.Titulo, 
				c => c.CampoDeveSerMaiorOuIgualAoCampo);

			return sender;
		}

		public static IValidadorValor<T, K> MaiorOuIgual<T, K>(this IValidadorValor<T, K> sender, K valor)
		{
			if (!(sender is ValidadorCampo<T, K> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => (
					(validador.Valor == null) ||
					(valor == null) ||
					((validador.Valor is IComparable comparador) && (comparador.CompareTo(valor) >= 0))
				),
				() => valor, 
				c => c.CampoDeveSerMaiorOuIgualAoValor);

			return sender;
		}

	}
}
