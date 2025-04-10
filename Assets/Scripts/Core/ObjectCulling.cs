using UnityEngine;

public class ObjectCulling : MonoBehaviour
{
    public static bool IsCullable(Object3D object3D)
    {
        Camera cam = Camera.main;
        
        Vector3 cameraPosition = cam.transform.position;
        Vector3 cameraDirection = cam.transform.forward;

        Vector3 direction = (object3D.transform.position - cameraPosition).normalized;
        float fovThreshold = Mathf.Cos((cam.fieldOfView * .5f) * Mathf.Deg2Rad);
        float dot = Vector3.Dot(cameraDirection, direction);
        
        float scale = Mathf.Max(object3D.size.x, object3D.size.y, object3D.size.z);
        float scaleBias = Mathf.Clamp01(scale / cam.fieldOfView);
        
        float visibilityThreshold = Mathf.Lerp(fovThreshold, -1f, scaleBias);
        
        return dot >= visibilityThreshold;
    }
}
