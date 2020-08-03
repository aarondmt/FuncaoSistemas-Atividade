using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            long idCliente = cli.Incluir(cliente);

            foreach (var beneficiario in cliente.ListaBeneficiario)
            {
                beneficiario.Cliente.Id = idCliente;
                bene.Incluir(beneficiario);
            }

            return idCliente;
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoBeneficiario daoBene = new DAL.DaoBeneficiario();
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);

            List<Beneficiario> listaBeneficiarioAlterar = new List<Beneficiario>();
            List<Beneficiario> listaBeneficiarioInserir = new List<Beneficiario>();
            List<Beneficiario> listaBeneficiario = daoBene.ListarBeneficiarioDoCliente(cliente.Id);
            if (listaBeneficiario != null && listaBeneficiario.Count > 0)
            {
                foreach (var beneficiario in cliente.ListaBeneficiario)
                {
                    if (listaBeneficiario.Any(b => b.Cpf == beneficiario.Cpf))
                    {
                        beneficiario.Cliente.Id = cliente.Id;
                        beneficiario.Id = listaBeneficiario.First(b => b.Cpf == beneficiario.Cpf).Id;
                        listaBeneficiarioAlterar.Add(beneficiario);
                        listaBeneficiario.RemoveAll(b => b.Cpf == beneficiario.Cpf);
                    }
                    else
                    {
                        beneficiario.Cliente.Id = cliente.Id;
                        listaBeneficiarioInserir.Add(beneficiario);
                    }
                }

                foreach (var beneficiario in listaBeneficiarioInserir)
                {
                    daoBene.Incluir(beneficiario);
                }

                foreach (var beneficiario in listaBeneficiarioAlterar)
                {
                    daoBene.Alterar(beneficiario);
                }

                foreach (var beneficiario in listaBeneficiario)
                {
                    daoBene.Excluir(beneficiario.Id);
                }
            }
            else
            {
                foreach (var beneficiario in cliente.ListaBeneficiario)
                {
                    beneficiario.Cliente.Id = cliente.Id;
                    daoBene.Incluir(beneficiario);
                }
            }
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }
    }
}
