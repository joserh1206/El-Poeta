using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string datasetURL = @"C:\Users\joser\Desktop\ConsoleApp1\ConsoleApp1\all.csv";
            //string poema = File.ReadAllText(datasetURL).Replace(";", "").ToLowerInvariant();
            //string poema = "hola mundo mundo hola soy jose luis";
            string poema = "sin fotografías\r\nsin planear el viaje\r\nsin contar los días\r\n\r\nSintiendo la briza\r\nsin ninguna prisa\r\nsin complicaciones\r\nsin evitar emociones\r\n\r\nSingular y natural\r\nsin obligar ni forzar\r\nsin pensar mal\r\nsin dejar de amar".ToLowerInvariant();
            //string poemaMeta = "In the Shreve High football stadium, \r\nI think of Polacks nursing long beers in Tiltonsville, \r\nAnd gray faces of Negroes in the blast furnace at Benwood, \r\nAnd the ruptured night watchman of Wheeling Steel, \r\nDreaming of heroes. \r\n\r\nAll the proud fathers are ashamed to go home, \r\nTheir women cluck like starved pullets, \r\nDying for love. \r\n\r\nTherefore, \r\nTheir sons grow suicidally beautiful \r\nAt the beginning of October, \r\nAnd gallop terribly against each other’s bodies.";
            string poemaMeta = "hola mundo soy jose y tomo bonitas fotografías en los días soleados";
            N_gram nGram = new N_gram(poema, poemaMeta);
            Dictionary<int, string> dictionary = nGram.GetDictionary();
            Generate_Poem generate = new Generate_Poem();
            int tamanioMeta = poemaMeta.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length;
            string generated = generate.gen(dictionary, tamanioMeta);
            Histogram histogram = new Histogram(poemaMeta, generated);
            int[] histogramMeta = histogram.getHistogramMeta();
            int[] histogramGenerated = histogram.getHistogramGenerated();
            Distancia_Manhattan distancia = new Distancia_Manhattan(histogramMeta, histogramGenerated);
            List<int> prometedores = distancia.getIndicesPrometedores();
            Console.WriteLine("Poema:");
            Console.WriteLine(generated);

            Console.ReadKey();
        }
    }

    class N_gram
    {
        private string[] result;
        private int indexT = 0;
        Dictionary<int, string> indexDictionary = new Dictionary<int, string>();

        public Dictionary<int, string> GetDictionary()
        {
            return indexDictionary;
        }

        public string[] getResult()
        {
            return result;
        }

        public N_gram(string metaPoem, string poemGen)
        {
            var text = metaPoem.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            var generated = poemGen.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            result = text.Union(generated).ToArray();
            Create_Ngram(1);
            Create_Ngram(2);
            Create_Ngram(3);
            //Console.WriteLine(indexDictionary[20500]);
            //Console.ReadKey();
        }

        public void Create_Ngram(int ngram)
        {
            int index = -1;
            foreach (var word in result)
            {
                index++;
                int cont = 0;
                string ngramTemp = "";
                while (cont < ngram)
                {
                    if (index + cont < result.Length)
                    {
                        ngramTemp += result[index + cont] + " ";
                        cont++;
                    }
                    else
                    {
                        cont++;
                    }
                }

                indexDictionary.Add(indexT, ngramTemp);
                indexT++;
            }
            indexDictionary.Distinct();

        }
    }

    class Generate_Poem
    {
        private string poem = "";

        public string gen(Dictionary<int, string> dictionary, int tamanioMeta)
        {
            Random random = new Random();
            int c = 0;
            int tamanio = dictionary.Count;
            Console.Write("Cantidad Meta:");
            Console.Write(tamanioMeta);
            while (c < tamanioMeta)
            {
                int ranIndex = random.Next(tamanio);
                string[] cant = dictionary[ranIndex].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                poem += dictionary[ranIndex];
                c += cant.Length;
            }
            //Console.WriteLine("POEM:");
            //Console.WriteLine(poem);

            return poem;
            //Console.ReadKey();
        }
    }

    class Histogram
    {
        private List<string> generatedList = new List<string>();
        private List<string> metaPoemList = new List<string>();
        private List<string> totalList = new List<string>();
        private int[] histogramGenerated;
        private int[] histogramMeta;
        private string generatedPoem;
        private string meta;


        public Histogram(string metaPoem, string generated)
        {
            metaPoemList = metaPoem.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).ToList();
            generatedList = generated.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).ToList();
            totalList = metaPoemList.Union(generatedList).Distinct().ToList();
            int totalListSize = totalList.Count;
            histogramGenerated = new int[totalListSize];
            histogramMeta = new int[totalListSize];
            histogramGenerated.Initialize();
            histogramMeta.Initialize();
            generatedPoem = generated;
            meta = metaPoem;
            CreateHistogram();
        }

        private void CreateHistogram()
        {
            foreach (string s in metaPoemList)
            {
                int index = totalList.IndexOf(s);
                histogramMeta[index]++;
            }
            foreach (string s in generatedList)
            {
                int index = totalList.IndexOf(s);
                histogramGenerated[index]++;
            }
            //Console.ReadKey();
        }

        public int[] getHistogramMeta()
        {
            return histogramMeta;
        }

        public int[] getHistogramGenerated()
        {
            return histogramGenerated;
        }
    }

    class Distancia_Manhattan
    {
        private int[] metahistogram;
        private int[] generatedhistogram;
        private List<int> indicesPrometedores = new List<int>();

        public Distancia_Manhattan(int[] metahistogram, int[] generatedhistogram)
        {
            this.metahistogram = metahistogram;
            this.generatedhistogram = generatedhistogram;
            calcular_Distancia();
        }

        private void calcular_Distancia()
        {
            int tamanio = metahistogram.Length;
            int distancia = 0;
            for (int i = 0; i < tamanio; i++)
            {
                int d1 = metahistogram[i];
                int d2 = generatedhistogram[i];
                int suma = Math.Abs(d1 - d2);
                distancia += suma;
                if (suma > 2)
                {
                    indicesPrometedores.Add(i);
                }
            }
            Console.WriteLine("distancia:");
            Console.WriteLine(distancia);
        }

        public List<int> getIndicesPrometedores()
        {
            return indicesPrometedores;
        }
    }

}

