using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Infrastructure.Crawler
{
    public interface IProcessFile
    {
        void ProcessFIle(string filePath);
    }
}
