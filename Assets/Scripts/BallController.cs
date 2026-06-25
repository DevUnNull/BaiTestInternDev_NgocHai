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
        
        // Theo bóng ngay khi được đá
        if (CameraController.Instance != null)
        {
            CameraController.Instance.TargetSpecificBall(this.transform);
        }

        // Bắt đầu kiểm tra xem bóng có dừng lại mà không vào gôn không
        if (monitorCoroutine != null) StopCoroutine(monitorCoroutine);
        monitorCoroutine = StartCoroutine(MonitorBallMovement());
    }

    private System.Collections.IEnumerator MonitorBallMovement()
    {
        // Đợi một chút để bóng có gia tốc sau khi đá
        yield return new WaitForSeconds(0.5f);

        // Chờ đến khi bóng dừng hẳn
        while (isKicked)
        {
            if (rb != null && rb.velocity.sqrMagnitude < 0.05f)
            {
                // Bóng đã dừng nhưng chưa vào gôn, trả lại camera cho người chơi
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
        // Chỉ xử lý nếu quả bóng đang trong trạng thái được sút
        if (isKicked)
        {
            // Kiểm tra xem object va chạm hoặc cha của nó có chữ "goal" không
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
                isKicked = false; // Reset cờ để tránh gọi nhiều lần
                
                // Tìm và chạy hiệu ứng Confetti từ gốc khung thành
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
