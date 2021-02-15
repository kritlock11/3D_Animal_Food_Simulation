using System;
using UnityEngine;


public class Cell : MonoBehaviour
{
    private void Awake()
    {
        AnimalId = Guid.Empty;
        FoodId = Guid.Empty;
    }

    public bool HasAnimal => AnimalId != Guid.Empty;
    public bool HasFood => FoodId != Guid.Empty;

    public Guid AnimalId { get; private set; }

    public Guid FoodId { get; private set; }

    public Vector3 Pos => transform.position;

    public void RegAnimal(Guid id) => AnimalId = id;
    public void UnRegAnimal() => AnimalId = Guid.Empty;
    public void RegFood(Guid id) => FoodId = id;
    public void UnRegFood() => FoodId = Guid.Empty;
}