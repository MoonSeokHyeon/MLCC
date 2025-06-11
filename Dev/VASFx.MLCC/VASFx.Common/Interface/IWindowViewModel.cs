using VASFx.Common.Shared;

namespace VASFx.Common.Interface
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
