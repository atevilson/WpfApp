using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfApp.Models;
using WpfApp.Services.Contrato;
using WpfApp.Helpers; 

namespace WpfApp.ViewModels
{
    public class CadastroDeProdutoViewModel : INotifyPropertyChanged
    {
        private readonly IProdutoService _produtoService;

        private string _nomeFiltro;
        private int? _codigoFiltro;
        private decimal? _valorMinFiltro;
        private decimal? _valorMaxFiltro;
        private Produto _produtoSelecionado;

        public ObservableCollection<Produto> Produtos { get; }
        public ICommand FiltrarCommand { get; }
        public ICommand LimparFiltroCommand { get; }
        public ICommand IncluirCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand ExcluirCommand { get; }

        public string NomeFiltro
        {
            get => _nomeFiltro;
            set { _nomeFiltro = value; OnPropertyChanged(); }
        }

        public int? CodigoFiltro
        {
            get => _codigoFiltro;
            set { _codigoFiltro = value; OnPropertyChanged(); }
        }

        public decimal? ValorMinFiltro
        {
            get => _valorMinFiltro;
            set { _valorMinFiltro = value; OnPropertyChanged(); }
        }

        public decimal? ValorMaxFiltro
        {
            get => _valorMaxFiltro;
            set { _valorMaxFiltro = value; OnPropertyChanged(); }
        }

        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set
            {
                _produtoSelecionado = value;
                OnPropertyChanged();
                (EditarCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (ExcluirCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public CadastroDeProdutoViewModel(IProdutoService produtoService)
        {
            _produtoService = produtoService;
            Produtos = new ObservableCollection<Produto>(_produtoService.GetAll());

            FiltrarCommand = new RelayCommand(Filtrar);
            LimparFiltroCommand = new RelayCommand(LimparFiltro);
            IncluirCommand = new RelayCommand(IncluirProduto);
            SalvarCommand = new RelayCommand(SalvarProduto);
            ExcluirCommand = new RelayCommand(ExcluirProduto, () => ProdutoSelecionado != null);
        }

        private void Filtrar()
        {
            var lista = _produtoService.GetAll().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(NomeFiltro))
                lista = lista.Where(p =>
                    p.Nome.IndexOf(NomeFiltro, StringComparison.OrdinalIgnoreCase) >= 0);

            if (CodigoFiltro.HasValue)
                lista = lista.Where(p => p.Codigo == CodigoFiltro.Value);

            if (ValorMinFiltro.HasValue)
                lista = lista.Where(p => p.Valor >= ValorMinFiltro.Value);

            if (ValorMaxFiltro.HasValue)
                lista = lista.Where(p => p.Valor <= ValorMaxFiltro.Value);

            Produtos.Clear();
            foreach (var p in lista) Produtos.Add(p);
        }

        private void LimparFiltro()
        {
            NomeFiltro = null;
            CodigoFiltro = null;
            ValorMinFiltro = null;
            ValorMaxFiltro = null;
            Filtrar();
        }

        private void IncluirProduto()
        {
            var novo = new Produto();
            Produtos.Add(novo);
            ProdutoSelecionado = novo;
        }

        private void SalvarProduto()
        {
            try
            {
                if (ProdutoSelecionado == null) return;

                if (string.IsNullOrWhiteSpace(ProdutoSelecionado.Nome))
                    throw new ArgumentException("Nome é obrigatório.");

                if (ProdutoSelecionado.Codigo <= 0)
                    throw new ArgumentException("Código é obrigatório.");

                if (ProdutoSelecionado.Valor <= 0)
                    throw new ArgumentException("Valor deve ser maior que zero.");

                _produtoService.Save(ProdutoSelecionado);
                MessageBox.Show("Produto salvo com sucesso!", "Sucesso",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                Filtrar();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Atenção",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExcluirProduto()
        {
            if (ProdutoSelecionado == null) return;

            var result = MessageBox.Show(
                $"Deseja remover '{ProdutoSelecionado.Nome}'?",
                "Confirmar exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _produtoService.Delete(ProdutoSelecionado.Id);
                Produtos.Remove(ProdutoSelecionado);
                ProdutoSelecionado = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}