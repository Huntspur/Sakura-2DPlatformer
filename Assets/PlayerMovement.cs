using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float maxMovementSpeed = 5f;

    [Header("Acceleration Values")]
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float decceleration = 40f;
    [SerializeField] private float turningAcceleration = 40f;

    [Header("Jumping")]

    [Header("Componenets")]
    [SerializeField] private Rigidbody2D rb;


    [Header("Debug")]
    [SerializeField] private float horizontalInputDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        float newVolicty = CalculateHorizontalVelocity();

        rb.linearVelocityX = newVolicty;
    }



    private float CalculateHorizontalVelocity()
    {
        float targetSpeed = GetTargetSpeed() * horizontalInputDirection;
        float acceleration = GetAcceleration();

        float velocity = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, acceleration * Time.deltaTime);
        return velocity;
    }



    #region Helper methods
    private bool HasHorizontalInput()
    {
        return !Mathf.Approximately(horizontalInputDirection, 0);
    }

    private float GetTargetSpeed()
    {
        if (HasHorizontalInput())
        {
            return maxMovementSpeed;
        }
        else
        {
            return 0;
        }
    }

    private bool AreOpposite(float a, float b, float eps = 0.001f)
    {
        if (Mathf.Abs(a) < eps || Mathf.Abs(b) < eps) 
        {
            return false;
        }

        return !Mathf.Approximately(Mathf.Sign(a), Mathf.Sign(b));
    }

    private float GetAcceleration()
    {
        if (HasHorizontalInput())
        {
            if (AreOpposite(horizontalInputDirection, rb.linearVelocityX))
            {
                return turningAcceleration;
            }
            else
            {
                return acceleration;
            }
        }
        else
        {
            return decceleration;
        }
    }
    #endregion


    #region Input handling
    void Update()
    {
        ReadPlayerInputs();
    }
    private void ReadPlayerInputs()
    {
        if (Keyboard.current.dKey.isPressed)
        {
            horizontalInputDirection = 1;
        }
        else if (Keyboard.current.aKey.isPressed) 
        {
            horizontalInputDirection = -1;
        }
        else
        {
            horizontalInputDirection = 0;
        }
    }
    #endregion
}
