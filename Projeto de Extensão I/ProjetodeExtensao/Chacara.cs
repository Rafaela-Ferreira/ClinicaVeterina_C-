using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetodeExtensao
{
        public class Chacara
        {
            private string nomeChacara;
            private Endereco endereco;
            private string descricao;

            public Chacara(string nomeChacara, string descricao, Endereco endereco)
            {
                this.nomeChacara = nomeChacara;
                this.descricao = descricao;
                this.endereco = endereco;
            }

            public string GetNomeChacara() { return nomeChacara; }
            public Endereco GetEndereco() { return endereco; }
            public string GetDescricao() { return descricao; }

            public void SetNomeChacara(string nomeChacara) { this.nomeChacara = nomeChacara; }
            public void SetEndereco(Endereco endereco) { this.endereco = endereco; }
            public void SetDescricao(string descricao) { this.descricao = descricao; }
        }

}
