# WpfApp

Aplicação desktop em C# com WPF (.NET Framework 4.6) para cadastro e manipulação de:

* Pessoas
* Produtos
* Pedidos

## Tecnologias

* .NET Framework 4.6
* WPF (MVVM)
* Newtonsoft.Json para persistência em JSON
* LINQ para consultas

## Estrutura de Pastas

```
WpfApp/
├── Models/             # Classes de domínio (Pessoa, Produto, Pedido, PedidoItem)
├── Views/              # UserControls XAML (CadastroDePessoas, CadastroDeProdutos, CadastroDePedidos)
├── ViewModels/         # Lógica de apresentação (MVVM)
├── Services/# Serviços de persistência (JsonPessoaService, JsonProdutoService)
    |── contrato
├── Data/               # Arquivos JSON gerados em runtime
├── Helpers/            # Helpers (EnumHelpers, RelayCommand)
├── App.xaml            # Configuração de inicialização
├── MainWindow.xaml     # TabControl com as três telas
└── README.md           # Instruções de execução
```

## Configuração e Execução

1. **Pré-requisitos**

   * Visual Studio 2019 ou superior com suporte a .NET Framework 4.6

2. **Clone o repositório**

```bash
git clone https://github.com/atevilson/WpfApp.git
cd WpfApp
```

3. **Abra a solução**

   * Abra `WpfApp.sln` no Visual Studio.

4. **Construir e Executar**

   * Selecione o modo debug.
   * Pressione **F5** ou **iniciar**.

5. **Dados**

   * Os arquivos de dados JSON são criados no diretório padrão:
     `%USERPROFILE%\Documents\WpfApp\Data`
   * esse caminho pode ser alterarado em `App.xaml.cs` no método `OnStartup`.

## Observações

* Ao incluir ou editar registros, as validações de campos (Nome, CPF, Valor) são executadas e erros mostrados via `MessageBox`.
* Pedidos são vinculados à Pessoa selecionada e persistidos em `pedidos.json`.
* Enumerações de pagamento são manipuladas via `EnumHelpers` e exibidas corretamente nos `ComboBox`.

---

## Autor

---
 <sub><b>Atevilson Freitas</b></sub></a> <a href="">🧑‍💻</a>

*Teste técnico Benner.*