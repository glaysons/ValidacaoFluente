using ValidacaoFluente;
using ValueObjects;

namespace Business
{
	public class ValidadorHistoricoVendaValidador : ValidadorBase<ParametrosHistoricoVenda>
	{

		protected override void Configurar()
		{
			Se(c => c.ExibirComparativoDeVendas)
				.Campo(c => c.QuantidadeDeAnosAnteriores)
				.Obrigatorio();

			Campo(c => c.MargemDeLucroMinima)
				.Menor(c => c.MargemDeLucroMaxima);
		}

	}
}
