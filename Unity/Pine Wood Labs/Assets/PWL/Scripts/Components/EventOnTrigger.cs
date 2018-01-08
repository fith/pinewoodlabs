using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnTrigger : MonoBehaviour {
	public GameObject receiver;
	public string eventName;

	void OnTriggerEnter(Collider other) {
		receiver.SendMessage(eventName);
	}
}
