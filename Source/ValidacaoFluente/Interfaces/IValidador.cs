using System.Collections.Generic;

namespace ValidacaoFluente
{
	public interface IValidador<T>
	{

		bool Valido();

		IList<Mensagem> Mensagens();

	}
}
