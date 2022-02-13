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
    public partial class Form2 : Form
    {
        bool deterministico;
        AFD automata;
        int col, row;
        string hilera;
        public Form2(AFD automata, bool deterministico)
        {
            this.automata = automata;
            this.deterministico = deterministico;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] simbolos = automata.getSimbolos().ToString().Split(',');
            col = simbolos.Length; row = automata.getEstados().Count;
            for (int i = 0; i < col; i++)
            {
                dataGridView1.Columns.Add("Column" + i, simbolos[i]);
            }
            dataGridView1.Columns.Add("aceptacion", "");
            dataGridView1.Rows.Add(row);
            for (int j = 0; j < row; j++)
            {
                dataGridView1.Rows[j].HeaderCell.Value = String.Join("", automata.getEstados()[j]);
            }
            addDataToDatagridview(automata.getTransiciones());
            for (int k = 0; k < row; k++)
            {
                if (automata.isAceptacion(automata.getAceptacion(),automata.getEstados()[k]))
                {
                    dataGridView1.Rows[k].Cells[col].Value = "1";
                }
                else
                {
                    dataGridView1.Rows[k].Cells[col].Value = "0";
                }

            }
            if (deterministico)
            {
                this.Text = "Autómata determinístico";
                label1.Text = "Autómata determinístico";
            }
            else
            {
                this.Text = "Autómata no determinístico";
                label1.Text = "Autómata no determinístico";
            }
        }

        private void addDataToDatagridview(List<List<string[]>> matrix) //añade datos a la tabla datagriview
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = String.Join("", matrix[i][j]);
                }
            }
        }
        
        private void analisisDFA(string hilera,AFD automata) //Análisis de hilera con el automata
        {
          try{
                string[] chars = hilera.ToCharArray().Select(c => c.ToString()).ToArray();
                string[] actual = automata.getEstados()[0];
                int actualIndex = automata.stateIndex(automata.getEstados()[0]);
                //Console.WriteLine("actual:" + String.Join("",actual) + "| actualIndex:" + actualIndex);
                label4.Text = "La hilera es: " + hilera + "\n"; label4.Text = label4.Text + "El estado inicial es: " + String.Join("",actual) + "\n";
                for (int i = 0; i < chars.Length; i++)
                { //ciclo for que recorre hilera por caracter
                    label4.Text = label4.Text + "Analizando el caracter: " + chars[i] + "\n";
                    int actualSymbolIndex = symbolIndex(chars[i]);
                    actual = automata.getTransiciones()[actualIndex][actualSymbolIndex];
                    actualIndex = automata.stateIndex(actual);
                    label4.Text = label4.Text + "El estado actual es: " + String.Join("",actual) + "\n";
                    //Console.WriteLine("actual:" + String.Join("",actual) + "| actualIndex:" + actualIndex+"|actualsymbolindex:"+actualSymbolIndex);
                }
                string result;
                if (automata.isAceptacion(automata.getAceptacion(), actual)) result = "válida";
                else result = "inválida";
                label4.Text = label4.Text + "La hilera es: " + result + "\n";
           }catch (Exception e){MessageBox.Show("Ingrese sólo símbolos de entrada."); Console.WriteLine(e.ToString()); }
        }




        private int symbolIndex(string symbol) //devuelve el index de un simbolo de entrada
        {
            string aux = automata.getSimbolos();
            string[] symbols = aux.Split(',');
            for (int i = 0; i < symbols.Length; i++)
            {
                if (symbols[i].Equals(symbol)) return i;
            }
            return -1;
        }




        private void button2_Click(object sender, EventArgs e)
        {
            hilera = textBox1.Text;
            List<string[]> aceptacion = automata.getAceptacion();
            analisisDFA(hilera,automata);
        }
    }
}
