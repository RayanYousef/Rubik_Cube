using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastersHandler : MonoBehaviour
{
    [SerializeField] Raycaster[] raycasters = new Raycaster[9];
    [SerializeField] Color colorOfFirstChild;
    [SerializeField] bool winFace;

    public bool WinFace { get => winFace; }

    // Start is called before the first frame update
    void Awake()
    {
        raycasters = GetComponentsInChildren<Raycaster>();

    }

    private void Update()
    {
        CheckFaceColors();
    }
    // Update is called once per frame
    private void OnEnable()
    {
        CheckFaceColors();
    }
    public void CheckFaceColors()
    {
        bool temp = true;
        colorOfFirstChild = raycasters[0].Color;

            for (int i = 1; raycasters != null && i < raycasters.Length; i++)
                if (colorOfFirstChild != raycasters[i].Color) temp = false;

        winFace = temp;


    }
    private void OnDisable()
    {
        winFace=false;
    }
}
