namespace ValidacaoFluente
{
	public class Mensagem
    {

		public string Campo { get; }
	
		public string Descricao { get; }

		public Mensagem(string campo, string descricao)
		{
			Campo = campo;
			Descricao = descricao;
		}

	}
}
