using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace CubeTransformation
{
    public class CubeBodyRotation : MonoBehaviour
    {


        [SerializeField] GameObject objectToBeFollowed;
        [SerializeField] SORotateObject yPositive, yNegative, zPositive, zNegative;

        [SerializeField] Vector2 mouseDownPos, mouseUpPos, deltaMousePos;
        
        [SerializeField] float rotationDuration;
        [SerializeField] bool  buttonIsDown;


        void Update()
        {
            if (GameManager.Instance.Lerping) return;
            RotationMethod();

        }


        #region Mouse Down,Drag and Up
        public void RotationMethod()
        {


            if (Input.GetMouseButtonDown(1))
            {
                buttonIsDown = true;
                mouseDownPos = deltaMousePos = Input.mousePosition;
            }
            if (Input.GetMouseButton(1) && buttonIsDown)
            {
                deltaMousePos -= new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                transform.rotation = Quaternion.Euler(0, deltaMousePos.x * Time.deltaTime, deltaMousePos.y * Time.deltaTime) * transform.rotation;

                deltaMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(1) && buttonIsDown)
            {
                mouseUpPos = Input.mousePosition;

                float x = mouseUpPos.x - mouseDownPos.x;
                float y = mouseUpPos.y - mouseDownPos.y;


                if (Mathf.Abs(x) > Mathf.Abs(y))
                {


                    if (x < 0) yPositive.Execute(objectToBeFollowed.transform, CubeOrFace.Cube);
                    else if (x > 0) yNegative.Execute(objectToBeFollowed.transform, CubeOrFace.Cube);
                }
                else if (Mathf.Abs(x) < Mathf.Abs(y))
                {

                    if (y > 0) zNegative.Execute(objectToBeFollowed.transform, CubeOrFace.Cube);
                    else if (y < 0) zPositive.Execute(objectToBeFollowed.transform, CubeOrFace.Cube);
                }

                LerpRotationCoroutine();
            }

        }

        #endregion

        #region Rotation Coroutine
        public void LerpRotationCoroutine()
        {
            StartCoroutine(LerpQuatrenion(rotationDuration, transform.rotation, objectToBeFollowed.transform.rotation));
        }


        IEnumerator LerpQuatrenion(float duration, Quaternion firstRotationState, Quaternion finalRotationState)
        {
            GameManager.Instance.Lerping = true;
            Quaternion rotation;
            float time = 0;
            while (time < duration)
            {
                rotation = Quaternion.Lerp(firstRotationState, finalRotationState, time / duration);
                time += Time.deltaTime;
                transform.rotation = rotation;
                buttonIsDown = false;
                yield return null;
            }
            rotation = finalRotationState;
            transform.rotation = rotation;
            GameManager.Instance.Lerping = false;

        }
        #endregion

    }


}