using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cupom
{
    public partial class Tela_Principal : Form
    {
        public Tela_Principal()
        {
            InitializeComponent();
       
        
        
     
        }

        private string conteudoCupom = "Conteúdo do cupom"; // Conteúdo do cupom
       

        public class Cupom
        {
            public int ID { get; set; }
            public string TipoCupom { get; set; }
            public string CentroResultado { get; set; }
            public string NomeFuncionario { get; set; }

            public DateTime DataImpressao { get; set; }

            public int Status { get; set; }
        }

        PrintDocument pd = new PrintDocument();

        string connectionString = "server=localhost;database=real_estruturas_cupom;uid=root;pwd=escoteiro362;port=3306";

        private void Imprimir()
        {

            var hora = DateTime.Now;
            // Obtendo os valores dos campos
            string tipoCupom = comboBox1.Text;
            string nomeFuncionario = textBox2.Text;
            string centroResultado = comboBox2.Text;
            var dataUso = hora.Date;

            Cupom novoCupom = new Cupom
            {
                TipoCupom = tipoCupom,
                CentroResultado = centroResultado,
                NomeFuncionario = nomeFuncionario,
                DataImpressao = dataUso

            };

            // Salvando o novo cupom no banco de dados
            using (MySqlConnection conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();
                string query = "INSERT INTO Cupom (TipoCupom, CentroResultado, NomeFuncionario, DataImpressao) VALUES (@TipoCupom, @CentroResultado, @NomeFuncionario, @Dataimpressao);";
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@TipoCupom", novoCupom.TipoCupom);
                comando.Parameters.AddWithValue("@CentroResultado", novoCupom.CentroResultado);
                comando.Parameters.AddWithValue("@NomeFuncionario", novoCupom.NomeFuncionario);
                comando.Parameters.AddWithValue("@Dataimpressao", novoCupom.DataImpressao);

                comando.ExecuteNonQuery();

                // Recuperando o ID gerado para o novo cupom
                novoCupom.ID = (int)comando.LastInsertedId;
                MessageBox.Show("Dados Salvos no banco de dados com sucesso!");
            }



            //// Criando o conteúdo do cupom
            //string conteudoCupom = $"Tipo do Cupom: {tipoCupom}\nNome Funcionário: {nomeFuncionario}\nCentro de Resultado: {centroResultado}\nData de geração: {DateTime.Now}\nData de Utilização: {dateTimePicker1.Value}\nCódigo: {novoCupom.ID}";

            //string diretorioExecutavel = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //Console.WriteLine(diretorioExecutavel);

            //string caminhoLogo = "logo preta - fundo transparente ";
            //Image logotipo = Properties.Resources.logo_preta___fundo_transparente_; // Sua imagem de logotipo


            //// Criando um novo PrintDocument para cada impressão
            //PrintDocument pd = new PrintDocument();

            //// Configurando a impressão para a nova instância do PrintDocument
            //pd.DefaultPageSettings.PaperSize = new PaperSize("Custom Size", (int)(5.5 * 100), (int)(6 * 100)); // Convertendo para unidades em centímetros

            //// Evento PrintPage para cada cupom
            //pd.PrintPage += new PrintPageEventHandler(delegate (object o, PrintPageEventArgs ev)
            //{
            //    using (Font fonte = new Font("Lucida Console", 8))
            //    {
            //        float larguraPagina = ev.PageBounds.Width;

            //        int larguraMaximaLogotipo = 200;

            //        // Calculando as dimensões do logotipo de forma proporcional
            //        int larguraLogotipo, alturaLogotipo;
            //        if (logotipo.Width > larguraMaximaLogotipo)
            //        {
            //            larguraLogotipo = larguraMaximaLogotipo;
            //            alturaLogotipo = (int)((double)logotipo.Height / logotipo.Width * larguraMaximaLogotipo);
            //        }
            //        else
            //        {
            //            larguraLogotipo = logotipo.Width;
            //            alturaLogotipo = logotipo.Height;
            //        }

            //        // Calculando a posição de início para o logotipo (centralizado na largura da página)
            //        float xLogo = (larguraPagina - larguraLogotipo) / 2;
            //        float yLogo = 10; // Define a posição vertical do logotipo

            //        // Verifique se a imagem do logotipo não excede as margens da página
            //        if (xLogo < 0)
            //            xLogo = 0;

            //        // Calculando a posição de início para o texto (centralizado na largura da página)
            //        float xTexto = (larguraPagina - ev.Graphics.MeasureString(conteudoCupom, fonte).Width) / 2;
            //        float yTexto = yLogo + alturaLogotipo + 10; // Define a posição vertical do texto

            //        // Verificando se o texto não excede as margens da página
            //        if (xTexto < 0)
            //            xTexto = 0;

            //        // Desenhando o logotipo
            //        ev.Graphics.DrawImage(logotipo, new RectangleF(xLogo, yLogo, larguraLogotipo, alturaLogotipo));

            //        // Desenhando o texto
            //        ev.Graphics.DrawString(conteudoCupom, fonte, Brushes.Black, new PointF(xTexto, yTexto));
            //    }
            //});

            //{ 

            //    // Exibindo a caixa de diálogo de impressão 
            //    PrintDialog pdialog = new PrintDialog();
            //    pdialog.Document = pd;
            //    if (pdialog.ShowDialog() == DialogResult.OK)
            //    {
            //        pd.Print();
            //    }
            //}

        }

        private void GeraCupo()
        {
            string conexaoString = "server=localhost;database=real_estruturas_cupom;uid=root;pwd=escoteiro362;port=3306";
            using (MySqlConnection conexao = new MySqlConnection(conexaoString))
            {
                conexao.Open();

                // Suponha que sua tabela do MySQL seja chamada "Cupom" e tenha uma coluna chamada "ID"
                string consulta = "SELECT * FROM Cupom WHERE ID = (SELECT MAX(ID) FROM Cupom)"; // Substitua as reticências pela condição apropriada

                using (MySqlCommand comando = new MySqlCommand(consulta, conexao))
                {
                    int codigoCupom = (int)comando.ExecuteScalar();

                    var hora = DateTime.Now;
                    // Obtendo os valores dos campos
                    string tipoCupom = comboBox1.Text;
                    string nomeFuncionario = textBox2.Text;
                    string centroResultado = comboBox2.Text;
                    var dataUso = dateTimePicker1.Text;
                    DateTime dataSelecionada = dateTimePicker1.Value;
                    string dataFormatada = dataSelecionada.ToShortDateString();

                    // Criando o conteúdo do cupom
                    string conteudoCupom = $"Tipo do Cupom: {tipoCupom}\nNome Funcionário: {nomeFuncionario}\nCentro de Resultado: {centroResultado}\nData de geração: {DateTime.Now}\nData de Utilização: {dataFormatada}\nCódigo: {codigoCupom}";

                    // Criando um novo documento para impressão
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(delegate (object o, PrintPageEventArgs ev)
                    {
                        using (Font fonte = new Font("Lucida Console", 6))
                        {
                            // Calculando a largura da página
                            float larguraPagina = ev.PageBounds.Width;

                            // Carreguando a imagem do logotipo
                            Image logotipo = Properties.Resources.logo_preta___fundo_transparente_; // Sua imagem de logotipo

                            // Definindo a largura máxima para o logotipo (ajuste conforme necessário)
                            int larguraMaximaLogotipo = 100;

                            // Calculando as dimensões do logotipo de forma proporcional
                            int larguraLogotipo, alturaLogotipo;
                            if (logotipo.Width > larguraMaximaLogotipo)
                            {
                                larguraLogotipo = larguraMaximaLogotipo;
                                alturaLogotipo = (int)((double)logotipo.Height / logotipo.Width * larguraMaximaLogotipo);
                            }
                            else
                            {
                                larguraLogotipo = logotipo.Width;
                                alturaLogotipo = logotipo.Height;
                            }

                            // Calculando a posição de início para o logotipo (centralizado na largura da página)
                            float xLogo = (larguraPagina - larguraLogotipo) / 2;
                            float yLogo = 10; // Define a posição vertical do logotipo

                            // Verifique se a imagem do logotipo não excede as margens da página
                            if (xLogo < 0)
                                xLogo = 0;

                            // Calculando a posição de início para o texto (centralizado na largura da página)
                            float xTexto = (larguraPagina - ev.Graphics.MeasureString(conteudoCupom, fonte).Width) / 2;
                            float yTexto = yLogo + alturaLogotipo + 10; // Define a posição vertical do texto

                            // Verificando se o texto não excede as margens da página
                            if (xTexto < 0)
                                xTexto = 0;

                            // Desenhando o logotipo
                            ev.Graphics.DrawImage(logotipo, new RectangleF(xLogo, yLogo, larguraLogotipo, alturaLogotipo));

                            // Desenhando o texto
                            ev.Graphics.DrawString(conteudoCupom, fonte, Brushes.Black, new PointF(xTexto, yTexto));
                        }
                    });

                    // Exibindo a caixa de diálogo de impressão 
                    PrintDialog pdialog = new PrintDialog();
                    pdialog.Document = pd;
                    if (pdialog.ShowDialog() == DialogResult.OK)
                    {
                        pd.Print();
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "" && textBox2.Text != "" && comboBox2.Text != "")
            {
                this.Imprimir();
                this.GeraCupo();
            }
            else
            {
                MessageBox.Show("Preencha todos os campos!");
            }
        }

        private void darBaixaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var j2 = new Baixa();
            j2.Show();
        }
    }



}
