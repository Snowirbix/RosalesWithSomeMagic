using UnityEngine;
using UnityEngine.Networking;

public class CameraController : MonoBehaviour
{
    private void Awake ()
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
