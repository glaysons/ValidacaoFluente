using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ValidacaoFluente.Internals
{
	internal static class ReflectionUtils
	{

		internal static PropertyInfo ConsultarPropriedade<T, K>(Expression<Func<T, K>> campo)
		{
			if (campo == null)
				return null;
			var membro = campo.Body as MemberExpression;
			if (membro == null)
				return null;
			return membro.Member as PropertyInfo;
		}

		internal static string ConsultarTituloDaPropriedade(PropertyInfo propriedade)
		{
			var descricao = propriedade.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
			if (descricao.Any())
				return (descricao.First() as DescriptionAttribute).Description;
			return ConsultarNomeDaPropriedadeSeparadaPorEspacos(propriedade);
		}

		private static string ConsultarNomeDaPropriedadeSeparadaPorEspacos(PropertyInfo propriedade)
		{
			var nome = new StringBuilder();
			for (var n = 0; n < propriedade.Name.Length; n++)
			{
				if ((n > 0) && char.IsUpper(propriedade.Name[n]) && (n < 2 || propriedade.Name[n - 2] != ' '))
					nome.Append(' ');
				nome.Append(propriedade.Name[n]);
			}
			return nome.ToString();
		}

	}
}
