using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ValidacaoFluente.Internals;

namespace ValidacaoFluente
{
	public abstract class ValidadorBase<T>: IValidadorFluente<T>
	{

		private bool _configurado = false;

		internal ValidadorRaiz<T> Validador { get; }

		public ValidadorBase()
		{
			Validador = new ValidadorRaiz<T>();
		}

		public ValidadorBase(Constantes constantes)
		{
			Validador = new ValidadorRaiz<T>(constantes);
		}

		protected abstract void Configurar();

		public IEnumerable<Mensagem> Validar(T objeto)
		{
			ConfigurarRegrasDeValidacao();
			Validador.Objeto = objeto;
			if (Validador.Valido())
				return Enumerable.Empty<Mensagem>();
			return Validador.Mensagens();
		}

		private void ConfigurarRegrasDeValidacao()
		{
			if (_configurado)
				return;
			Configurar();
			_configurado = true;
		}

		protected IValidadorValor<T, K> Campo<K>(Expression<Func<T, K>> campo)
		{
			return Validador.Campo(campo);
		}

		protected IValidadorCondicional<T> Se(Func<T, bool> condicao)
		{
			return Validador.Se(condicao);
		}

		protected IValidadorCampoEnum<T, K> Quando<K>(Func<T, K> opcoes) where K : struct, Enum
		{
			return Validador.Quando(opcoes);
		}

		protected IValidadorCampoEnum<T, K> Quando<K>(Func<T, Nullable<K>> opcoes) where K : struct, Enum
		{
			return Validador.Quando(opcoes);
		}

	}
}
