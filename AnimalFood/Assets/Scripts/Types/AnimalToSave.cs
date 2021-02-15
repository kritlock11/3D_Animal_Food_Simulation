using System;

[Serializable]
public class AnimalToSave
{
    public string Id;
    public int Speed;
    public int ChasingTime;
    public float TimeLeft;
    public Vec3 Pos;
    public Colr Color;

    public override string ToString()
    {
        return $"Animal: Id: {Id}, Speed: {Speed}, HuntingTime: {ChasingTime}, TimeLeft: {TimeLeft}, {Pos}, {Color}";
    }
}