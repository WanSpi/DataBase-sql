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
    public partial class CreateDataBaseForm : Form {
        public CreateDataBaseForm() {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e) {
            DataBase.Create(textName.Text);
            
            MainForm main = this.Owner as MainForm;
            if (main != null) {
                main.DataBaseTree_Reload();
            }

            this.Close();
        }
    }
}
