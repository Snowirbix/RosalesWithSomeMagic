using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[Serializable]
public class Blackboard
{
    protected Dictionary<string, object> parameters;

    public Blackboard()
    {
        parameters = new Dictionary<string, object>();
    }

    public void AddParam<T>(string key, T value)
    {
        Assert.IsFalse(parameters.ContainsKey(key), $"Blackboard already contains the key ${key} !");
        parameters.Add(key, new BlackboardParam<T>(value));
    }

    public BlackboardParam<T> GetParam<T>(string key)
    {
        Assert.IsTrue(parameters.ContainsKey(key), $"Blackboard does not contain the param ${key}");
        return parameters[key] as BlackboardParam<T>;
    }
}
