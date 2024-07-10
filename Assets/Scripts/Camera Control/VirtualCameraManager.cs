using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : MonoBehaviour
{
    private static VirtualCameraManager _instance;
    public static VirtualCameraManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<VirtualCameraManager>();
                _instance.name = _instance.GetType().ToString();
                // DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    [SerializeField] private float cameraLensOrtoSize = 6.5f;

    private CinemachineVirtualCamera _currentVirtualCamera;
    public CinemachineVirtualCamera currentVirtualCamera
    {
        get { return _currentVirtualCamera; }
        set
        {
            if (_currentVirtualCamera != null)
                _currentVirtualCamera.enabled = false;

            _currentVirtualCamera = value;
            _currentVirtualCamera.m_Lens = new LensSettings(0, cameraLensOrtoSize, 0.3f, 1000, 0);

            if (_currentVirtualCamera != null)
            {
                _currentVirtualCamera.enabled = true;
                _currentVirtualCamera.Follow = Player.instance.followCameraPoint;
            }
        }
    }
}
