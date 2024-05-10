using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LearningText
{
    [TextArea(3, 10)]
    public string[] sentences;
}
