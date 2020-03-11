using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveTask : TaskNode
{
    protected float endZone = 3f;
    protected Transform target;
    protected MovementController move;

    public override void Init(BehaviorTree bt)
    {
        base.Init(bt);

        move = bt.GetComponent<MovementController>();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        move.moveDirection = (target.position - bt.gameObject.transform.position).normalized;
    }

    public override bool IsComplete()
    {
        return (Vector3.Distance(target.position, bt.gameObject.transform.position) < endZone);
    }
}
