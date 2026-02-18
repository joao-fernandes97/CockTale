using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    private Camera _camera;
    private int _lastWidth;
    private int _lastHeight;

    private const float targetAspect = 16f / 9f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        UpdateAspect();
    }

    private void Update()
    {
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            UpdateAspect();
        }
    }

    private void UpdateAspect()
    {
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Rect rect = new Rect();

        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        _camera.rect = rect;
    }
}