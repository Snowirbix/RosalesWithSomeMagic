using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDecorator
{
    void Init(Node node, BehaviorTree bt);
    Node Up();
    IDecorator SetAbortMode(DecoratorAbortMode abortMode);
    IDecorator SetBBKey(string key);
    bool TestCondition();
}
