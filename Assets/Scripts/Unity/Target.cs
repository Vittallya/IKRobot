using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform Torus;
    public Transform Axis0;

    private float zAxisAngle = 0f;

    public void SetZAxisAngleDeg(float angle)
    {
        zAxisAngle = angle;
    }



    void Update()
    {
        var angle = Mathf.PI - Mathf.Atan2(Axis0.position.z - transform.position.z, Axis0.position.z - transform.position.x);
        this.transform.rotation = Quaternion.Euler(0, angle * Mathf.Rad2Deg, zAxisAngle);
    }
}
