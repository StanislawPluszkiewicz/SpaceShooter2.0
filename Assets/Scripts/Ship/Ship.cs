namespace ESIEE_UNITY_ETS
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Ship : MonoBehaviour
    {
		//	  Système Tir(paterne de mouvement des projectiles)
		//    Déplacements (sur le pavé droit)
		//    Défense(shield bleu -> ionique / rouge -> laser)


		#region Settings
		[SerializeField]  public ShipSettings m_Settings;
		[HideInInspector] public bool OnShipSettingsFoldout;
		#endregion

		[HideInInspector] public List<Weapon> m_Weapons;

		protected Transform m_gfx;
		protected Rigidbody m_rb;
		[HideInInspector] public LayerMask m_PlatformLayerMask;

		public float m_ShipWidth;
		private int m_MovementEdgeResolveIterations = 50;
		private float m_MovementEdgeDstThreshold = 2f;
		private float m_MovementMaxRotationDegrees = 180f;
		private float m_MovementViewRadius = 120f;
		
        private enum MovementState { ABOVE_FACE, SWAPPING_FACE }
        private MovementState m_MovementState;
        private Quaternion m_dstRotation;
        private Quaternion m_startRotation;

		private Vector3 forward;


		#region Init
		public void Awake()
		{
			m_PlatformLayerMask = LayerMask.GetMask("Plateforme");
            m_MovementState = MovementState.ABOVE_FACE;
			m_Weapons = new List<Weapon>(m_Settings.m_WeaponPrefabs);
		}
		public void Start()
		{
			// Get basic components
			m_rb = GetComponent<Rigidbody>();
			m_gfx = transform.GetChild(0);
			this.m_Settings.m_CurrentHealthPoints = this.m_Settings.m_MaxHealthPoints;

			Transform weaponsEmplacement = transform.Find("weaponsEmplacement");
			int i = 0;
			foreach (Weapon weaponPrefab in m_Settings.m_WeaponPrefabs)
			{
				Transform weaponEmplacementParent = weaponsEmplacement.GetChild(i);
				Weapon instance = Instantiate(weaponPrefab, weaponEmplacementParent.position, Quaternion.identity, weaponsEmplacement) as Weapon;
				instance.transform.position = weaponEmplacementParent.position;
				m_Weapons[i] = instance;
				Destroy(weaponEmplacementParent.gameObject);
				i++;
			}

			// l'instruction ci-dessous ne marche pas dans la boucle ci-dessus
			foreach (Weapon weapon in m_Settings.m_WeaponPrefabs)
			{
				weapon.m_CooldownTimeStamp = 0f;
			}
		}

		#endregion

		public void Update()
		{
		}

		public void LateUpdate()
		{
			forward = transform.forward;
			transform.LookAt(transform.position + forward, transform.up);
		}

		#region Shoot
		protected void Shoot(Vector3 shooterPosition, Vector3 direction)
		{
			foreach (Weapon weapon in m_Weapons)
			{
				weapon.Shoot(shooterPosition, direction);
			}
		}
		#endregion
		#region Movement
//TODO: the player rotation isn't properly set once changing a face -> rotation has to perpendicular to the face on which the player is otherwise the ship player will move by itself which is a behaviour to remove.

		private bool GetRaycastDownAtNewPosition(Vector3 movementDirection, out RaycastHit hitInfo)
		{
			Vector3 newPosition = transform.position;
			Ray ray = new Ray(transform.position + movementDirection * (m_Settings.m_MoveSpeed * .01f + m_Settings.m_AutoSpeedForward * .01f), -transform.up);
			if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, LayerMask.GetMask("World")))
			{
				return true;
			}
			return false;
		}
		private bool GetDoubleRaycastDown(float halfWidth, out RaycastHit rightHitInfo, out RaycastHit leftHitInfo)
		{
			/// source: http://www.asteroidbase.com/devlog/7-learning-how-to-walk/
			Ray leftRay = new Ray(transform.position - halfWidth * transform.right, -transform.up);
			Ray rightRay = new Ray(transform.position + halfWidth * transform.right, -transform.up);

			bool bLeft = Physics.Raycast(leftRay, out leftHitInfo, float.PositiveInfinity, m_PlatformLayerMask);
			bool bRight = Physics.Raycast(rightRay, out rightHitInfo, float.PositiveInfinity, m_PlatformLayerMask);

			return bLeft && bRight;
		}


		private void StickShipToGround(RaycastHit leftHitInfo, RaycastHit rightHitInfo)
		{
			Vector3 averageNormal = (leftHitInfo.normal + rightHitInfo.normal) / 2;
			Vector3 averagePoint = (leftHitInfo.point + rightHitInfo.point) / 2;

			Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, averageNormal);
			Quaternion finalRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_MovementMaxRotationDegrees);

			transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.deltaTime * m_Settings.m_LIRotation);
			transform.position = Vector3.Lerp(transform.position, averagePoint + (averageNormal * LevelManager.Instance.m_ShipDistanceToGround), Time.deltaTime * m_Settings.m_LIPosition);
		}

		private void HandlePlateformeSideChangement(RaycastHit leftHitInfo, RaycastHit rightHitInfo)
		{
			Quaternion rotation = Quaternion.identity;
			EdgeInfo edgeInfo;
			float distanceMinToEdge, distanceMaxToEdge;

			ViewCastInfo rightViewCast = ViewCast(Vector3.Angle(-transform.up, rightHitInfo.point));
			ViewCastInfo leftViewCast = ViewCast(Vector3.Angle(-transform.up, leftHitInfo.point));

            ViewCastInfo outVc;
            ViewCastInfo inVc;

            RaycastHit outHit;
            RaycastHit inHit;


			if (leftHitInfo.distance == 0)
			{
                m_dstRotation = Quaternion.AngleAxis(90, Vector3.forward);
                inVc = rightViewCast;
                inHit = rightHitInfo;

                outVc = leftViewCast;
                outHit = leftHitInfo;
			}
			else
			{
                m_dstRotation = Quaternion.AngleAxis(90, -Vector3.forward);
                inVc = leftViewCast;
                inHit = leftHitInfo;

                outVc = rightViewCast;
                outHit = rightHitInfo;
			}

            edgeInfo = FindEdge(inVc, outVc);

            Vector3 edge = edgeInfo.pointA;
            edge.z = 0; 

            //edge += rightHitInfo.normal * LevelManager.Instance.m_ShipDistanceToGround;

            distanceMinToEdge = Vector3.Distance(inHit.point, edge);
            distanceMaxToEdge = Vector3.Distance(outHit.point, edge);

            float t = distanceMinToEdge / (distanceMinToEdge + distanceMaxToEdge); // todo fix this t variable to smooth the rotation
			transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * m_dstRotation, t * Time.deltaTime);
		}
		protected void KeepAboveGround(Vector3 movementDirection)
		{
            if (GetDoubleRaycastDown(m_ShipWidth, out RaycastHit rightHitInfo, out RaycastHit leftHitInfo))
            {
                m_MovementState = MovementState.ABOVE_FACE;
                StickShipToGround(leftHitInfo, rightHitInfo);
            }
            else
            {
                if (m_MovementState == MovementState.ABOVE_FACE)
                {
                    m_startRotation = transform.rotation;
                    m_MovementState = MovementState.SWAPPING_FACE;
                }
                HandlePlateformeSideChangement(leftHitInfo, rightHitInfo);
            }

            //if (m_MovementState == MovementState.SWAPPING_FACE && (transform.rotation == m_startRotation || transform.rotation == m_dstRotation || transform.rotation == Quaternion.Lerp(m_startRotation, m_dstRotation, 0.5f)))
            //// ca c'est ca https://gyazo.com/1d011f6df36221b7d5d18c3624e9bdee
            //{
            //    m_MovementState = MovementState.ABOVE_FACE;
            //}
        }
		
		EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
		{
			float minAngle = minViewCast.angle;
			float maxAngle = maxViewCast.angle;
			Vector3 minPoint = Vector3.zero;
			Vector3 maxPoint = Vector3.zero;

			for (int i = 0; i < m_MovementEdgeResolveIterations; i++)
			{
				float angle = (minAngle + maxAngle) / 2;
				ViewCastInfo newViewCast = ViewCast(angle);

				bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > m_MovementEdgeDstThreshold;
				if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
				{
					minAngle = angle;
					minPoint = newViewCast.point;
				}
				else
				{
					maxAngle = angle;
					maxPoint = newViewCast.point;
				}
			}

			return new EdgeInfo(minPoint, maxPoint);
		}

		ViewCastInfo ViewCast(float globalAngle)
		{
			Vector3 dir = DirFromAngle(globalAngle, true);
			RaycastHit hit;

			if (Physics.Raycast(transform.position, dir, out hit, m_MovementViewRadius, m_PlatformLayerMask))
			{
				return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
			}
			else
			{
				return new ViewCastInfo(false, transform.position + dir * m_MovementViewRadius, m_MovementViewRadius, globalAngle);
			}
		}

		public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
			{
				angleInDegrees += transform.eulerAngles.y;
			}
			return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
		}

		public struct ViewCastInfo
		{
			public bool hit;
			public Vector3 point;
			public float dst;
			public float angle;

			public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
			{
				hit = _hit;
				point = _point;
				dst = _dst;
				angle = _angle;
			}
		}

		public struct EdgeInfo
		{
			public Vector3 pointA;
			public Vector3 pointB;

			public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
			{
				pointA = _pointA;
				pointB = _pointB;
			}
		}


		protected void Move(Vector3 movementDirection)
		{
			transform.Translate(movementDirection * m_Settings.m_MoveSpeed * Time.deltaTime);
			KeepAboveGround(movementDirection.normalized);
		}
		#endregion
		#region Debug
		public void OnDrawGizmos()
		{
			// FOR DEBUG PURPOSES
		}
		#endregion
		#region General

		#endregion
	}
}