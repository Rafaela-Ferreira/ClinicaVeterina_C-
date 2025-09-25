using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetodeExtensao
{
    public class Telefone
    {
        private string numero;
        private Cliente cliente;

        public Telefone(string numero, Cliente cliente)
        {
            this.numero = numero;
            this.cliente = cliente;
        }

        public string GetNumero() { return numero; }
        public void SetNumero(string numero) { this.numero = numero; }
        public Cliente GetCliente() { return cliente; }
        public void SetCliente(Cliente cliente) { this.cliente = cliente; }
    }
}
