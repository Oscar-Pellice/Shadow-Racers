using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Controladors de les rodes
    [SerializeField] private WheelCollider frontLeftWheelCollider = null;
    [SerializeField] private WheelCollider frontRightWheelCollider = null;
    [SerializeField] private WheelCollider rearLeftWheelCollider = null;
    [SerializeField] private WheelCollider rearRightWheelCollider = null;

    [SerializeField] private Transform frontLeftWheelTransform = null;
    [SerializeField] private Transform frontRightWheeTransform = null;
    [SerializeField] private Transform rearLeftWheelTransform = null;
    [SerializeField] private Transform rearRightWheelTransform = null;

    // Cos del cotxe
    [SerializeField] private Rigidbody rb = null;

    // Factors de control del cotxe
    [SerializeField] private float motorForce = 0;
    [SerializeField] private float breakForce = 0;
    [SerializeField] private float maxSteerAngle = 0;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    // Inputs
    private float horizontalInput;
    private float verticalInput;

    public int playerId = 0;
    
    public void Create(int id)
    {
        playerId = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Resituem el centre de massa
        rb.centerOfMass = new Vector3 (0,-0.25f,0.1f); // Movem el centre de massa per que no giri
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    // Detectem inputs del teclat
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    // Apliquem força de motor
    private void HandleMotor()
    {
        rearLeftWheelCollider.motorTorque = motorForce * verticalInput;
        rearRightWheelCollider.motorTorque = motorForce * verticalInput; 
        frontLeftWheelCollider.motorTorque = motorForce * verticalInput;
        frontRightWheelCollider.motorTorque = motorForce * verticalInput;

        currentbreakForce = isBreaking ? breakForce : 0f;
        //frontRightWheelCollider.brakeTorque = currentbreakForce;
        //frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;

    }

    // Rota les rodes
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    // Situa els meshes on els transforms
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
