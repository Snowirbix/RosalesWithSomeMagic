using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public abstract class Node
{
    public Node parentNode = null;
    public List<Node> childNodes = new List<Node>();
    public List<IDecorator> decorators = new List<IDecorator>();

    public Node AddChild<T>() where T : Node
    {
        Node child = Activator.CreateInstance<T>();
        childNodes.Add(child);

        return child;
    }

    public Node Up()
    {
        return parentNode;
    }

    public Decorator<T> Decorate<T>()
    {
        Decorator<T> decorator = Activator.CreateInstance<Decorator<T>>();
        decorators.Add(decorator);
        return decorator;
    }

    protected BehaviorTree bt;

    public virtual void Init(BehaviorTree bt)
    {
        this.bt = bt;

        decorators.ForEach(d => d.Init(this, bt));
        
        childNodes.ForEach(n => n.parentNode = this);
        childNodes.ForEach(n => n.Init(bt));
    }

    public virtual void Update(float deltaTime)
    {
        childNodes.ForEach(n => n.Update(deltaTime));
    }

    public Node GetRoot()
    {
        Node root = this;
        while (root.parentNode != null)
        {
            root = root.parentNode;
        }
        return root;
    }

    public bool HasDecorator()
    {
        return decorators.Count > 0;
    }

    public bool TestDecorator()
    {
        if (!HasDecorator())
            return true;
        else
            return decorators.All(d => d.TestCondition());
    }
}
