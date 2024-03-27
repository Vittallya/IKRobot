using UnityEngine;

public class OnSplacePressed : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //this.transform.position = Quaternion.Euler(30, 0, 0) * transform.position;

            var eulers = Quaternion.LookRotation(transform.position).eulerAngles;

            Debug.Log($"{eulers.x} ; {eulers.y} ; {eulers.z}");

        }
    }
}