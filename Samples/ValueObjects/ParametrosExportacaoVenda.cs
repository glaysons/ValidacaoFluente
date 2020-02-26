using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ValueObjects
{
	public class ParametrosExportacaoVenda
    {

		[Description("Data de Emissão Inicial")]
		public DateTime? DataEmissaoInicial { get; set; }

		[Description("Data de Emissão Final")]
		public DateTime? DataEmissaoFinal { get; set; }

		public IList<int> Lojas { get; set; }

		public IList<int> Clientes { get; set; }

		public IList<int> Produtos { get; set; }

		[Description("Exibir Apenas Resumo?")]
		public bool ExibirApenasResumo { get; set; }

		public Agrupamento? AgruparPor { get; set; }

		public string NomeClienteContendo { get; set; }

		public ParametrosHistoricoVenda HistoricoDeVenda { get; set; }

	}
}
