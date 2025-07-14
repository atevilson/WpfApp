using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WpfApp.Models;
using WpfApp.Services.contrato;
using Formatting = Newtonsoft.Json.Formatting;

namespace WpfApp.Services
{
    class JsonPessoaService : IPessoaService
    {
        private readonly string _dataDir;
        private readonly string _pessoasFile;
        private readonly string _pedidosFile;

        private List<Pessoa> _pessoas;
        private List<Pedido> _pedidos;
        private int _nextPessoaId;
        private int _nextPedidoId;

        public JsonPessoaService(string dataDir)
        {
            _dataDir = dataDir;
            Directory.CreateDirectory(_dataDir);

            _pessoasFile = Path.Combine(_dataDir, "pessoas.json");
            _pedidosFile = Path.Combine(_dataDir, "pedidos.json");

            _pessoas = File.Exists(_pessoasFile)
                ? JsonConvert.DeserializeObject<List<Pessoa>>(File.ReadAllText(_pessoasFile)) ?? new List<Pessoa>()
                : new List<Pessoa>();

            _pedidos = File.Exists(_pedidosFile)
                ? JsonConvert.DeserializeObject<List<Pedido>>(File.ReadAllText(_pedidosFile)) ?? new List<Pedido>()
                : new List<Pedido>();

            _nextPessoaId = _pessoas.Any() ? _pessoas.Max(p => p.Id) + 1 : 1;
            _nextPedidoId = _pedidos.Any() ? _pedidos.Max(p => p.Id) + 1 : 1;
        }

        public IEnumerable<Pessoa> GetAll() => _pessoas;

        public void Save(Pessoa pessoa)
        {
            if (pessoa.Id == 0)
            {
                pessoa.Id = _nextPessoaId;
                _nextPessoaId++;
                _pessoas.Add(pessoa);
            }
            else
            {
                var idx = _pessoas.FindIndex(p => p.Id == pessoa.Id);
                if (idx >= 0) _pessoas[idx] = pessoa;
            }
            Persist();
        }

        public void Delete(int id)
        {
            _pessoas.RemoveAll(p => p.Id == id);
            _pedidos.RemoveAll(o => o.Pessoa.Id == id);
            Persist();
        }

        public IEnumerable<Pedido> GetPedidos(int pessoaId)
        {
            return _pedidos.Where(o => o.Pessoa.Id == pessoaId);
        }

        public void UpdatePedido(Pedido pedido)
        {
            if (pedido.Id == 0)
            {
                pedido.Id = _nextPedidoId++;
                _pedidos.Add(pedido);
            }
            else
            {
                var idx = _pedidos.FindIndex(o => o.Id == pedido.Id);
                if (idx >= 0) _pedidos[idx] = pedido;
            }
            Persist();
        }

        private void Persist()
        {
            File.WriteAllText(_pessoasFile,
                JsonConvert.SerializeObject(_pessoas, Formatting.Indented));
            File.WriteAllText(_pedidosFile,
                JsonConvert.SerializeObject(_pedidos, Formatting.Indented));
        }
    }
}
