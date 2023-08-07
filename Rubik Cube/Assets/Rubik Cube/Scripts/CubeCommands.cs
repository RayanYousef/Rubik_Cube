using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCommands
{

    [SerializeField] SORotateObject command;
    [SerializeField] Transform trans;
    [SerializeField] CubeOrFace shape;

    public CubeCommands (Transform trans, SORotateObject command, CubeOrFace shape)
    {
        this.trans = trans;
        this.command = command;
        this.shape = shape;

    }

    public SORotateObject Command { get => command; }
    public Transform Trans { get => trans;  }

    public CubeOrFace Shape { get => shape; }
}
