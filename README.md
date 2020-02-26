# ValidacaoFluente

ValidacaoFluente � um conjunto de componentes que facilita o desenvolvimento centralizado de valida��es, facilitando a coes�o das regras, a facilidade de leitura e a padroniza��o das regras.

### Porque ValidacaoFluente? ###

 - Utiliza o padr�o de desenvolvimento **fluente**
 - Suporte a **Testes Automatizados**
 - Permite integrar com injetores de depend�ncias

## Como Utilizar ##

### Instalar ###

Dispon�vel via **nuget**.

```
  PM> Install-Package ValidacaoFluente
```

### Objeto com as Propriedades ###

Ao utilizar objetos, as mensagens ser�o geradas com base no nome das propriedades, caso haja necessidade de tratar as mensagens com base em um nome mais elaborado, basta utiliza o atributo **Description**.

Caso o nome da propriedade n�o possua o atributo, um espa�o em branco ser� inserido automaticamente na separa��o das palavras mai�sculo/min�sculo.

```
public class ParametrosExportacaoVenda
{

  [Description("Data de Emiss�o Inicial")]
  public DateTime? DataEmissaoInicial { get; set; }

  [Description("Data de Emiss�o Final")]
  public DateTime? DataEmissaoFinal { get; set; }

  public string NomeClienteContendo { get; set; }

  public ParametrosHistoricoVenda HistoricoDeVenda { get; set; }

}

public class ParametrosHistoricoVenda
{
  public bool ExibirComparativoDeVendas { get; set; }

  public int? QuantidadeDeAnosAnteriores { get; set; }

  public decimal? MargemDeLucroMinima { get; set; }

  public decimal? MargemDeLucroMaxima { get; set; }
}
```

### Valida��o Via Factory ###

A cria��o de validadores via Factory permite a combina��o de m�ltiplos validadores em um �nico local.

Sugere-se a cria��o de uma fun��o p�blica que retorne a interface IValidador<T>, referente ao objeto a ser validado.

```
public class ValidadorExportacaoVenda
{

  public IValidador<ParametrosExportacaoVenda> Validar(ParametrosExportacaoVenda parametros)
  {
    var validador = Factory.Criar(parametros);

    validador.Campo(p => p.DataEmissaoInicial).Obrigatorio();
	
	...

    return validador;
  }

}
```

Para utilizar este validador, basta trabalhar a camada de visualiza��o da seguinte forma:

```
  var parametros = new ParametrosExportacaoVenda();
  var validador = new ValidadorExportacaoVenda();
  
  var resultado = validador.Validar(parametros);
  
  if (resultado.Valido())
    System.Console.WriteLine("Show !!!");
  else
    foreach (var mensagem in resultado.Mensagens())
      System.Console.WriteLine(mensagem.Descricao);
```

**O processo de execu��o dos validadores ocorre sempre que a fun��o Valido() � executada!**

### Valida��o Via Heran�a ###

Para centraliza��o das regras de valida��o em objetos espec�ficos, basta que os mesmos herdem a classe ValidadorBase<T>.

```
public class ValidadorExportacaoVendaValidator : ValidadorBase<ParametrosExportacaoVenda>
{
  protected override void Configurar()
  {

    Campo(p => p.DataEmissaoInicial).Obrigatorio();

    ...  

  }
}
```

### Valida��es ###

Existem valida��es de campos, condicionais e de enumera��es:

#### Campos ####

S�o valida��es aplicadas aos atributos da classe, exclusivamente, facilitando a leitura e defini��o das regras de neg�cio sobre tais atributos.

