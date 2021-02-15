#pragma warning disable 0649
using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;


public class Animal : PoolObject
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider _collider;

    private bool _chasing;
    private float _chasingSpan;

    public Action<Food> OnFoodEat;
    public Action<Animal> OnReturn;

    public int WalkDistance => Speed * ChasingTime;
    public Vector3 Pos => transform.position;
    public int Speed { get; private set; }
    public int ChasingTime { get; private set; } = 5;
    public Color Color { get; private set; }


    private void FixedUpdate()
    {
        ChaseTimer();
    }

    public void ChaseFood(Vector3 pos)
    {
        _chasingSpan = 0;

        _agent.SetDestination(pos);
        _chasing = true;
    }

    private void ChaseTimer()
    {
        if (!_chasing) return;

        if (_chasingSpan <= ChasingTime)
        {
            _chasingSpan += Time.fixedDeltaTime;
            TimeLeft = ChasingTime - _chasingSpan;
        }
        else
        {
            OnReturn?.Invoke(this);
        }
    }

    public void SetId(Guid id) => Id = id;
    public void SetSpeed(int speed) => _agent.speed = Speed = speed;
    public void SetChasingTime(int t) => ChasingTime = t;
    public void SetLeftTime(float t) => TimeLeft = t;
    public void SetColor(Color c) => Color = _renderer.material.color = c;

    public void SetPos(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Die()
    {
        _renderer.enabled = false;
        _collider.enabled = false;
        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        var food = other.GetComponent<Food>();

        if (!food || food.Id != Id) return;
        _chasing = false;
        food.Die();
        OnFoodEat?.Invoke(food);
    }
}
