using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValueObjects;
using FluentAssertions;
using System.Collections.Generic;

namespace Business.Tests
{
	[TestClass]
	public class ValidadorExportacaoVendaValidatorUnitTest
	{

		[TestMethod]
		public void AoValidarExportacaoDeVendaSemParametrosInformadosDeveEncontrarDoisErros()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda();

			var resultado = validador.Validar(parametros);

			resultado.Should().HaveCount(2, "porque existem dois campos não preenchidos!");

			var validacaoDataInicial = resultado.FirstOrDefault(m => m.Campo.Equals("DataEmissaoInicial"));

			validacaoDataInicial
				.Should()
				.NotBeNull("porque existe erro de preenchimento de data inicial!");

			validacaoDataInicial.Descricao
				.Should()
				.Be("Favor preencher corretamente a data de emissão inicial para exportação dos dados!");

			var validacaoDataFinal = resultado.FirstOrDefault(m => m.Campo.Equals("DataEmissaoFinal"));

			validacaoDataFinal
				.Should()
				.NotBeNull("porque existe erro de preenchimento de data final!");

			validacaoDataFinal.Descricao
				.Should()
				.Be("O campo [Data de Emissão Final] é de preenchimento obrigatório!");

			parametros.DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000);
			parametros.DataEmissaoFinal = new DateTime(day: 10, month: 1, year: 2000);

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque os parâmetros foram preenchidos corretamente!");
		}

		[TestMethod]
		public void AoValidarExportacaoDeVendasComParametrosInvertidosDeveExibirMensagemDeErro()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 10, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 1, month: 1, year: 2000)
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque parametros invertidos foram informados!");

			var validacaoDataFinal = resultado.FirstOrDefault(m => m.Campo.Equals("DataEmissaoFinal"));

			validacaoDataFinal
				.Should()
				.NotBeNull("porque existe erro de preenchimento de data final!");

			validacaoDataFinal.Descricao
				.Should()
				.Be("O campo [Data de Emissão Final] deve possuir um valor maior ou igual ao campo [Data de Emissão Inicial]!");

			parametros.DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000);
			parametros.DataEmissaoFinal = new DateTime(day: 10, month: 1, year: 2000);

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque os parâmetros foram preenchidos corretamente!");
		}

		[TestMethod]
		public void AoValidarExportacaoDeVendasComDataEmissaoFinalSuperiorDezDiasDaDataAtualDeveExibirMensagemDeErro()
		{
			var dataAtual = new DateTime(day: 10, month: 1, year: 2000);

			var validador = new ValidadorExportacaoVendaValidator(dataAtual);
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 21, month: 1, year: 2000)
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque a data final foi excedida!");

			var validacaoDataFinal = resultado.FirstOrDefault(m => m.Campo.Equals("DataEmissaoFinal"));

			validacaoDataFinal
				.Should()
				.NotBeNull("porque existe erro de preenchimento de data final!");

			validacaoDataFinal.Descricao
				.Should()
				.Be("O campo [Data de Emissão Final] deve possuir um valor menor ou igual a [" + dataAtual.AddDays(10).ToString() + "]!");

			parametros.DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000);
			parametros.DataEmissaoFinal = new DateTime(day: 20, month: 1, year: 2000);

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque os parâmetros foram preenchidos corretamente!");
		}

		[TestMethod]
		public void AoValidarExportacaoDeVendasComDataEmissaoFinalInferiorDezDiasDaDataAtualDeveSerValido()
		{
			var dataAtual = new DateTime(day: 10, month: 1, year: 2000);

			var validador = new ValidadorExportacaoVendaValidator(dataAtual);
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 20, month: 1, year: 2000)
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque os parâmetros foram preenchidos corretamente!");

			var validacao = resultado.FirstOrDefault();

			validacao
				.Should()
				.BeNull("porque não pode existir mensagens de erro!");
		}

		[TestMethod]
		public void AoValidarExportacaoDeVendasEmHorarioComercialComPeriodoSuperior60DiasDeveExibirMensagemDeErro()
		{
			var validador = new ValidadorExportacaoVendaValidator(dataHoraParaValidacao: new DateTime(day: 1, month: 1, year: 2001, hour: 11, minute: 22, second: 33));
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 31, month: 12, year: 2000)
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque um período maior que 60 dias foi solicidado durante o dia!");

			var validacaoPeriodo = resultado.First();

			validacaoPeriodo
				.Should()
				.NotBeNull("porque existe erro de preenchimento de tamanho de período!");

			validacaoPeriodo.Descricao
				.Should()
				.Be("A exportação de dados com mais de 60 dias está disponível apenas fora de horário comercial!");

			parametros.DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000);
			parametros.DataEmissaoFinal = new DateTime(day: 1, month: 3, year: 2000);

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque os parâmetros foram preenchidos corretamente!");
		}

		[TestMethod]
		public void AoValidarExportacaoDeVendasEmHorarioComercialComPeriodoInferior60DiasDeveSerValido()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 1, month: 3, year: 2000),
				HistoricoDeVenda = new ParametrosHistoricoVenda()
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque os parâmetros foram preenchidos corretamente!");

			var validacao = resultado.FirstOrDefault();

			validacao
				.Should()
				.BeNull("porque não pode existir mensagens de erro!");
		}

		[TestMethod]
		public void AoValidarExportacaoDeVendasAPartirDoDiaOnzeOCampoLojasDeveSerDePreenchimentoObrigatorio()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 15, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 30, month: 1, year: 2000)
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "pois ao exportar dados a partir do dia 11 a loja é obrigatória!");

			var validacaoLoja = resultado.First();

			validacaoLoja
				.Should()
				.NotBeNull("porque existe erro de preenchimento da loja!");

			validacaoLoja.Descricao
				.Should()
				.Be("O campo [Lojas] é de preenchimento obrigatório!");

			parametros.Lojas = new List<int>();

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque nenhuma loja foi informada!");

			parametros.Lojas.Add(1);

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque já existem lojas informadas!");
		}

		[TestMethod]
		public void AoValidarExportacaoComDadosAgrupadosOTipoDeAgrupamentoDeveSerDePreenchimentoObrigatorio()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 1, month: 3, year: 2000),
				ExibirApenasResumo = true
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque os parâmetros foram preenchidos faltando o campo [Agrupar Por]!");

			var validacaoAgrupamento = resultado.First();

			validacaoAgrupamento
				.Should()
				.NotBeNull("porque deve existir um erro de obrigatoriedade!");

			validacaoAgrupamento.Descricao
				.Should()
				.Be("O campo [Agrupar Por] é de preenchimento obrigatório!");

			parametros.AgruparPor = Agrupamento.Cliente;

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque o preenchimento de agrupamento foi corrigido, porém, nenhum cliente foi informado!");

			var validacaoCliente = resultado.First(c => c.Campo == "Clientes");

			validacaoCliente
				.Should()
				.NotBeNull("porque deve existir uma obrigatoriedade de cliente!");

			validacaoCliente.Descricao
				.Should()
				.Be("O campo [Clientes] é de preenchimento obrigatório!");

			parametros.Clientes = new List<int>() { 1, 2, 3 };

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque o preenchimento do cliente do agrupamento foi corrigido!");
		}

		[TestMethod]
		public void AoValidarExportacaoComDadosAgrupadosPorClienteOCampoClienteDeveSerDePreenchimentoObrigatorio()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 1, month: 3, year: 2000),
				ExibirApenasResumo = true,
				AgruparPor = Agrupamento.Cliente
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque os parâmetros não foram preenchidos corretamente!");

			var validacaoCliente = resultado.First();

			validacaoCliente
				.Should()
				.NotBeNull("porque deve existir um erro de obrigatoriedade do campo cliente!");

			validacaoCliente.Descricao
				.Should()
				.Be("O campo [Clientes] é de preenchimento obrigatório!");
		}

		[TestMethod]
		public void AoValidarExportacaoComDadosAgrupadosPorPeriodoAsDatasDevemTerMaisQueCentoEVinteDias()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 1, month: 3, year: 2000),
				ExibirApenasResumo = true,
				AgruparPor = Agrupamento.Bimestre
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque os parâmetros não foram preenchidos corretamente!");

			var validacaoPeriodo = resultado.First();

			validacaoPeriodo
				.Should()
				.NotBeNull("porque deve existir um erro de cálculo do período!");

			validacaoPeriodo.Descricao
				.Should()
				.Be("Favor exportar no minimo 120 dias para agrupamento por período!");
		}

		[TestMethod]
		public void AoValidarExportacaoComHistoricoDeVendasExibindoComparativoDeveObrigarPreenchimentoDosAnos()
		{
			var validador = new ValidadorExportacaoVendaValidator();
			var parametros = new ParametrosExportacaoVenda()
			{
				DataEmissaoInicial = new DateTime(day: 1, month: 1, year: 2000),
				DataEmissaoFinal = new DateTime(day: 1, month: 3, year: 2000),
				HistoricoDeVenda = new ParametrosHistoricoVenda()
				{
					ExibirComparativoDeVendas = true
				}
			};

			var resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCountGreaterThan(0, "porque os parâmetros não foram preenchidos corretamente!");

			var validacaoPeriodo = resultado.First();

			validacaoPeriodo
				.Should()
				.NotBeNull("porque deve existir um erro de preenchimento de histórico!");

			validacaoPeriodo.Descricao
				.Should()
				.Be("O campo [Quantidade De Anos Anteriores] é de preenchimento obrigatório!");

			parametros.HistoricoDeVenda.QuantidadeDeAnosAnteriores = 3;

			resultado = validador.Validar(parametros);

			resultado
				.Should()
				.HaveCount(0, "porque a quantidade de anos para o histórico foi preenchida!");
		}

	}
}
