using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : AIAction
{
    private Vector3Int _beforeTargetPos = Vector3Int.zero;
    // 목표지점이 바뀔떄마다 경로를 계산할꺼니까 타일범위를 벗어나는 움직임이 아니라면
    private Vector3 _nextPos; // 다음으로 갈 경로
    public override void TakeAction()
    {
        //Vector2 direction = _brain.target.position - transform.position;

        //_brain.Move(direction.normalized, _brain.target.position);\\

        Vector3Int targetPos = MapManager.Instance.GetTilePos(_brain.target.position);
        if(targetPos != _beforeTargetPos)
        {
            _brain.Agent.Destination = targetPos;
            _beforeTargetPos = targetPos;
            SetNextPos();
        }

        if(Vector3.Distance(_nextPos, transform.position) <= 0.2f)
        {
            SetNextPos();
        }

        _brain.Move((_nextPos - transform.position).normalized, _brain.target.position);
    }

    private void SetNextPos()
    {
        Vector3Int nextTarget = _brain.Agent.GetNextTarget();
        if(_brain.Agent.CanMovePath == false)
        {
            _brain.Move(Vector3.zero, _brain.target.position);
            _nextPos = transform.position;
        }
        else
        {
            _nextPos = MapManager.Instance.GetWorldPos(_brain.Agent.GetNextTarget());
        }

    }
}
