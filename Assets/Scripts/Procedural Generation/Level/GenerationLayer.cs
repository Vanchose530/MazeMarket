using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenerationLayer : ScriptableObject
{
    [Header("Generation Layer")]
    public bool enabled = true;
    // ����� ��� ���������� �������� ��� �������
    public virtual void Layer(LevelTemplate levelTemplate) { }
}
