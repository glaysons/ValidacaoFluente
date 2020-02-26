namespace ValidacaoFluente
{
	public class Constantes
	{

		public virtual string CampoObrigatorio { get { return "O campo [[?]] é de preenchimento obrigatório!"; } }



		public virtual string CampoDeveSerMenorAoValor { get { return "O campo [[?]] deve possuir um valor menor que [[#]]!"; } }

		public virtual string CampoDeveSerMenorAoCampo { get { return "O campo [[?]] deve possuir um valor menor que o campo [[#]]!"; } }



		public virtual string CampoDeveSerMenorOuIgualAoValor { get { return "O campo [[?]] deve possuir um valor menor ou igual a [[#]]!"; } }

		public virtual string CampoDeveSerMenorOuIgualAoCampo { get { return "O campo [[?]] deve possuir um valor menor ou igual ao campo [[#]]!"; } }



		public virtual string CampoDeveSerMaiorAoValor { get { return "O campo [[?]] deve possuir um valor maior que [[#]]!"; } }

		public virtual string CampoDeveSerMaiorAoCampo { get { return "O campo [[?]] deve possuir um valor maior que o campo [[#]]!"; } }



		public virtual string CampoDeveSerMaiorOuIgualAoValor { get { return "O campo [[?]] deve possuir um valor maior ou igual a [[#]]!"; } }

		public virtual string CampoDeveSerMaiorOuIgualAoCampo { get { return "O campo [[?]] deve possuir um valor maior ou igual ao campo [[#]]!"; } }



		public virtual string CampoDeveSerDiferenteDoValor { get { return "O campo [[?]] deve possuir um valor diferente de [[#]]!"; } }

		public virtual string CampoDeveSerDiferenteDoCampo { get { return "O campo [[?]] deve possuir um valor diferente do campo [[#]]!"; } }



		public virtual string CampoDeveTerTamanhoMinimo { get { return "O campo [[?]] deve ser preenchido com no mínimo [[#]] caracteres!"; } }

		public virtual string CampoDeveTerTamanhoMaximo { get { return "O campo [[?]] deve ser preenchido com no máximo [[#]] caracteres!"; } }

		public virtual string CampoDeveTerTamanhoFixo { get { return "O campo [[?]] deve ser preenchido com exatamente [[#]] caracteres!"; } }

	}
}
