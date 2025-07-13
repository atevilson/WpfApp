using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfApp.Models
{
    // Enums de pedido
    public enum FormaDePagamento
    {
        Dinheiro,
        Cartao,
        Boleto
    }

    public enum Status
    {
        Pendente,
        Pago,
        Enviado,
        Recebido
    }
    public class Pedido : INotifyPropertyChanged
    {
        private Pessoa _pessoa;
        private ObservableCollection<PedidoItem> _produtos;
        private FormaDePagamento _formaDePagamento;
        private Status _status;
        public int Id { get; }

        public Pessoa Pessoa
        {
            get => _pessoa;
            set
            {
                _pessoa = value ?? throw new ArgumentNullException(nameof(Pessoa)); OnPropertyChanged();
            }
        }

        public ObservableCollection<PedidoItem> Produtos
        {
            get => _produtos;
            set
            {
                _produtos = value ?? new ObservableCollection<PedidoItem>(); OnPropertyChanged(); OnPropertyChanged(nameof(ValorTotal));
            }
        }

        public decimal ValorTotal => Produtos.Sum(i => i.Subtotal);

        public DateTime DataDaVenda { get; }

        public FormaDePagamento FormaDePagamento
        {
            get => _formaDePagamento;
            set
            {
                _formaDePagamento = value; OnPropertyChanged();
            }
        }

        public Status Status
        {
            get => _status;
            set
            {
                _status = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string nome = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
        }
    }
}
