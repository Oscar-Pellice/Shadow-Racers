using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float translateSpeed = 0;
    [SerializeField] private float rotationSpeed = 0;

    private Transform target = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            HandleTranslation();
            HandleRotation();
        }
    }

    private void HandleTranslation()
    {
        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public void setTarget (Transform player)
    {
        target = player;
    }
}
