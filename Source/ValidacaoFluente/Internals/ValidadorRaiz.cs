using System.Collections.Generic;
using System.Linq;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorRaiz<T> : IValidador<T>, IValidadorRaiz<T>
	{

		private readonly IList<Mensagem> _mensagens;
		private readonly IList<IItemValidacao> _validadores;

		public T Objeto { get; set; }

		public Constantes Constantes { get; private set; }

		internal ValidadorRaiz()
			: this(new Constantes())
		{
		}

		internal ValidadorRaiz(Constantes constantes)
		{
			Constantes = constantes;
			_mensagens = new List<Mensagem>();
			_validadores = new List<IItemValidacao>();
		}

		public bool Valido()
		{
			_mensagens.Clear();
			ValidarObjetoAtual();
			return !_mensagens.Any();
		}

		private void ValidarObjetoAtual()
		{
			foreach (var validador in _validadores)
				foreach (var item in validador.ConsultarMensagensDeValidacao())
					_mensagens.Add(item);
		}

		public IList<Mensagem> Mensagens()
		{
			return _mensagens;
		}

		public V AdicionarValidador<V>(V validador) where V : IItemValidacao
		{
			_validadores.Add(validador);
			return validador;
		}

	}
}
