using System;
using System.Collections.Generic;

namespace Ofset
{
    public class OfsPresenter
    {
        private static OfsPresenter instance;
        private static IOfsReader reader;

        public static Int32[] getInstance(String chain)
        {
            if (instance == null)
                throw new Exception("Отсутствуют оффсеты");
            String[] temp = chain.Split('+');
            List<Int32> chainOfs = new List<Int32>();
            foreach(string str in temp)
            {
                    chainOfs.Add(instance.listOfs[str]);
            }
            return chainOfs.ToArray();
        }

        public static void setConfig(String path, Type type)
        {
            if (path.EndsWith(".txt"))
            {
                if (instance == null) instance = new OfsPresenter();
                reader = (IOfsReader)Activator.CreateInstance(type);
                instance.listOfs = reader.SetOfs(path);
            }
        }

        private Dictionary<String, Int32> listOfs { get; set; } = new Dictionary<string, Int32>();
    }
}
