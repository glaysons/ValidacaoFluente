namespace ValueObjects
{
	public class ParametrosHistoricoVenda
	{

		public bool ExibirComparativoDeVendas { get; set; }

		public int? QuantidadeDeAnosAnteriores { get; set; }

		public decimal? MargemDeLucroMinima { get; set; }

		public decimal? MargemDeLucroMaxima { get; set; }

	}
}