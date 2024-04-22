using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour 
{
    public bool isFinished { get; protected set; }

    // � ���������� ������� ������ ���������

    public virtual void Init() { isFinished = false; }

    public virtual void Run() { }

    public virtual void Exit() { }
}
