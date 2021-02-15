#pragma warning disable 0649
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour
{
    [SerializeField] private StartGameData _data;

    [SerializeField] private Cell _cell;
    [SerializeField] private Animal _animalPrefab;
    [SerializeField] private Food _foodPrefab;
    [SerializeField] private Transform _containerCells;
    [SerializeField] private Transform _containerAnimal;
    [SerializeField] private Transform _containerFood;
    [SerializeField] private NavMeshSurface _surface;

    private PoolManager<Animal> _animalPool;
    private PoolManager<Food> _foodPool;

    private List<Cell> _cellList = new List<Cell>();
    private Dictionary<Animal, Food> _animalFoodDict = new Dictionary<Animal, Food>();

    public Action<StartGameData> OnGameLoad;

    public void SetData(StartGameData data)
    {
        _data = data;
    }

    public void InitPools()
    {
        _animalPool = new PoolManager<Animal>(_containerAnimal);
        _animalPool.Initialize(_data.M, _animalPrefab);

        _foodPool = new PoolManager<Food>(_containerFood);
        _foodPool.Initialize(_data.M, _foodPrefab);
    }

    public void CellsSpawn()
    {
        var xScale = _cell.transform.localScale.x;
        var zScale = _cell.transform.localScale.z;

        var dataFloor = _data.N;

        for (var i = 0; i < dataFloor; i++)
        {
            for (var j = 0; j < dataFloor; j++)
            {
                var cell = Instantiate(_cell, new Vector3(j * xScale, 0, i * -zScale), Quaternion.identity, _containerCells);
                _cellList.Add(cell);
            }
        }

        _surface.BuildNavMesh();
    }


    public void SaveAll()
    {
        SaveData();
        SaveCells();
        SaveAnimals();
        SaveFood();

        Debug.Log(GLobalVars.Saved);
    }

    public void LoadAll()
    {
        _data = LoadData();

        InitPools();

        var cells = SaveLoad.Load<List<CellToSave>>(GLobalVars.SavedCells);
        CellLoad(cells);

        var savedAnimals = SaveLoad.Load<List<AnimalToSave>>(GLobalVars.SavedAnimals);
        var animals = AnimalLoad(savedAnimals);

        var savedFood = SaveLoad.Load<List<FoodToSave>>(GLobalVars.SavedFood);
        var food = FoodLoad(savedFood);

        foreach (var animal in animals)
        {
            animal.ChaseFood(food.First(x => x.Id == animal.Id).Pos);

            animal.OnFoodEat += OnFoodEat;
            animal.OnReturn += OnAnimalDead;
        }

        for (var i = 0; i < _data.M; i++)
        {
            _animalFoodDict.Add(animals[i], food[i]);
        }

        OnGameLoad?.Invoke(_data);

        Debug.Log(GLobalVars.Loaded);
    }

    public void SaveData()
    {
        var m = Mathf.Min(_data.M, _animalFoodDict.Count);
        _data.M = m;

        SaveLoad.Save(_data, GLobalVars.SavedData);
    }

    public void SaveCells()
    {
        var cells = _cellList
            .Select(c => new CellToSave
            {
                AnimalId = c.AnimalId.ToString(),
                FoodId = c.FoodId.ToString(),
                Pos = new Vec3(c.transform.position.x, c.transform.position.y, c.transform.position.z)
            })
            .ToList();

        SaveLoad.Save(cells, GLobalVars.SavedCells);
    }

    public void SaveAnimals()
    {
        var animals = new List<AnimalToSave>();

        foreach (var kvp in _animalFoodDict)
        {
            var animal = kvp.Key;
            var food = kvp.Value;
            var color = animal.Color;

            animals.Add(new AnimalToSave
            {
                Color = new Colr(color.r, color.g, color.b, color.a),
                Id = animal.Id.ToString(),
                ChasingTime = animal.ChasingTime,
                Speed = animal.Speed,
                Pos = new Vec3(animal.transform.position.x, animal.transform.position.y, animal.transform.position.z),
                TimeLeft = animal.TimeLeft
            });
        }

        SaveLoad.Save(animals, GLobalVars.SavedAnimals);
    }

    public void SaveFood()
    {
        var foods = new List<FoodToSave>();

        foreach (var kvp in _animalFoodDict)
        {
            var animal = kvp.Key;
            var food = kvp.Value;
            var color = animal.Color;

            foods.Add(new FoodToSave
            {
                Color = new Colr(color.r, color.g, color.b, color.a),
                Id = animal.Id.ToString(),
                Pos = new Vec3(food.transform.position.x, food.transform.position.y, food.transform.position.z)
            });
        }

        SaveLoad.Save(foods, GLobalVars.SavedFood);
    }

    public StartGameData LoadData()
    {
        return SaveLoad.Load<StartGameData>(GLobalVars.SavedData);
    }

    public void CellLoad(List<CellToSave> list)
    {
        foreach (var t in list)
        {
            var p = t.Pos;
            var cell = Instantiate(_cell, new Vector3(p.x, p.y, p.z), Quaternion.identity, _containerCells);
            var aId = t.AnimalId;
            var fId = t.FoodId;

            if (!string.IsNullOrWhiteSpace(aId))
            {
                cell.RegAnimal(Guid.Parse(aId));
            }

            if (!string.IsNullOrWhiteSpace(fId))
            {
                cell.RegFood(Guid.Parse(fId));
            }
            _cellList.Add(cell);
        }
        _surface.BuildNavMesh();
    }

    public List<Animal> AnimalLoad(List<AnimalToSave> list)
    {
        var aList = new List<Animal>();

        foreach (var a in list)
        {
            var p = a.Pos;
            var c = a.Color;
            var animal = _animalPool.GetObject();
            animal.SetId(Guid.Parse(a.Id));
            animal.SetSpeed(a.Speed);
            animal.SetChasingTime(a.ChasingTime);
            animal.SetLeftTime(a.TimeLeft);
            animal.SetColor(new Color(c.r, c.g, c.b, c.a));
            animal.SetPos(new Vector3(p.x, p.y, p.z), Quaternion.identity);
            animal.OnCreate();
            aList.Add(animal);
        }
        return aList;
    }

    public List<Food> FoodLoad(List<FoodToSave> list)
    {
        var fList = new List<Food>();

        foreach (var f in list)
        {
            var p = f.Pos;
            var c = f.Color;
            var food = _foodPool.GetObject();
            food.SetId(Guid.Parse(f.Id));
            food.SetColor(new Color(c.r, c.g, c.b, c.a));
            food.SetPos(new Vector3(p.x, p.y, p.z), Quaternion.identity);
            food.OnCreate();
            fList.Add(food);
        }

        return fList;
    }

    public void AnimalFoodSpawn()
    {
        var animalsAmount = _data.M;

        while (animalsAmount > 0)
        {
            var cell = GetRandomCell();
            if (cell.HasAnimal) continue;
            var animal = AnimalSpawn(cell);
            cell.RegAnimal(animal.Id);
            var food = FoodSpawn(animal);
            animal.ChaseFood(food.Pos);
            animal.OnFoodEat += OnFoodEat;
            animal.OnReturn += OnAnimalDead;
            _animalFoodDict.Add(animal, food);
            animalsAmount--;
        }

        OnGameLoad?.Invoke(_data);
    }

    private Food FoodSpawn(Animal animal)
    {
        Food food = null;
        var foodPlaced = false;

        while (!foodPlaced)
        {
            var cell = GetRandomCell();
            if (cell.HasFood || !ValidateDistance(cell, animal)) continue;
            food = FoodSpawn(cell, animal);
            cell.RegFood(food.Id);
            foodPlaced = true;
        }

        return food;
    }

    private bool ValidateDistance(Cell cell, Animal animal)
    {
        var dist = (cell.Pos - animal.Pos).sqrMagnitude;
        return dist < animal.WalkDistance * animal.WalkDistance;
    }

    private Animal AnimalSpawn(Cell cell)
    {
        var animal = _animalPool.GetObject();
        animal.SetId(Guid.NewGuid());
        animal.SetSpeed(_data.V);
        animal.SetColor(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
        animal.SetPos(cell.Pos, Quaternion.identity);
        animal.OnCreate();
        return animal;
    }

    private Food FoodSpawn(Cell cell, Animal animal)
    {
        var food = _foodPool.GetObject();
        food.SetId(animal.Id);
        food.SetColor(animal.Color);
        food.SetPos(cell.Pos, Quaternion.identity);
        food.OnCreate();
        food.Birth();
        return food;
    }

    private void OnFoodEat(Food food)
    {
        var animal = _animalFoodDict.FirstOrDefault(x => x.Value.Id == food.Id).Key;
        _cellList.FirstOrDefault(x => x.FoodId == food.Id)?.UnRegFood();

        var newFood = FoodSpawn(animal);
        _animalFoodDict[animal] = newFood;
        animal.ChaseFood(newFood.Pos);
    }

    private void OnAnimalDead(Animal animal)
    {
        animal.OnFoodEat -= OnFoodEat;

        _cellList.FirstOrDefault(x => x.AnimalId == animal.Id)?.UnRegAnimal();
        var food = _animalFoodDict.FirstOrDefault(x => x.Key.Id == animal.Id).Value;
        _animalFoodDict.Remove(animal);
        food.Die();
        animal.Die();
    }

    private Cell GetRandomCell() => _cellList.ElementAt(Random.Range(0, _cellList.Count));
}