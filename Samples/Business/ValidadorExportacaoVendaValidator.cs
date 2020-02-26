using System;
using ValidacaoFluente;
using ValueObjects;

namespace Business
{
	public class ValidadorExportacaoVendaValidator : ValidadorBase<ParametrosExportacaoVenda>
	{

		public DateTime DataHoraParaValidacao { get; }

		public ValidadorExportacaoVendaValidator()
			: this(DateTime.Now)
		{

		}

		public ValidadorExportacaoVendaValidator(DateTime dataHoraParaValidacao)
		{
			DataHoraParaValidacao = dataHoraParaValidacao;
		}

		protected override void Configurar()
		{
			ConfigurarPreenchimentoPeriodo();
			ConfigurarPeriodoDeEmissao();
			ConfigurarLojas();
			ConfigurarCliente();
			ConfigurarAgrupamento();
			ConfigurarHistoricoDeVendas();
		}

		private void ConfigurarPreenchimentoPeriodo()
		{
			Campo(p => p.DataEmissaoInicial)
				.Obrigatorio().ExibirMensagem("Favor preencher corretamente a data de emissão inicial para exportação dos dados!");

			Campo(p => p.DataEmissaoFinal)
				.Obrigatorio()
				.MaiorOuIgual(p => p.DataEmissaoInicial)
				.MenorOuIgual(DataHoraParaValidacao.Date.AddDays(10));
		}

		private void ConfigurarPeriodoDeEmissao()
		{
			Se(p => ExtracaoComPeriodoSuperiorSessentaDias(p) && EstaNoHorarioComercial())
				.ForVerdadeiro("A exportação de dados com mais de 60 dias está disponível apenas fora de horário comercial!");
		}

		private bool ExtracaoComPeriodoSuperiorSessentaDias(ParametrosExportacaoVenda parametros)
		{
			if (parametros.DataEmissaoInicial.HasValue && parametros.DataEmissaoFinal.HasValue)
				return (parametros.DataEmissaoFinal.Value.Subtract(parametros.DataEmissaoInicial.Value).TotalDays > 60);
			return false;
		}

		private bool EstaNoHorarioComercial()
		{
			var agora = DataHoraParaValidacao;
			return ((agora.Hour >= 8) && (agora.Hour < 18));
		}

		private void ConfigurarLojas()
		{
			Se(p => p.DataEmissaoInicial?.Day > 10)
				.Campo(p => p.Lojas)
				.Obrigatorio();
		}

		private void ConfigurarCliente()
		{
			Campo(p => p.NomeClienteContendo)
				.TamanhoMinimo(15)
				.TamanhoMaximo(30);
		}

		private void ConfigurarAgrupamento()
		{
			Se(p => p.ExibirApenasResumo)
				.Campo(p => p.AgruparPor)
				.Obrigatorio();

			Quando(p => p.AgruparPor)

				.For(Agrupamento.Cliente)
				.Validar(v => v
					.Campo(p => p.Clientes)
					.Obrigatorio()
				)

				.For(Agrupamento.Loja)
				.Validar(v => v
					.Campo(p => p.Lojas)
					.Obrigatorio()
				)

				.For(Agrupamento.Produto).Validar(DefinirValidacoesAoAgruparPorProduto)

				.Senao() // Bimestre
				.Validar(v => v
					.Se(p => p.DataEmissaoFinal < p.DataEmissaoInicial?.AddDays(120))
					.ForVerdadeiro("Favor exportar no minimo 120 dias para agrupamento por período!")
				);
		}

		private void DefinirValidacoesAoAgruparPorProduto()
		{
			Campo(p => p.Produtos)
				.Obrigatorio();
		}

		private void ConfigurarHistoricoDeVendas()
		{
			Campo(p => p.HistoricoDeVenda)
				.Validar().Com<ValidadorHistoricoVendaValidador>();
		}

	}
}