```
  Campo(p => p.DataEmissaoInicial)
    .Obrigatorio() // Define que a propriedade � de preenchimento obrigat�rio, n�o permitindo nenhum valor default

    // Validadores espec�ficos para tipos IComparable

	.Maior(**constante**) // Define que a propriedade deve ser maior que uma constante espec�fica
	.Maior(p => p.NomeOutroCampo) // Define que a propriedade deve ser maior que outro campo do pr�prio objeto validado

	.MaiorOuIgual(**constante**) // Define que a propriedade deve ser maior ou igual a uma constante espec�fica
	.MaiorOuIgual(p => p.NomeOutroCampo) // Define que a propriedade deve ser maior ou igual a outro campo do pr�prio objeto validado

	.Menor(**constante**) // Define que a propriedade deve ser menor que uma constante espec�fica
	.Menor(p => p.NomeOutroCampo) // Define que a propriedade deve ser menor que outro campo do pr�prio objeto validado

	.MenorOuIgual(**constante**) // Define que a propriedade deve ser menor ou igual a uma constante espec�fica
	.MenorOuIgual(p => p.NomeOutroCampo) // Define que a propriedade deve ser menor ou igual a outro campo do pr�prio objeto validado

	.Diferente(**constante**) // Define que a propriedade deve ser diferente de uma constante espec�fica
	.Diferente(p => p.NomeOutroCampo) // Define que a propriedade deve ser diferente de outro campo do pr�prio objeto validado

	.Entre(**constante m�nima**, **constante m�xima**) // Define um intervalo de valores que o campo pode estar

    // Validadores espec�ficos para o tipo String

	.TamanhoMinimo(**N**) // Especifica em N o tamanho m�nimo que a string deve ter
	.TamanhoMaximo(**N**) // Especifica em N o tamanho m�ximo que a string deve ter
	.TamanhoFixo(**N**) // Especifica em N o tamanho exato que a string deve ter
  ;
```

Para valida��o de uma propriedade que � definida a partir de uma classe personalizada, � poss�vel vincular um validador espec�fico para esta classe:

```
  Campo(p => p.HistoricoDeVenda)
    .Validar().Com<ValidadorHistoricoVenda>(); 
```

Para que a classe **ValidadorHistoricoVenda** possa validar essa propriedade, ela:
 - Deve herdar a classe ValidadorBase<P> e n�o pode ter par�metros no construtor, ou;
 - Implementar a interface IValidadorPersonalizado<P>;

Lembrando que o tipo **P** deve ser o mesmo da propriedade indicada na fun��o **Campo**.

Caso seje necess�rio realizar o v�nculo da propriedade com um objeto validador mais complexo, h� a op��o de indicar uma Factory no seguinte formato:

```
  Campo(p => p.HistoricoDeVenda)
    .Validar().Com(historico => new ValidadorHistoricoVenda(historico)); 
```

#### Condicionais ####

S�o condi��es que, se forem verdadeiras, as demais regras passam a ser validadas.

```
  Se(condicaoForVerdadeira)
    .Campo(p => p.DataEmissaoInicial)
	.Obrigatorio();
```

A fun��o **condicaoForVerdadeira** deve ser escrito no formato:

```
  Func<T, bool>
```

Onde o objeto **T** � o mesmo definido no validador.

Caso deseje utilizar apenas a valida��o das condi��es, basta utilizar as seguintes op��es:

```
  Se(condicao)
    .ForVerdadeiro("Exibe mensagem se o resultado da condi��o for verdadeiro!");

  Se(outraCondicao)
    .ForFalso("Exibe mensagem se o resultado da condi��o for falso!");
```

### Enumera��es ###

Vamos adotar o seguinte enumerador:

```
public enum Agrupamento
{
  Loja,
  Cliente,
  Produto,
  Bimestre
}
```

Vamos acrescentar no objeto testado, a seguinte propriedade:

```
  public IList<int> Lojas { get; set; }
  public IList<int> Clientes { get; set; }
  public Agrupamento? AgruparPor { get; set; }
```

A valida��o de enumera��es permite facilitar a leitura das op��es existentes do enumerador, permitindo aplicar todos os recursos dos validadores, exclusivamente, quando a op��o especifica for selecionada.

```
  Quando(p => p.AgruparPor) // Atributo enumerador ...
    .For(Agrupamento.Cliente) // Item da enumera��o ...

	// Dever� ser utilizado o m�todo [Validar], que fornece um par�metro [Action<IValidador<T>>], permitindo acrescentar novas regras ...
	.Validar(v => v
	  .Campo(p => p.Clientes)
	  .Obrigatorio())

    .For(Agrupamento.Lojas)
	.Validar(v => v
	  .Campo(p => p.Lojas)
	  .Obrigatorio())

    .For(Agrupamento.Produto)
	.Validar(DefinirValidacoesAoAgruparPorProduto) // As regras de valida��o podem ser agrupadas em m�todos distintos, para simplificar a leitura ...

    .Senao() // Caso nenhuma dos itens da enumera��o seja selecionado ...
	.Validar(v => v
	  .Campo(p => p.DataEmissaoFinal)
	  .Obrigatorio())
	;

void DefinirValidacoesAoAgruparPorProduto(IValidador<ParametrosExportacaoVenda> validador)
{
  ...
}

```
