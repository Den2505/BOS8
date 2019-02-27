using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BOS8
{
    public static class IniLoader
    {
        private static String pathToIni;

    
        public static Dictionary<String, Dictionary<String, String>> readFile(String path,  bool local = false)
        {
            if (!local) { pathToIni = path;}
            
            Dictionary<String, Dictionary<String, String>> paramsBlock = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<String, String> map = null;
            StreamReader reader = new StreamReader(new FileStream(path, FileMode.OpenOrCreate));
            String blockName = null;
            int i = 0;

            do
            {

                i++;
                String str = reader.ReadLine().Trim(new char[] { ' ' });
                String[] splitedStr;
                if (reader.EndOfStream)
                {
                    paramsBlock.Add(blockName, map);
                }

                if (str.Length == 0) { continue; }
                if (str.Contains('['))
                {
                    if (map == null)
                    {
                        blockName = str;
                        map = new Dictionary<String, String>();
                    }
                    else
                    {

                        paramsBlock.Add(blockName, map);
                        blockName = str;
                        map = new Dictionary<String, String>();

                    }
                    continue;
                }
                else if (str.Contains('#')) { continue; }
                if (str.IndexOf('=') != -1)
                {
                    splitedStr = str.Split(new char[] { '=' });
                   // foreach (String part in splitedStr) { part.Trim(new char[] {' '});}
                    if (splitedStr.Length != 2) { throw new IndexOutOfRangeException(); }
                    try { map.Add(splitedStr[0], splitedStr[1]); }
                    catch (ArgumentException e) { map.Add("#" + splitedStr[0], splitedStr[1]); continue; }

                }
                else
                {
                    try
                    { map.Add(str, null); }
                    catch (ArgumentException e) { map.Add("#" + str, null); continue; }
                }
            }
            while (!reader.EndOfStream);

            reader.Close();
            return paramsBlock;
            // Console.WriteLine(this.paramsBlock);
            //    saveFile(this.paramsBlock);

        }
        public static void saveFile(Dictionary<String, Dictionary<String, String>> paramsBlock)
        {
            File.Copy(pathToIni, pathToIni + ".back");
            StreamWriter writer = new StreamWriter(new FileStream(pathToIni, FileMode.Truncate));
            foreach (String keyBlock in paramsBlock.Keys)
            {
                List<String> keys = paramsBlock[keyBlock].Keys.ToList();
                List<String> values = paramsBlock[keyBlock].Values.ToList();
                writer.WriteLine(keyBlock);
                for (int i = 0; i < paramsBlock[keyBlock].Count; i++)
                {
                    String line;
                    if (values[i] != null)
                    {
                        line = keys[i] + "=" + values[i];
                    }
                    else
                    {
                        line = keys[i];
                    }
                    writer.WriteLine(line);
                }
            }

            writer.Close();
        }
    }
}
