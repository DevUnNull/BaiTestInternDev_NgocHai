using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("Cameras")]
    public CinemachineVirtualCamera vcamCharacter;
    public CinemachineVirtualCamera vcamBall;

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Thêm component chống đâm xuống sàn cho vcamBall
        if (vcamBall != null)
        {
            CameraYClamp yClamp = vcamBall.GetComponent<CameraYClamp>();
            if (yClamp == null)
            {
                yClamp = vcamBall.gameObject.AddComponent<CameraYClamp>();
                yClamp.minY = 0.5f; // Đảm bảo camera không đâm xuống sàn (có thể điều chỉnh số này)
            }
        }
    }

    public void TargetSpecificBall(Transform ballTransform)
    {
        if (vcamBall != null)
        {
            vcamBall.Follow = ballTransform;
            vcamBall.LookAt = ballTransform;
            vcamBall.Priority = 20; // Vượt mức ưu tiên của vcamCharacter (10)
        }
    }

    public void OnBallHitGoal()
    {
        StartCoroutine(ResetCameraRoutine());
    }

    public void ResetCamera()
    {
        if (vcamBall != null)
        {
            vcamBall.Priority = 5; // Giảm xuống thấp hơn vcamCharacter
            vcamBall.Follow = null;
            vcamBall.LookAt = null;
        }
    }

    private IEnumerator ResetCameraRoutine()
    {
        yield return new WaitForSeconds(2f); // Đợi 2 giây
        ResetCamera();
    }
}

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraYClamp : CinemachineExtension
{
    public float minY = 0.5f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // Chỉ giới hạn ở Body (sau khi tính toán Follow)
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            if (pos.y < minY)
            {
                pos.y = minY;
                state.RawPosition = pos;
            }
        }
    }
}
