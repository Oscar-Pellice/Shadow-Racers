using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Car : MonoBehaviour
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
    private Transform IAcar_transform;

    // Factors de control de vehicle
    [SerializeField] private float motorForce = 0;
    [SerializeField] private float maxSteerAngle = 0;

    // Control de nodes
    int nextNode = 0;
    private GameManager.PathInfo[] info;
    private Vector3 targetToGet;

    private float tResta = 0;
    private float tActual = 0;
    private const float DistMin = 2f;

    // Info de GameManager
    GameManager gameManager;
    int readerId;

    LineRenderer lineRenderer;

    // Crida al fer instance
    public void Create(int id)
    {
        readerId = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Recolleix info del gameManager
        gameManager = FindObjectOfType<GameManager>();
        info = gameManager.pathRegister[readerId].ToArray();

        // Resitua el centre de massa
        rb.centerOfMass = new Vector3(0, -0.25f, 0);

        //Troba el transform del cotxe
        IAcar_transform = transform.GetChild(0);

        tResta = Time.time;


        //For creating line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = info.Length;
        lineRenderer.useWorldSpace = true;

        //For drawing line in the world space, provide the x,y,z values
        for(int i = 0; i < info.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(info[i].GetPosition().x,1, info[i].GetPosition().z)); //x,y and z position of the starting point of the line
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Busquem al seguent node
        tActual = Time.time - tResta;
        if ( tActual > info[nextNode].GetTime() && Vector3.Distance(rb.position, info[nextNode].GetPosition()) < DistMin)
        {
            tResta = tActual - info[nextNode].GetTime();
            targetToGet = info[nextNode].GetPosition();
            nextNode = (nextNode+1) % info.Length;
            Debug.Log(nextNode);
        }
    }


    private void FixedUpdate()
    {
        HandleVelocity();
        HandleSteering(CalculateAngle());
        UpdateWheels();
    }

    // Calcula el angle entre el cotxe i el node
    private int CalculateAngle()
    {
        Vector3 targetDir = new Vector3(targetToGet.x - IAcar_transform.position.x,
                                        targetToGet.y - IAcar_transform.position.y,
                                        targetToGet.z - IAcar_transform.position.z);
        Vector3 forward = IAcar_transform.forward;
        float angle = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        if (angle < -5.0f) return -1;
        else if (angle > 5.0f) return 1;
        return 0;
    }

    private void HandleVelocity()
    {
        //HandleMotor(0.5f);
        // Com fer que es posi a la velocitat que hauria d'anar
        if (info[nextNode].GetVelocity() > rb.velocity.magnitude)
        {
            HandleMotor((rb.velocity.magnitude + info[nextNode].GetVelocity()/2)/30);
            Debug.Log(rb.velocity.magnitude + " -> " + info[nextNode].GetVelocity());
        }
        else if (info[nextNode].GetVelocity() < rb.velocity.magnitude)
        {
            Debug.Log(rb.velocity.magnitude + " -> " + info[nextNode].GetVelocity());
            HandleMotor(0);
        }
        // Si diferencia entre actual i futura es molt inferior frenar
    }

    // Aplica velocitat a totes les rodes
    private void HandleMotor(float multiply)
    {
        rearLeftWheelCollider.motorTorque = motorForce * multiply;
        rearRightWheelCollider.motorTorque = motorForce * multiply;
        frontLeftWheelCollider.motorTorque = motorForce * multiply;
        frontRightWheelCollider.motorTorque = motorForce * multiply;
    }

    // Aplica direcció a les rodes
    private void HandleSteering(float multiply)
    {
        frontLeftWheelCollider.steerAngle = maxSteerAngle * multiply;
        frontRightWheelCollider.steerAngle = maxSteerAngle * multiply;
    }

    // Situa els meshes a on estan els transforms
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
