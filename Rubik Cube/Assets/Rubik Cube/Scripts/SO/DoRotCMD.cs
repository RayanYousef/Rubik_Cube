using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/RotateObject/DoRotCMD")]
public class DoRotCMD : SORotateObject
{
    public override void Execute(Transform trans, CubeOrFace checkShape)
    {
        DoRotation(trans, checkShape);

        CubeCommands thisAction = new CubeCommands(trans, this, checkShape);
        GameManager.Instance.SolveCommands.Add(thisAction);
        GameManager.Instance.CubeUndoCommands.Add(thisAction);
        GameManager.Instance.CubeRedoCommands.Clear();
        Debug.Log(trans.name);
    }

    public override void DoRotation(Transform trans, CubeOrFace checkShape)
    {



        switch (checkShape)
        {
            case CubeOrFace.Cube:
                trans.Rotate(rotationDirection, Space.World);
                break;

            case CubeOrFace.Face:
                GameManager.Instance.StartCoroutine
                (LerpQuatrenion(trans, lerpDuration, trans.rotation, PositiveRotation(trans.rotation)));
                break;
        }

    }

    public override void UndoRotation(Transform trans, CubeOrFace checkShape)
    {


        switch (checkShape)
        {
            case CubeOrFace.Cube:
                trans.Rotate(-rotationDirection, Space.World);
                break;

            case CubeOrFace.Face:
                GameManager.Instance.StartCoroutine
                (LerpQuatrenion(trans, lerpDuration, trans.rotation, NegativeRotation(trans.rotation)));
                break;
        }
    }
}
