using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Factors de camera
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float translateSpeed = 0;
    [SerializeField] private float rotationSpeed = 0;

    // Cotxe a seguir
    private Transform target = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null) {
            HandleTranslation();
            HandleRotation();
        } else {
            transform.position = new Vector3(0, 200, 0);
            transform.rotation = Quaternion.LookRotation(-transform.position,Vector3.up);
        }
    }

    // Es mou a la direcció del cotxe
    private void HandleTranslation()
    {
        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }

    // Rota per estar en direcció del cotxe
    private void HandleRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    // Serveix per designar el transform a mirar
    public void SetTarget (Transform player)
    {
        target = player;
    }
}
