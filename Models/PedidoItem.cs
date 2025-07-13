using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class PedidoItem
    {
        private Produto _produto;
        private int _quantidade;

        public Produto Produto
        {
            get => _produto;
            set
            {
                _produto = value ?? throw new ArgumentNullException(nameof(Produto));
            }
        }

        public int Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
            }
        }

        public decimal Subtotal => Produto != null ? Produto.Valor * Quantidade : 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string nome = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));

        public PedidoItem(Produto produto, int quantidade)
        {
            Produto = produto;
            Quantidade = quantidade;
        }
    }
}
