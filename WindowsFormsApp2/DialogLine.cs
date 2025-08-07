using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class DialogLine: Form
    {
        public DialogLine(string startName, int indexRare)
        {
            InitializeComponent();



            var f = File.ReadLines("Rarity.txt");
            foreach (string line in f)
            {
                Rare.Items.Add(line);
            }

            Rare.SelectedIndex = indexRare;
            nameNewLine.Text = startName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nameNewLine.Text == "")
            {
                MessageBox.Show("Name cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (nameNewLine.Text.Length > 17)
            {
                MessageBox.Show("Name cannot contain more than 17 letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (nameNewLine.Text.Contains(" "))
            {
                MessageBox.Show("Name cannot contain spaces", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string LineTable = "";

                LineTable += nameNewLine.Text + " ";
                LineTable += Rare.SelectedIndex.ToString();

                try
                {
                    File.AppendAllText("Table.txt", LineTable + "\n");
                }
                catch
                {
                    File.Create("Table.txt");
                }

                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
