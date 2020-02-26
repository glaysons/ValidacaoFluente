namespace ValidacaoFluente
{
	public interface IValidadorPersonalizado<T> : IValidadorFluente<T>
	{

		IValidador<T> Validar(T objeto);

	}
}
