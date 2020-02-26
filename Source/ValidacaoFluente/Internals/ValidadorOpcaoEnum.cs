using System;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorOpcaoEnum<T, K> : IValidadorOpcaoEnum<T, K> where K : struct, Enum
	{

		internal ValidadorEnum<T, K> CampoEnum { get; }

		internal Enum Opcao { get; }

		internal Action<IValidador<T>> Validacao { get; set; }

		public ValidadorOpcaoEnum(ValidadorEnum<T, K> campoEnum, Enum opcao)
		{
			CampoEnum = campoEnum;
			Opcao = opcao;
		}

	}
}
