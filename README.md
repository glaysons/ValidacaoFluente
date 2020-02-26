# ValidacaoFluente

ValidacaoFluente é um conjunto de componentes que facilita o desenvolvimento centralizado de validações, facilitando a coesão das regras, a facilidade de leitura e a padronização das regras.

### Porque ValidacaoFluente? ###

 - Utiliza o padrão de desenvolvimento **fluente**
 - Suporte a **Testes Automatizados**
 - Permite integrar com injetores de dependências

## Como Utilizar ##

### Instalar ###

Disponível via **nuget**.

```
  PM> Install-Package ValidacaoFluente
```

### Objeto com as Propriedades ###

Ao utilizar objetos, as mensagens serão geradas com base no nome das propriedades, caso haja necessidade de tratar as mensagens com base em um nome mais elaborado, basta utiliza o atributo **Description**.

Caso o nome da propriedade não possua o atributo, um espaço em branco será inserido automaticamente na separação das palavras maiúsculo/minúsculo.

```
public class ParametrosExportacaoVenda
{

  [Description("Data de Emissão Inicial")]
  public DateTime? DataEmissaoInicial { get; set; }

  [Description("Data de Emissão Final")]
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

### Validação Via Factory ###

A criação de validadores via Factory permite a combinação de múltiplos validadores em um único local.

Sugere-se a criação de uma função pública que retorne a interface IValidador<T>, referente ao objeto a ser validado.

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

Para utilizar este validador, basta trabalhar a camada de visualização da seguinte forma:

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

**O processo de execução dos validadores ocorre sempre que a função Valido() é executada!**

### Validação Via Herança ###

Para centralização das regras de validação em objetos específicos, basta que os mesmos herdem a classe ValidadorBase<T>.

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

### Validações ###

Existem validações de campos, condicionais e de enumerações:

#### Campos ####

São validações aplicadas aos atributos da classe, exclusivamente, facilitando a leitura e definição das regras de negócio sobre tais atributos.

```
  Campo(p => p.DataEmissaoInicial)
    .Obrigatorio() // Define que a propriedade é de preenchimento obrigatório, não permitindo nenhum valor default

    // Validadores específicos para tipos IComparable

	.Maior(**constante**) // Define que a propriedade deve ser maior que uma constante específica
	.Maior(p => p.NomeOutroCampo) // Define que a propriedade deve ser maior que outro campo do próprio objeto validado

	.MaiorOuIgual(**constante**) // Define que a propriedade deve ser maior ou igual a uma constante específica
	.MaiorOuIgual(p => p.NomeOutroCampo) // Define que a propriedade deve ser maior ou igual a outro campo do próprio objeto validado

	.Menor(**constante**) // Define que a propriedade deve ser menor que uma constante específica
	.Menor(p => p.NomeOutroCampo) // Define que a propriedade deve ser menor que outro campo do próprio objeto validado

	.MenorOuIgual(**constante**) // Define que a propriedade deve ser menor ou igual a uma constante específica
	.MenorOuIgual(p => p.NomeOutroCampo) // Define que a propriedade deve ser menor ou igual a outro campo do próprio objeto validado

	.Diferente(**constante**) // Define que a propriedade deve ser diferente de uma constante específica
	.Diferente(p => p.NomeOutroCampo) // Define que a propriedade deve ser diferente de outro campo do próprio objeto validado

	.Entre(**constante mínima**, **constante máxima**) // Define um intervalo de valores que o campo pode estar

    // Validadores específicos para o tipo String

	.TamanhoMinimo(**N**) // Especifica em N o tamanho mínimo que a string deve ter
	.TamanhoMaximo(**N**) // Especifica em N o tamanho máximo que a string deve ter
	.TamanhoFixo(**N**) // Especifica em N o tamanho exato que a string deve ter
  ;
```

Para validação de uma propriedade que é definida a partir de uma classe personalizada, é possível vincular um validador específico para esta classe:

```
  Campo(p => p.HistoricoDeVenda)
    .Validar().Com<ValidadorHistoricoVenda>(); 
```

Para que a classe **ValidadorHistoricoVenda** possa validar essa propriedade, ela:
 - Deve herdar a classe ValidadorBase<P> e não pode ter parâmetros no construtor, ou;
 - Implementar a interface IValidadorPersonalizado<P>;

Lembrando que o tipo **P** deve ser o mesmo da propriedade indicada na função **Campo**.

Caso seje necessário realizar o vínculo da propriedade com um objeto validador mais complexo, há a opção de indicar uma Factory no seguinte formato:

```
  Campo(p => p.HistoricoDeVenda)
    .Validar().Com(historico => new ValidadorHistoricoVenda(historico)); 
```

#### Condicionais ####

São condições que, se forem verdadeiras, as demais regras passam a ser validadas.

```
  Se(condicaoForVerdadeira)
    .Campo(p => p.DataEmissaoInicial)
	.Obrigatorio();
```

A função **condicaoForVerdadeira** deve ser escrito no formato:

```
  Func<T, bool>
```

Onde o objeto **T** é o mesmo definido no validador.

Caso deseje utilizar apenas a validação das condições, basta utilizar as seguintes opções:

```
  Se(condicao)
    .ForVerdadeiro("Exibe mensagem se o resultado da condição for verdadeiro!");

  Se(outraCondicao)
    .ForFalso("Exibe mensagem se o resultado da condição for falso!");
```

### Enumerações ###

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

A validação de enumerações permite facilitar a leitura das opções existentes do enumerador, permitindo aplicar todos os recursos dos validadores, exclusivamente, quando a opção especifica for selecionada.

```
  Quando(p => p.AgruparPor) // Atributo enumerador ...
    .For(Agrupamento.Cliente) // Item da enumeração ...

	// Deverá ser utilizado o método [Validar], que fornece um parâmetro [Action<IValidador<T>>], permitindo acrescentar novas regras ...
	.Validar(v => v
	  .Campo(p => p.Clientes)
	  .Obrigatorio())

    .For(Agrupamento.Lojas)
	.Validar(v => v
	  .Campo(p => p.Lojas)
	  .Obrigatorio())

    .For(Agrupamento.Produto)
	.Validar(DefinirValidacoesAoAgruparPorProduto) // As regras de validação podem ser agrupadas em métodos distintos, para simplificar a leitura ...

    .Senao() // Caso nenhuma dos itens da enumeração seja selecionado ...
	.Validar(v => v
	  .Campo(p => p.DataEmissaoFinal)
	  .Obrigatorio())
	;

void DefinirValidacoesAoAgruparPorProduto(IValidador<ParametrosExportacaoVenda> validador)
{
  ...
}

```
