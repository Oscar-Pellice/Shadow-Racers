using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Car : MonoBehaviour
{
    [SerializeField] private WheelCollider frontLeftWheelCollider = null;
    [SerializeField] private WheelCollider frontRightWheelCollider = null;
    [SerializeField] private WheelCollider rearLeftWheelCollider = null;
    [SerializeField] private WheelCollider rearRightWheelCollider = null;

    [SerializeField] private Transform frontLeftWheelTransform = null;
    [SerializeField] private Transform frontRightWheeTransform = null;
    [SerializeField] private Transform rearLeftWheelTransform = null;
    [SerializeField] private Transform rearRightWheelTransform = null;

    [SerializeField] private Rigidbody rb = null;

    [SerializeField] private float motorForce = 0;
    [SerializeField] private float maxSteerAngle = 0;

    private float t0Time = 0;
    private float t1Time = 0;

    int timer = 0;
    private List<GameManager.PathInfo> info = null;
    private GameManager.PathInfo currentNode;

    private Transform IAcar_transform;

    [SerializeField] public Vector3 targetToGet;

    // Start is called before the first frame update
    void Start()
    {
        info = GameManager.PathRegister[0];

        t0Time = Time.time;
        rb.centerOfMass = new Vector3(0, -0.25f, 0);

        IAcar_transform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        t1Time = Time.time;

        currentNode = info[timer];
        targetToGet = currentNode.getPosition();

        if (Vector3.Distance(currentNode.getPosition(), IAcar_transform.position) < 5 
            /*&& (currentTime - startTime) >= 1*/)
        {
            timer++;
            t0Time = t1Time;
            Debug.Log("Path");
        }
    }

    private void FixedUpdate()
    {
        HandleMotor(1);
        /*if (currentNode.player_rpm > frontLeftWheelCollider.rpm)
        {
            HandleMotor(1);
        } else if (currentNode.player_rpm < frontLeftWheelCollider.rpm)
        {
            HandleMotor(-1);
        } else
        {
            HandleMotor(0);
        }*/

        HandleSteering(CalculateAngle());
        UpdateWheels();
    }

    private int CalculateAngle()
    {

        Vector3 targetDir = new Vector3(currentNode.getPosition().x - IAcar_transform.position.x,
                                        currentNode.getPosition().y - IAcar_transform.position.y,
                                        currentNode.getPosition().z - IAcar_transform.position.z);
        Vector3 forward = IAcar_transform.forward;
        float angle = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        //Debug.Log("Between vector " + targetDir + "and vector " + forward + "-> " + angle);
        if (angle < -5.0f)
        {
            return -1;
        }
        else if (angle > 5.0f)
        {
            return 1;
        }
        return 0;
    }

    private void HandleMotor(float multiply)
    {
        rearLeftWheelCollider.motorTorque = motorForce * multiply;
        rearRightWheelCollider.motorTorque = motorForce * multiply;
        frontLeftWheelCollider.motorTorque = motorForce * multiply;
        frontRightWheelCollider.motorTorque = motorForce * multiply;
    }

    private void HandleSteering(float multiply)
    {
        frontLeftWheelCollider.steerAngle = maxSteerAngle * multiply;
        frontRightWheelCollider.steerAngle = maxSteerAngle * multiply;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
