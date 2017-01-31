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
        private int rows;
        private int offset;

        public void Limit(List<string[]> data) {
            if (this.offset != 0) {
                int offset = this.offset;
                while (offset != 0) {
                    data.RemoveAt(0);
                    offset--;
                    if (data.Count == 0) {
                        return;
                    }
                }
            }
            if (this.rows < data.Count) {
                while (this.rows != data.Count) {
                    data.RemoveAt(data.Count - 1);
                }
            }
        }
        public RequestLimit(int rows) {
            this.offset = 0;
            this.rows = rows;
        }
        public RequestLimit(int offset, int rows) {
            this.offset = offset;
            this.rows = rows;
        }
    }
    class RequestOrder {
        int type;
        string col;

        private bool checkMax(string v1, string v2, int type) {
            switch (type) {
                case 0: // int
                    return Convert.ToInt32(v1) > Convert.ToInt32(v2);
                case 2: // char
                    return (int)v1[0] > (int)v2[0];
                /*case 1: // float
                    return col.getLength() + 8;
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
                    return 44;*/
                default:
                    return false;
            }
        }
        public void Sort(List<string[]> data, Column[] cols) {
            int colNum = -1, colType = 0;
            int i;
            for (i = 0; i != cols.Length; i++) {
                if (cols[i].getName() == this.col) {
                    colNum = i;
                    colType = cols[i].getType();
                    break;
                }
            }
            if (colNum == -1) {
                return;
            }

            bool ret = false, max;
            string[] buf;
            while(!ret) {
                ret = true;
                for (i = 0; i != data.Count - 1; i++) {
                    max = checkMax(data[i][colNum], data[i + 1][colNum], colType);
                    if ((this.type == 1 && max) || (this.type == 0 && !max)) {
                        buf = data[i + 1];
                        data[i + 1] = data[i];
                        data[i] = buf;
                        ret = false;
                    }
                }
            }
        }
        public RequestOrder(string column, string type = "ASC") {
            this.col = column;
            this.type = type == "ASC" ? 1 : 0;
        }
    }
    class RequestWhere {
        class WhereLevelOne {
            string name1;
            string name2;
            int oper = -1;
            /*
             *  0 =
             *  1 !=
             *  2 <
             *  3 >
             *  4 <=
             *  5 >=
             */
            private string getValue(string value, bool[] levels, string[] row, Column[] cols) {
                if (value[0] >= '0' && value[0] <= '9') {
                    return value;
                } else if (value[0] == '\'') {
                    return value.Substring(1, value.Length - 2);
                } else {
                    for (int i = 0; i != cols.Length; i++) {
                        if (cols[i].getName() == value) {
                            return row[i];
                        }
                    }
                }

                return "";
            }
            public bool Parse(bool[] levels, string[] row, Column[] cols) {
                if (this.name1 == "") {
                    return levels[this.oper];
                } else {
                    string n1 = getValue(this.name1, levels, row, cols),
                           n2 = getValue(this.name2, levels, row, cols);

                    switch (this.oper) {
                        case 0: return (n1 == n2);
                        case 1: return (n1 != n2);
                        case 2: return (Convert.ToInt32(n1) < Convert.ToInt32(n2));
                        case 3: return (Convert.ToInt32(n1) > Convert.ToInt32(n2));
                        case 4: return (Convert.ToInt32(n1) <= Convert.ToInt32(n2));
                        case 5: return (Convert.ToInt32(n1) >= Convert.ToInt32(n2));
                    }

                    return false;
                }
            }
            public WhereLevelOne(string str) {
                str = str.Trim();
                if (str[0] == '$') {
                    this.name1 = "";
                    this.oper = Convert.ToInt32(str.Substring(1));
                } else {
                    for (int i = 0; i != str.Length; i++) {
                        switch (str[i]) {
                            case '=': this.oper = 0; break;
                            case '!': this.oper = 1; break;
                            case '<': this.oper = 2; break;
                            case '>': this.oper = 3; break;
                        }
                        if (this.oper != -1) {
                            this.name1 = str.Substring(0, i - 1).Trim();
                            if (str[i + 1] == '=') {
                                this.name2 = str.Substring(i + 2).Trim();
                                switch (this.oper) {
                                    case 2: this.oper = 4; break;
                                    case 3: this.oper = 5; break;
                                }
                            } else {
                                this.name2 = str.Substring(i + 1).Trim();
                            }
                            break;
                        }
                    }
                }
            }
        }
        class WhereLevel {
            private WhereLevelOne[] levelsOne;
            private int[] oper;

            public bool Parse(bool[] levels, string[] row, Column[] cols) {
                bool[] levelsOneValid = new bool[levelsOne.Length];
                bool valid = true;
                for (int i = 0; i != levelsOne.Length; i++) {
                    if (!valid) {
                        if (this.oper[i - 1] == 1) {
                            continue;
                        } else {
                            valid = true;
                        }
                    }
                    
                    levelsOneValid[i] = levelsOne[i].Parse(levels, row, cols);
                    if (!levelsOneValid[i]) {
                        valid = false;
                    }
                    if (valid) {
                        if (levelsOne.Length == i + 1) {
                            return true;
                        } else if (this.oper[i] == 0) {
                            return true;
                        }
                    }
                }

                return false;
            }
            public WhereLevel(string level) {
                int count = 1;
                for (int i = 0; i <= level.Length - 5; i++) {
                    if (level.Substring(i, 4) == " OR ") {
                        i += 3;
                        count++;
                    } else if (level.Substring(i, 5) == " AND ") {
                        i += 4;
                        count++;
                    }
                }

                this.oper = new int[count - 1];
                this.levelsOne = new WhereLevelOne[count];
                int bi = 0, o = 0;
                for (int i = 0; i <= level.Length - 5; i++) {
                    if (level.Substring(i, 4) == " OR ") {
                        this.oper[o] = 0;
                        this.levelsOne[o++] = new WhereLevelOne(level.Substring(bi, i - bi));
                        i += 4;
                        bi = i;
                    } else if (level.Substring(i, 5) == " AND ") {
                        this.oper[o] = 1;
                        this.levelsOne[o++] = new WhereLevelOne(level.Substring(bi, i - bi));
                        i += 5;
                        bi = i;
                    }
                }
                this.levelsOne[o] = new WhereLevelOne(level.Substring(bi, level.Length - bi));
            }
        }
        /*
         * ()
         * OR AND
         * (
         *  0 =
         *  1 !=
         *  2 <
         *  3 >
         *  4 <=
         *  5 >=
         * )
         * IN BETWEEN
         */
        public bool Parse(string[] row, Column[] cols) {
            bool[] levelValid = new bool[levels.Length];

            for (int i = levels.Length - 1; i >= 0; i--) {
                levelValid[i] = levels[i].Parse(levelValid, row, cols);
            }

            return levelValid[0];
        }

        private WhereLevel[] levels;
        public RequestWhere(string where) {
            int count = 1;
            for (int i = 0; i != where.Length; i++) {
                if (where[i] == '(') {
                    count++;
                }
            }
            
            string[] whereAll = new string[count];
            int[] r = new int[count];
            int kr = 0, len = 1;

            whereAll[0] = "";
            r[0] = 0;
            for (int i = 0; i != where.Length; i++) {
                if (where[i] == '(') {
                    r[++kr] = len++;
                    whereAll[r[kr]] = "";
                } else if (where[i] == ')') {
                    whereAll[r[--kr]] += '$' + r[kr + 1].ToString();
                } else {
                    whereAll[r[kr]] += where[i];
                }
            }

            this.levels = new WhereLevel[len];
            for (int i = 0; i != len; i++) {
                this.levels[i] = new WhereLevel(whereAll[i]);
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
        static private string[] rowDecode(string row, Column[] cols) {
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

            return data;
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
        static public List<string[]> Select(string table, RequestWhere where = null, RequestOrder order = null, RequestLimit limit = null) {
            Column[] cols = DataBase.getColumns(table);
            List<string[]> data = new List<string[]>();

            while(!DataBase.sr.EndOfStream) {
                data.Add(DataBase.rowDecode(DataBase.sr.ReadLine(), cols));
            }

            if (where != null && data.Count != 0) {
                for (int i = data.Count - 1; i >= 0; i--) {
                    if (!where.Parse(data[i], cols)) {
                        data.RemoveAt(i);
                    }
                }
            }
            if (order != null && data.Count > 1) {
                order.Sort(data, cols);
            }
            if (limit != null && data.Count != 0) {
                limit.Limit(data);
            }

            DataBase.sr.Close();
            return data;
        }
        static public Column[] GetColumns(string table) {
            Column[] data = DataBase.getColumns(table);
            DataBase.sr.Close();

            return data;
        }
    }
}
