using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Core
{
    public interface ICore
    {
        void Init();
        void InitPLC(string filePath);
    }
}
