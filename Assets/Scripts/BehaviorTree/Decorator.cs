using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecoratorAbortMode
{
    NONE,
    LOWER,
    SELF,
    BOTH,
}

public class Decorator<T> : IDecorator
{

    protected DecoratorAbortMode abortMode;
    protected string key;
    protected BlackboardParam<T> param;
    public bool notNull;
    public T matchValue;

    protected Node node;
    protected BehaviorTree bt;

    public void Init(Node node, BehaviorTree bt)
    {
        this.node = node;
        this.bt = bt;
        param = bt.GetParam<T>(key);
        param.OnChanged += Param_OnChanged;
    }
    
    public Node Up()
    {
        return node;
    }

    public IDecorator SetAbortMode(DecoratorAbortMode mode)
    {
        this.abortMode = mode;
        return this;
    }

    public IDecorator SetBBKey(string key)
    {
        this.key = key;
        return this;
    }

    private void Param_OnChanged(object sender, System.EventArgs e)
    {
        // if condition true and set to abort lower priorities, send event to the parent node ?
        // if condition true wait lower priority finishing and take the lead
        // if false send event to parent node to select the next available node
    }

    public bool TestCondition()
    {
        return param.Get().Equals(matchValue);
    }
}
