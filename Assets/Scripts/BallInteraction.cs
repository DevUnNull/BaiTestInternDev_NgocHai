using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BallInteraction : MonoBehaviour
{
    public Button kickButton;
    public float interactionDistance = 2.5f;
    public float kickForce = 15f;
    public float upwardKickForce = 5f;

    private GameObject closestBall;

    void Start()
    {
        if (kickButton != null)
        {
            kickButton.gameObject.SetActive(false);
            kickButton.onClick.AddListener(KickBall);
        }
    }

    void Update()
    {
        FindClosestBall();

        if (closestBall != null && Vector3.Distance(transform.position, closestBall.transform.position) <= interactionDistance)
        {
            if (kickButton != null && !kickButton.gameObject.activeSelf)
            {
                kickButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (kickButton != null && kickButton.gameObject.activeSelf)
            {
                kickButton.gameObject.SetActive(false);
            }
        }
    }

    private void FindClosestBall()
    {
        GameObject[] balls = GameObject.FindObjectsOfType<GameObject>();
        float minDistance = float.MaxValue;
        closestBall = null;

        foreach (GameObject ball in balls)
        {
            if (ball.name.StartsWith("Soccer Ball"))
            {
                float distance = Vector3.Distance(transform.position, ball.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestBall = ball;
                }
            }
        }
    }

    public void KickBall()
    {
        if (closestBall == null) return;

        Rigidbody rb = closestBall.GetComponent<Rigidbody>();
        if (rb == null) return;

        GameObject closestGoal = FindClosestGoal(closestBall.transform.position);
        if (closestGoal == null) return;

        // Calculate direction to goal
        Vector3 targetPosition = closestGoal.transform.position;
        // Adjust target position to center of the goal loosely (goals might have their pivot at the base)
        targetPosition.y += 1f; 

        Vector3 direction = (targetPosition - closestBall.transform.position).normalized;

        // Apply force
        Vector3 force = direction * kickForce + Vector3.up * upwardKickForce;
        rb.AddForce(force, ForceMode.Impulse);

        // Hide button immediately after kicking
        if (kickButton != null)
        {
            kickButton.gameObject.SetActive(false);
        }
    }

    private GameObject FindClosestGoal(Vector3 ballPosition)
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        float minDistance = float.MaxValue;
        GameObject closestGoal = null;

        foreach (GameObject obj in objects)
        {
            if (obj.name.ToLower().Contains("goal"))
            {
                float distance = Vector3.Distance(ballPosition, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestGoal = obj;
                }
            }
        }
        return closestGoal;
    }
}
