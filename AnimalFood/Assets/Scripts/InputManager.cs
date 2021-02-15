using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector3> OnMove;
    public static event Action<float> OnRotate;
    public static event Action<float> OnZoom;

    private void Update()
    {
        if (Input.GetKey( KeyCode.W))
        {
            OnMove?.Invoke(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            OnMove?.Invoke(-Vector3.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            OnMove?.Invoke(-Vector3.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            OnMove?.Invoke(Vector3.right);
        }

        if (Input.GetKey(KeyCode.E))
        {
            OnRotate?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            OnRotate?.Invoke(1f);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            OnZoom?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.X))
        {
            OnZoom?.Invoke(1f);
        }
    }

}
