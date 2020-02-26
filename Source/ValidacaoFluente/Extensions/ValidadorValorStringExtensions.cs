namespace ValidacaoFluente
{
	public static class ValidadorValorStringExtensions
	{

		public static IValidadorValor<T, string> TamanhoMinimo<T>(this IValidadorValor<T, string> sender, int tamanho)
		{
			if (!(sender is Internals.ValidadorCampo<T, string> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => (validador.Valor == null) || (validador.Valor.Length >= tamanho),
				() => tamanho,
				c => c.CampoDeveTerTamanhoMinimo);

			return sender;
		}

		public static IValidadorValor<T, string> TamanhoMaximo<T>(this IValidadorValor<T, string> sender, int tamanho)
		{
			if (!(sender is Internals.ValidadorCampo<T, string> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => (validador.Valor == null) || (validador.Valor.Length <= tamanho),
				() => tamanho,
				c => c.CampoDeveTerTamanhoMaximo);

			return sender;
		}

		public static IValidadorValor<T, string> TamanhoFixo<T>(this IValidadorValor<T, string> sender, int tamanho)
		{
			if (!(sender is Internals.ValidadorCampo<T, string> validador))
				throw new Exceptions.ValidadorInvalidoException();

			validador.AdicionarValidacao(() => (validador.Valor == null) || (validador.Valor.Length <= tamanho),
				() => tamanho,
				c => c.CampoDeveTerTamanhoFixo);

			return sender;
		}

	}
}
