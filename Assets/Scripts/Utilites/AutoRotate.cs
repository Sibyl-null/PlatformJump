using System;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private Vector3 rotation;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.Rotate(rotation * Time.deltaTime);
    }
}
