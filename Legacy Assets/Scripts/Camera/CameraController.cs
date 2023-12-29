using UnityEngine;

public class CameraController : MonoBehaviour
{
    // public variables
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 0.15f;
    [SerializeField] private float distance = -0.5f;

    // updates every frame
    void FixedUpdate()
    {
        if (transform.position != target.position)
        {
            // move camera to target
            Vector3 targetPos = new(target.position.x, target.position.y, distance);

            // smooth camera movement
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
    }
}