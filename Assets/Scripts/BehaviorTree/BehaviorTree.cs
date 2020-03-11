using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[Serializable]
public class BehaviorTree
{
    public Node rootNode;
    protected Blackboard board;
    public GameObject gameObject;

    public BehaviorTree(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.board = new Blackboard();
        this.rootNode = new RootNode();
    }

    public void Init ()
    {
        rootNode.Init(this);
    }

    public void Update (float deltaTime)
    {
        rootNode.Update(deltaTime);
    }

    public Node AddChild<T>() where T : Node
    {
        return rootNode.AddChild<T>();
    }
    
    public T GetComponent<T>()
    {
        return gameObject.GetComponent<T>();
    }
    
    public void AddParam<T>(string key, T value)
    {
        board.AddParam<T>(key, value);
    }

    public BlackboardParam<T> GetParam<T>(string key)
    {
        return board.GetParam<T>(key);
    }
}
