using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardParam<T>
{
    public event EventHandler OnChanged;
    private T value;

    public BlackboardParam(T value)
    {
        this.value = value;
    }

    public T Get()
    {
        return value;
    }

    public void Set(T value)
    {
        this.value = value;
        OnChanged?.Invoke(this, EventArgs.Empty);
    }
}
