namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SolarSystem : MonoBehaviour
	{
		public int sphereCount = 1;
		public int maxSphere = 1;
		//public GameObject[] spheres;
		public Object[] mats;
		public Object[] meshes;

		void Awake()
		{
			sphereCount = 5;
			//sphereCount = Random.Range(0,maxSphere);
			//spheres = new GameObject[sphereCount];
		}
		// Start is called before the first frame update
		void Start()
		{
			mats = Resources.LoadAll("Asteroids/Materials", typeof(Material));
			meshes = Resources.LoadAll("Asteroids/Meshes", typeof(Mesh));
			//this.GetComponent<Renderer>().material = newMat;
			//this.GetComponent<MeshFilter>().sharedMesh = (Mesh)meshes[Random.Range(0,meshes.Length)];
			//spheres = CreateSpheres(sphereCount);
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
				Destroy(sp, 20f);
				//spheres[i] = sp;
				//createCloud(sp,Random.Range(50,100));
			}

			GameObject.Destroy(sphereToCopy);
			//return spheres;
		}

		public void createCloud(GameObject test, int count)
		{
			var astToCopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			astToCopy.transform.parent = test.transform;
			astToCopy.gameObject.AddComponent<RotateAndOrbit>();
			astToCopy.gameObject.GetComponent<RotateAndOrbit>().target = test;
			var rand = Random.Range(-5, 20);
			for (int i = 0; i < count; i++)
			{
				var ast = GameObject.Instantiate(astToCopy);
				ast.transform.parent = GameManager.Instance.m_OrbitalsParent.transform;
				ast.GetComponent<MeshFilter>().sharedMesh = (Mesh)meshes[Random.Range(0, meshes.Length)];

				RotateAndOrbit orbit = ast.gameObject.GetComponent<RotateAndOrbit>();

				orbit.orbitingObject = ast.transform;
				orbit.orbitPath.xAxis = 20 + rand;
				orbit.orbitPath.yAxis = 20 + rand;
				orbit.orbitProgress = 1 / count * i;
				orbit.orbitProgress = i * Random.Range(0.0f, 1.0f);
				orbit.orbitPeriod = 10;
				ast.transform.localScale *= Random.Range(0.1f, 0.5f);
				//spheres[i] = sp;
			}

			GameObject.Destroy(astToCopy);
		}
		// Update is called once per frame
		void Update()
		{
		}
	}
}