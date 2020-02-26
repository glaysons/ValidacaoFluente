using System;
using System.Runtime.Serialization;

namespace ValidacaoFluente.Exceptions
{
	[Serializable]
	internal class ExpressaoComNomeDePropriedadeInvalidaException : Exception
	{
		public ExpressaoComNomeDePropriedadeInvalidaException()
		{
		}

		public ExpressaoComNomeDePropriedadeInvalidaException(string message) : base(message)
		{
		}

		public ExpressaoComNomeDePropriedadeInvalidaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ExpressaoComNomeDePropriedadeInvalidaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}