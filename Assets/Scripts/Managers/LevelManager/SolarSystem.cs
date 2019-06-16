namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SolarSystem : MonoBehaviour
	{
		private int sphereCount;
		public Object[] mats;
		public Object[] meshes;

		void Awake()
		{
			sphereCount = 2;
			Destroy(this, 5);
		}
		// Start is called before the first frame update
		void Start()
		{
			mats = Resources.LoadAll("Asteroids/Materials", typeof(Material));
			meshes = Resources.LoadAll("Asteroids/Meshes", typeof(Mesh));
			CreateSpheres(sphereCount);
		}

		public void CreateSpheres(int count)
		{
			var sphs = new GameObject[count];
			var sphereToCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphereToCopy.transform.parent = this.gameObject.transform;
			sphereToCopy.gameObject.AddComponent<OrbitMotion>();
			for (int i = 0; i < count; i++)
			{
				var sp = GameObject.Instantiate(sphereToCopy);
				sp.transform.parent = GameManager.Instance.m_OrbitalsParent.transform;
				sp.GetComponent<Renderer>().material = (Material)mats[Random.Range(0, mats.Length)];
				sp.GetComponent<MeshFilter>().sharedMesh = (Mesh)meshes[Random.Range(0, meshes.Length)];

				OrbitMotion orbit = sp.gameObject.GetComponent<OrbitMotion>();

				orbit.orbitingObject = sp.transform;
				orbit.orbitPath.xAxis = Random.Range(30, (float)i * 25f + 50f);
				orbit.orbitPath.yAxis = Random.Range(30, (float)i * 25f + 50f);
				orbit.orbitPath.z = this.gameObject.transform.position.z;
				orbit.orbitPeriod = i + 10;
				orbit.orbitProgress = i * Random.Range(0.0f, 1.0f);
				sp.transform.localScale *= Random.Range(0.1f, 3f);
			}
			GameObject.Destroy(sphereToCopy);
		}
		// Update is called once per frame
		void Update()
		{
		}
	}
}