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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (vcamBall != null)
        {
            CameraYClamp yClamp = vcamBall.GetComponent<CameraYClamp>();
            if (yClamp == null)
            {
                yClamp = vcamBall.gameObject.AddComponent<CameraYClamp>();
                yClamp.minY = 0.5f; 
            }
        }
    }

    public void TargetSpecificBall(Transform ballTransform)
    {
        if (vcamBall != null)
        {
            vcamBall.Follow = ballTransform;
            vcamBall.LookAt = ballTransform;
            vcamBall.Priority = 20; 
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
            vcamBall.Priority = 5; 
            vcamBall.Follow = null;
            vcamBall.LookAt = null;
        }
    }

    private IEnumerator ResetCameraRoutine()
    {
        yield return new WaitForSeconds(2f); 
        ResetCamera();
    }
}

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] 
public class CameraYClamp : CinemachineExtension
{
    public float minY = 0.5f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
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
