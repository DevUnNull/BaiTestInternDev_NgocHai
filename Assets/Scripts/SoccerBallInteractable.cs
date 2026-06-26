using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoccerBallInteractable : MonoBehaviour, IInteractable
{
    public float kickForce = 15f;
    public float upwardKickForce = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public string GetInteractionPrompt()
    {
        return "KICK";
    }

    public void Interact(GameObject interactor)
    {
        GameObject closestGoal = GoalLocator.FindClosestGoal(transform.position);
        if (closestGoal == null) return;

        Vector3 targetPosition = closestGoal.transform.position;
        targetPosition.y += 1f;

        Vector3 direction = (targetPosition - transform.position).normalized;

        Vector3 force = direction * kickForce + Vector3.up * upwardKickForce;

        BallController ballController = GetComponent<BallController>();
        if (ballController != null)
        {
            ballController.PrepareAndKick(force);
        }
        else
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}
