using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDB2 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void DataBaseTree_Reload() {
            string[] tables, dbs = DataBase.GetDataBases();
            TreeNode tn;

            for (int i = 0; i != dbs.Length; i++) {
                tn = DataBaseTree.Nodes.Add(dbs[i]);
                tn.ImageIndex = 0;

                DataBase.Use(dbs[i]);
                tables = DataBase.GetTables();
                for (int j = 0; j != tables.Length; j++) {
                    tn.Nodes.Add(dbs[i] + "_" + tables[j], tables[j], 1, 1);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            DataBaseTree_Reload();
        }

        private void DataBaseTree_MouseDoubleClick(object sender, MouseEventArgs e) {
            TreeNode tn = DataBaseTree.SelectedNode;
            TreeNode ptn = tn.Parent;

            if (ptn != null) {
                DataBase.Use(ptn.Text);
                Column[] cols = DataBase.GetColumns(tn.Text);

                if (cols != null) {
                    TabPage tp = new TabPage(ptn.Text + "." + tn.Text);
                    tabControl.TabPages.Add(tp);

                    DataGridView grid = new DataGridView();
                    grid.Dock = System.Windows.Forms.DockStyle.Fill;
                    tp.Controls.Add(grid);
                    for (int i = 0; i != cols.Length; i++) {

                        grid.Columns.Add(i.ToString(), cols[i].getName());
                    }
                }
            }
        }
    }
}
