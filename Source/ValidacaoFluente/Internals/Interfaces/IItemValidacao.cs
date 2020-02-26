using System.Collections.Generic;

namespace ValidacaoFluente.Internals
{
	internal interface IItemValidacao
	{
		IEnumerable<Mensagem> ConsultarMensagensDeValidacao();
	}
}