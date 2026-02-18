using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIRaycastAlphaThreshold : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float _alphaThreshold = 0.5f;
    private Image _image;

    // All raycast events inside the Image rectangle are considered a hit. In order for greater than 0 to values to work, the sprite used by the Image must have readable pixels.
    
    /// <summary>
    /// Enable Read/Write in the advanced Texture Import Settings for the sprite and disable atlassing.
    /// </summary>
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = _alphaThreshold;
    }
}
