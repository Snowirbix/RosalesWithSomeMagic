using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Test : MonoBehaviour
{
    [SerializeField]
    BehaviorTree bt;

    private void Start()
    {
        bt = new BehaviorTree(gameObject);
        bt.AddChild<SequenceNode>()
            .AddChild<SelectorNode>()
                .Decorate<int>()
                    .SetAbortMode(DecoratorAbortMode.BOTH)
                    .SetBBKey("salut")
                    .Up()
                .AddChild<SimpleMoveTask>().Up()
                .AddChild<SimpleMoveTask>().Up()
                .Up()
            .AddChild<SequenceNode>()
                .AddChild<SimpleMoveTask>().Up();

        bt.Init();

        bt.AddParam("salut", 1);
        Debug.Log(bt.GetParam<int>("salut"));
    }

    private void Update()
    {
        bt.Update(Time.deltaTime);
    }
}
