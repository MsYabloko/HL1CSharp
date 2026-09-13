namespace XashGameDLL;

public static unsafe class Util
{
#if X64
    public const int IntPtrSize = 8;
#else
    public const int IntPtrSize = 4;
#endif
    
    public static void MemSet(void* ptr, byte value, int size)
    {
        byte* bptr = (byte*)ptr;
        for (int i = 0; i < size; i++)
        {
            bptr[i] = value;
        }
    }
}