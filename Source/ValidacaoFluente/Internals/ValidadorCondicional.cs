using System;
using System.Collections.Generic;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorCondicional<T, K> : ValidadorValor<T, K>, IValidadorRaiz<T>, IValidadorCondicional<T>
	{

		private readonly Func<T, K> _condicao;
		private readonly IList<IItemValidacao> _validacoes;

		private K _resultadoEsperado;
		private string _mensagemEsperada = string.Empty;

		public override K Valor => _condicao(Validador.Objeto);

		T IValidadorRaiz<T>.Objeto => Validador.Objeto;

		public Constantes Constantes => Validador.Constantes;

		internal ValidadorCondicional(IValidadorRaiz<T> validador, Func<T, K> condicao, K resultadoEsperado)
			: this(validador, condicao)
		{
			_resultadoEsperado = resultadoEsperado;
		}

		internal ValidadorCondicional(IValidadorRaiz<T> validador, Func<T, K> condicao)
			: base(validador)
		{
			_condicao = condicao;
			_validacoes = new List<IItemValidacao>();
		}

		public IList<Mensagem> Mensagens()
		{
			throw new NotImplementedException();
		}

		public bool Valido()
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Mensagem> ConsultarMensagensDeValidacao()
		{
			if (!Valor.Equals(_resultadoEsperado))
				yield break;
			if (string.IsNullOrEmpty(_mensagemEsperada))
			{
				foreach (var validacao in _validacoes)
					foreach (var item in validacao.ConsultarMensagensDeValidacao())
						yield return item;
			}
			else
				yield return new Mensagem(string.Empty, _mensagemEsperada);
		}

		public V AdicionarValidador<V>(V validador) where V : IItemValidacao
		{
			_validacoes.Add(validador);
			return validador;
		}

		public void DefinirResultadoEsperado(K resultado, string mensagem)
		{
			_resultadoEsperado = resultado;
			_mensagemEsperada = mensagem;
		}

	}
}
