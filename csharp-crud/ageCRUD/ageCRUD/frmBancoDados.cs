using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ageCRUD
{
    public partial class frmBancoDados : Form
    {
        public MySqlConnection objConexao = new MySqlConnection();
        public MySqlCommand objCommand = new MySqlCommand();
        public MySqlDataReader objDados;


        public frmBancoDados()
        {
            InitializeComponent();
        }

        public void frmBancoDados_Load(object sender, EventArgs e)
        {
            conecta();
        }



        public void conecta()
        {
            try
            {
                objConexao.ConnectionString = "Server=localhost;Database=bdagenda;user=root;password=root123";
                objConexao.Open();
               
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                objConexao.Close();
            }


        }
        private void btnSair_Click(object sender, EventArgs e)
        {
            objConexao.Close();
            Application.Exit();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtCPF.Text == "" | txtTelefone.Text == "" | txtNome.Text == "" | txtEmail.Text == "")
                {
                    MessageBox.Show("Existem campos não preenchidos");
                }
                else
                {
                    string strSQL = "insert into tblagenda(agdid, agdnome, agdemail, agdtelefone, agdcpf) values(NULL,";
                    strSQL += "'" + txtNome.Text + "',";
                    strSQL += "'" + txtEmail.Text + "',";
                    strSQL += "'" + txtTelefone.Text + "',";
                    strSQL += "'" + txtCPF.Text + "')";


                    objCommand.Connection = objConexao;
                    objCommand.CommandText = strSQL;
                    objCommand.ExecuteNonQuery();

                    MessageBox.Show("Registrado com sucesso!");
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro ao inserir");
            }
            
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCodigo.Text != "" |  txtCPF.Text!="")
                {
                    string strSQL = "select * from tblagenda where ";
                    if (txtCodigo.Text.Trim() != "")
                    {
                        strSQL += "agdid = " + txtCodigo.Text.Trim();
                    }
                    else 
                    { 
                        strSQL+= "  agdcpf = " + txtCPF.Text.Trim();
                    }


                    objCommand.Connection = objConexao; 
                    objCommand.CommandText = strSQL;
                    objDados = objCommand.ExecuteReader();


                    if (!objDados.HasRows)
                    {
                        MessageBox.Show("Registro não encontrado!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCodigo.Focus();
                    }
                    else
                    {
                        objDados.Read();
                        txtNome.Text = objDados["agdnome"].ToString();
                        txtEmail.Text = objDados["agdemail"].ToString();
                        txtTelefone.Text = objDados["agdtelefone"].ToString();
                        txtCPF.Text = objDados["agdcpf"].ToString();
                    }

                    if (!objDados.IsClosed) { objDados.Close(); }
                }
                else
                {
                    MessageBox.Show("Preencha o codigo ou Cpf para efetuar a consulta!", "Valor não informado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                string strSQL = "select * from tblagenda where agdid = " + txtCodigo.Text.Trim();

                objCommand.Connection = objConexao;
                objCommand.CommandText = strSQL;
                objDados = objCommand.ExecuteReader();

                if (!objDados.HasRows)
                {
                    MessageBox.Show("Registro não encontrado!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodigo.Focus();
                }
                else
                {
                    if (!objDados.IsClosed) { objDados.Close(); }

                    strSQL = "update tblagenda  set ";
                    strSQL += "agdnome = '" + txtNome.Text + "',";
                    strSQL += "agdemail = '" + txtEmail.Text + "',";
                    strSQL += "agdtelefone = '" + txtTelefone.Text + "',";
                    strSQL += "agdcpf = '" + txtCPF.Text + "'";
                    strSQL += "where agdid = " + txtCodigo.Text.Trim();

                    objCommand.Connection = objConexao;
                    objCommand.CommandText = strSQL;
                    objCommand.ExecuteNonQuery();

                    MessageBox.Show("Registro alterado com sucesso!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro na alteração do registro!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTelefone.Text = "";
            txtCPF.Text = "";
            txtCodigo.Focus();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Excluir o código selecionado", "Banco de Dados", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string strSQL = "delete from tblagenda where agdid =" + txtCodigo.Text.Trim();

                    objCommand.Connection = objConexao;
                    objCommand.CommandText = strSQL;
                    objCommand.ExecuteNonQuery();

                    MessageBox.Show("Registro eliminado com sucesso!", "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultarListaDados_Click(object sender, EventArgs e)
        {
            frmConsultarListaDados objTela = new frmConsultarListaDados();
       
            objTela.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
