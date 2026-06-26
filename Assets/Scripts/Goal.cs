using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        BallController ball = other.GetComponent<BallController>();
        if (ball == null)
            return;

        ball.OnGoalScored(this.transform);
    }
}
