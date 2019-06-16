using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustRotate : MonoBehaviour {

 	[SerializeField] float m_speedValue;
	void Update () {
		transform.Rotate(m_speedValue*Vector3.forward*Time.deltaTime);
	}
}
