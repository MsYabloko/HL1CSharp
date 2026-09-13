using System.Runtime.InteropServices;

namespace XashGameDLL;


[StructLayout(LayoutKind.Sequential)]
public unsafe struct Vector
{
    public float X;
    public float Y;
    public float Z;

    public float Length => MathF.Sqrt(X * X + Y * Y + Z * Z);
    
    public Vector(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    public static Vector operator +(Vector left, Vector right)
    {
        return new Vector(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }
    public static Vector operator -(Vector left, Vector right)
    {
        return new Vector(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public Vector Normalize()
    {
        if (Length != 0)
        {
            Vector vec = new Vector(X, Y, Z);
            float ilength = 1.0f / Length;
            vec.X *= ilength;
            vec.Y *= ilength;
            vec.Z *= ilength;
            return vec;
        }

        return this;
    }
    
    public const int Size = sizeof(float) * 3;

    public override string ToString()
    {
        return X + " " + Y + " " + Z;
    }
}