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
using WpfApp.Services.contrato;

namespace WpfApp.ViewModels
{
    public class CadastroDePedidosViewModel : INotifyPropertyChanged
    {
        private readonly IPessoaService _pessoaService;
        private readonly IProdutoService _produtoService;

        private Pessoa _pessoaSelecionada;
        private Produto _produtoSelecionado;
        private int _quantidade = 1;
        private FormaDePagamento _formaPagamento;

        public ObservableCollection<Pessoa> Pessoas { get; }
        public ObservableCollection<Produto> ProdutosDisponiveis { get; }
        public ObservableCollection<PedidoItem> Itens { get; }

        public ObservableCollection<FormaDePagamento> FormasDePagamento { get; set; }

        public FormaDePagamento FormaDePagamento
        {
            get => _formaPagamento;
            set
            {
                _formaPagamento = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand FinalizarCommand { get; }
        public ICommand CancelarCommand { get; }

        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set { _pessoaSelecionada = value; OnPropertyChanged(); }
        }
        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set 
            { 
              _produtoSelecionado = value; 
              OnPropertyChanged();
              (FinalizarCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public int Quantidade
        {
            get => _quantidade;
            set { _quantidade = value; OnPropertyChanged(); }
        }
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);

        public CadastroDePedidosViewModel(IPessoaService ps, IProdutoService pr)
        {
            _pessoaService = ps;
            _produtoService = pr;

            Pessoas = new ObservableCollection<Pessoa>(_pessoaService.GetAll());
            ProdutosDisponiveis = new ObservableCollection<Produto>(_produtoService.GetAll());
            Itens = new ObservableCollection<PedidoItem>();

            FormasDePagamento = new ObservableCollection<FormaDePagamento>(
               EnumHelpers.GetValues<FormaDePagamento>());

            AddItemCommand = new RelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand<PedidoItem>(RemoveItem);
            FinalizarCommand = new RelayCommand(Finalizar, () => PessoaSelecionada != null && Itens.Any());
            CancelarCommand = new RelayCommand(Cancelar);

            Itens.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(ValorTotal));
                (FinalizarCommand as RelayCommand)?.RaiseCanExecuteChanged();
            };
        }

        private void AddItem()
        {
            var existente = Itens.FirstOrDefault(i => i.Produto.Id == ProdutoSelecionado.Id);
            if (existente != null)
            {
                existente.Quantidade += Quantidade;
                existente.OnPropertyChanged(nameof(PedidoItem.Subtotal));
            }
            else
            {
                Itens.Add(new PedidoItem(ProdutoSelecionado, Quantidade));
                Itens.Last().PropertyChanged += (_, __) => OnPropertyChanged(nameof(ValorTotal));
            }
            OnPropertyChanged(nameof(ValorTotal));
        }

        private void RemoveItem(PedidoItem item)
        {
            if (item == null) return;
            Itens.Remove(item);
            OnPropertyChanged(nameof(ValorTotal));
        }

        private void Finalizar()
        {
            try
            {
                var pedido = new Pedido
                {
                    Pessoa = PessoaSelecionada,
                    Produtos = new ObservableCollection<PedidoItem>(Itens),
                    FormaDePagamento = this.FormaDePagamento,
                    Status = Status.Pendente
                };

                _pessoaService.UpdatePedido(pedido);
                MessageBox.Show("Pedido finalizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                Itens.Clear();
                PessoaSelecionada = null;
                Quantidade = 1;
                OnPropertyChanged(nameof(ValorTotal));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível salvar o pedido: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar()
        {
            if (MessageBox.Show("Descartar este pedido?", "Cancelar", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Itens.Clear();
                PessoaSelecionada = null;
                Quantidade = 1;
                OnPropertyChanged(nameof(ValorTotal));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}
