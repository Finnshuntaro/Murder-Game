using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;

    public enum BillboardType { LookAtCamera, CameraForward };

    void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 directionToCamera = cameraPosition - transform.position;

        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                // Zero out Y component of the direction to keep it upright
                directionToCamera.y = 0;
                if (directionToCamera != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(directionToCamera);
                break;

            case BillboardType.CameraForward:
                // Get camera's forward direction and project it onto the XZ plane
                Vector3 cameraForward = Camera.main.transform.forward;
                cameraForward.y = 0;
                if (cameraForward != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(cameraForward);
                break;
        }
    }
}
