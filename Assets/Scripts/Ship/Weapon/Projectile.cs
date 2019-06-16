namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Projectile : MonoBehaviour
	{
		[SerializeField] public ProjectileSettings m_Settings;

		[Header("Movement")]
		public float m_Speed;
		public float m_Acceleration;
		public float m_MaxSpeed;


		protected ProjectileType m_Type;
		public GameObject hitPrefab;

		public void Awake()
		{			
			m_Type = ProjectileTypeFactory.Create(m_Settings);
			//Destroy(gameObject,5);
		}
		public void Start(){	
			var pos = GameManager.Instance.m_Player.transform.position;
			AudioClip clip = GetComponent<AudioSource>().clip;			
			GetComponent<AudioSource>().volume = (pos.z)/(10*(this.transform.position.z));
			float volume = GetComponent<AudioSource>().volume;
			AudioSource.PlayClipAtPoint(clip,this.gameObject.transform.position,volume);
		}
		public void InitMove(Vector3 targetDirection)
		{
			StartCoroutine(m_Type.MoveProjectile(this, targetDirection));
		}

		void OnBecameInvisible()
		{
			Destroy(gameObject);
		}
		
		void OnCollisionEnter(Collision other){
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			if (other.gameObject.layer == LayerMask.NameToLayer("undestructibleProps"))
			{
				// don't detroy undestructible props
				CollislionHit(pos,rot);
				Destroy(gameObject);
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("destructibleProps"))
			{
				CollislionHit(pos,rot);
				Destroy(other.gameObject);
				Destroy(gameObject);
			}
			if (gameObject.layer == LayerMask.NameToLayer("foeProjectile") && other.gameObject.layer == LayerMask.NameToLayer("player"))
			{
				CollislionHit(pos,rot);
				GameManager.Instance.m_Player.TakeDamage(this.m_Settings.m_Damage);
				DestroyImmediate(gameObject);
			}
			if (gameObject.layer == LayerMask.NameToLayer("playerProjectile") && other.gameObject.layer == LayerMask.NameToLayer("foe"))
			{
				CollislionHit(pos,rot);
				GameManager.Instance.m_Player.TakeDamage(this.m_Settings.m_Damage);
				DestroyImmediate(gameObject);
			}
		}
		
		void CollislionHit(Vector3 position, Quaternion rotation){
			if(hitPrefab != null){
				//Instantiate prefab
				var hitVFX = Instantiate(hitPrefab, position, rotation, GameManager.Instance.m_VFXParent.transform);	
				
				//HitSound settings
				var pos = GameManager.Instance.m_Player.transform.position;
				hitVFX.GetComponent<AudioSource>().volume = (pos.z)/(10*(this.transform.position.z));
				AudioClip clip = hitVFX.GetComponent<AudioSource>().clip;
				float volume = hitVFX.GetComponent<AudioSource>().volume;
				AudioSource.PlayClipAtPoint(clip, position, volume);
				
				//Destroy Hit
				var psHit = hitVFX.GetComponent<ParticleSystem>();
				Destroy(hitVFX,psHit.main.duration);
			}
		}
	

	}

}