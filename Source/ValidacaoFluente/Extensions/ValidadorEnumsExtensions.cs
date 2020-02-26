using System;

namespace ValidacaoFluente
{
	public static class ValidadorEnumsExtensions
	{

		public static IValidadorOpcaoEnum<T, K> For<T, K>(this IValidadorCampoEnum<T, K> sender, K opcao) where K : struct, Enum
		{
			if (sender is Internals.ValidadorEnum<T, K> validador)
				return validador.DefinirOpcao(opcao);
			throw new Exceptions.ValidadorInvalidoException();
		}

		public static IValidadorOpcaoEnum<T, K> Senao<T, K>(this IValidadorCampoEnum<T, K> sender) where K : struct, Enum
		{
			if (sender is Internals.ValidadorEnum<T, K> validador)
				return validador.DefinirOpcao(opcao: null);
			throw new Exceptions.ValidadorInvalidoException();
		}

		public static IValidadorCampoEnum<T, K> Validar<T, K>(this IValidadorOpcaoEnum<T, K> sender, Action<IValidador<T>> validacao) where K : struct, Enum
		{
			if (sender is Internals.ValidadorOpcaoEnum<T, K> validadorEnum)
			{
				validadorEnum.Validacao = validacao;
				return validadorEnum.CampoEnum;
			}
			throw new Exceptions.ValidadorInvalidoException();
		}

		public static IValidadorCampoEnum<T, K> Validar<T, K>(this IValidadorOpcaoEnum<T, K> sender, Action validacao) where K : struct, Enum
		{
			if (sender is Internals.ValidadorOpcaoEnum<T, K> validadorEnum)
			{
				validadorEnum.Validacao = t => validacao();
				return validadorEnum.CampoEnum;
			}
			throw new Exceptions.ValidadorInvalidoException();
		}

	}
}
