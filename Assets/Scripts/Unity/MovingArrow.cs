using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingArrow : MonoBehaviour
{
    public Transform target;

    private bool mouseDown;
    private Vector3 previewMousePos;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseDown)
        {
            var delta = Input.mousePosition - previewMousePos;

        }
    }


    private void OnMouseDown()
    {
        mouseDown = true;
        previewMousePos = Input.mousePosition;
        target.GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnMouseUp()
    {
        target.GetComponent<Renderer>().material.color = Color.white;
        mouseDown = false;
    }
}
