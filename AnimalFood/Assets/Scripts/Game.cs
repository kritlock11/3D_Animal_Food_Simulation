#pragma warning disable 0649
using Assets.Scripts;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Ui _ui;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private CameraManager _camManager;
    

    private void Awake()
    {
        Time.timeScale = 0.0f;
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 60;

    }

    private void OnEnable()
    {
        _ui.OnStart += StartGame;
        _ui.OnSave += Save;
        _ui.OnLoad += Load;
        _spawner.OnGameLoad += _camManager.CreateDummy;
    }

    private void OnDisable()
    {
        _ui.OnStart -= StartGame;
        _ui.OnSave -= Save;
        _ui.OnLoad -= Load;
        _spawner.OnGameLoad -= _camManager.CreateDummy;
    }

    private void StartGame()
    {
        _spawner.SetData(new StartGameData(_ui.N, _ui.M, _ui.V));
        _spawner.InitPools();
        _spawner.CellsSpawn();
        _spawner.AnimalFoodSpawn();
    }


    public void Save()
    {
        _spawner.SaveAll();
    }

    public void Load()
    {
        _spawner.LoadAll();
    }
}