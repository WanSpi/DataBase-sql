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
    class RequestLimit {
        private int row;
        private int offset;

        public RequestLimit(int n1, int n2 = 0) { // [offset,] rows
            if (n2 == 0) {
                this.offset = 0;
                this.row = n1;
            } else {
                this.offset = n1;
                this.row = n2;
            }
        }
    }
    class RequestOrder {
        int type;
        string col;

        public RequestOrder(string column, string type = "ASC") {
            this.col = column;
            this.type = type == "ASC" ? 1 : 0;
        }
    }
    class RequestWhere {
        /*
         * ()
         * OR AND
         * = <= >= < > != 
         * IN BETWEEN
         */
        public RequestWhere(string where) {
            int count = 1;
            for (int i = 0; i != where.Length; i++) {
                if (where[i] == '(') {
                    count++;
                }
            }
            
            string[] whereAll = new string[count];

            for (int i = 0; i != whereAll.Length; i++) {
                Console.WriteLine(whereAll[i]);
            }
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
        static public bool Insert(string table, string[] values, Column[] cols = null) {
            if (cols == null) {
                cols = DataBase.GetColumns(table);
            }

            if (values.Length != cols.Length) {
                return false;
            } else if (!DataBase.ExistTables(table)) {
                return false; // DataBase is not found
            }

            StreamWriter sw = new StreamWriter("DataBase\\" + db + "\\" + table + ".table", true);
            sw.WriteLine(
                DataBase.rowEncode(values, cols)
            );

            sw.Close();
            return true;
        }
        static private char[] rowEncode(string[] values, Column[] cols) {
            int bits = 0;
            for (int i = 0; i != cols.Length; i++) {
                bits += getBits(cols[i]);
            }

            char[] data = new char[(int)Math.Ceiling(bits / 8F)];

            int bufInt;
            string stringBuf, stringData = "";
            for (int i = 0; i != values.Length; i++) {
                stringBuf = "";
                bits = getBits(cols[i]);

                switch (cols[i].getType()) {
                    case 0: // int
                    case 5: // text
                        bufInt = Convert.ToInt32(values[i]);
                        for (int j = 0; j != bits; j++) {
                            stringBuf = (bufInt & 1).ToString() + stringBuf;
                            bufInt >>= 1;
                        }
                        break;
                    case 4: // bolean
                        stringBuf = values[i];
                        break;
                }

                stringData += stringBuf;
            }

            while (stringData.Length % 8 != 0) {
                stringData += "0";
            }

            int bitNum = 0, byteNum = 0;
            for (int i = 0; i != stringData.Length; i++) {
                if (stringData[i] == '1') {
                    data[byteNum]++;
                }
                bitNum++;

                if (bitNum == 8) {
                    bitNum = 0;
                    byteNum++;
                } else {
                    data[byteNum] <<= 1;
                }
            } 

            return data;
        }
        static private string bitsToString(string data, Column col) {
            switch (col.getType()) {
                case 0: // int
                case 5: // text
                    int num = 0;

                    for (int i = 0; i != data.Length; i++) {
                        num <<= 1;
                        if (data[i] == '1') {
                            num = num | 1;
                        }
                    }

                    data = num.ToString();
                    break;
            }

            return data;
        }
        static private void rowDecode(string row, Column[] cols) {
            string[] data = new string[cols.Length];

            string rowBit = ""; char al;
            for (int i = 0; i != row.Length; i++) {
                al = row[i];
                for (int j = 0; j != 8; j++) {
                    rowBit += ((int)(al & 128) == 128 ? '1' : '0');
                    al <<= 1;
                }
            }

            int bits = 0, colBits;
            for (int i = 0; i != cols.Length; i++) {
                colBits = DataBase.getBits(cols[i]);

                data[i] = DataBase.bitsToString(
                    rowBit.Substring(bits, colBits),
                    cols[i]
                );
                bits += colBits;
            }
        }

        static private StreamReader sr = null;
        static private Column[] getColumns(string table) {
            if (!DataBase.ExistTables(table)) {
                return null;
            }

            DataBase.sr = new StreamReader("DataBase\\" + db + "\\" + table + ".table");
            string b = DataBase.sr.ReadLine();
            string[] sb = b.Split(new char[] { ':' });

            if (sb[0] == "1") {
                int count = Convert.ToInt32(sb[1]);
                Column[] cols = new Column[count];

                for (int i = 0; i != cols.Length; i++) {
                    sb = DataBase.sr.ReadLine().Split(new char[] { ':' });
                    cols[i] = new Column(sb[0], Convert.ToInt32(sb[1]), Convert.ToInt32(sb[2]), sb[3]);
                }

                return cols;
            } else {
                return null;
            }
        }
        static public string[] Select(string table, RequestWhere where = null, RequestOrder order = null, RequestLimit limit = null) {
            Column[] cols = DataBase.getColumns(table);

            while(!DataBase.sr.EndOfStream) {
                DataBase.rowDecode(DataBase.sr.ReadLine(), cols);
            }

            DataBase.sr.Close();
            string[] data = new string[0];
            return data;
        }
        static public Column[] GetColumns(string table) {
            Column[] data = DataBase.getColumns(table);
            DataBase.sr.Close();

            return data;
        }
    }
}
