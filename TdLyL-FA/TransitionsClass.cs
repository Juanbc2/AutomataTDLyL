using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TdLyL_FA
{
    public partial class Form3 : Form
    {
        bool deterministico;
        AFD automata;
        int col, row;
        public Form3(AFD automata,bool deterministico)
        {
            this.automata = automata;
            this.deterministico = deterministico;
            InitializeComponent();
        }

        private void tran_Load(object sender, EventArgs e)
        {
            string[] simbolos = automata.getSimbolos().ToString().Split(',');
            List<string[]> estados = automata.getEstados();
            col = simbolos.Length; row = estados.Count;
            for (int i = 0; i < col; i++)
            {
                dataGridView1.Columns.Add("Column"+i,simbolos[i]);
            }
            dataGridView1.Rows.Add(row);
            for (int j = 0; j < row; j++)
            {
                dataGridView1.Rows[j].HeaderCell.Value = String.Join("",estados[j]);

            }
        }

        private List<List<string[]>> datagridviewToMatrix(DataGridView datagridview, int row, int col)
        {
            List<List<string[]>> matrix = new List<List<string[]>>();
            for (int i = 0; i < row; i++)
            {
                List<string[]> stateRow = new List<string[]>();

                for (int j = 0; j < col; j++)
                {
                    try
                    {
                        if (datagridview.Rows[i].Cells[j].Value.ToString().Equals(""))
                        {
                            stateRow.Add(("¬").Split(','));
                        }
                        else
                        {
                            stateRow.Add(datagridview.Rows[i].Cells[j].Value.ToString().Trim(' ').Split(','));
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.GetBaseException().ToString());
                        stateRow.Add(("¬").Split(','));
                    }
                }
                matrix.Add(stateRow);
            }
            return matrix;
        }

        private bool isInAcceptationList(string[] state) //Ver si el estado es de aceptación
        {
            List<string[]> acceptList = automata.getAceptacion();
            foreach (string[] acpState in acceptList)
            {
                if (acpState.SequenceEqual(state)) { return true;}
            }
            return false;
        }


        private Tuple<List<List<string[]>>, List<string[]>> AFNDtoAFD(AFD automata) //Conversor de no deterministico a deterministico
        {
            List<string[]> estadosAux = new List<string[]>(); estadosAux.Add(automata.getEstados()[0]);
            List<List<string[]>> transitionsAux = new List<List<string[]>>(); transitionsAux.Add(automata.getTransiciones()[0]);
            for (int i = 0; i < transitionsAux.Count; i++)
            {
                for (int j = 0; j < transitionsAux[i].Count; j++)
                {
                    for (int k = 0; k < transitionsAux[i][j].Length; k++)
                    {
                        if (!isInList(estadosAux, transitionsAux[i][j]))
                        {
                            string[] newTransAux = transitionsAux[i][j];
                            List<string[]> newTransState = setNewState(automata.getTransiciones(),automata.getEstados(), newTransAux);
                            estadosAux.Add(newTransAux);
                            transitionsAux.Add(newTransState);
                        }
                    }
                }
            }
            return new Tuple<List<List<string[]>>, List<string[]>>(transitionsAux, estadosAux);
        }

        private List<string[]> setNewState(List<List<string[]>> transitions, List<string[]> states, string[] chars) //crear nuevo estado
        {
            List<string[]> trans = new List<string[]>();
            bool accept = false;
            for (int i = 0; i < col; i++)
            {
                string state = "";
                for (int k = 0; k < chars.Length; k++)
                {
                    for (int j = 0; j < states.Count; j++)
                    {
                        if (states[j].SequenceEqual(chars[k].Split(',')))
                        {
                            if (k == 0)
                            {
                                state = String.Join("", transitions[j][i]);
                            }
                            else
                            {
                                state = state + "," + String.Join("", transitions[j][i]);
                            }
                        }
                    }
                }
                string[] auxState = doubleArrayToUnique(state.Split(','));
                foreach (string str in auxState) accept = isInAcceptationList(str.Split(','));
                trans.Add(auxState);
            }
            if (accept)
            {
                List<string[]> auxAccp = automata.getAceptacion();
                auxAccp.Add(chars);
                automata.setAceptacion(auxAccp);
            }
            return trans;
        }

        private string[] doubleArrayToUnique(string[] array) //unir valores de un vector de 2 posiciones en otro vector de 1 posición
        {
            string aux = "";
            foreach (string str in array)
            {
                aux += str;
            }
            var unique = new HashSet<char>(aux);
            List<string> stringList = new List<string>();
            foreach (char c in unique)
               stringList.Add(c.ToString());
            return stringList.ToArray();
        }

        

        
        private bool isInList(List<string[]> list, string[] array) //revisar si un array está en la lista
        {
            string[] strReverse = reverseArray(array);
            for (int i = 0; i < list.Count; i++)
            {

                if (list[i].SequenceEqual(array) || list[i].SequenceEqual(strReverse))
                {
                   return true;
                }

            }

            return false;
        }

        private string[] reverseArray(string[] array) //invertir array 
        {
            string[] aux = new string[array.Length];
            int index = array.Length-1;
            for(int i = 0;i<array.Length;i++)
            {
                aux[i] = array[index];
                index--;
            }
            return aux;
        }



        //Función principal
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                automata.setTransiciones(datagridviewToMatrix(dataGridView1, row, col));
                if (!deterministico)
                {
                    Tuple<List<List<string[]>>, List<string[]>> tuplaTrans = AFNDtoAFD(automata);
                    automata.setTransiciones(tuplaTrans.Item1);
                    automata.setEstados(tuplaTrans.Item2);
                }
                automata.deleteRareStates();
                ResultsView fm = new ResultsView(automata, deterministico);
                fm.Show();
                this.Hide();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message.ToString());
                MessageBox.Show("Ha ocurrido un error,¿Ingresó correctamente el autómata?");
            }
        }

        public List<string[]> TrimArray(int rowToRemove, List<string[]> array) //remueve una posición de un array
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

        public List<List<string[]>> TrimMatrix(int rowToRemove, List<List<string[]>> matrix) //remueve una fila de una matriz
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

        private bool isInMatrix(List<List<string[]>> matrix, string[] str) //revisar si un array está en la matriz
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (matrix[i][j] == str)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void printList(List<string[]> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                {
                    Console.Write("|"+list[i][j]+"|");
                }
            Console.WriteLine();
            }
        }

        private void printMatrix(List<List<string[]>> matrix)
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    for (int k = 0; k < matrix[i][j].Length; k++)
                    {
                        Console.Write("|" + matrix[i][j][k]+ "|");
                    }
                }
                Console.WriteLine();
            }
        }
        






    }
}
