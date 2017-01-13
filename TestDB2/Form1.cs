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

        public void DataBaseTree_Reload() {
            string[] tables, dbs = DataBase.GetDataBases();
            DataBaseTree.Nodes.Clear();
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

            DataBase.Use("mydb");
            DataBase.Select("new2");
            // Console.WriteLine(Convert.ToInt32("$12".Substring(1)));
            RequestWhere rw = new RequestWhere("id = 12 AND (a > 'a' OR (a < 'a') AND (a > 'a'))");
            /*
            DataBase.Insert(
                "new2", new string[] { 
                    "1",
                    "2",
                    "3",
                    "4"
                }
            );*/
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

        private void buttonAddDataBase_Click(object sender, EventArgs e) {
            CreateDataBaseForm f = new CreateDataBaseForm();
            f.Owner = this;
            f.Show();
        }
    }
}