namespace ValidacaoFluente
{
	public static class ValidadorPersonalizadoExtensions
	{

		public static IPersonalizacaoValidador<T, K> Validar<T, K>(this IValidadorValor<T, K> sender)
		{
			if (!(sender is Internals.ValidadorCampo<T, K> validadorRaiz))
				throw new Exceptions.ValidadorInvalidoException();

			var validadorPersonalizado = new Internals.ValidadorPersonalizado<T, K>(validadorRaiz);
			validadorRaiz.AdicionarValidacao(() => validadorPersonalizado.Valor, validador: validadorPersonalizado);

			return validadorPersonalizado;
		}

	}
}
