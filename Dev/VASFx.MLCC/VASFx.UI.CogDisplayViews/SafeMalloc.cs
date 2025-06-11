using System;
using System.Runtime.InteropServices;

namespace VASFx.UI.CogDisplayViews
{
    /// A wrapper around malloc so that FreeHGlobal() is called
    /// when the object is disposed.
    class SafeMalloc : SafeBuffer
    {

        /// Allocates memory and initialises the SaveBuffer
        ///The number of bytes to allocate
        public SafeMalloc(int size) : base(true)
        {
            this.SetHandle(Marshal.AllocHGlobal(size));
            this.Initialize((ulong)size);
        }

        /// Called when the object is disposed, ferr the
        /// memory via FreeHGlobal().
        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(this.handle);
            return true;
        }

        /// Cast to IntPtr
        public static implicit operator IntPtr(SafeMalloc h)
        {
            return h.handle;
        }
    }
}
