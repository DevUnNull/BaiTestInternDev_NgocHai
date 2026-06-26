using UnityEngine;
using Cinemachine;

public class BallController : MonoBehaviour
{
    private bool isKicked = false;

    private Rigidbody rb;
    private Coroutine monitorCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PrepareAndKick(Vector3 force)
    {
        StartCoroutine(KickRoutine(force));
    }

    private System.Collections.IEnumerator KickRoutine(Vector3 force)
    {
        if (CameraController.Instance != null)
        {
            CameraController.Instance.TargetSpecificBall(this.transform);
        }

        yield return new WaitForSeconds(0.1f);

        CinemachineBrain brain = Camera.main != null ? Camera.main.GetComponent<CinemachineBrain>() : null;
        if (brain != null)
        {
            while (brain.IsBlending)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(0.2f);

        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }

        isKicked = true;

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
