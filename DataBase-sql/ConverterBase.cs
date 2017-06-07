using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseSQL {
    public static class ConverterBase {
        private static string VersionUp01(string path) {
            StreamReader sr = new StreamReader(path);
            string b = sr.ReadLine();
            string[] sb = b.Split(new char[] { ':' });

            if (sb[0] != "0.1") {
                return sb[0];
            }

            sb[0] = "0.2";
            int count = Convert.ToInt32(sb[1]);

            StreamWriter sw = new StreamWriter("tmp");
            sw.WriteLine(string.Join(":", sb));

            while ((count--) != 0) {
                sw.WriteLine(sr.ReadLine());
            }

            while(!sr.EndOfStream) {
                sw.Write(sr.ReadLine());
            }

            sw.Close();
            sr.Close();

            File.Delete(path);
            File.Move("tmp", path);

            return "0.2";
        }

        public static void Converter(string path, string oldVersion, string newVersion) {
            while (true) {
                switch (oldVersion) {
                    case "0.1":
                        VersionUp01(path);
                        break;
                    default:
                        return;
                }

                if (oldVersion == newVersion) {
                    return;
                }
            }
        }
    }
}
