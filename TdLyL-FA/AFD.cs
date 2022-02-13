using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TdLyL_FA
{
    public class AFD
    {
        private string simbolos;
        private List<string[]> estados;
        private List<string[]> aceptacion;
        private List<List<string[]>> transiciones;

        public AFD(string simbolos,
         List<string[]> estados,
        List<string[]> aceptacion)
        {
            this.simbolos = simbolos;
            this.estados = estados;
            this.aceptacion = aceptacion;
        }

        public string getSimbolos()
        {
            return this.simbolos;
        }

        public void setSimbolos(string simbolos)
        {
            this.simbolos = simbolos;
        }

        public List<string[]> getEstados()
        {
            return this.estados;
        }

        public List<string[]> getAceptacion()
        {
            return this.aceptacion;
        }

        public void setAceptacion(List<string[]> aceptacion)
        {
            this.aceptacion = aceptacion;
        }


        public void setTransiciones(List<List<string[]>> transiciones)
        {
            this.transiciones = transiciones;
        }

        public List<List<string[]>> getTransiciones()
        {
            return this.transiciones;
        }

        public void setEstados(List<string[]> estados)
        {
            this.estados = estados;
        }


        public void deleteRareStates() //elimina estados extraños
        {
            List<List<string[]>> auxTransiciones = new List<List<string[]>>(); auxTransiciones.Add(getTransiciones()[0]);
            List<string[]> auxEstados = new List<string[]>(); auxEstados.Add(getEstados()[0]);
            for (int i = 0; i < getEstados().Count; i++)
            {
               if (isInMatrixList(auxTransiciones, getEstados()[i]) && !auxEstados.Contains(getEstados()[i]))
                {
                    auxTransiciones.Add(getTransiciones()[i]);
                    auxEstados.Add(getEstados()[i]);
                    i = 0;
                }
            }
            setEstados(auxEstados);
            setTransiciones(auxTransiciones);
        }




        private bool isInMatrixList(List<List<string[]>> matrix, string[] str) //true si está, false si no
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[0].Count; j++)
                {
                    if (matrix[i][j].SequenceEqual(str))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<string[]> TrimList(int rowToRemove, List<string[]> array) //elimina posición de una lista
        {
            List<string[]> auxArray = new List<string[]>();
            for (int i = 0; i < array.Count; i++)
            {
                if (i != rowToRemove)
                {
                    auxArray.Add(array[i]);
                }
            }
            return auxArray;
        }

        private List<List<string[]>> TrimListMatrix(int rowToRemove, List<List<string[]>> matrix) //elimina una fila de una matriz
        {
            List<List<string[]>> auxMatrix = new List<List<string[]>>();
            for (int i = 0; i < matrix.Count; i++)
            {
                if (i != rowToRemove)
                {
                    auxMatrix.Add(matrix[i]);
                }
            }
            return auxMatrix;
        }

        public bool isAceptacion(List<string[]> aceptacion, string[] state)//ver si es de aceptacion o no
        {
            string[] stateReverse = reverseArray(state);
            for (int i = 0; i < aceptacion.Count; i++)
            {
                if (aceptacion[i].SequenceEqual(state) || aceptacion[i].SequenceEqual(stateReverse)) return true;
            }
            return false;
        }

        public int stateIndex(string[] state) //index de un estado dentro de la lista de estados
        {
            string[] stateReverse = reverseArray(state);
            for (int i = 0; i < getEstados().Count; i++)
            {
                if (getEstados()[i].SequenceEqual(state) || getEstados()[i].SequenceEqual(stateReverse)) return i;
            }
            return -1;
        }

        private string[] reverseArray(string[] array) //voltea un array
        {
            string[] aux = new string[array.Length];
            int index = array.Length - 1;
            for (int i = 0; i < array.Length; i++)
            {
                aux[i] = array[index];
                index--;
            }
            return aux;
        }

        private void printMatrix(List<List<string[]>> matrix) 
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    for (int k = 0; k < matrix[i][j].Length; k++)
                    {
                        Console.Write("|" + matrix[i][j][k] + "|");
                    }
                }
                Console.WriteLine();
            }
        }

        private void printList(List<string[]> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                {
                    Console.Write("|" + list[i][j] + "|");
                }
                Console.WriteLine();
            }
        }





    }
}
