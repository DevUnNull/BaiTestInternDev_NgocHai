using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool isKicked = false;

    private Rigidbody rb;
    private Coroutine monitorCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnGetKicked()
    {
        isKicked = true;

        if (CameraController.Instance != null)
        {
            CameraController.Instance.TargetSpecificBall(this.transform);
        }

        if (monitorCoroutine != null) StopCoroutine(monitorCoroutine);
        monitorCoroutine = StartCoroutine(MonitorBallMovement());
    }

    private System.Collections.IEnumerator MonitorBallMovement()
    {
        yield return new WaitForSeconds(0.5f);

        while (isKicked)
        {
            if (rb != null && rb.velocity.sqrMagnitude < 0.05f)
            {
                isKicked = false;
                if (CameraController.Instance != null)
                {
                    CameraController.Instance.ResetCamera();
                }
                break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckGoalCollision(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckGoalCollision(collision.gameObject);
    }

    private void CheckGoalCollision(GameObject other)
    {
        if (isKicked)
        {
 
            Transform current = other.transform;
            Transform goalTransform = null;

            while (current != null)
            {
                if (current.name.ToLower().Contains("goal"))
                {
                    goalTransform = current;
                    break;
                }
                current = current.parent;
            }

            if (goalTransform != null)
            {
                isKicked = false; 
                
                ParticleSystem[] particleSystems = goalTransform.GetComponentsInChildren<ParticleSystem>(true);
                foreach (ParticleSystem ps in particleSystems)
                {
                    if (ps.gameObject.name == "Confetti Explosion - Stars")
                    {
                        ps.Play();
                    }
                }

                if (CameraController.Instance != null)
                {
                    CameraController.Instance.TargetSpecificBall(this.transform);
                    CameraController.Instance.OnBallHitGoal();
                }
            }
        }
    }
}
