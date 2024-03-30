using UnityEngine;

public class RotateTorus : MonoBehaviour
{
    public Target target;

    private bool mouseDown;
    private Vector3 previewMousePos;

    public MonoBehaviour cameraRotation;


    void Update()
    {
        if (mouseDown)
        {
            var delta = Input.mousePosition - previewMousePos;

            var sign =
                Input.mousePosition.x < previewMousePos.x ? 1 : -1;

            var angle = delta.magnitude * sign / Mathf.PI;

            target.SetZAxisAngleDeg(angle);

        }
    }


    private void OnMouseDown()
    {
        cameraRotation.enabled = false;
        mouseDown = true;
        previewMousePos = Input.mousePosition;
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnMouseUp()
    {
        cameraRotation.enabled = true;
        this.GetComponent<Renderer>().material.color = Color.white;
        mouseDown = false;
    }
}
