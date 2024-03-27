using UnityEditor;
using UnityEngine;

public class dirTest : MonoBehaviour
{
    public Transform Center;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Quaternion prevewRot;
    Vector3 prevewPos;

    // Update is called once per frame
    void Update()
    {
        if (transform.position != prevewPos || transform.rotation != prevewRot)
        {
            prevewPos = transform.position;
            prevewRot = transform.rotation;

            var delta = transform.position - Center.position;
            delta.y = 0;

            var norm = Vector3.Normalize(delta);

            var v1 = new Vector3(transform.position.x, 0, transform.position.z);
            var v2 = transform.rotation * Vector3.down;
            var angle = Vector3.Angle(v1, v2) - 90;
            //Debug.Log(angle);

            var v3 = Quaternion.Euler(0, 90, 0) * delta;

            var q = Quaternion.AngleAxis(angle, v3);

            var point = transform.position - q * norm * 2;
            transform.GetChild(0).transform.position = point;

            var angle2 = Vector3.Angle(transform.forward, transform.rotation * Vector3.forward);

            Debug.Log(Vector3.Angle(transform.up, Vector3.up));

            //Debug.Log(angle2);
        }
    }

    private void OnDrawGizmos()
    {
        var vector = transform.rotation * transform.up;
        //var pr = Vector3.Project(vector, Vector3.up);

        Handles.DrawLine(vector, vector * 5);
        Handles.color = Color.red;
        Handles.DrawLine(Vector3.up, Vector3.up * 5);
        Debug.Log(Vector3.Angle(vector, Vector3.up));

    }
}
