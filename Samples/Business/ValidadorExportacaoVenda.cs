using System;
using ValidacaoFluente;
using ValueObjects;

namespace Business
{
	public class ValidadorExportacaoVenda: IValidadorPersonalizado<ParametrosExportacaoVenda>
    {
		public DateTime DataHoraParaValidacao { get; }

		public ValidadorExportacaoVenda()
			: this(DateTime.Now)
		{

		}

		public ValidadorExportacaoVenda(DateTime dataHoraParaValidacao)
		{
			DataHoraParaValidacao = dataHoraParaValidacao;
		}

		public IValidador<ParametrosExportacaoVenda> Validar(ParametrosExportacaoVenda parametros)
		{
			var validador = Factory.Criar(parametros);

			ConfigurarPreenchimentoPeriodo(validador);
			ConfigurarPeriodoDeEmissao(validador);
			ConfigurarLojas(validador);
			ConfigurarCliente(validador);
			ConfigurarAgrupamento(validador);
			ConfigurarHistoricoDeVendas(validador);

			return validador;
		}

		private void ConfigurarPreenchimentoPeriodo(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador.Campo(p => p.DataEmissaoInicial)
				.Obrigatorio().ExibirMensagem("Favor preencher corretamente a data de emissão inicial para exportação dos dados!");

			validador.Campo(p => p.DataEmissaoFinal)
				.Obrigatorio()
				.MaiorOuIgual(p => p.DataEmissaoInicial)
				.MenorOuIgual(DataHoraParaValidacao.Date.AddDays(10));
		}

		private void ConfigurarPeriodoDeEmissao(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador.Se(p => ExtracaoComPeriodoSuperiorSessentaDias(p) && EstaNoHorarioComercial())
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

		private void ConfigurarLojas(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador.Se(p => p.DataEmissaoInicial?.Day > 10)
				.Campo(p => p.Lojas)
				.Obrigatorio();
		}

		private void ConfigurarCliente(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador.Campo(p => p.NomeClienteContendo)
				.TamanhoMinimo(15)
				.TamanhoMaximo(30);
		}

		private void ConfigurarAgrupamento(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador.Se(p => p.ExibirApenasResumo)
				.Campo(p => p.AgruparPor)
				.Obrigatorio();

			validador.Quando(p => p.AgruparPor)

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

		private void ConfigurarHistoricoDeVendas(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador.Campo(c => c.HistoricoDeVenda)
				.Validar().Com<ValidadorHistoricoVenda>();
		}

		private void DefinirValidacoesAoAgruparPorProduto(IValidador<ParametrosExportacaoVenda> validador)
		{
			validador
				.Campo(p => p.Produtos)
				.Obrigatorio();
		}

	}
}
