using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform background;
    [SerializeField] private float smoothSpeed = 5f;

    private Camera cam;
    private float minX;
    private float maxX;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        SpriteRenderer[] sprites =
            background.GetComponentsInChildren<SpriteRenderer>();

        if (sprites.Length == 0)
        {
            Debug.LogError("No background sprites found");
            enabled = false;
            return;
        }

        Bounds bounds = sprites[0].bounds;

        foreach (SpriteRenderer sprite in sprites)
        {
            bounds.Encapsulate(sprite.bounds);
        }

        float cameraHalfWidth = cam.orthographicSize * cam.aspect;

        minX = bounds.min.x + cameraHalfWidth;
        maxX = bounds.max.x - cameraHalfWidth;

        if (minX > maxX)
        {
            minX = maxX = bounds.center.x;
        }
    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        float targetX = Mathf.Clamp(player.position.x, minX, maxX);

        Vector3 targetPosition = new(
            targetX,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Mathf.Clamp01(smoothSpeed * Time.deltaTime)
        );
    }
}