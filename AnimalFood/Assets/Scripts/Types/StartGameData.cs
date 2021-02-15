using System;

[Serializable]
public class StartGameData
{
    public int N;
    public int M;
    public int V;

    public StartGameData(int n, int m, int v)
    {
        N = n;
        M = m;
        V = v;
    }

    public override string ToString()
    {
        return $"DATA: {N}/{M}/{V}";
    }
}
