using System;

[Serializable]
public class Colr
{
    public float r;
    public float g;
    public float b;
    public float a;

    public Colr(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }
    public override string ToString()
    {
        return $"Colr: ({r},{g},{b},{a})";
    }
}