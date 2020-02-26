using System;

namespace ValidacaoFluente.Exceptions
{
	[Serializable]
	internal class UtilizacaoDeOpcaoDeEnumEmDuplicidadeException : Exception
	{
		public UtilizacaoDeOpcaoDeEnumEmDuplicidadeException(Enum opcao)
		{

		}
	}
}