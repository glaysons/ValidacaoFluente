using System;
using System.Collections.Generic;
using System.Linq;

namespace ValidacaoFluente.Internals
{
	internal class ValidadorPersonalizado<T, K> : ValidadorValor<T, bool>, IPersonalizacaoValidador<T, K>, IDisposable
	{
		private readonly ValidadorValor<T, K> _valorValidado;

		private IValidadorPersonalizado<K> _interfaceValidadorPersonalizado;
		private Func<K, IValidador<K>> _funcaoValidadorPersonalizado;
		private ValidadorBase<K> _objetoValidadorPersonalizado;

		private IDisposable _objetoValidador;
		private Func<bool> _valido;
		private IList<Mensagem> _mensagens;

		public override bool Valor
		{
			get { return ValidadorSelecionadoEstaValido(); }
		}

		public ValidadorPersonalizado(ValidadorValor<T, K> validador)
			: base(validador.Validador)
		{
			_valorValidado = validador;
		}

		public ValidadorPersonalizado(ValidadorValor<T, K> validador, ValidadorBase<K> validadorPersonalizado)
			: base(validador.Validador)
		{
			_valorValidado = validador;
			_objetoValidadorPersonalizado = validadorPersonalizado;
			_valido = ObjetoValidadorEstaValido;
		}

		private bool ValidadorSelecionadoEstaValido()
		{
			if (_valorValidado.Valor == null)
				return true;

			_mensagens = null;

			if (_valido == null)
				throw new Exceptions.NaoFoiPossivelIdentificarValidadorPersonalizadoException();

			return _valido();
		}

		private bool InterfaceValidadoraEstaValida()
		{
			var validador = _interfaceValidadorPersonalizado.Validar(_valorValidado.Valor);
			return ValidadorEstaValido(validador);
		}

		private bool ValidadorEstaValido(IValidador<K> validador)
		{
			try
			{
				return validador.Valido();
			}
			finally
			{
				_mensagens = validador.Mensagens();
			}
		}

		private bool FuncaoValidadoraEstaValida()
		{
			var validador = _funcaoValidadorPersonalizado(_valorValidado.Valor);
			return ValidadorEstaValido(validador);
		}

		private bool ObjetoValidadorEstaValido()
		{
			_mensagens = _objetoValidadorPersonalizado.Validar(_valorValidado.Valor).ToList();
			return (_mensagens.Count == 0);
		}

		public override IEnumerable<Mensagem> ConsultarMensagensDeValidacao()
		{
			return _mensagens;
		}

		public void Com<V>() where V : IValidadorFluente<K>, new()
		{
			if (typeof(IValidadorPersonalizado<K>).IsAssignableFrom(typeof(V)))
			{
				_interfaceValidadorPersonalizado = (IValidadorPersonalizado<K>)new V();
				_valido = InterfaceValidadoraEstaValida;
				_objetoValidador = _interfaceValidadorPersonalizado as IDisposable;
			}
			else 
				if (typeof(ValidadorBase<K>).IsAssignableFrom(typeof(V)))
				{
					_objetoValidadorPersonalizado = new V() as ValidadorBase<K>;
					_valido = ObjetoValidadorEstaValido;
					_objetoValidador = _objetoValidadorPersonalizado as IDisposable;
				}
				else
	 				throw new Exceptions.ValidadorInvalidoException();
		}

		public void Com(IValidadorPersonalizado<K> validador)
		{
			_interfaceValidadorPersonalizado = validador;
			_valido = InterfaceValidadoraEstaValida;
		}

		public void Com(Func<K, IValidador<K>> validador)
		{
			_funcaoValidadorPersonalizado = validador;
			_valido = FuncaoValidadoraEstaValida;
		}

		public void Dispose()
		{
			if (_objetoValidador != null)
				_objetoValidador.Dispose();
		}

	}
}
