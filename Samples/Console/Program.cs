using Business;
using ValueObjects;

namespace Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var parametros = new ParametrosExportacaoVenda();
			var validador = new ValidadorExportacaoVenda();

			var resultado = validador.Validar(parametros);

			if (resultado.Valido())
				System.Console.WriteLine("Show !!!");
			else
				foreach (var mensagem in resultado.Mensagens())
					System.Console.WriteLine(mensagem.Descricao);
		}
	}
}
