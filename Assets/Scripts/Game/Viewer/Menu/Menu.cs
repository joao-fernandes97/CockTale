using UnityEngine;

public class Menu : MonoBehaviour
{
    protected Settings _settings;
    [SerializeField] private Canvas canvas;
    protected static string _mainMenu = "Menu";

    private void Awake()
    {
        if ( canvas == null )
            canvas = GetComponentInChildren<Canvas>();
        UpdateCamera();

        _settings = FindFirstObjectByType<Settings>();
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateCamera()
    {
        if (ActiveUICam.ActiveUICamera != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = ActiveUICam.ActiveUICamera;
            canvas.planeDistance = 1f;
        }
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = null;
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Invoke(nameof(UpdateCamera), 0.1f);
    }

    private void Update()
    {
        if (canvas.worldCamera == null)
            UpdateCamera();
    }


    /// <summary>
    /// Opens the Settings menu, these changes will be applied everywhere in all scenes of the game.
    /// </summary>
    public void Settings()
    {
        if ( _settings == null )
            _settings = FindFirstObjectByType<Settings>();
            
        _settings.TurnOnSettings();
    }

    public virtual void Continue() {}
    public virtual void Quit()
    {
        Application.Quit();
    }
}
