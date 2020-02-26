using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidacaoFluente
{
	public static class ValidadorValorMensagemExtensions
	{
		public static IValidadorValor<T, K> ExibirMensagem<T, K>(this IValidadorValor<T, K> sender, string mensagem)
		{
			if (!(sender is Internals.ValidadorValor<T, DateTime?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.PersonalizarMensagemDaUltimaValidacao(mensagem);
			return sender;
		}

		public static IValidadorValor<T, K> ExibirMensagem<T, K>(this IValidadorValor<T, K> sender, params string[] mensagem)
		{
			if (!(sender is Internals.ValidadorValor<T, DateTime?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.PersonalizarMensagemDaUltimaValidacao(mensagem);
			return sender;
		}

		public static IValidadorValor<T, K> ExibirMensagem<T, K>(this IValidadorValor<T, K> sender, Func<Constantes, string> consultarMensagem)
		{
			if (!(sender is Internals.ValidadorValor<T, DateTime?> validador))
				throw new Exceptions.ValidadorInvalidoException();
			validador.PersonalizarMensagemDaUltimaValidacao(consultarMensagem);
			return sender;
		}

	}
}
