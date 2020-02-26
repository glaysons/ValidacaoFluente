using System;

namespace ValidacaoFluente.Internals
{
	internal class RegraParaValidacao
	{
		internal Func<bool> Valido { get; set;  }

		internal Func<Constantes, string> ConsultarMensagem { get; set; }

		internal Func<object> ConsultarValor { get; set;  }

		public IItemValidacao Validador { get; set; }

	}
}
