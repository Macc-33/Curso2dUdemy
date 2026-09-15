using Unity.Cinemachine;
using UnityEngine;

public class TargetCam : MonoBehaviour
{
    private CinemachineCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        if (GameManager.instance.PlayerController != null)
        {
            SetCameraTarget(GameManager.instance.PlayerController);
        }
    }

    private void OnEnable()
    {
        GameManager.OnPlayerSpawned += SetCameraTarget;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerSpawned -= SetCameraTarget;
    }

    private void SetCameraTarget(PlayerController newPlayer)
    {
        if (newPlayer == null) return;

        virtualCamera.Follow = newPlayer.transform;
        virtualCamera.LookAt = newPlayer.transform;
    }
}
