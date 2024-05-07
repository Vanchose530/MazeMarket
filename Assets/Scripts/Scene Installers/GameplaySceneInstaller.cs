using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneInstaller : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform playerStart;

    [Header("Player Camera")]
    [SerializeField] private Camera playerCameraPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCameraPrefab;
    [SerializeField] private Collider2D cameraConfinerShape;

    [Header("Level Music")]
    [SerializeField] private AudioClip levelMusic;

    [Header("Level UI")]
    [SerializeField] private GameObject gameplayCanvasPrefab;

    private void Awake()
    {
        AudioManager.instance.SetMusic(levelMusic);
        AudioManager.instance.normalSnapshot.TransitionTo(0.1f);
        CursorManager.instance.aimVisible = true;

        if (gameplayCanvasPrefab.GetComponent<Canvas>() != null)
        {
            Instantiate(gameplayCanvasPrefab);
        }
        else
        {
            Debug.LogError("Gamplay Canvas prefab doesn't contains Canvas");
        }

        Player playerInGame = Instantiate(playerPrefab, playerStart.position, playerStart.rotation);

        Instantiate(playerCameraPrefab); // сделать проверку на наличие камеры в сцене
        CinemachineVirtualCamera virtualCameraInGame =  Instantiate(virtualCameraPrefab);

        virtualCameraInGame.Follow = playerInGame.followCameraPoint;

        if (cameraConfinerShape != null)
        {
            virtualCameraInGame.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = cameraConfinerShape;
        }
    }
}
