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
            /*
            DataBase.Use("mydb");
            List<string[]> strs = DataBase.Select(
                "new2",
                new RequestWhere("id = 2 OR id = 1"),
                new RequestOrder("id", "DESC")
            );*/
            /*
            for (int i = 0; i != strs.Count; i++) {
                for (int j = 0; j != strs[i].Length; j++) {
                    Console.Write(strs[i][j] + " ");
                }
                Console.WriteLine();
            }*/

            //DateObject date = new DateObject("02.02.2017");
            //Console.WriteLine(date.GetDate("yyyy-mm-dd"));

            //DataBase.Remove("new2", new RequestWhere("id = 2"));
            //DataBase.Update("new2", new string[,] { { "name", "20" } });
            // Console.WriteLine(Convert.ToInt32("$12".Substring(1)));
            //RequestWhere rw = new RequestWhere("id = 12 AND (a > 'a' OR (a < 'a') AND (a > 'a'))");
            /*
            DataBase.Insert(
                "new2", new string[] { 
                    "2",
                    "3",
                    "4",
                    "4"
                }
            );*/
        }

        private List<string> DataBaseName = new List<string>();
        private List<string> DataBaseTable = new List<string>();
        private List<DataGridView> DataBaseGrid = new List<DataGridView>();
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
                    this.DataBaseName.Add(ptn.Text);
                    this.DataBaseTable.Add(tn.Text);
                    this.DataBaseGrid.Add(grid);

                    grid.Dock = System.Windows.Forms.DockStyle.Fill;
                    tp.Controls.Add(grid);
                    for (int i = 0; i != cols.Length; i++) {
                        grid.Columns.Add(i.ToString(), cols[i].getName());
                    }

                    List<string[]> list = DataBase.Select(tn.Text);
                    if (list.Count != 0) {
                        for (int i = 0; i != list.Count; i++) {
                            grid.Rows.Add();
                            for (int j = 0; j != list[i].Length; j++) {
                                grid[j, i].Value = list[i][j];
                            }
                        }
                    }
                }
            }
        }

        private void buttonAddDataBase_Click(object sender, EventArgs e) {
            CreateDataBaseForm f = new CreateDataBaseForm();
            f.Owner = this;
            f.Show();
        }

        private void SaveTableButton_Click(object sender, EventArgs e) {
            DataBase.Use(this.DataBaseName[tabControl.SelectedIndex]);

            string table = this.DataBaseTable[tabControl.SelectedIndex];
            Column[] cols = DataBase.GetColumns(table);

            DataBase.RemoveTable(table);
            DataBase.CreateTable(table, cols);

            string[] row;
            DataGridView grid = this.DataBaseGrid[tabControl.SelectedIndex];
            for (int i = 0; i != grid.RowCount - 1; i++) {
                row = new string[cols.Length];

                for (int j = 0; j != cols.Length; j++) {
                    row[j] = grid[j, i].Value.ToString();
                }

                DataBase.Insert(table, row, cols);
            }
        }

        private void buttonAddTable_Click(object sender, EventArgs e) {
            CreateTableForm f = new CreateTableForm();
            f.Owner = this;
            f.Show();
        }
    }
}