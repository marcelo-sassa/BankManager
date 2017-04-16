using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManager
{
    class DaoConta
    {
        SqlConnection conexao;

        public DaoConta()
        {
            try
            {
                var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Banco;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                conexao = new SqlConnection(stringConexao);
                conexao.Open();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Conta CarregaUnicaConta(int Id)
        {
            Conta conta = new Conta();

            try
            {
                var comandoSelect = "select * from conta where Id = @id";
                SqlCommand comando = new SqlCommand(comandoSelect, conexao);
                comando.Parameters.AddWithValue("@id", Id);
                SqlDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    Conta Conta;
                    if (reader["Tipo"].ToString() == "C")
                    {
                        Conta = new ContaCorrente();
                    }
                    else
                    {
                        Conta = new ContaPoupanca();
                    }
                    Conta.Id = Convert.ToInt32(reader["Id"]);
                    Conta.Numero = reader["Numero"].ToString();
                    Conta.Agencia = reader["Agencia"].ToString();
                    Conta.Tipo = reader["Tipo"].ToString();
                    Conta.Saldo = Convert.ToDecimal(reader["Saldo"]);

                    reader.Close();
                    return Conta;
                }
                else
                {
                    reader.Close();
                    throw new Exception("Erro ao acessar o Banco de Dados");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Conta> CarregaContas()
        {
            List<Conta> Contas = new List<Conta>();

            try
            {
                var comandoSelect = "select * from conta";
                SqlCommand comando = new SqlCommand(comandoSelect, conexao);
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Conta conta = new Conta();
                    conta.Id = Convert.ToInt32(reader["Id"]);
                    conta.Numero = reader["Numero"].ToString();
                    conta.Agencia = reader["Agencia"].ToString();
                    conta.Tipo = reader["Tipo"].ToString();
                    conta.Saldo = Convert.ToDecimal(reader["Saldo"]);

                    Contas.Add(conta);
                }
                reader.Close();

                return Contas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidaConta(int Id, string Numero, string Agencia)
        {

            var comandoSelect = "select * from conta where Id <> @id and Numero = @numero and agencia = @agencia ";
            SqlCommand comando = new SqlCommand(comandoSelect, conexao);
            comando.Parameters.AddWithValue("@id", Id);
            comando.Parameters.AddWithValue("@numero", Numero);
            comando.Parameters.AddWithValue("@agencia", Agencia);
            SqlDataReader reader = comando.ExecuteReader();

            if(reader.HasRows)
            {
                reader.Close();
                return false;
            }
            reader.Close();

            return true;
        }

        public void InsereConta(Conta c)
        {
            try
            {
                if (!ValidaConta(c.Id, c.Numero, c.Agencia))
                {
                    throw new Exception("Não foi possível cadastrar! Já existe outra conta com este número nesta agência!");
                }

                using (conexao)
                {
                    SqlTransaction sqlTran = conexao.BeginTransaction();
                    SqlCommand comando = conexao.CreateCommand();
                    comando.Transaction = sqlTran;
                    
                    comando.CommandText = "insert into conta (Numero, Agencia, Tipo, Saldo) values (@numero, @agencia, @tipo, @saldo)";
                    comando.Parameters.AddWithValue("@numero", c.Numero);
                    comando.Parameters.AddWithValue("@agencia", c.Agencia);
                    comando.Parameters.AddWithValue("@tipo", c.Tipo);
                    comando.Parameters.AddWithValue("@saldo", c.Saldo);
                    var retorno = comando.ExecuteNonQuery();
                    if (retorno != 1)
                    {
                        sqlTran.Rollback();
                        throw new Exception("Erro ao cadastrar nova conta");
                    }
                    sqlTran.Commit();
                    
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaConta(Conta c)
        {
            try
            {
                if (!ValidaConta(c.Id, c.Numero, c.Agencia))
                {
                    throw new Exception("Não foi possível Atualizar! Já existe outra conta com este número nesta agência!");
                }

                var comandoUpdate = "update conta set Agencia = @agencia, Tipo = @tipo, Saldo = @saldo where Id = @id";
                SqlCommand comando = new SqlCommand(comandoUpdate, conexao);
                comando.Parameters.AddWithValue("@id", c.Id);
                comando.Parameters.AddWithValue("@agencia", c.Agencia);
                comando.Parameters.AddWithValue("@tipo", c.Tipo);
                comando.Parameters.AddWithValue("@saldo", c.Saldo);
                var retorno = comando.ExecuteNonQuery();
                if (retorno != 1)
                {
                    throw new Exception("Erro ao atualizar conta");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiConta(int Id)
        {
            try
            {
                var comandoDelete = "delete from conta where Id = @id";
                SqlCommand comando = new SqlCommand(comandoDelete, conexao);
                comando.Parameters.AddWithValue("@id", Id);
                var retorno = comando.ExecuteNonQuery();
                if (retorno != 1)
                {
                    throw new Exception("Erro ao excluir conta");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
