using System;
using System.Collections.Generic;

namespace ElPoeta
{
    class PoemGenerator
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
            
            while (c < tamanioMeta)
            {
                int ranIndex = random.Next(tamanio);
                string[] cant = dictionary[ranIndex].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                poem += dictionary[ranIndex];
                c += cant.Length;
            }           
            return poem;            
        }

        public List<T> DesordenarLista<T>(List<T> input)
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
                for (int j = 0; j < cantP; j++)
                {
                    newGenList.Add(listaTotal[prometedores[i]]);                    
                }
            }
            int c = newGenList.Count;
            Random random = new Random();
            tamanio = dicDictionary.Count;
            while (c < metaSize)
            {
                int ranIndex = random.Next(tamanio);
                string[] cant = dicDictionary[ranIndex].Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                if (newGenList.Contains(dicDictionary[ranIndex]) == false)
                {
                    newGenList.Add(dicDictionary[ranIndex]);
                    c += cant.Length;
                }
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
}
