using System;
using System.Collections.Generic;
using System.Linq;

namespace ElPoeta
{
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
}
