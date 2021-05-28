﻿using Photon.Pun;
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
    private bool ableAutomatic = false;

    private PhotonView PV;

    private List<PathReader.Moment> raceInfo;
    private int nextNode = 0;
    private Vector3 targetToGet;
    private float tResta = 0;
    private float tActual = 0;
    private const float DistMin = 5f;
    //private LineRenderer lineRenderer;
    private Transform car_transform;
    private Transform front_transform;
    private Transform back_transform;

    private bool isMovable = false;

    public AudioSource audioSource;

    private Queue<PowerUp> powerUp;

    private void Awake()
    {
        powerUp = new Queue<PowerUp>();
        PV = GetComponent<PhotonView>();
        raceInfo = SaveInfo.Instance.ReturnJson();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!PV.IsMine)
        {
            Destroy(GameObject.Find(this.name + "/Camera"));
            Destroy(GameObject.Find(this.name + "/Car/Minimap"));
        }

        // Resituem el centre de massa
        rb.centerOfMass = new Vector3 (0,-0.25f,0f); // Movem el centre de massa per que no giri

        ////For creating line renderer object
        //lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.red;
        //lineRenderer.startWidth = 0.01f;
        //lineRenderer.endWidth = 0.01f;
        //lineRenderer.positionCount = raceInfo.Count;
        //lineRenderer.useWorldSpace = true;

        ////For drawing line in the world space, provide the x,y,z values
        //for (int i = 0; i < raceInfo.Count; i++)
        //{
        //    lineRenderer.SetPosition(i, new Vector3(raceInfo[i].position.x, 1, raceInfo[i].position.z)); //x,y and z position of the starting point of the line
        //}

        car_transform = transform.GetChild(0);
        front_transform = transform.GetChild(2);
        back_transform = transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;
        GetInput();

        if (isMovable)
        {
            if (ableAutomatic == true)
            {
                // Busquem al seguent node
                tActual = Time.time - tResta;
                if (tActual > raceInfo[nextNode].time && Vector3.Distance(rb.position, raceInfo[nextNode].position) < DistMin)
                {
                    //tResta = tActual - raceInfo[nextNode].time;
                    targetToGet = raceInfo[nextNode].position;
                    nextNode = (nextNode + 1) % raceInfo.Count;
                    //MultiplayerInforHolder.Instance.AddToTable(gameObject.name, nextNode);
                }
            }
        }
        EngineSound();
    }

    // Detectem inputs del teclat
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Q))
        {
            ableAutomatic = !ableAutomatic;
        } 
        isBreaking = Input.GetKey(KeyCode.Space);
        isTabing = Input.GetKey(KeyCode.Tab);
        if (Input.GetKeyDown(KeyCode.Space) && this.powerUp.Count != 0)
        {
            //powerUp activate
            powerUp.Peek().StartPoweUp();
            GameManager.Instance.pathReader.addPowerup(powerUp.Peek().id);
            powerUp.Dequeue();
            UIManager.Instance.advancePUQueue();
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;

        if (isMovable)
        {
            if (ableAutomatic == false)
            {
                HandleMotor(verticalInput);
                HandleSteering(horizontalInput);
                UpdateWheels();
                UpdateEmptyOBJS();
            }
            else
            {
                HandleVelocity();
                HandleSteering(CalculateAngle());
                UpdateWheels();
                UpdateEmptyOBJS();
            }
        }
    }

    // Calcula el angle entre el cotxe i el node
    private int CalculateAngle()
    {
        Vector3 targetDir = new Vector3(targetToGet.x - car_transform.position.x,
                                        targetToGet.y - car_transform.position.y,
                                        targetToGet.z - car_transform.position.z);
        Vector3 forward = car_transform.forward;
        float angle = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        if (angle < -5.0f) return -1;
        else if (angle > 5.0f) return 1;
        return 0;
    }

    private void HandleVelocity()
    {
        //HandleMotor(0.5f);
        // Com fer que es posi a la velocitat que hauria d'anar
        if (raceInfo[nextNode].velocity > rb.velocity.magnitude)
        {
            HandleMotor((rb.velocity.magnitude + raceInfo[nextNode].velocity / 2) / 30);
        }
        else if (raceInfo[nextNode].velocity < rb.velocity.magnitude)
        {
            HandleMotor(0);
        }
        // Si diferencia entre actual i futura es molt inferior frenar
    }

    // Apliquem força de motor
    public void HandleMotor(float multiply)
    {
        rearLeftWheelCollider.motorTorque = motorForce * multiply;
        rearRightWheelCollider.motorTorque = motorForce * multiply; 
        frontLeftWheelCollider.motorTorque = motorForce * multiply;
        frontRightWheelCollider.motorTorque = motorForce * multiply;
        
        currentbreakForce = isBreaking ? breakForce * 100 : 0f;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    // Rota les rodes
    private void HandleSteering(float multiply)
    {
        frontLeftWheelCollider.steerAngle = maxSteerAngle * multiply;
        frontRightWheelCollider.steerAngle = maxSteerAngle * multiply;
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
        Debug.Log("PowerUp From Controller Name: " + power.Name + " duration: " + power.duration + "s");
        if(powerUp.Count < 3)
        {
            powerUp.Enqueue(power);
            UIManager.Instance.addPUToQueue(power.id);
        }
    }
    public Vector3 getPosition()
    {
        return rb.position;
    }
    public void changeScale(Vector3 scale)
    {
        Vector3 pos = this.transform.localPosition;
        pos.y += 6f;
        this.transform.localPosition = pos;
        this.transform.localScale = scale;
    }
    public void jump(float hight)
    {
        rb.velocity += hight * Vector3.up;
    }
    public void SetMovement(bool valor)
    {
        isMovable = valor;

    }

    public void ResetTimer()
    {
        tActual = Time.time;
    }

    private void EngineSound()
    {
        audioSource.pitch = ((GameManager.Instance.playerGameObject.GetComponentInChildren<Rigidbody>().velocity.magnitude * 3) / 50);
    }

    public void AddBoost(int v)
    {
        rb.AddForce(rb.transform.forward * v, ForceMode.Impulse);
    }
    public void SlowDown()
    {
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        motorForce /= 2;
    }
    public void RestoreMotor()
    {
        motorForce *= 2;
    }
    public Vector3 getVelocity()
    {
        return rb.velocity;
    }
    public Vector3 getFrontPosition()
    {
        GameObject front = this.transform.GetChild(2).gameObject;
        return front.transform.position;
    }
    public Vector3 getBackPosition()
    {
        GameObject back = this.transform.GetChild(3).gameObject;
        return back.transform.position;
    }
    private void UpdateEmptyOBJS()
    {
        Vector3 pos_front = car_transform.position;
        pos_front.z += 3.22f;

        Vector3 pos_back = car_transform.position;
        
        pos_back.z += -2.6f;
     

        Quaternion rot = car_transform.rotation;

        front_transform.position = pos_front;

        back_transform.position = pos_back;

        back_transform.rotation = rot;
        front_transform.rotation = rot;
    }
}
