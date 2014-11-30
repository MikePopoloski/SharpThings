using System;
using System.Runtime.InteropServices;

namespace SharpThings {
    unsafe struct AlignedArray : IDisposable {
        void* memory;
        byte* ptr;

        public byte* Pointer => ptr;

        public int Length {
            get;
            private set;
        }

        public byte* this[int index] {
            get { return Pointer + index; }
        }

        public AlignedArray (int size) {
            Length = size;
            memory = Alloc(size);

            // make sure we get a 16-byte aligned pointer
            ptr = (byte*)(((long)memory + 15) & ~0x0F);
        }

        public void GrowToCount (int newCount) {
            if (newCount <= Length)
                return;

            var newMemory = Alloc(newCount);
            byte* newPtr;

            try {
                // make sure we get a 16-byte aligned pointer
                newPtr = (byte*)(((long)newMemory + 15) & ~0x0F);
                int byteCount = Length;
                for (int i = 0; i < byteCount; i++)
                    newPtr[i] = ptr[i];

                Free(memory);
            }
            catch {
                Free(newMemory);
                throw;
            }

            memory = newMemory;
            ptr = newPtr;
            Length = newCount;
        }

        public void Fill (byte value) {
            var end = ptr + Length;
            for (var b = ptr; b != end; b++)
                *b = value;
        }

        public void Dispose () {
            if (memory != null) {
                Free(memory);
                memory = null;
            }
        }

        static void* Alloc (int count) => Marshal.AllocHGlobal(count + 15).ToPointer();

        static void Free (void* ptr) => Marshal.FreeHGlobal(new IntPtr(ptr));
    }
}
