using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ElPoeta
{
    public partial class Form1 : Form
    {
        private List<string> poemsList;
        private string poema;

        public Form1()
        {
            InitializeComponent();
            init();         
        }

        private void init()
        {
            comboBox1.SelectedIndex = 0;
            poemsList = new List<string>();
            string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "ElPoeta");
            startupPath = Path.Combine(startupPath, "all.csv");
            poema = File.ReadAllText(startupPath).Replace(";", "").ToLowerInvariant();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                System.IO.StreamReader(openFileDialog1.FileName);
                richTextBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                richTextBox2.Text = "";
            }
            else
            {
                richTextBox2.Text = poemsList[Int32.Parse(listView1.SelectedItems[0].SubItems[0].Text) - 1];
            }
        }

        public void applyManhattanDistance()
        {
            string poemaMeta = richTextBox1.Text;
            int iteraciones = Int32.Parse(textBox1.Text);

            NGram nGram = new NGram(poema, poemaMeta);
            Dictionary<int, string> dictionary = nGram.GetDictionary();
            int tamanioMeta = poemaMeta.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length;
            PoemGenerator generate = new PoemGenerator();
            string generated = generate.gen(dictionary, tamanioMeta);

            for (int i = 0; i < iteraciones; i++)
            {
                Histogram histogram = new Histogram(poemaMeta, generated, dictionary);
                int[] histogramMeta = histogram.getHistogramMeta();
                int[] histogramGenerated = histogram.getHistogramGenerated();
                List<string> totalList = histogram.GetTotaList();
                ManhattanDistance distancia = new ManhattanDistance(histogramMeta, histogramGenerated);
                List<int> prometedores = distancia.getIndicesPrometedores();
                List<int> cantPrometedores = distancia.getCantPrometedores();
                generated = generate.new_Generation(prometedores, cantPrometedores, totalList);
                poemsList.Add(generated);
            }            
            loadPoemList();            
        }

        public void applychebyshevDistance()
        {
            string poemaMeta = richTextBox1.Text;
            int iteraciones = Int32.Parse(textBox1.Text);

            NGram nGram = new NGram(poema, poemaMeta);
            Dictionary<int, string> dictionary = nGram.GetDictionary();
            int tamanioMeta = poemaMeta.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length;
            PoemGenerator generate = new PoemGenerator();
            string generated = generate.gen(dictionary, tamanioMeta);

            for (int i = 0; i < iteraciones; i++)
            {
                Histogram histogram = new Histogram(poemaMeta, generated, dictionary);
                int[] histogramMeta = histogram.getHistogramMeta();
                int[] histogramGenerated = histogram.getHistogramGenerated();
                List<string> totalList = histogram.GetTotaList();
                ChebyshevDistance distancia = new ChebyshevDistance(histogramMeta, histogramGenerated);
                List<int> prometedores = distancia.getIndicesPrometedores();
                List<int> cantPrometedores = distancia.getCantPrometedores();
                generated = generate.new_Generation(prometedores, cantPrometedores, totalList);
                poemsList.Add(generated);
            }                
            loadPoemList();
        }

        public void applyOwnDistance()
        {
            string poemaMeta = richTextBox1.Text;
            int iteraciones = Int32.Parse(textBox1.Text);

            NGram nGram = new NGram(poema, poemaMeta);
            Dictionary<int, string> dictionary = nGram.GetDictionary();
            int tamanioMeta = poemaMeta.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length;
            PoemGenerator generate = new PoemGenerator();
            string generated = generate.gen(dictionary, tamanioMeta);

            for (int i = 0; i < iteraciones; i++)
            {
                Histogram histogram = new Histogram(poemaMeta, generated, dictionary);
                int[] histogramMeta = histogram.getHistogramMeta();
                int[] histogramGenerated = histogram.getHistogramGenerated();
                List<string> totalList = histogram.GetTotaList();
                OwnDistance distancia = new OwnDistance(histogramMeta, histogramGenerated);
                List<int> prometedores = distancia.getIndicesPrometedores();
                List<int> cantPrometedores = distancia.getCantPrometedores();
                generated = generate.new_Generation(prometedores, cantPrometedores, totalList);
                poemsList.Add(generated);
            }            
            loadPoemList();
        }

        private void loadPoemList()
        {
            int j = 0;
            for (int i = poemsList.Count - 1; i >= 0; i--)
            {
                ListViewItem lvi = new ListViewItem(j++ + 1 + "");
                lvi.SubItems.Add(comboBox1.SelectedItem.ToString());
                lvi.SubItems.Add(i + 1 + "");

                listView1.Items.Add(lvi);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0)
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Ingrese el número de generaciones");
                } else
                {
                    poemsList = new List<string>();
                    listView1.Items.Clear();            
                    if (comboBox1.SelectedIndex == 1)
                    {
                        applyManhattanDistance();
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        applychebyshevDistance();
                    } else if (comboBox1.SelectedIndex == 3)
                    {
                        applyOwnDistance();
                    }
                }                
            }
            else
            {
                MessageBox.Show("Ingrese el método de cálculo");
            }            
        }
    }
}
