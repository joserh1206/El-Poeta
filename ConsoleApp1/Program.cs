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
            string poema = "sin fotografías\r\nsin planear el viaje\r\nsin contar los días jose\r\n\r\nSintiendo la briza\r\nsin ninguna prisa\r\nsin complicaciones\r\nsin evitar emociones\r\n\r\nSingular y natural\r\nsin obligar ni en forzar\r\nsin pensar mal\r\nsin dejar de tomo y amar".ToLowerInvariant();
            //string poemaMeta = "In the Shreve High football stadium \r\nI think of Polacks nursing long beers in Tiltonsville \r\nAnd gray faces of Negroes in the blast furnace at Benwood \r\nAnd the ruptured night watchman of Wheeling Steel \r\nDreaming of heroes \r\n\r\nAll the proud fathers are ashamed to go home \r\nTheir women cluck like starved pullets \r\nDying for love \r\n\r\nTherefore \r\nTheir sons grow suicidally beautiful \r\nAt the beginning of October \r\nAnd gallop terribly against each other’s bodies";
            string poemaMeta = "hola mundo soy jose y tomo bonitas fotografías en los días soleados";
            int iteraciones = 50; //Cantidad de generaciones, entre más el poema se va a parecer más
            N_gram nGram = new N_gram(poema, poemaMeta);
            Dictionary<int, string> dictionary = nGram.GetDictionary();
            int tamanioMeta = poemaMeta.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).Length;
            Generate_Poem generate = new Generate_Poem();
            string generated = generate.gen(dictionary, tamanioMeta);
            for (int i = 0; i < iteraciones; i++)
            {
                Console.WriteLine("\nGenerated:");
                Console.WriteLine(generated);
                Histogram histogram = new Histogram(poemaMeta, generated, dictionary);
                int[] histogramMeta = histogram.getHistogramMeta();
                int[] histogramGenerated = histogram.getHistogramGenerated();
                List<string> totalList = histogram.GetTotaList();
                Distancia_Manhattan distancia = new Distancia_Manhattan(histogramMeta, histogramGenerated);
                List<int> prometedores = distancia.getIndicesPrometedores();
                //List<int> noPrometedores = distancia.getIndicesNoPrometedores();
                List<int> cantPrometedores = distancia.getCantPrometedores();
                generated = generate.new_Generation(prometedores, cantPrometedores, totalList);
                //Console.WriteLine("generated:");
                //Console.WriteLine(generated);
                //Console.WriteLine("\nNewPoem:");
                //Console.WriteLine(newPoem);
            }
            Console.WriteLine("\nFinal:");
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
        private int metaSize;
        private Dictionary<int, string> dicDictionary;

        public string gen(Dictionary<int, string> dictionary, int tamanioMeta)
        {
            dicDictionary = dictionary;
            metaSize = tamanioMeta;
            Random random = new Random();
            int c = 0;
            int tamanio = dictionary.Count;
            //Console.Write("Cantidad Meta:");
            //Console.Write(tamanioMeta);
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

        public static List<T> DesordenarLista<T>(List<T> input)
        {
            List<T> arr = input;
            List<T> arrDes = new List<T>();

            Random randNum = new Random();
            while (arr.Count > 0)
            {
                int val = randNum.Next(0, arr.Count - 1);
                arrDes.Add(arr[val]);
                arr.RemoveAt(val);
            }

            return arrDes;
        }

        public string new_Generation(List<int> prometedores, List<int> cantPrometedores, List<string> listaTotal)
        {
            int tamanio = prometedores.Count;
            poem = "";
            List<string> newGenList = new List<string>();
            for (int i = 0; i < tamanio; i++)
            {
                int cantP = cantPrometedores[i];
                Console.WriteLine("\ncantP:");
                Console.WriteLine(cantP);
                for (int j = 0; j < cantP; j++)
                {
                    newGenList.Add(listaTotal[prometedores[i]]);
                    Console.WriteLine("\nPrometedor aniadido:");
                    Console.WriteLine(listaTotal[prometedores[i]]);
                }
            }
            int c = newGenList.Count;
            Random random = new Random();
            tamanio = dicDictionary.Count;
            while (c < metaSize)
            {
                int ranIndex = random.Next(tamanio);
                string[] cant = dicDictionary[ranIndex].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                newGenList.Add(dicDictionary[ranIndex]);
                c += cant.Length;
            }
            newGenList = DesordenarLista(newGenList);
            foreach (string s in newGenList)
            {
                poem += s + " ";
            }
            poem.Replace(" ", "");
            return poem;
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
        private Dictionary<int, string> dictionary;


        public Histogram(string metaPoem, string generated, Dictionary<int, string> diccionario)
        {
            dictionary = diccionario;
            metaPoemList = metaPoem.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).ToList();
            generatedList = generated.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> ls = dictionary.Values.ToList();
            totalList = metaPoemList.Union(generatedList).Union(ls).Distinct().ToList();
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

            //Console.WriteLine("Meta:");

            //foreach (int i in histogramMeta)
            //{
            //    Console.WriteLine(i);
            //}
            //Console.WriteLine("\nGenerado:");

            //foreach (int i in histogramGenerated)
            //{
            //    Console.WriteLine(i);
            //}
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

        public List<string> GetTotaList()
        {
            return totalList;
        }
    }

    class Distancia_Manhattan
    {
        private int[] metahistogram;
        private int[] generatedhistogram;
        private List<int> indicesPrometedores = new List<int>();
        private List<int> cantPrometedores = new List<int>();
        private List<int> indicesNoPrometedores = new List<int>();

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
                if (suma == 0)
                {
                    if (d1 != 0)
                    {
                        indicesPrometedores.Add(i);
                        cantPrometedores.Add(d2);
                    }
                }
                else
                {
                    if (d2 == 0)
                    {
                        indicesNoPrometedores.Add(i);
                    }
                    else if (d2 != 0 && d1 != 0)
                    {
                        indicesPrometedores.Add(i);
                        cantPrometedores.Add(d2);
                    }
                }
                
            }

        }

        public List<int> getIndicesPrometedores()
        {
            return indicesPrometedores;
        }

        public List<int> getIndicesNoPrometedores()
        {
            return indicesNoPrometedores;
        }
        public List<int> getCantPrometedores()
        {
            return cantPrometedores;
        }
    }

}

