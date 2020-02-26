using System;
using System.Collections;
using ValidacaoFluente.Internals;

namespace ValidacaoFluente
{
	public static class ValidadorValorObrigatorioExtensions
	{

		public static IValidadorValor<T, string> Obrigatorio<T>(this IValidadorValor<T, string> sender)
		{
			if (!(sender is ValidadorValor<T, string> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (!string.IsNullOrEmpty(validador.Valor)), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, int> Obrigatorio<T>(this IValidadorValor<T, int> sender)
		{
			if (!(sender is ValidadorValor<T, int> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor != 0), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, int?> Obrigatorio<T>(this IValidadorValor<T, int?> sender)
		{
			if (!(sender is ValidadorValor<T, int?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor.HasValue && (validador.Valor != 0)), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, double> Obrigatorio<T>(this IValidadorValor<T, double> sender)
		{
			if (!(sender is ValidadorValor<T, double> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor != 0), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, double?> Obrigatorio<T>(this IValidadorValor<T, double?> sender)
		{
			if (!(sender is ValidadorValor<T, double?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor.HasValue && (validador.Valor != 0)), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, decimal> Obrigatorio<T>(this IValidadorValor<T, decimal> sender)
		{
			if (!(sender is ValidadorValor<T, decimal> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor != 0), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, decimal?> Obrigatorio<T>(this IValidadorValor<T, decimal?> sender)
		{
			if (!(sender is ValidadorValor<T, decimal?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor.HasValue && (validador.Valor != 0)), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, DateTime> Obrigatorio<T>(this IValidadorValor<T, DateTime> sender)
		{
			if (!(sender is ValidadorValor<T, DateTime> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor > DateTime.MinValue), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, DateTime?> Obrigatorio<T>(this IValidadorValor<T, DateTime?> sender)
		{
			if (!(sender is ValidadorValor<T, DateTime?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor.HasValue && (validador.Valor > DateTime.MinValue)), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, bool?> Obrigatorio<T>(this IValidadorValor<T, bool?> sender)
		{
			if (!(sender is ValidadorValor<T, bool?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => (validador.Valor.HasValue), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, object> Obrigatorio<T>(this IValidadorValor<T, object> sender)
		{
			if (!(sender is ValidadorValor<T, object> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => ((validador.Valor != null) && (validador.Valor != DBNull.Value)), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

		public static IValidadorValor<T, K> Obrigatorio<T, K>(this IValidadorValor<T, K> sender)
		{
			if (!(sender is ValidadorValor<T, K> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.AdicionarValidacao(() => ((validador.Valor != null) && (!(validador.Valor is ICollection v) || (v.Count != 0))), 
				consultarMensagem: c => c.CampoObrigatorio);
			return sender;
		}

	}
}
