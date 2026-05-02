using UnityEngine;

public class ImageScaller : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        ScaleImage();
    }

    void ScaleImage()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWeight = screenHeight * aspectRatio;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float scaleY = screenHeight / spriteRenderer.sprite.bounds.size.y;
        float scaleX = screenWeight/ spriteRenderer.sprite.bounds.size.x;
        float scale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
