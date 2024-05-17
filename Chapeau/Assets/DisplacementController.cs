using Seacore;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Movement speed of the objects
    public float repelForce = 5f; // Force to repel from other objects
    public float borderRepelForce = 5f; // Force to repel from the screen borders
    public float circleRepelForce = 5f; // Force to repel from the custom positioned circle
    public float circleRadius = 5f; // Radius of the custom positioned circle
    public Transform circleCenter; // Center of the custom positioned circle

    [SerializeField]
    private List<Die> Dice;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        MoveObjects();
    }

    void MoveObjects()
    {
        foreach (Die die in Dice)
        {
            MoveObject(die);
        }
    }

    void MoveObject(Die obj)
    {
        // Calculate the target position
        Vector3 targetPosition = obj.transform.position;

        // Repel from the screen borders
        Vector3 screenBorderRepel = RepelFromScreenBorder(obj.transform.position);
        targetPosition += screenBorderRepel * borderRepelForce;

        // Repel from the custom positioned circle
        Vector3 circleRepel = RepelFromCircle(obj.transform.position);
        targetPosition += circleRepel * circleRepelForce;

        // Repel from other objects
        Vector3 objectRepel = RepelFromObjects(obj.transform.position);
        targetPosition += objectRepel * repelForce;

        // Move the object towards the target position
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    Vector3 RepelFromScreenBorder(Vector3 position)
    {
        Vector3 repel = Vector3.zero;

        // Get the screen borders
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(position);
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Repel from the borders
        if (screenPosition.x < 0)
            repel.x = 1;
        if (screenPosition.x > screenWidth)
            repel.x = -1;
        if (screenPosition.y < 0)
            repel.y = 1;
        if (screenPosition.y > screenHeight)
            repel.y = -1;

        return repel.normalized;
    }

    Vector3 RepelFromCircle(Vector3 position)
    {
        Vector3 repel = Vector3.zero;

        // Repel from the circle
        if (circleCenter != null)
        {
            float distance = Vector3.Distance(position, circleCenter.position);
            if (distance < circleRadius)
            {
                repel = position - circleCenter.position;
                repel.Normalize();
            }
        }

        return repel;
    }

    Vector3 RepelFromObjects(Vector3 position)
    {
        Vector3 repel = Vector3.zero;

        // Find all objects with ObjectDisplacement script

        foreach (Die obj in Dice)
        {
            if (obj.transform.position != position)
            {
                float distance = Vector3.Distance(position, obj.transform.position);
                if (distance < 1) // Adjust this value according to your needs
                {
                    repel += (position - obj.transform.position).normalized;
                }
            }
        }

        return repel.normalized;
    }
}
