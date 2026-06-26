using UnityEngine;
using UnityEngine.UI;

public class AutoKickController : MonoBehaviour
{
    public Button autoKickButton;
    public Transform playerTransform;

    private void Start()
    {
        if (autoKickButton != null)
        {
            autoKickButton.onClick.AddListener(OnAutoKickClicked);
        }
        
        if (playerTransform == null)
        {
            GameObject jammo = GameObject.Find("Jammo");
            if (jammo != null)
            {
                playerTransform = jammo.transform;
            }
        }
    }

    private void OnAutoKickClicked()
    {
        if (playerTransform == null) return;

        SoccerBallInteractable[] balls = FindObjectsOfType<SoccerBallInteractable>();
        if (balls.Length == 0) return;

        float maxDistance = -1f;
        SoccerBallInteractable furthestBall = null;

        foreach (var ball in balls)
        {
            float dist = Vector3.Distance(playerTransform.position, ball.transform.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                furthestBall = ball;
            }
        }

        if (furthestBall != null)
        {
            furthestBall.Interact(playerTransform.gameObject);
        }
    }
}
