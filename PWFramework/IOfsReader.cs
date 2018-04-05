using System;
using System.Collections.Generic;
using System.Text;

namespace PWFramework
{
    public interface IOfsReader
    {
        Dictionary<String, Int32> SetOfs(String path);
    }
}
