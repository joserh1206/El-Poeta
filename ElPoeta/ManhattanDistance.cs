using System;
using System.Collections.Generic;

namespace ElPoeta
{
    class ManhattanDistance
    {
        private int[] metahistogram;
        private int[] generatedhistogram;
        private List<int> indicesPrometedores = new List<int>();
        private List<int> cantPrometedores = new List<int>();
        private List<int> indicesNoPrometedores = new List<int>();

        public ManhattanDistance(int[] metahistogram, int[] generatedhistogram)
        {
            this.metahistogram = metahistogram;
            this.generatedhistogram = generatedhistogram;
            this.calcular_Distancia();
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
                        if (d2 >= 2)
                        {
                            cantPrometedores.Add(d2 - 1);
                        }
                        else
                        {
                            cantPrometedores.Add(d2);
                        }
                    }
                }
                else
                {
                    if (d2 != 0 && d1 != 0)
                    {
                        indicesPrometedores.Add(i);
                        if (d2 >= 2)
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
