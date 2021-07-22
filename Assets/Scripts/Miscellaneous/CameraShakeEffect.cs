using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    public static CameraShakeEffect camse;

    #region Public Members
    [Header("Main Camera")]
    public Camera mainCamera;

    [SerializeField]
    private float _intensity;

    [SerializeField]
    private float _duration;

    #endregion

    #region Private Members
    private float intensity;
    private Transform initPosition;
    #endregion

    void Awake()
    {
        if (camse == null)
        {
            camse = this;
            DontDestroyOnLoad(camse);
        }
        else
        {
            Destroy(gameObject);
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        
    }

    private void Start()
    {
        initPosition = mainCamera.transform;
    }

    //List of Different Camera Effects
    public void Shake()
    {
        intensity = _intensity;
        InvokeRepeating("BeginShake", 0, 0.025f);
        Invoke("StopShake", _duration);
    }

    void BeginShake()
    {
        if (intensity > 0)
        {
            Vector3 camPosition = mainCamera.transform.localPosition;

            float offsetX = Random.value * intensity * 2 - intensity;
            float offSetY = Random.value * intensity * 2 - intensity;

            camPosition.x += offsetX;
            camPosition.y += offSetY;

            mainCamera.transform.localPosition = camPosition;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
    }
}
