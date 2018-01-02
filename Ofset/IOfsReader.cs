using System;
using System.Collections.Generic;
using System.Text;

namespace Ofset
{
    public interface IOfsReader
    {
        Dictionary<String, Int32> SetOfs(String path);
    }
}
