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
    public partial class InputView : Form
    {
        bool deterministico = true;
        Form3 fm3;
        public InputView()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                deterministico = false;
            }
            else
            {
                deterministico = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) //botón procesar
        {
            string[] estadosAux = textBox2.Text.Trim(' ').Split(',');
            List<string[]> estados = new List<string[]>();
            string[] aceptacionAux = textBox4.Text.Trim(' ').Split(',');
            List<string[]> aceptacion = new List<string[]>();
            foreach (string str in estadosAux) estados.Add(str.Trim(' ').Split(','));
            foreach (string str in aceptacionAux) aceptacion.Add(str.Trim(' ').Split(','));
            AFD automata = new AFD(textBox1.Text, estados, aceptacion);
            fm3 = new Form3(automata,deterministico);
            fm3.Show();
        }

        
    }
}
