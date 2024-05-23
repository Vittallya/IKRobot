using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI3DCoords : MonoBehaviour
{

    public Vector3 RotationOffset;

    public bool LookAt;

    void Start()
    {
        GetComponent<Renderer>().material.renderQueue = currentQueue++;
    }

    private static int currentQueue = 4000;

    void Update()
    {
        if (LookAt)
        {
            transform.LookAt(Camera.main.transform.position);
            transform.rotation = transform.rotation * Quaternion.Euler(RotationOffset);
        }
    }

    public void SetText(Vector3 v)
    {
        var text = $"x = {v.x:f3} \ny ={v.y:f3}\nz = {v.z:f3}";
        GetComponent<TextMeshPro>().text = text;
    }

    public void SetTextAngleY(Transform transform)
    {
        var val = transform.localRotation.eulerAngles.y;
        GetComponent<TextMeshPro>().text = val.ToString("f2");
    }
}
