using UnityEngine;

public class CanvasSetup : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _orderInLayer;
    private void Update()
    {
        if (_canvas.worldCamera == null)
        {
            Camera cam = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
            if (cam == null) return;

            _canvas.renderMode = RenderMode.ScreenSpaceCamera;

            _canvas.planeDistance = 2f;
            _canvas.sortingOrder = _orderInLayer;
            _canvas.worldCamera = cam;
        }
    }
}
