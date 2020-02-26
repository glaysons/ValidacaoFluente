using ValidacaoFluente;
using ValueObjects;

namespace Business
{
	public class ValidadorHistoricoVenda : IValidadorPersonalizado<ParametrosHistoricoVenda>
	{

		public IValidador<ParametrosHistoricoVenda> Validar(ParametrosHistoricoVenda historico)
		{
			var validador = ValidacaoFluente.Factory.Criar(historico);

			validador.Se(c => c.ExibirComparativoDeVendas)
				.Campo(c => c.QuantidadeDeAnosAnteriores)
				.Obrigatorio();

			validador.Campo(c => c.MargemDeLucroMinima)
				.Menor(c => c.MargemDeLucroMaxima);

			return validador;
		}

	}
}
