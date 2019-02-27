using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOS8
{
    public class IniController
    {
        public Dictionary<String, Dictionary<String, String>> LoadIni(String path)
        {
            Dictionary<String, Dictionary<String, String>> test = IniLoader.readFile(path);
            return test;

        }

        public Dictionary<String, Dictionary<String, String>> LoadLocalConfiguration(String path)
        {
            Dictionary<String, Dictionary<String, String>> local = IniLoader.readFile(path, true);
            return local;
        }

        public Dictionary<String, Dictionary<String, String>> SetUpdedParams(
            Dictionary<String, Dictionary<String, String>> newParams, Dictionary<String, Dictionary<String, String>> oldParams)
        {
            foreach (String oldBlockName in oldParams.Keys)
            {
                foreach (String newBlockName in newParams.Keys)
                {
                    if (oldBlockName.Equals(newBlockName))
                    {

                        foreach (String newParamName in newParams[newBlockName].Keys)
                        {
                            if (oldParams[oldBlockName].ContainsKey(newParamName))
                            {
                                oldParams[oldBlockName][newParamName] = newParams[newBlockName][newParamName];
                            }
                            else
                            {
                                oldParams[oldBlockName].Add(newParamName, newParams[newBlockName][newParamName]);
                            }

                        }
                        break;
                    }
                }
            }
            return oldParams;
        }



    }
}
