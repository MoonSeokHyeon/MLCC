using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VASFx.MLCC.Common.Interface
{
    public interface IWindowViewModel
    {
        IWindowView View { get; set; }

        void Init();
        void Query();
        void Subscribe();
        void Unsubscribe();
        void Clear();
    }
}
