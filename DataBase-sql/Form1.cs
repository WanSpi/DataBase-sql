using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseSQL {
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
            resize();
            /*
            StreamReader sr = new StreamReader("a.txt");
            DataBase.Use("Game");

            DataBase.CreateTable("MapCells", new Column[] {
                new Column("id", ColumnType.INT, 16),
                new Column("id_map", ColumnType.INT),
                new Column("position_x", ColumnType.INT),
                new Column("position_y", ColumnType.INT),
                new Column("id_cell", ColumnType.INT)
            });

            int y = 1, id = 1;
            string buf = "";
            while (!sr.EndOfStream) {
                buf = sr.ReadLine();
                for (int x = 0; x != buf.Length; x++) {
                    DataBase.Insert("MapCells", new string[] {
                        (id++).ToString(),
                        "1",
                        (x + 1).ToString(),
                        y.ToString(),
                        buf[x].ToString()
                    });
                }

                y++;
            }

            this.Close();*/

            /*
            DataBase.Use("Game");

            Column[] cols = DataBase.GetColumns("AnimationFrame");
            for (int i = 0; i != cols.Length; i++) {
                if (cols[i].getName() == "id") {
                    cols[i] = new Column(cols[i].getName(), cols[i].getType(), 16);
                }
            }
            DataBase.ChangeColumns("AnimationFrame", cols);
            */
            /*ConverterBase.Converter("DataBase\\Game\\AnimationData.table", "0.1", "0.2");
            ConverterBase.Converter("DataBase\\Game\\Animations.table", "0.1", "0.2");
            ConverterBase.Converter("DataBase\\Game\\CharacteristicAttack.table", "0.1", "0.2");
            ConverterBase.Converter("DataBase\\Game\\CharacteristicElements.table", "0.1", "0.2");
            ConverterBase.Converter("DataBase\\Game\\Characteristics.table", "0.1", "0.2");
            ConverterBase.Converter("DataBase\\Game\\MovementPoints.table", "0.1", "0.2");
            ConverterBase.Converter("DataBase\\Game\\Movements.table", "0.1", "0.2");*/
            /*
            DataBase.Use("Game");
            ResponseObject res = DataBase.Select("Parent");
            if (res != null) {
                res.Join("Child", "id", "id_parent");

                while (res.NextIndex()) {
                    System.Diagnostics.Debug.Print("Parent id: " + res.GetValue("id"));

                    ResponseObject r = res.GetRowChildren("Child");
                    while (r.NextIndex()) {
                        System.Diagnostics.Debug.Print("Child name: " + r.GetValue("name"));
                    }
                }
            }
            */
            /*
            ConverterBase.Converter("DataBase\\Game\\New.table", "0.1", "0.2");
            */
            /*
            DataBase.Use("Game");

            ResponseRow row = new ResponseRow("New");
            row.SetValue("name", "asd");
            row.Save();

            ResponseObject res = DataBase.Select("New");
            while(res.NextIndex()) {

                Debug.Print(res.GetValue("id"));
            }
             */

            /*
            DataBase.Use("Game");
            Column[] cols = DataBase.GetColumns("New");

            Array.Resize<Column>(ref cols, cols.Length + 1);
            cols[cols.Length - 1] = new Column("Date 1", ColumnType.VARCHAR, 100);

            DataBase.ChangeColumns("New", cols).ToString();
            */

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
                        grid.Columns.Add(i.ToString(), cols[i].Name);
                    }

                    ResponseObject res = DataBase.Select(tn.Text);
                    //List<string[]> list = DataBase.Select(tn.Text);
                    if (res != null) {
                        int ind = 0;

                        while (res.SetIndex(ind++)) {
                            grid.Rows.Add();
                            for (int i = 0; i != cols.Length; i++) {
                                grid[i, ind - 1].Value = res.GetValue(cols[i].Name);
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
                    if (grid[j, i].Value != null) {
                        row[j] = grid[j, i].Value.ToString();
                    } else {
                        row[j] = "";
                    }
                }

                DataBase.Insert(table, row, cols);
            }
        }

        private void buttonAddTable_Click(object sender, EventArgs e) {
            CreateTableForm f = new CreateTableForm();
            f.Owner = this;
            f.Show();
        }

        private void tabControl_MouseClick(object sender, MouseEventArgs e) {
            Debug.Print(sender.ToString());

            Debug.Print(e.Location.ToString());
        }

        private int formWidth = 0;
        private int formHeight = 0;
        private void resize() {
            if (formWidth == 0 && formHeight == 0) {
                formWidth = this.Size.Width;
                formHeight = this.Size.Height;
                return;
            }

            int width = this.Size.Width - formWidth;
            int height = this.Size.Height - formHeight;

            SaveTableButton.Location = new Point(SaveTableButton.Location.X + width, SaveTableButton.Location.Y + height);
            DataBaseTree.Size = new System.Drawing.Size(DataBaseTree.Size.Width, DataBaseTree.Size.Height + height);
            tabControl.Size = new System.Drawing.Size(tabControl.Size.Width + width, tabControl.Size.Height + height);
            
            formWidth = this.Size.Width;
            formHeight = this.Size.Height;
        }

        private void toolStripContainer1_ContentPanel_Resize(object sender, EventArgs e) {
            resize();
        }
    }
}