using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDB2 {
    static class ColumnType {
        static public int INT = 0;
        static public int FLOAT = 1;
        static public int CHAR = 2;
        static public int VARCHAR = 3;
        static public int BOOLEAN = 4;
        static public int TEXT = 5;
        static public int DATE = 6;
        static public int TIME = 7;
        static public int DATETIME = 8;
    }
    class Column {
        private string n, dv;
        private int t, l;

        public Column(string name, int type, int length = 8, string defValue = "") {
            n = name;
            t = type;
            l = length;
            dv = defValue;
        }

        public string getName() {
            return n;
        }
        public string getDefValue() {
            return dv;
        }
        public int getType() {
            return t;
        }
        public int getLength() {
            return l;
        }
    }
    static class DataBase {
        static private string db = null;

        static public bool Use(string name) {
            if (Directory.Exists("DataBase\\" + name)) {
                db = name;
                return true;
            } else {
                return false;
            }
        }

        static public string[] GetDataBases() {
            string[] sts = Directory.GetDirectories("DataBase");

            for (int i = 0; i != sts.Length; i++) {
                sts[i] = sts[i].Replace("DataBase\\", "");
            }

            return sts;
        }

        static public bool Create(string name) {
            name = "DataBase/" + name;

            if (!Directory.Exists(name)) {
                Directory.CreateDirectory(name);
                return true;
            } else {
                return false;
            }
        }

        static public bool ExistTables(string table) {
            return File.Exists("DataBase\\" + db + "\\" + table + ".table");
        }

        static public string[] GetTables() {
            string[] sts = Directory.GetFiles("DataBase\\" + db, "*.table");

            for (int i = 0; i != sts.Length; i++) {
                sts[i] = sts[i].Replace("DataBase\\" + db + "\\", "");
                sts[i] = sts[i].Replace(".table", "");
            }

            return sts;
        }

        static public bool CreateTabel(string table, Column[] columns) {
            if (DataBase.ExistTables(table)) {
                return false;
            }

            StreamWriter sw = new StreamWriter("DataBase\\" + db + "\\" + table + ".table", false);
            sw.WriteLine("1:" + columns.Length);
            for (int i = 0; i != columns.Length; i++) {
                sw.WriteLine(
                    columns[i].getName() + ":" +
                    columns[i].getType() + ":" +
                    columns[i].getLength() + ":" +
                    columns[i].getDefValue()
                );
            }
            sw.Close();

            return true;
        }

        static private int getBits(Column col) {
            switch (col.getType()) {
                case 0: // int
                    return col.getLength();
                case 1: // float
                    return col.getLength() + 8;
                case 2: // char
                    return 8;
                case 3: // varchar
                    return col.getLength() * 8;
                case 4: // boolean
                    return 1;
                case 5: // text
                    return 24;
                case 6: // date
                    return 33; // (yyyy-mm-dd) (24, 4, 5)
                case 7: // time
                    return 11; // (hh:mm) (5, 6)
                case 8: // datetime
                    return 44;
                default:
                    return 0;
            }
        }

        static private char[] rowEncode(string[] values, Column[] cols) {
            int bits = 0;
            for (int i = 0; i != cols.Length; i++) {
                bits += getBits(cols[i]);
            }

            char[] data = new char[(int)Math.Ceiling(bits / 8F)];



            return data;
        }
        static private void rowDecode() {

        }

        static public Column[] GetColumns(string table) {
            if (!DataBase.ExistTables(table)) {
                return null;
            }

            StreamReader sr = new StreamReader("DataBase\\" + db + "\\" + table + ".table");
            string b = sr.ReadLine();
            string[] sb = b.Split(new char[] { ':' });

            if (sb[0] == "1") {
                int count = Convert.ToInt32(sb[1]);
                Column[] cols = new Column[count];

                for (int i = 0; i != cols.Length; i++) {
                    sb = sr.ReadLine().Split(new char[] { ':' });
                    cols[i] = new Column(sb[0], Convert.ToInt32(sb[1]), Convert.ToInt32(sb[2]), sb[3]);
                }

                return cols;
            } else {
                return null;
            }
        }
    }
}
