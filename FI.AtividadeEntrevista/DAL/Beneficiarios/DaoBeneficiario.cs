using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        /// <returns></returns>
        internal long Incluir(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.Cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.Cliente.Id));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Consulta um cliente beneficiario
        /// </summary>
        /// <param name="Id">Id do Cliente</param>
        /// <returns></returns>
        internal Beneficiario Consultar(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> listaBeneficiario = Converter(ds);

            return listaBeneficiario.FirstOrDefault();
        }

        /// <summary>
        /// Lista todos os beneficiarios
        /// </summary>
        /// <returns></returns>
        internal List<Beneficiario> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> bene = Converter(ds);

            return bene;
        }

        /// <summary>
        /// Lista todos os beneficiarios do cliente
        /// </summary>
        /// <param name="idCliente">Id do Cliente</param>
        /// <returns></returns>
        internal List<Beneficiario> ListarBeneficiarioDoCliente(long idCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", idCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> bene = Converter(ds);

            return bene;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iniciarEm"></param>
        /// <param name="quantidade"></param>
        /// <param name="campoOrdenacao"></param>
        /// <param name="crescente"></param>
        /// <param name="qtd"></param>
        /// <returns></returns>
        internal List<Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("iniciarEm", iniciarEm));
            parametros.Add(new System.Data.SqlClient.SqlParameter("quantidade", quantidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("campoOrdenacao", campoOrdenacao));
            parametros.Add(new System.Data.SqlClient.SqlParameter("crescente", crescente));

            DataSet ds = base.Consultar("FI_SP_PesqCliente", parametros);
            List<Beneficiario> bene = Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return bene;
        }

        /// <summary>
        /// Veirifica se existe o beneficiario no Banco de Dados de acordo com o CPF
        /// </summary>
        /// <param name="CPF">string com CPF do beneficiario</param>
        /// <returns></returns>
        internal bool VerificarExistencia(string CPF)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Altera os dados do beneficiario
        /// </summary>
        /// <param name="beneficiario"></param>
        internal void Alterar(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.Cpf));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiario.Id));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.Cliente.Id));

            base.Executar("FI_SP_AltBeneficiario", parametros);
        }

        /// <summary>
        /// Excluir beneficiario
        /// </summary>
        /// <param name="Id">Id do beneficiario</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario bene = new Beneficiario();
                    bene.Id = row.Field<long>("Id");
                    bene.Nome = row.Field<string>("Nome");
                    bene.Cpf = row.Field<string>("CPF");
                    bene.Cliente.Id = row.Field<long>("IdCliente");
                    lista.Add(bene);
                }
            }

            return lista;
        }
    }
}
