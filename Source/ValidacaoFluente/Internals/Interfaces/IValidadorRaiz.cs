namespace ValidacaoFluente.Internals
{
	internal interface IValidadorRaiz<T>
	{

		T Objeto { get; }

		Constantes Constantes { get; }

		V AdicionarValidador<V>(V validador) where V : IItemValidacao;

	}
}
