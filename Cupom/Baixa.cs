using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Cupom
{
    public partial class Baixa : Form
    {
        public Baixa()
        {
            InitializeComponent();
        }
        


        string data_source = "server=localhost;database=real_estruturas_cupom;uid=root;pwd=escoteiro362;port=3306";
        

        private void CarregarDataGridView()
        {
            try
            {
                MySqlConnection Cn;
                Cn = new MySqlConnection(data_source);
                Cn.Open();
                var exibirVagas = "SELECT ID, TipoCupom, CentroResultado, NomeFuncionario, Stts FROM cupom WHERE ID = '" + textBox1.Text + "'";
                using (MySqlDataAdapter da = new MySqlDataAdapter(exibirVagas, Cn))
                {
                    using (DataTable dt = new DataTable())
                    {
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }

                Cn.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DarBaixa()
        {
            try
            {
                MySqlConnection Conexao;
                Conexao = new MySqlConnection(data_source);
                string sql = "UPDATE CUPOM SET Stts = 1 WHERE ID = '" + textBox1.Text + "'";
                MySqlCommand comando = new MySqlCommand(sql, Conexao);
                Conexao.Open();
                comando.ExecuteReader();
                Conexao.Close();
                MessageBox.Show("Baixa realizada com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void PesquisarCupom()
        {
            // Obtendo o código inserido na TextBox txtCodigo
            int codigoCupom = Convert.ToInt32(textBox1.Text);

            // Configurando a conexão com o banco de dados (substitua connectionString conforme necessário)
            using (MySqlConnection conexao = new MySqlConnection(data_source))
            {
                conexao.Open();
                string query = "SELECT Stts FROM Cupom WHERE ID = @ID;";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@ID", codigoCupom);

                // Execute a consulta SQL
                object resultado = comando.ExecuteScalar();

                if (resultado != null) // Se o resultado não for nulo, o ID existe
                {
                    int stts = Convert.ToInt32(resultado);

                    // Atualize a TextBox txtStatus com base no valor de stts
                    if (stts == 0)
                    {
                        label1.Text = "Cupom não utilizado";
                        label1.BackColor = Color.Green; // Define a cor de fundo como verde
                    }
                    else if (stts == 1)
                    {
                        label1.Text = "Cupom já utilizado";
                        label1.BackColor = Color.Red; // Define a cor de fundo como vermelho
                    }
                }
                else // Se o resultado for nulo, o ID não existe
                {
                    label1.Text = "Cupom não encontrado";
                    label1.BackColor = Color.White; // Define a cor de fundo como branca (ou outra cor padrão)
                }
            }
        }

        private void ExportToExcel(string connectionString, string tableName, string excelFilePath)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {tableName}";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                using (XLWorkbook workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(tableName);

                    // Preencher a planilha com os dados da tabela
                    worksheet.Cell(1, 1).InsertTable(dataTable);

                    // Salvar o arquivo Excel
                    workbook.SaveAs(excelFilePath);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                this.CarregarDataGridView();
            }
            else
            {
                MessageBox.Show("Digite o Código do Cupom!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                this.DarBaixa();
            }
            else
            {
                MessageBox.Show("Digite o Código no campo a cima da tabela!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                this.PesquisarCupom();
            }
            else
            {
                MessageBox.Show("Digite o Código no campo a cima da tabela!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Arquivos Excel (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Salvar Arquivo Excel";
            saveFileDialog.InitialDirectory = "C:\\";

            string connectionString = "server=localhost;database=real_estruturas_cupom;uid=root;pwd=escoteiro362;port=3306"; // Substitua pela sua string de conexão
            string tableName = "Cupom"; // Substitua pelo nome da sua tabela
                                        // Crie uma instância do SaveFileDialog
          

            // Defina o título do diálogo
            saveFileDialog.Title = "Salvar arquivo Excel";

            // Defina o filtro de extensão para Excel
            saveFileDialog.Filter = "Arquivos do Excel|*.xlsx";

            // Defina a extensão padrão
            saveFileDialog.DefaultExt = "xlsx";

            // Verifique se o usuário selecionou um local e clicou em "Salvar"
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtém o caminho completo do arquivo escolhido pelo usuário
                string excelFilePath = saveFileDialog.FileName;

                // Chame o método ExportToExcel com o caminho do arquivo Excel
                

                ExportToExcel(connectionString, tableName, excelFilePath);

                MessageBox.Show("Exportação feita com sucesso!");
            }
        }
    }
}
