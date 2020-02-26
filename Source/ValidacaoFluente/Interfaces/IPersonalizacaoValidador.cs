using System;

namespace ValidacaoFluente
{
	public interface IPersonalizacaoValidador<T, K>
	{

		void Com<V>() where V: IValidadorFluente<K>, new();

		void Com(IValidadorPersonalizado<K> validador);

		void Com(Func<K, IValidador<K>> validador);

	}
}
