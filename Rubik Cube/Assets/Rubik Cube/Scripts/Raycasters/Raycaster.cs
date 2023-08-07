using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask layer;

    public Color Color { get => color; }


    // Update is called once per frame
    void Update()
    {

        if(Physics.Raycast(transform.position, transform.forward, out hit, 1.5f,layer))
        {
            MeshRenderer meshRenderer;
            hit.transform.TryGetComponent<MeshRenderer>(out meshRenderer);
            if(meshRenderer != null) 
            color = meshRenderer.material.color;
        }
    }
}
