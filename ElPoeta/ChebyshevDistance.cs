using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElPoeta
{
    class ChebyshevDistance
    {
        private int[] metahistogram;
        private int[] generatedhistogram;
        private List<int> indicesPrometedores = new List<int>();
        private List<int> cantPrometedores = new List<int>();
        private List<int> indicesNoPrometedores = new List<int>();
        private List<int> distancias = new List<int>();
        private List<int> ordenList = new List<int>();
        private List<int> indexList = new List<int>();        

        public ChebyshevDistance(int[] metahistogram, int[] generatedhistogram)
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

                if (suma > distancia)
                {
                    distancia = suma;
                }
                else
                {
                    if (d1 != 0)
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
