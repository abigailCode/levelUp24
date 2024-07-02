using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform[] rotationPoints;
    public float transitionSpeed = 2f;
    private int currentRotationIndex = 0;

    private bool isTransitioning = false;
    private Transform targetRotationPoint;

    void Start() {
        targetRotationPoint = rotationPoints[currentRotationIndex];
    }

    void Update() {
        if (isTransitioning) {
            transform.position = Vector3.Lerp(transform.position, targetRotationPoint.position, transitionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationPoint.rotation, transitionSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetRotationPoint.position) < 0.1f && Quaternion.Angle(transform.rotation, targetRotationPoint.rotation) < 1f) {
                isTransitioning = false;
            }
        }
    }

    public void Rotate(bool movingForward) {
        if (movingForward) currentRotationIndex = (currentRotationIndex + 1) % rotationPoints.Length;
        else currentRotationIndex = (currentRotationIndex - 1 + rotationPoints.Length) % rotationPoints.Length;

        targetRotationPoint = rotationPoints[currentRotationIndex];
        isTransitioning = true;
    }
}
