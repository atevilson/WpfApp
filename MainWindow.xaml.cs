using System;
using System.IO;
using System.Windows;
using WpfApp.Services;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp
{
    public partial class MainWindow : Window
    {

        private CadastroDePedidosViewModel _pedidoVm;
        public MainWindow()
        {
            InitializeComponent();

            var mydoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dataDir = Path.Combine(mydoc, "WpfApp", "Data");

            var pessoaSvc = new JsonPessoaService(dataDir);
            var produtoSvc = new JsonProdutoService(dataDir);

            var pessoaVm = new CadastroDePessoaViewModel(pessoaSvc);
            var produtoVm = new CadastroDeProdutoViewModel(produtoSvc);
            _pedidoVm = new CadastroDePedidosViewModel(pessoaSvc, produtoSvc);

            PessoasView.DataContext = pessoaVm;
            ProdutosView.DataContext = produtoVm;
            PedidosView.DataContext = _pedidoVm;

            pessoaVm.RequestIncluirPedido += pessoa =>
            {
                MainTabControl.SelectedIndex = 2;
                _pedidoVm.PessoaSelecionada = pessoa;
            };
        }
    }
}
