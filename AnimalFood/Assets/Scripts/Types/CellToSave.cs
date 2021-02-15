using System;

[Serializable]
public class CellToSave
{
    public string AnimalId;
    public string FoodId;
    public Vec3 Pos;

    public override string ToString()
    {
        return $"Cell: AnimalId: {AnimalId}, FoodId: {FoodId}, {Pos}";
    }

}