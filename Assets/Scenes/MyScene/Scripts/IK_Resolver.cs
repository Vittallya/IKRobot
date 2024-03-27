using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using S7.Net;

public class IK_Resolver : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] Axes;


    public int ChainLength = 4;

    public Transform Target;



    private void Start()
    {
        plc.Open();
    }


    /// <summary>
    /// Функция рачсета углов в зависимости от осей и заданной точки
    /// 
    /// </summary>
    /// <param name="point">координата позиционирования робота</param>
    /// <param name="axes">оси вращения</param>
    /// <returns></returns>
    private List<float> GetAngles(Vector3 point, Transform[] axes)
    {

        float b1 = point.x - axes[0].position.x;
        float a1 = point.z - axes[0].position.z;
        float angle = (Mathf.Atan2(b1, a1) + Mathf.PI / 2) * Mathf.Rad2Deg;


        float targetX = Vector2.Distance(Vector2.zero, new Vector2(point.x, point.z));
        float targetY = point.y;


        float c = Vector3.Distance(axes[1].position, point);


        float a = Vector3.Distance(axes[2].position, axes[1].position);
        float b = Vector3.Distance(axes[3].position, axes[2].position);


        float q1 = Mathf.Atan((targetY - axes[1].position.y) / targetX);
        float angle1;
        float angle2 = 0.0f;

        if (a + b < c)
        {
            angle1 = (Mathf.PI / 2 - q1) * Mathf.Rad2Deg;
        }
        else
        {
            float q2 = Mathf.Acos((a * a + c * c - b * b) / (2 * a * c));
            float q3 = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b));

            angle1 = Mathf.PI / 2 - q1 - q2;
            //float q4 = Mathf.PI / 2 - angle1;
            angle2 = Mathf.PI - q3;

            angle1 *= Mathf.Rad2Deg;
            angle2 *= Mathf.Rad2Deg;

        }

        if (angle < 0)
            angle += 360;

        return new List<float> { angle, angle1, angle2 };
    }

    private void LateUpdate()
    {

        if (Input.GetButton("Jump"))
        {
            Target.transform.position = new Vector3(0, 2, 0);
        }

        float hL = Input.GetAxis("HorizontalLeft");
        //float hR = Input.GetAxis("HorizontalRight");
        float vL = Input.GetAxis("VerticalLeft");
        float vR = Input.GetAxis("VerticalRight");

        //plc.Write("DB8.DBX136.6", Input.GetButton("Jump"));

        var current = Target.position;

        current.x += hL * Time.deltaTime;
        current.z += vL * Time.deltaTime;
        current.y += vR * Time.deltaTime;

        Target.position = current;

        List<float> angles = GetAngles(Target.position, Axes);

        Debug.Log(string.Join("; ", angles));

        SetAnglesToModel(angles);
        //SendToPlc(angles, plc);
        //await SendToPlc(angles);
    }

    Plc plc = new(CpuType.S71500, "172.25.25.50", 0, 0);

    private void OnDestroy()
    {
        plc.Close();
    }

    private void SetAnglesToModel(List<float> angles)
    {
        Axes[0].rotation = Quaternion.Euler(0, angles[0], 0);
        Axes[1].localRotation = Quaternion.Euler(0, 0, angles[1]);
        Axes[2].localRotation = Quaternion.Euler(0, 0, angles[2]);
    }

    bool pressed = false;


    void SendToPlc(List<float> angles, Plc plc)
    {
        try
        {
            plc.Write("DB8.DBD2", angles[0]);
            plc.Write("DB8.DBD132", angles[1]);
            plc.Write("DB8.DBD6", angles[2]);
        }
        finally
        {

        }
    }



    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var current = Axes[^1];

        for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position,
                Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position),
                new Vector3(scale, Vector3.Distance(current.parent.position, current.position),
                scale));

            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
#endif
    }
}
