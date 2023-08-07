using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeFaceRotation : MonoBehaviour
{
    [SerializeField] SORotateObject zPositive, zNegative;
    [SerializeField] Transform rotatingChild, cubies, rubikCube;
    [SerializeField] Collider col;

    [SerializeField] LayerMask layer;
    [SerializeField] Vector3 rotation;
    [SerializeField] Quaternion firstRotationState, finalRotationState;
    [SerializeField] Vector2 mouseDownPos, mouseUpPos, deltaMousePos;


    //*** Virtual Box Collider ***\\
    Vector3 virtualBoxExtents, virtualBoxCenter, virtualBoxRotation;

    public Vector3 Rotation
    {
        get => rotation;
        set
        {
            rotation = value;
            transform.rotation= Quaternion.Euler(rotation);

        }
    }

    void Start()
    {
        
        col = GetComponent<Collider>();
        cubies = GameObject.Find("Cubies").transform ;
        virtualBoxCenter = col.bounds.center;
        virtualBoxRotation = transform.rotation.eulerAngles;
        virtualBoxExtents= col.bounds.size;
        Rotation = transform.rotation.eulerAngles;
    }


    // Update is called once per frame
    void Update()
    {


    }



    private void OnMouseDown()
    {

        if (GameManager.Instance.Lerping) return;
        firstRotationState = rotatingChild.rotation;
        mouseDownPos = deltaMousePos= Input.mousePosition;

    }

        //private void OnMouseDrag()
        //{
        //    if (lerping) return;
        //    deltaMousePos -= new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //    //transform.rotation = Quaternion.Euler(0, 0, deltaMousePos.y * Time.deltaTime * 20) * transform.rotation;
        //    rotatingChild.Rotate(new Vector3(0, 0, deltaMousePos.y * Time.deltaTime * 10));

        //    deltaMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        //}

    private void OnMouseUp() 
    {
        if (GameManager.Instance.Lerping) return;
        mouseUpPos = Input.mousePosition;   
        
        float yDifference= mouseUpPos.y - mouseDownPos.y;
        //float rotationDegree = 0;
        if (yDifference < 0) zPositive.Execute(rotatingChild, CubeOrFace.Face);
        if (yDifference > 0) zNegative.Execute(rotatingChild, CubeOrFace.Face);






    }

    public void SetChildren()
    {

        Collider[] colliders =
            Physics.OverlapBox(virtualBoxCenter, virtualBoxExtents,
            Quaternion.LookRotation(virtualBoxRotation), layer);

        foreach (Collider collider in colliders)
        {
            collider.transform.parent = rotatingChild;
        }

    }

    public void ReleaseChildren()
    {
        Collider[] colliders =
            Physics.OverlapBox(virtualBoxCenter, virtualBoxExtents,
            Quaternion.LookRotation(virtualBoxRotation), layer);

        foreach (Collider collider in colliders)
        {
            collider.transform.parent = cubies;
        }
    }





    private void OnDrawGizmos()
    {
        //var oldMatrix = Gizmos.matrix;

        //// create a matrix which translates an object by "position", rotates it by "rotation" and scales it by "halfExtends * 2"
        //Gizmos.matrix = Matrix4x4.TRS(transform.position,
        //    Quaternion.LookRotation(transform.eulerAngles), col.bounds.size);
        //// Then use it one a default cube which is not translated nor scaled
        //Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        //Gizmos.matrix = oldMatrix;
    }


}
