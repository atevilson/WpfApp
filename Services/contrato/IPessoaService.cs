using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Models;

namespace WpfApp.Services.contrato
{
    public interface IPessoaService
    {
        IEnumerable<Pessoa> GetAll();
        void Save(Pessoa pessoa);
        void Delete(int id);
        IEnumerable<Pedido> GetPedidos(int pessoaId);
        void UpdatePedido(Pedido pedido);
    }
}
