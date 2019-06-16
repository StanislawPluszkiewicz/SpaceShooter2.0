namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class RotateAndOrbit : MonoBehaviour
	{
		public Transform orbitingObject;
		public GameObject target;
		public Ellipse orbitPath;

		[Range(0f, 1f)]
		public float orbitProgress = 0f;
		public float orbitPeriod = 3f;
		public bool orbitActive = true;

		// Start is called before the first frame update
		void Start()
		{
			if (orbitingObject == null)
			{
				orbitActive = false;
				return;
			}
			SetOrbitingObjectPosition();
			StartCoroutine(AnimateOrbit());

		}
		void SetOrbitingObjectPosition()
		{
			Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
			Vector2 targetPos = target.GetComponent<OrbitMotion>().orbitPath.Evaluate(target.GetComponent<OrbitMotion>().orbitProgress);
			orbitingObject.localPosition = new Vector3(orbitPos.x + targetPos.x, 0, orbitPos.y + targetPos.y);
		}
		IEnumerator AnimateOrbit()
		{
			if (orbitPeriod < 0.1f)
			{
				orbitPeriod = 0.1f;
			}
			float orbitSpeed = 1f / orbitPeriod;
			while (orbitActive)
			{
				orbitProgress += Time.deltaTime * orbitSpeed;
				orbitProgress %= 1f;
				SetOrbitingObjectPosition();
				yield return null;
			}
		}

		void Update()
		{
		}
	}
}