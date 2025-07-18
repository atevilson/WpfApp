﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                OnPropertyChanged();
                OnPropertyChanged(nameof(Subtotal));
            }
        }

        public int Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Subtotal));
            }
        }

        public decimal Subtotal => Produto != null ? Produto.Valor * Quantidade : 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string nome = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));

        public PedidoItem(Produto produto, int quantidade)
        {
            Produto = produto;
            Quantidade = quantidade;
        }
    }
}
