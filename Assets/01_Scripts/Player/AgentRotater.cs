using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentRotater : MonoBehaviour
{
    private float _targetDegree;
    private float _rotateSpeed;
    float _delay = 0;

    public void SetRotateSpeed(float value) 
    {
        _rotateSpeed = value;
    }
    public void SetTarget(Vector3 pos)
    {
        Vector3 delta = pos - transform.position;
        _targetDegree = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        Debug.Log(_targetDegree);
        //transform.rotation = Quaternion.AngleAxis(targetDegree, Vector3.forward);
    }
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, (Quaternion.AngleAxis(_targetDegree, Vector3.forward)),Time.deltaTime * _rotateSpeed);
    }
}
