namespace ValidacaoFluente
{
	public static class Factory
	{

		public static IValidador<T> Criar<T>(T objeto)
		{
			return new Internals.ValidadorRaiz<T>()
			{
				Objeto = objeto
			};
		}

		public static IValidador<T> Criar<T>(T objeto, Constantes constantes)
		{
			return new Internals.ValidadorRaiz<T>(constantes)
			{ 
				Objeto = objeto
			};
		}

	}
}
