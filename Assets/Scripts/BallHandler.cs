using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float deatachDelay;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;

    private bool isDraging;
    private void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }
    private void Update()
    {
        if (currentBallRigidbody == null) return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDraging)
            {
                LaunchBall();
            }
            isDraging = false;
            return;
        };

        isDraging = true;
        currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        
        Invoke(nameof(DeatachBall), deatachDelay);
    }

    private void SpawnNewBall()
    {
        GameObject newBall = Instantiate(ballPrefab, pivot.transform.position, Quaternion.identity);
        currentBallSpringJoint = newBall.GetComponent<SpringJoint2D>();
        currentBallRigidbody = newBall.GetComponent<Rigidbody2D>();
        currentBallSpringJoint.connectedBody = pivot;
        
    }

    private void DeatachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
