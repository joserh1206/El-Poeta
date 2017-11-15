using System;
using System.Collections.Generic;
using System.Linq;

namespace ElPoeta
{
    class NGram
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

        public NGram(string metaPoem, string poemGen)
        {
            var text = metaPoem.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            var generated = poemGen.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            result = text.Union(generated).ToArray();
            Create_Ngram(1);
            Create_Ngram(2);
            Create_Ngram(3);
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
}
