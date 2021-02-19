using Photon.Pun;
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
    private bool isTabing;

    // Inputs
    private float horizontalInput;
    private float verticalInput;

    private PhotonView PV;
    private Func<GameManager> gameManager;

    private PowerUp powerUp;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(this.transform.Find("Camera").gameObject);
            Destroy(this.transform.Find("Minimap").gameObject);
        }

        // Resituem el centre de massa
        rb.centerOfMass = new Vector3 (0,-0.25f,0f); // Movem el centre de massa per que no giri
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;
        GetInput();
    }

    // Detectem inputs del teclat
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        isTabing = Input.GetKey(KeyCode.Tab);
        if (Input.GetKeyDown("Space"))
        {
            //powerUp activate
            this.powerUp.Start();
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if (UIManager.Instance.tab && !isTabing) UIManager.Instance.tab = false;
        if (!UIManager.Instance.tab && isTabing) UIManager.Instance.tab = true;

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

    public void addPowerUp(PowerUp power)
    {
        this.powerUp = power;
    }
}
