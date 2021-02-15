#pragma warning disable 0649
using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;


public class Food : PoolObject
{
    [SerializeField] private ParticleSystem _fxDeath;
    [SerializeField] private ParticleSystem _fxBirth;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Collider _collider;

    public Color Color { get; private set; }
    public Vector3 Pos
    {
        get => transform.position;
        private set => transform.position = value;
    }

    public void SetColor(Color color) => Color = _renderer.material.color = color;
    public void SetId(Guid id) => Id = id;
    
    public void SetPos(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void Birth()
    {
        _fxBirth.Play();
        _renderer.enabled = true;
        _collider.enabled = true;
    }

    public void Die()
    {
        _fxDeath.Play();
        _renderer.enabled = false;
        _collider.enabled = false;

        StartCoroutine(Elapsing(_fxDeath.main.duration));
    }

    public IEnumerator Elapsing(float duration)
    {
        var s = 0f;

        while (s <= duration)
        {
            s += Time.deltaTime;
            TimeLeft = duration - s;

            yield return null;
        }

        ReturnToPool();
    }
}
