using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseSQL {
    public partial class CreateTableForm : Form {
        public CreateTableForm() {
            InitializeComponent();
        }

        private string[] types = new string[] {
            "INT",
            "FLOAT",
            "CHAR",
            "VARCHAR",
            "BOOLEAN",
            "TEXT",
            "DATE",
            "TIME",
            "DATETIME"
        };

        private List<TextBox> names = new List<TextBox>();
        private List<ComboBox> comboTypes = new List<ComboBox>();
        private List<TextBox> lengths = new List<TextBox>();
        private List<TextBox> defaults = new List<TextBox>();
        private void createColumn() {
            int borderSize = 25;
            int width = (this.Width - borderSize * 4) / 4;

            TextBox input = new TextBox();
            for (int i = 0; i != 4; i++) {
                if (i == 1) {
                    ComboBox combo = new ComboBox();
                    combo.Width = width;
                    combo.Location = new Point(borderSize + i * (width + borderSize / 2), this.Height - borderSize * 2);

                    for (int j = 0; j != types.Length; j++) {
                        combo.Items.Add(types[j]);
                    }
                    combo.SelectedIndex = 0;

                    this.Controls.Add(combo);
                    this.comboTypes.Add(combo);
                } else {
                    input = new TextBox();
                    input.Width = width;
                    input.Location = new Point(borderSize + i * (width + borderSize / 2), this.Height - borderSize * 2);

                    this.Controls.Add(input);

                    switch (i) {
                        case 0: names.Add(input); break;
                        case 2: lengths.Add(input); break;
                        case 3: defaults.Add(input); break;
                    }
                }
            }

            this.Height += input.Height + borderSize / 2;
        }

        private string[] baseNames;
        private void CreateTableForm_Load(object sender, EventArgs e) {
            baseNames = DataBase.GetDataBases();

            for (int i = 0; i != baseNames.Length; i++) {
                comboBoxBaseName.Items.Add(baseNames[i]);
            }
            comboBoxBaseName.SelectedIndex = 0;

            createColumn();
        }

        private void buttonAddColumn_Click(object sender, EventArgs e) {
            createColumn();
        }

        private void buttonCreate_Click(object sender, EventArgs e) {
            Column[] cols = new Column[names.Count];
            for (int i = 0; i != names.Count; i++) {
                if (lengths[i].Text == "") {
                    lengths[i].Text = "8";
                }
                
                cols[i] = new Column(names[i].Text, comboTypes[i].SelectedIndex.ToEnum<ColumnType>(), Convert.ToInt32(lengths[i].Text), defaults[i].Text);
            }

            DataBase.Use(baseNames[comboBoxBaseName.SelectedIndex]);
            DataBase.CreateTable(textBoxTableName.Text, cols);

            Form1 main = this.Owner as Form1;
            if (main != null) {
                main.DataBaseTree_Reload();
            }

            this.Close();
        }
    }
}
