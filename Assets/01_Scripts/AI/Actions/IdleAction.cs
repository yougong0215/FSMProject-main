using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : AIAction
{
    private void Start()
    {
        
    }
    public override void TakeAction()
    {

       _brain.Move(Vector2.zero, transform.position + transform.right);
    }
}
