using UnityEngine;

public class WeaponFollow : MonoBehaviour
{
    public Transform cameraTransform;   
    public Vector3 offset = new Vector3(0.3f, -0.3f, 0.6f); 
    public float followSpeed = 20f;     

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        
        transform.rotation = cameraTransform.rotation;

        
        Vector3 targetPosition = cameraTransform.position + cameraTransform.TransformVector(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
