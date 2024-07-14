using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(BoxCollider2D))]
public class VirtualCameraTrigger : MonoBehaviour
{
    [Header("Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera virtualCamera { get { return _virtualCamera; } }

    private void OnValidate()
    {
        _virtualCamera.enabled = false;
        _virtualCamera.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SetVirtualCamera();
    }

    public void SetVirtualCamera()
        => VirtualCameraManager.instance.currentVirtualCamera = _virtualCamera;
}
