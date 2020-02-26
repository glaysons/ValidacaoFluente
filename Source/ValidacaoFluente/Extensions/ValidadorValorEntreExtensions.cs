using System;
using ValidacaoFluente.Internals;

namespace ValidacaoFluente
{
	public static class ValidadorValorEntreExtensions
	{

		public static IValidadorValor<T, int> Entre<T>(this IValidadorValor<T, int> sender, int de, int ate)
		{
			if (!(sender is ValidadorValor<T, int> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => 
				(validador.Valor == 0) || 
				((validador.Valor is IComparable c) && (c.CompareTo(de) >= 0) && (c.CompareTo(ate) <= 0)), 
				consultarMensagem: c => c.CampoObrigatorio);

			return sender;
		}

		public static IValidadorValor<T, double> Entre<T>(this IValidadorValor<T, double> sender, double de, double ate)
		{
			if (!(sender is ValidadorValor<T, double> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => 
				(validador.Valor == 0) || 
				((validador.Valor is IComparable c) && (c.CompareTo(de) >= 0) && (c.CompareTo(ate) <= 0)), 
				consultarMensagem: c => c.CampoObrigatorio);

			return sender;
		}

		public static IValidadorValor<T, decimal> Entre<T>(this IValidadorValor<T, decimal> sender, decimal de, decimal ate)
		{
			if (!(sender is ValidadorValor<T, decimal> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => 
				(validador.Valor == 0) || 
				((validador.Valor is IComparable c) && (c.CompareTo(de) >= 0) && (c.CompareTo(ate) <= 0)), 
				consultarMensagem: c => c.CampoObrigatorio);

			return sender;
		}

		public static IValidadorValor<T, DateTime> Entre<T>(this IValidadorValor<T, DateTime> sender, DateTime de, DateTime ate)
		{
			if (!(sender is ValidadorValor<T, DateTime> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => 
				(validador.Valor == null) || 
				((validador.Valor is IComparable c) && (c.CompareTo(de) >= 0) && (c.CompareTo(ate) <= 0)), 
				consultarMensagem: c => c.CampoObrigatorio);

			return sender;
		}

		public static IValidadorValor<T, IComparable> Entre<T>(this IValidadorValor<T, IComparable> sender, IComparable de, IComparable ate)
		{
			if (!(sender is ValidadorValor<T, IComparable> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => 
				(validador.Valor == null) || 
				((validador.Valor is IComparable c) && (c.CompareTo(de) >= 0) && (c.CompareTo(ate) <= 0)), 
				consultarMensagem: c => c.CampoObrigatorio);

			return sender;
		}

	}
}
