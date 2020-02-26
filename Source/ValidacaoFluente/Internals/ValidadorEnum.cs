using System;
using System.Collections.Generic;
using System.Linq;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorEnum<T, K> : ValidadorValor<T, K>, IValidadorCampoEnum<T, K> where K : struct, Enum
	{

		private readonly IList<ValidadorOpcaoEnum<T, K>> _opcoes;
		private readonly Func<T, K> _consultarOpcoes;
		private readonly Func<T, K?> _consultarOpcoesNula;

		internal ValidadorEnum(IValidadorRaiz<T> validador, Func<T, K> opcoes) 
			: base(validador)
		{
			_opcoes = new List<ValidadorOpcaoEnum<T, K>>();
			_consultarOpcoes = opcoes;
		}

		internal ValidadorEnum(IValidadorRaiz<T> validador, Func<T, Nullable<K>> opcoesNula)
			: base(validador)
		{
			_opcoes = new List<ValidadorOpcaoEnum<T, K>>();
			_consultarOpcoesNula = opcoesNula;
		}

		internal IValidadorOpcaoEnum<T, K> DefinirOpcao(Enum opcao)
		{
			var item = _opcoes.FirstOrDefault(o => o.Opcao == opcao);

			if (item == null)
			{
				item = new ValidadorOpcaoEnum<T, K>(this, opcao);
				_opcoes.Add(item);
			}
			else
			{
				if (opcao == null)
					throw new Exceptions.UtilizacaoDoSenaoDeEnumEmDuplicidadeException();
				throw new Exceptions.UtilizacaoDeOpcaoDeEnumEmDuplicidadeException(opcao);
			}

			return item;
		}

		public override IEnumerable<Mensagem> ConsultarMensagensDeValidacao()
		{
			var opcaoAtual = ConsultarOpcaoAtual();
			if (opcaoAtual == null)
				yield break;

			var validacao = ConsultarValidacaoOpcaoSelecionada(opcaoAtual);

			if (validacao == null)
				yield break;

			var validador = new ValidadorRaiz<T>(Validador.Constantes)
			{
				Objeto = Validador.Objeto
			};

			validacao(validador);

			if (!validador.Valido())
				foreach (var item in validador.Mensagens())
					yield return item;
		}

		private Enum ConsultarOpcaoAtual()
		{
			if (_consultarOpcoes != null)
				return _consultarOpcoes(Validador.Objeto);
			if (_consultarOpcoesNula != null)
				return _consultarOpcoesNula(Validador.Objeto);
			throw new Exceptions.OpcaoDeEnumeracaoNaoDefinidaException();
		}

		private Action<IValidador<T>> ConsultarValidacaoOpcaoSelecionada(Enum opcaoAtual)
		{
			var item = _opcoes.FirstOrDefault(o => opcaoAtual.Equals(o.Opcao));
			if (item == null)
				item = _opcoes.FirstOrDefault(O => O.Opcao == null);
			return item?.Validacao;
		}

	}
}
