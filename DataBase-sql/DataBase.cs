using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBaseSQL {
    public enum ColumnType {
        INT = 0,
        FLOAT = 1,
        CHAR = 2,
        VARCHAR = 3,
        BOOLEAN = 4,
        TEXT = 5,
        DATE = 6,
        TIME = 7,
        DATETIME = 8
    }
    public static class Extensions {
        public static T ToEnum<T>(this int v) {
            return (T)Enum.ToObject(typeof(T), v);
        }
        public static T ToEnum<T>(this string v) {
            return (T)Enum.Parse(typeof(T), v, true);
        }
    }
    public class Column {
        private string n, dv;
        private int l;
        private ColumnType t;

        public Column(string name, ColumnType type, int length = 8, string defValue = "") {
            n = name;
            t = type;
            l = length;
            dv = defValue;
        }

        public string Name {
            get {
                return n;
            }
        }
        public string DefValue {
            get {
                return dv;
            }
        }
        public ColumnType Type {
            get {
                return t;
            }
        }
        public int Length {
            get {
                return l;
            }
        }
    }

    public class TimeObject {
        private int h = 0, i = 0, s = 0;
        private static Regex[] formats = new Regex[] {
            new Regex(@"([0-2]?[0-9])\:([0-6]?[0-9])"), // HH:II
            new Regex(@"([0-2]?[0-9])\:([0-6]?[0-9])\:([0-6]?[0-9])") // HH:II:SS
        };

        private void setValue(string value, char f) {
            switch (f) {
                case 'h': h = Convert.ToInt32(value); break;
                case 'i': i = Convert.ToInt32(value); break;
                case 's': s = Convert.ToInt32(value); break;
            }
        }
        public TimeObject(string time, string format = null) {
            if (format == null) {
                Match match = null;

                for (int i = 0; i != formats.Length; i++) {
                    match = formats[i].Match(time);

                    if (match.Success) {

                    }
                }
            } else {
                string buf = time[0].ToString();

                for (int i = 1; i != format.Length; i++) {
                    if (format[i] != format[i - 1]) {
                        setValue(buf, format[i - 1]);
                        buf = "";
                    }
                    buf += time[i];
                }
                setValue(buf, format[format.Length - 1]);
            }
        }

        private string formatValue(int len, int f) {
            string data = f.ToString();

            if (data.Length < len) {
                while (data.Length < len) {
                    data = '0' + data;
                }
            } else if (data.Length > len) {
                data = data.Substring(data.Length - len);
            }

            return data;
        }
        private string formatValue(int len, string f) {
            string data = "";

            for (int i = 0; i != len; i++) {
                data += f;
            }

            return data;
        }
        private string getValue(int len, char f) {
            switch (f) {
                case 'h': return formatValue(len, h);
                case 'i': return formatValue(len, i);
                case 's': return formatValue(len, s);
                default: return formatValue(len, f.ToString());
            }
        }
        public string GetTime(string format = "hh:ii:ss") {
            string date = "";

            int len = 1;
            for (int i = 1; i != format.Length; i++) {
                if (format[i] != format[i - 1]) {
                    date += getValue(len, format[i - 1]);
                    len = 0;
                }
                len++;
            }
            date += getValue(len, format[format.Length - 1]);

            return date;
        }

        public string ToString() {
            return this.GetTime();
        }

        public int Hour {
            get { return h; }
            set {
                h = value;

                if (h > 59) {
                    h = h % 60;
                } else if (h < 0) {

                }
            }
        }
        public int Minute {
            get {
                return i;
            }
        }
        public int Seconds {
            get {
                return s;
            }
        }
    }
    public class DateObject {
        private int d = 0, m = 0, y = 0;
        private static Regex[] formats = new Regex[] {
            new Regex(@"([0-3]?[0-9])\.([0-1]?[0-9])\.([0-9]{4})"), // DD.MM.YYYY
            new Regex(@"([0-3]?[0-9])\-([0-1]?[0-9])\-([0-9]{4})"), // DD-MM-YYYY
            new Regex(@"([0-3]?[0-9])\/([0-1]?[0-9])\/([0-9]{4})"), // DD/MM/YYYY
            new Regex(@"([0-1]?[0-9])\-([0-3]?[0-9])\-([0-9]{4})"), // MM-DD-YYYY
            new Regex(@"([0-9]{4})\-([0-1]?[0-9])\-([0-3]?[0-9])") // YYYY-MM-DD
        };

        private void setValue(string value, char f) {
            switch (f) {
                case 'd': d = Convert.ToInt32(value); break;
                case 'm': m = Convert.ToInt32(value); break;
                case 'y': y = Convert.ToInt32(value); break;
            }
        }
        public DateObject(string date, string format = null) {
            if (format == null) {
                Match match = null;

                for (int i = 0; i != formats.Length; i++) {
                    match = formats[i].Match(date);

                    if (match.Success) {

                    }
                }
            } else {
                string buf = date[0].ToString();

                for (int i = 1; i != format.Length; i++) {
                    if (format[i] != format[i - 1]) {
                        setValue(buf, format[i - 1]);
                        buf = "";
                    }
                    buf += date[i];
                }
                setValue(buf, format[format.Length - 1]);
            }
        }

        private string formatValue(int len, int f) {
            string data = f.ToString();

            if (data.Length < len) {
                while (data.Length < len) {
                    data = '0' + data;
                }
            } else if (data.Length > len) {
                data = data.Substring(data.Length - len);
            }

            return data;
        }
        private string formatValue(int len, string f) {
            string data = "";

            for (int i = 0; i != len; i++) {
                data += f;
            }

            return data;
        }
        private string getValue(int len, char f) {
            switch (f) {
                case 'd': return formatValue(len, d);
                case 'm': return formatValue(len, m);
                case 'y': return formatValue(len, y);
                default: return formatValue(len, f.ToString());
            }
        }
        public string GetDate(string format = "dd.mm.yyyy") {
            string date = "";

            int len = 1;
            for (int i = 1; i != format.Length; i++) {
                if (format[i] != format[i - 1]) {
                    date += getValue(len, format[i - 1]);
                    len = 0;
                }
                len++;
            }
            date += getValue(len, format[format.Length - 1]);

            return date;
        }

        public string ToString() {
            return GetDate();
        }

        public int Day {
            get {
                return d;
            }
        }
        public int Month {
            get {
                return m;
            }
        }
        public int Year {
            get {
                return y;
            }
        }
    }
    public class DateTimeObject {
        private DateObject date = null;
        private TimeObject time = null;

        public DateTimeObject(string dateTime, string format = null) {
            string dateFormat = null;
            string timeFormat = null;

            string dateString = "1900-01-01";
            string timeString = "12:00:00";

            string[] dateTimeSplit = dateTime.Split(' ');
            string[] formatSplit = format.Split(' ');

            dateString = dateTimeSplit[0];
            if (dateTimeSplit.Length != 1) {
                timeString = dateTimeSplit[1];
            }

            if (format != null) {
                dateFormat = formatSplit[0];
                if (formatSplit.Length != 1) {
                    timeFormat = formatSplit[1];
                }
            }

            date = new DateObject(dateString, dateFormat);
            time = new TimeObject(timeString, timeFormat);
        }
        /*
        public string GetTime() {
        }
        public string GetDate() {
        }
        public string GetDateTime() {
        }*/
       /* public string ToString() {
            return GetDateTime();
        }*/

        public int Day {
            get {
                return this.date.Day;
            }
        }
        public int Month {
            get {
                return this.date.Month;
            }
        }
        public int Year {
            get {
                return this.date.Year;
            }
        }
        public int Hour {
            get {
                return this.time.Hour;
            }
        }
        public int Minute {
            get {
                return this.time.Minute;
            }
        }
        public int Seconds {
            get {
                return this.time.Seconds;
            }
        }
    }

    public class RequestLimit {
        private int rows;
        private int offset;

        public void Limit(List<ResponseRow> data) {
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
    public class RequestOrder {
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
        public void Sort(List<ResponseRow> data, Column[] cols) {
            bool ret = false, max;
            ResponseRow buf;
            while(!ret) {
                ret = true;
                for (int i = 0; i != data.Count - 1; i++) {
                    max = checkMax(data[i].GetValue(col), data[i + 1].GetValue(col), 0);
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
    public class RequestWhere {
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
                        if (cols[i].Name == value) {
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
    public class ResponseRow {
        private string table = null;
        private int identificator = 0;
        private Column[] cols = null;
        private string[] res = null;

        private Dictionary<string, ResponseObject> JoinTable = null;

        private int getIndex(string name) {
            for (int i = 0; i != cols.Length; i++) {
                if (cols[i].Name == name) {
                    return i;
                }
            }

            return -1;
        }

        public int GetValueInt(string name) {
            string value = GetValue(name);

            return Convert.ToInt32(value);
        }
        public string GetValue(string name) {
            int ind = getIndex(name);

            if (ind == -1) {
                return "";
            }

            return this.res[ind];
        }
        public bool SetValue(string name, string value) {
            int ind = getIndex(name);

            if (ind == -1) {
                return false;
            }

            res[ind] = value;
            return true;
        }

        public int GetIdentificator() {
            return this.identificator;
        }
        public void SetIdentificator(int i) {
            this.identificator = i;
        }

        public void SetTable(string table) {
            this.table = table;
        }

        public bool Save() {
            if (this.table == null) {
                return false;
            }

            if (this.identificator == 0) {
                return DataBase.Insert(this.table, this.ToString(), this.cols);
            } else {
                return DataBase.Update(this.table, this);
            }
        }

        public string[] ToString() {
            return res;
        }

        public ResponseObject GetChild(string name) {
            if (JoinTable == null) {
                return null;
            }

            if (!JoinTable.ContainsKey(name)) {
                return null;
            }

            return JoinTable[name];
        }
        public void SetChild(string name, ResponseObject response) {
            if (JoinTable == null) {
                JoinTable = new Dictionary<string, ResponseObject>();
            }

            JoinTable[name] = response;
        }

        public ResponseRow(string table, string[] res = null) {
            this.table = table;
            this.cols = DataBase.GetColumns(table);

            if (res == null) {
                res = new string[this.cols.Length];

                for (int i = 0; i != res.Length; i++) {
                    res[i] = "";
                }

                this.res = res;
            } else {
                this.res = res;
            }
        }

        public ResponseRow(Column[] cols, string[] res) {
            this.cols = cols;
            this.res = res;
        }
    }
    public class ResponseObject {
        private bool first = true;
        private int index = 0;

        private Column[] cols = null;
        private List<ResponseRow> list = null;

        public int Count = 0;

        public string GetValue(string name) {
            return this.list[this.index].GetValue(name);
        }
        public int GetValueInt(string name) {
            return this.list[this.index].GetValueInt(name);
        }

        public bool NextIndex() {
            if (first && list.Count != 0) {
                first = false;
                return true;
            }

            if (this.index + 1 < list.Count) {
                this.index++;
                return true;
            }

            if (this.index + 1 >= list.Count) {
                first = true;
                this.index = 0;
            }

            return false;
        }
        public bool SetIndex(int index) {
            if (-1 < index && index < list.Count) {
                this.index = index;
                return true;
            }

            return false;
        }
        public int GetIndex() {
            return this.index;
        }

        public ResponseRow GetRow() {
            return list[index];
        }
        public ResponseRow GetRow(int index) {
            if (-1 < index && index < list.Count) {
                return list[index];
            }

            return null;
        }
        public void AddRow(ResponseRow row) {
            list.Add(row);
            this.Count++;
        }

        public Column[] GetColumns() {
            return cols;
        }

        public ResponseObject(Column[] cols, List<ResponseRow> list) {
            this.cols = cols;
            this.list = list;

            this.Count = list.Count;
        }

        public ResponseObject GetRowChildren(string name) {
            return list[index].GetChild(name);
        }

        private void setResponseObject(ResponseObject res, string table, string thisColumn, string tableColumn) {
            string val = null;
            ResponseObject resNew = null;
            for (int i = 0; i != list.Count; i++) {
                val = list[i].GetValue(thisColumn);

                if (val == "") {
                    continue;
                }

                resNew = new ResponseObject(res.GetColumns(), new List<ResponseRow>());
                for (int j = 0; j != res.Count; j++) {
                    res.SetIndex(j);

                    if (val == res.GetValue(tableColumn)) {
                        resNew.AddRow(res.GetRow());
                    }
                }

                list[i].SetChild(table, resNew);
            }
        }
        public bool Join(string table, string thisColumn, string tableColumn) {
            List<string> values = new List<string>();

            string val = null;
            for (int i = 0; i != list.Count; i++) {
                val = list[i].GetValue(thisColumn);

                if (val == "") {
                    continue;
                }

                if (!values.Contains(val)) {
                    values.Add(val);
                }
            }

            string where = "";
            for (int i = 0; i != values.Count; i++) {
                where += tableColumn + " = '" + values[i] + "'" + (i != values.Count - 1 ? " OR " : "");
            }

            ResponseObject res = DataBase.Select(table, new RequestWhere(where));
            if (res == null) {
                return false;
            }

            setResponseObject(res, table, thisColumn, tableColumn);
            return true;
        }
    }
    public static class DataBase {
        static private string version = "0.2";
        static private string db = null;

        static public string DataBaseName {
            get {
                return db;
            }
        }
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

        static public bool ExistTable(string table) {
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
        static public bool RemoveTable(string table) {
            if (DataBase.ExistTable(table)) {
                File.Delete(DataBase.getPath(table));

                return true;
            } else {
                return false;
            }
        }

        static private void createTable(string table, Column[] columns) {
            DataBase.sw = new StreamWriter(DataBase.getPath(table), false);
            DataBase.sw.WriteLine(version + ":" + columns.Length);
            for (int i = 0; i != columns.Length; i++) {
                DataBase.sw.WriteLine(
                    columns[i].Name + ":" +
                    (int)columns[i].Type + ":" +
                    columns[i].Length + ":" +
                    columns[i].DefValue
                );
            }
        }
        static public bool CreateTable(string table, Column[] columns) {
            if (DataBase.ExistTable(table)) {
                return false;
            }

            DataBase.createTable(table, columns);
            DataBase.sw.Close();

            return true;
        }
        static public bool ChangeColumns(string table, Column[] columns) {
            if (!DataBase.ExistTable(table)) {
                return false;
            }

            ResponseObject res = DataBase.Select(table);
            DataBase.RemoveTable(table);
            DataBase.CreateTable(table, columns);

            if (res != null) {
                while (res.NextIndex()) {
                    DataBase.Insert(table, res.GetRow().ToString(), columns);
                }
            }

            return true;
        }

        private static int getBits(Column col) {
            switch (col.Type) {
                case ColumnType.INT:
                    return col.Length;
                case ColumnType.FLOAT:
                    return 32;
                case ColumnType.CHAR:
                    return 8;
                case ColumnType.VARCHAR:
                    return col.Length * 8;
                case ColumnType.BOOLEAN:
                    return 1;
                case ColumnType.TEXT:
                    return 24;
                case ColumnType.DATE:
                    return 33; // (yyyy-mm-dd) (24, 4, 5)
                case ColumnType.TIME:
                    return 11; // (hh:mm) (5, 6)
                case ColumnType.DATETIME: // (yyyy-mm-dd hh:mm) (24, 4, 5, 5, 6)
                    return 44;
                default:
                    return 0;
            }
        }
        public static bool Insert(string table, string[] values, Column[] cols = null) {
            if (cols == null) {
                cols = DataBase.GetColumns(table);
            }

            if (values.Length != cols.Length) {
                return false;
            } else if (!DataBase.ExistTable(table)) {
                return false; // DataBase is not found
            }

            for (int i = 0; i != values.Length; i++) {
                if (values[i] == "" || values[i] == null) {
                    values[i] = cols[i].DefValue;
                }
            }

            StreamWriter sw = new StreamWriter(DataBase.getPath(table), true);
            sw.Write(
                DataBase.rowEncode(values, cols)
            );

            sw.Close();
            return true;
        }
        static private string encodeInteger(int integer, int bits) {
            string data = "";

            for (int j = 0; j != bits; j++) {
                data = (integer & 1).ToString() + data;
                integer >>= 1;
            }

            return data;
        }
        static private string encodeInteger(string integer, int bits) {
            return encodeInteger(Convert.ToInt32(integer), bits);
        }
        static private char[] rowEncode(string[] values, Column[] cols) {
            int bits = 0;
            for (int i = 0; i != cols.Length; i++) {
                bits += getBits(cols[i]);
            }

            char[] data = new char[(int)Math.Ceiling(bits / 8F)];

            string stringBuf, stringData = "";
            for (int i = 0; i != values.Length; i++) {
                stringBuf = "";
                bits = getBits(cols[i]);

                switch (cols[i].Type) {
                    case ColumnType.INT:
                        int bufInt = values[i].Length == 0 ? 0 : Convert.ToInt32(values[i]);
                        if (bufInt < 0) {
                            stringBuf = "1";
                            bufInt = -bufInt;
                        } else {
                            stringBuf = "0";
                        }
                        stringBuf += encodeInteger(bufInt, bits - 1);
                        break;
                    case ColumnType.FLOAT:
                        values[i] = values[i].Replace(',', '.');
                        float bufFloat = Convert.ToSingle(values[i], System.Globalization.CultureInfo.InvariantCulture);
                        
                        byte[] bufByte = BitConverter.GetBytes(bufFloat);

                        for (int j = 0; j != bufByte.Length; j++) {
                            stringBuf += encodeInteger((int)bufByte[j], 8);
                        }

                        break;
                    case ColumnType.TEXT:
                        stringBuf = encodeInteger(values[i], bits);
                        break;
                    case ColumnType.BOOLEAN:
                        stringBuf = values[i];
                        break;
                    case ColumnType.CHAR:
                    case ColumnType.VARCHAR:
                        stringBuf = "";
                        for (int j = 0; j != values[i].Length; j++) {
                            stringBuf += encodeInteger(((int)values[i][j]).ToString(), 8);
                        }
                        if (stringBuf.Length < bits) {
                            while (stringBuf.Length < bits) {
                                stringBuf = '0' + stringBuf;
                            }
                        } else if (stringBuf.Length > bits) {
                            stringBuf = stringBuf.Substring(stringBuf.Length - bits);
                        }
                        break;
                    case ColumnType.TIME: // (11) (hh:ii) (5, 6)
                        TimeObject time = new TimeObject(values[i], "hh:ii");
                        stringBuf =
                            DataBase.encodeInteger(time.Hour, 5) +
                            DataBase.encodeInteger(time.Minute, 6);
                        break;
                    case ColumnType.DATE: // (33) (dd.mm.yyyy) (5, 4, 24)
                        DateObject date = new DateObject(values[i]);
                        stringBuf =
                            DataBase.encodeInteger(date.Day, 5) +
                            DataBase.encodeInteger(date.Month, 4) +
                            DataBase.encodeInteger(date.Year, 24);
                        break;
                    case ColumnType.DATETIME:  // (yyyy-mm-dd hh:mm) (24, 4, 5, 5, 6)
                        DateTimeObject dateTime = new DateTimeObject(values[i]);
                        stringBuf =
                            DataBase.encodeInteger(dateTime.Day, 5) +
                            DataBase.encodeInteger(dateTime.Month, 4) +
                            DataBase.encodeInteger(dateTime.Year, 24) +
                            DataBase.encodeInteger(dateTime.Hour, 5) +
                            DataBase.encodeInteger(dateTime.Minute, 6);
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
        static private string fixStringNumber(string num, int len) {
            if (num.Length > len) {
                num = num.Substring(num.Length - len);
            } else if (num.Length < len) {
                while (num.Length < len) {
                    num = '0' + num;
                }
            }

            return num;
        }
        static private int bitsToInteger(string data) {
            int num = 0;

            for (int i = 0; i != data.Length; i++) {
                num <<= 1;
                if (data[i] == '1') {
                    num = num | 1;
                }
            }

            return num;
        }
        static private string bitsToString(string data, Column col) {
            int bufInt;
            DateObject date;
            switch (col.Type) {
                case ColumnType.INT:
                    char op = data[0];
                    bufInt = bitsToInteger(data.Substring(1));

                    if (op == '1') {
                        bufInt = -bufInt;
                    }

                    data = bufInt.ToString();
                    break;
                case ColumnType.FLOAT:
                    byte[] bufByte = new byte[4];
                    for (int i = 0; i != bufByte.Length; i++) {
                        bufByte[i] = (byte)bitsToInteger(data.Substring(i * 8, 8));
                    }

                    float bufFloat = BitConverter.ToSingle(bufByte, 0);
                    data = bufFloat.ToString();

                    break;
                case ColumnType.TEXT:
                    data = bitsToInteger(data).ToString();
                    break;
                case ColumnType.CHAR:
                case ColumnType.VARCHAR:
                    int bytes = data.Length / 8;
                    string varchar = "";
                    for (int i = bytes - 1; i != -1; i--) {
                        bufInt = 0;

                        for (int j = 0; j != 8; j++) {
                            bufInt <<= 1;

                            if (data[i * 8 + j] == '1') {
                                bufInt = bufInt | 1;
                            }
                        }

                        if (bufInt == 0) {
                            break;
                        } else {
                            varchar = (char)bufInt + varchar;
                        }
                    }
                    
                    data = varchar;
                    break;
                case ColumnType.TIME: // (11) (hh:ii) (5, 6)
                    date = new DateObject(
                        fixStringNumber(bitsToInteger(data.Substring(0, 5)).ToString(), 2) +
                        fixStringNumber(bitsToInteger(data.Substring(5, 6)).ToString(), 2),
                        "hhii"
                    );
                    data = date.GetDate("hh:ii");
                    break;
                case ColumnType.DATE: // (33) (dd.mm.yyyy) (5, 4, 24)
                    date = new DateObject(
                        fixStringNumber(bitsToInteger(data.Substring(0, 5)).ToString(), 2) +
                        fixStringNumber(bitsToInteger(data.Substring(5, 4)).ToString(), 2) +
                        fixStringNumber(bitsToInteger(data.Substring(9, 24)).ToString(), 4),
                        "ddmmyyyy"
                    );
                    data = date.GetDate();
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

        static private StreamWriter sw = null;
        static private StreamReader sr = null;
        static private string getPath(string table) {
            return "DataBase\\" + db + "\\" + table + ".table";
        }
        static private Column[] getColumns(string table) {
            if (!DataBase.ExistTable(table)) {
                return null;
            }

            DataBase.sr = new StreamReader(DataBase.getPath(table));
            string b = DataBase.sr.ReadLine();
            string[] sb = b.Split(new char[] { ':' });

            if (sb[0] == DataBase.version) {
                int count = Convert.ToInt32(sb[1]);
                Column[] cols = new Column[count];

                for (int i = 0; i != cols.Length; i++) {
                    sb = DataBase.sr.ReadLine().Split(new char[] { ':' });
                    cols[i] = new Column(sb[0], sb[1].ToEnum<ColumnType>(), Convert.ToInt32(sb[2]), sb[3]);
                }

                return cols;
            } else {
                return null;
            }
        }
        private static List<string> breakRows(string date, int bytes) {
            List<string> rows = new List<string>();

            string buf = "";
            for (int i = 0; i != date.Length; i++) {
                if (i % bytes == 0 && i != 0) {
                    rows.Add(buf);
                    buf = "";
                }

                buf += date[i];
            }
            rows.Add(buf);

            return rows;
        }
        static private List<ResponseRow> select(string table, Column[] cols, RequestWhere where = null, RequestOrder order = null, RequestLimit limit = null) {
            List<ResponseRow> data = new List<ResponseRow>();

            int id = 1;
            ResponseRow buf;
            string dataLine = DataBase.sr.ReadToEnd();

            if (dataLine == "" || dataLine == null) {
                DataBase.sr.Close();
                return data;
            }

            int bits = 0;
            for (int i = 0; i != cols.Length; i++) {
                bits += DataBase.getBits(cols[i]);
            }
            bits += bits % 8;
            List<string> l = breakRows(dataLine, bits / 8);

            for (int i = 0; i != l.Count; i++) {
                buf = new ResponseRow(cols, DataBase.rowDecode(l[i], cols));
                buf.SetIdentificator(id++);
                buf.SetTable(table);
                data.Add(buf);
            }

            if (where != null && data.Count != 0) {
                for (int i = data.Count - 1; i >= 0; i--) {
                    if (!where.Parse(data[i].ToString(), cols)) {
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
        static public ResponseObject Select(string table, RequestWhere where = null, RequestOrder order = null, RequestLimit limit = null) {
            Column[] cols = DataBase.getColumns(table);
            List<ResponseRow> list = DataBase.select(table, cols, where, order, limit);

            if (list.Count == 0) {
                return null;
            }

            return new ResponseObject(cols, list);
        }
        static private void update(string[] oldValues, string[,] newValues, Column[] cols) {
            for (int i = 0; i != cols.Length; i++) {
                for (int j = 0; j != newValues.GetLength(0); j++) {
                    if (cols[i].Name == newValues[j, 0]) {
                        oldValues[i] = newValues[j, 1];
                        break;
                    }
                }
            }
        }
        static public int Remove(string table, RequestWhere where = null) {
            Column[] cols = DataBase.getColumns(table);

            if (cols != null) {
                List<ResponseRow> list = DataBase.select(table, cols);

                if (DataBase.RemoveTable(table)) {
                    DataBase.createTable(table, cols);

                    if (where == null) {
                        DataBase.sw.Close();
                        return list.Count;
                    } else {
                        int updCount = 0;

                        for (int i = 0; i != list.Count; i++) {
                            if (!where.Parse(list[i].ToString(), cols)) {
                                DataBase.sw.Write(
                                    DataBase.rowEncode(list[i].ToString(), cols)
                                );
                                updCount++;
                            }
                        }

                        DataBase.sw.Close();
                        return updCount;
                    }
                } else {
                    return -1;
                }
            } else {
                return -1;
            }
        }
        public static bool Update(string table, ResponseRow res) {
            Column[] cols = DataBase.getColumns(table);

            if (cols != null) {
                List<ResponseRow> list = DataBase.select(table, cols);

                if (DataBase.RemoveTable(table)) {
                    DataBase.createTable(table, cols);

                    for (int i = 0; i != list.Count; i++) {
                        if (i + 1 == res.GetIdentificator()) {
                            DataBase.sw.Write(
                                DataBase.rowEncode(res.ToString(), cols)
                            );
                        } else {
                            DataBase.sw.Write(
                                DataBase.rowEncode(list[i].ToString(), cols)
                            );
                        }
                    }

                    DataBase.sw.Close();
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }
        public static int Update(string table, string[,] upd, RequestWhere where = null) {
            Column[] cols = DataBase.getColumns(table);

            if (cols != null) {
                List<ResponseRow> list = DataBase.select(table, cols);

                if (DataBase.RemoveTable(table)) {
                    int updCount = 0;
                    DataBase.createTable(table, cols);

                    for (int i = 0; i != list.Count; i++) {
                        if (where == null || where.Parse(list[i].ToString(), cols)) {
                            DataBase.update(list[i].ToString(), upd, cols);
                            updCount++;
                        }

                        DataBase.sw.Write(
                            DataBase.rowEncode(list[i].ToString(), cols)
                        );
                    }

                    DataBase.sw.Close();
                    return updCount;
                } else {
                    return -1;
                }
            } else {
                return -1;
            }
        }
        static public Column[] GetColumns(string table) {
            Column[] data = DataBase.getColumns(table);
            DataBase.sr.Close();

            return data;
        }
    }
}