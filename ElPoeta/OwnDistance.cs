using System;
using System.Collections.Generic;

namespace ElPoeta
{
    class OwnDistance
    {
        private int[] metahistogram;
        private int[] generatedhistogram;
        private List<int> indicesPrometedores = new List<int>();
        private List<int> cantPrometedores = new List<int>();
        private List<int> indicesNoPrometedores = new List<int>();
        private List<int> distancias = new List<int>();
        private List<int> ordenList = new List<int>();
        private List<int> indexList = new List<int>();

        public OwnDistance(int[] metahistogram, int[] generatedhistogram)
        {
            this.metahistogram = metahistogram;
            this.generatedhistogram = generatedhistogram;
            calcular_Distancia();
        }

        private void calcular_Distancia()
        {
            int tamanio = metahistogram.Length;
            double distancia = 0;
            for (int i = 0; i < tamanio; i++)
            {
                int d1 = metahistogram[i];
                int d2 = generatedhistogram[i];
                double suma;
                suma = Math.Pow(((d1 + d1) / (d2 + d1 + 1)) - ((d2 + d1) / (d1 + d2 + 1)), 2);
                distancia += suma;
                if (suma < 2)
                {
                    if (d1 != 0)
                    {
                        indicesPrometedores.Add(i);
                        if (d2 == 2)
                        {
                            cantPrometedores.Add(d2 - 1);
                        }
                        else
                        {
                            cantPrometedores.Add(d2);
                        }
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
