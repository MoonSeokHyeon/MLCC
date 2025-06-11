using System;
using System.Runtime.InteropServices;

namespace GSG.NET.Utils
{
    public static unsafe class Unmanaged
    {
        public static void* New<T>(int elementCount)
            where T : struct
        {
            return Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)) *
                        elementCount).ToPointer();
        }

        public static void* NewAndInit<T>(int elementCount)
            where T : struct
        {
            int newSizeInBytes = Marshal.SizeOf(typeof(T)) * elementCount;
            byte* newArrayPointer =
            (byte*)Marshal.AllocHGlobal(newSizeInBytes).ToPointer();

            for (int i = 0; i < newSizeInBytes; i++)
                *(newArrayPointer + i) = 0;

            return (void*)newArrayPointer;
        }

        public static void Free(void* pointerToUnmanagedMemory)
        {
            Marshal.FreeHGlobal(new IntPtr(pointerToUnmanagedMemory));
        }

        public static void* Resize<T>(void* oldPointer, int newElementCount)
            where T : struct
        {
            return (Marshal.ReAllocHGlobal(new IntPtr(oldPointer),
                new IntPtr(Marshal.SizeOf(typeof(T)) * newElementCount))).ToPointer();
        }


        static unsafe void Copy(byte[] source, int sourceOffset, byte[] target, int targetOffset, int count)
        { 
            // If either array is not instantiated, you cannot complete the copy.
            if ((source == null) || (target == null)) { throw new System.ArgumentException(); } 

            // If either offset, or the number of bytes to copy, is negative, you // cannot complete the copy.
            if ((sourceOffset < 0) || (targetOffset < 0) || (count < 0)) { throw new System.ArgumentException(); }
            
            // If the number of bytes from the offset to the end of the array is 
            // less than the number of bytes you want to copy, you cannot complete // the copy.
            if ((source.Length - sourceOffset < count) || (target.Length - targetOffset < count)) { throw new System.ArgumentException(); } 

            // The following fixed statement pins the location of the source and // target objects in memory so that they will not be moved by garbage // collection.
            fixed (byte* pSource = source, pTarget = target) 
            { 
                // Set the starting points in source and target for the copying.
                byte* ps = pSource + sourceOffset; byte* pt = pTarget + targetOffset; 

                // Copy the specified number of bytes from source to target.
                for (int i = 0; i < count; i++) { *pt = *ps; pt++; ps++; } 
            } 
        }

    }
}
