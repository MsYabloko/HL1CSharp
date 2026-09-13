using System.Runtime.InteropServices;

namespace XashGameDLL;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct KeyValueData
{
    public IntPtr szClassName;
    public IntPtr szKeyName;
    public IntPtr szValue;
    private int fHandled;

    public string? ClassName => Marshal.PtrToStringAnsi(szClassName);
    public string? KeyName => Marshal.PtrToStringAnsi(szKeyName);
    public string? Value => Marshal.PtrToStringAnsi(szValue);

    public bool Handled
    {
        get
        {
            return fHandled > 0;
        }
        set
        {
            fHandled = value ? 1 : 0;
        }
    }
}