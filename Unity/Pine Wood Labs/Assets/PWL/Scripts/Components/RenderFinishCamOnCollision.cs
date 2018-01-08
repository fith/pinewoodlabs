using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFinishCamOnCollision : MonoBehaviour
{
    static private bool shot = false;
    void Start()
    {
        var cameras = GameObject.FindGameObjectsWithTag("FinishCam");
        foreach (GameObject go in cameras)
        {
            go.GetComponent<Camera>().enabled = false;
        }
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (!shot)
        {
            shot = true;
            var cameras = GameObject.FindGameObjectsWithTag("FinishCam");
            int i = Random.Range(0, cameras.Length);
            cameras[i].GetComponent<Camera>().Render();
            StartCoroutine(ResetCamera());
        }
    }

    IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(5);
        shot = false;
    }
}
