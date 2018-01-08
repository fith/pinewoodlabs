using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnCollision : MonoBehaviour {
	public GameObject obj;
	public Material FirstPlaceMaterial;
	public Material OtherPlaceMaterial;
	static private bool done = false;

	void OnTriggerEnter(Collider other) {
		if(!done) {
			done = true;
        	obj.GetComponent<Renderer>().material = FirstPlaceMaterial;
		} else {
			obj.GetComponent<Renderer>().material = OtherPlaceMaterial;
		}
		StartCoroutine(ResetMaterial());
	}
    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(5);
        obj.GetComponent<Renderer>().material = OtherPlaceMaterial;
		done = false;
    }
}
