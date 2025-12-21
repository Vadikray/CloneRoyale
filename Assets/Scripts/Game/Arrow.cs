using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [field: SerializeField] public float speed { get; private set; } = 5f;
    private Vector3 _targetPosition = Vector3.zero;
    
    public void Init(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
        
        if(transform.position == _targetPosition) Destroy(gameObject);
    }
}
