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
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();

            Timer timer = new Timer();
            timer.Interval = 1000*30*60;
            timer.Tick += Timer_Tick;
            timer.Start();

            this.Activated += UpdateTable;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var rarities = File.ReadLines("Rarity.txt")
            .Select(line =>
            {
                var parts = line.Trim().Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) return null;
                var chanceStr = parts.Last().TrimEnd('%');
                if (!double.TryParse(chanceStr, out var chance)) return null;
                var name = string.Join(" ", parts.Take(parts.Length - 1));
                return new { Name = name, Chance = chance };
            })
            .Where(r => r != null)
            .ToList();
            var rares = File.ReadLines("Rarity.txt").Select(line =>
            {
                return line.Split()[0];
            }).ToList();


            var total = rarities.Sum(r => r.Chance);
            if (total == 0) return;

            var rnd = new Random().NextDouble();
            double cum = 0;
            var selected = rarities.First(r => (cum += r.Chance / total) >= rnd);

            int indexRare = rares.IndexOf(selected.Name);

            var listLines = File.ReadLines("Table.txt").Select(line =>
            {
                if (int.Parse(line.Split()[1]) == indexRare) { return line.Split()[0]; }
                else { return null; }
            }).Where(x => x != null).ToList();

            if (listLines.Count > 0)
            {
                MessageBox.Show(listLines[new Random().Next(listLines.Count)], "result", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            string name = "";
            int index = 0;

            if (btn.Text == "Edit")
            {
                name = btn.Name.Split()[0];
                index = int.Parse(btn.Name.Split()[1]);
            }

            DialogLine dd = new DialogLine(name, index);
            dd.FormClosed += UpdateTable;

            

            dd.ShowDialog();

        }

        private void UpdateTable(object sender, EventArgs e)
        {
            TableLines.Controls.Clear();
            TableLines.ColumnCount = 1;
            TableLines.HorizontalScroll.Maximum = 0;
            TableLines.AutoScroll = false;
            TableLines.VerticalScroll.Visible = false;
            TableLines.AutoScroll = true;
            TableLines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize, 100F));

            int i = 0;
            foreach (string line in File.ReadLines("Table.txt").ToList().OrderBy(x => int.Parse(x.Split()[1])))
            {
                if (i != 0) { TableLines.RowCount++; }
                var lineTable = new TableLayoutPanel();
                lineTable.ColumnCount = 4;
                lineTable.RowCount = 1;
                lineTable.Dock = DockStyle.Top;
                lineTable.Height = 30;
                for (int k = 0; k < 4; k++)
                {
                    lineTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25));
                }

                var btnDel = new Button();
                btnDel.Text = "Del";
                btnDel.Name = line;
                btnDel.Tag = i;
                btnDel.Click += this.DeleteLine;
                btnDel.Width = 100;
                btnDel.Anchor = AnchorStyles.None;

                var btnEdit = new Button();
                btnEdit.Text = "Edit";
                btnEdit.Width = 100;
                btnEdit.Anchor = 0;
                btnEdit.Name = line;
                btnEdit.Tag = i;
                btnEdit.Anchor = AnchorStyles.None;
                btnEdit.Click += this.EditLinebtn;

                var lbName = new Label();
                lbName.Text = line.Split()[0];
                lbName.TextAlign = ContentAlignment.MiddleCenter;
                lbName.Anchor = 0;

                var Rares = File.ReadLines("Rarity.txt").ToList();
                var lbRare = new Label();
                lbRare.Text = Rares[int.Parse(line.Split()[1])];
                lbRare.TextAlign = ContentAlignment.MiddleCenter;
                lbRare.Anchor = 0;

                lineTable.Controls.Add(lbName, 0, i);
                lineTable.Controls.Add(lbRare, 1, i);
                lineTable.Controls.Add(btnDel, 2, i);
                lineTable.Controls.Add(btnEdit, 3, i);
                    

                TableLines.Controls.Add(lineTable, 0, i);

                i++;
            }


            MainTable.Controls.Add(TableLines, 0, 1);
        }

        private void DeleteLine(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int i = int.Parse(btn.Tag.ToString());

            var lines = File.ReadLines("Table.txt").ToList().OrderBy(x => int.Parse(x.Split()[1])).ToList();
            lines.RemoveAt(i);

            File.WriteAllLines("Table.txt", lines);

            this.UpdateTable(sender, e);

        }

        private void EditLinebtn(object sender, EventArgs e)
        {
            this.DeleteLine(sender, e);
            this.btnAddLine_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
