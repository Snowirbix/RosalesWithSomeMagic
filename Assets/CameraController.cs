using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{
    private void Start ()
    {
        if (isLocalPlayer)
        {
            GameObject cam = GameObject.Find("vCam");
            if (cam)
            {
                Cinemachine.CinemachineVirtualCamera vCam = cam.GetComponent<Cinemachine.CinemachineVirtualCamera>();
                vCam.Follow = transform;
                vCam.LookAt = transform;
            }
        }
    }
}
