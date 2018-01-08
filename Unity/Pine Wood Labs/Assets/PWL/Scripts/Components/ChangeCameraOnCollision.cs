using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraOnCollision : MonoBehaviour
{
    public Camera[] cameras;
    void OnTriggerEnter(Collider other)
    {
		foreach(Camera camera in Camera.allCameras) {
			camera.gameObject.SetActive(false);
		}
        int i = Random.Range(0, cameras.Length);
        cameras[i].gameObject.SetActive(true);
    }
}
