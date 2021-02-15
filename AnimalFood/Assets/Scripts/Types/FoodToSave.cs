using System;

[Serializable]
public class FoodToSave
{
    public string Id;
    public Vec3 Pos;
    public Colr Color;

    public override string ToString()
    {
        return $"Food: Id: {Id}, Pos: {Pos}, Color: {Color}";
    }
}