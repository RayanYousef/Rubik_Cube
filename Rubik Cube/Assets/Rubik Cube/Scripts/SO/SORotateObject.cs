using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class SORotateObject : ScriptableObject
{
    [SerializeField] protected Vector3 rotationDirection;
    [SerializeField]protected float lerpDuration;

    public virtual void Execute(Transform trans, CubeOrFace checkShape)
    {

    }

    public virtual void DoRotation(Transform trans, CubeOrFace checkShape)
    {

    }

    public virtual void UndoRotation(Transform trans, CubeOrFace checkShape)
    {

    }



    protected Quaternion PositiveRotation(Quaternion currentRotation)
    {
        return (Quaternion.Euler(currentRotation.eulerAngles + rotationDirection));
    }
    protected Quaternion NegativeRotation(Quaternion currentRotation)
    {
        return (Quaternion.Euler(currentRotation.eulerAngles - rotationDirection));
    }


    protected IEnumerator LerpQuatrenion(Transform trans, float duration, Quaternion firstRotationState, Quaternion finalRotationState)
    {
        GameManager.Instance.Lerping = true;
        trans.parent.GetComponent<CubeFaceRotation>().SetChildren();
        Quaternion rotation;
        float time = 0;
        while (time < duration)
        {
            rotation = Quaternion.Lerp(firstRotationState, finalRotationState, time / duration);
            time += Time.deltaTime;
            trans.rotation = rotation;
            yield return null;
        }
        rotation = finalRotationState;
        trans.rotation = rotation;
        trans.parent.GetComponent<CubeFaceRotation>().ReleaseChildren();
        GameManager.Instance.SetWinBoolean();
        GameManager.Instance.Lerping = false;





    }
}
