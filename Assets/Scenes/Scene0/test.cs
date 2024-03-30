using System.Linq;
using UnityEngine;

public class test : MonoBehaviour
{

    public Transform target1;
    Vector3 previewRot;
    Vector3 previewPos;

    public Transform[] Axises;


    public IPlcConnection plcConnection;

    public RobotUnion[] Unions;

    private RobotConfigurationComponent config;

    private void Start()
    {
        previewRot = target1.eulerAngles;
        plcConnection = PlcConnection.MockConnection;
        plcConnection.Open(msg => Debug.LogError(msg));
        config = new RobotConfigurationComponent(new Assets.Scripts.Common.Models.RobotConfiguration()
        {
            RobotUnions = Unions.ToList()
        });
    }

    void Update()
    {
        if(target1.eulerAngles != previewRot || target1.position != previewPos)
        {
            previewRot = target1.eulerAngles;
            previewPos = target1.position;
            //this.transform.position = CommonMethods.GetAxisPosition2(target1, Axises, config).point;


            //var angles = CommonMethods.GetAngles(target1, Axises, config);
            //Axises[0].rotation = Quaternion.Euler(0, angles[0], 0);
            //Axises[1].localRotation = Quaternion.Euler(0, angles[1], 0);
            //Axises[2].localRotation = Quaternion.Euler(0, angles[2], 0);
            //Axises[3].localRotation = Quaternion.Euler(0, angles[3], 0);
            //Axises[4].localRotation = Quaternion.Euler(0, angles[4], 0);
            //plcConnection.SendToPlc(angles, msg => Debug.LogError(msg));

        }
    }


}
