namespace ESIEE_UNITY_ETS
{
	using UnityEngine;

	public class CameraController : SimpleGameStateObserver
	{
		Transform m_Target;
		[SerializeField] float m_Height;
		[SerializeField] float m_Distance;
		[SerializeField] float m_Damping;
		[SerializeField] float m_RotationDamping;


		Transform m_Transform;
		Vector3 m_InitPosition;

		void ResetCamera()
		{
			m_Transform.position = m_InitPosition;
		}

		public static void Init()
		{
			GameManager.Instance.m_Camera = Instantiate(GameManager.Instance.m_CameraPrefab, GameManager.Instance.m_CameraSpawnPosition, Quaternion.identity, GameManager.Instance.m_DynamicParent.transform) as CameraController;
		}

		protected override void Awake()
		{
			base.Awake();
			m_Transform = transform;
			m_InitPosition = m_Transform.position;
		}

		private void Start()
		{
			m_Target = GameManager.Instance.m_Player.transform;
			GameManager.Instance.m_Camera.transform.LookAt(GameManager.Instance.m_Player.transform);
		}

		private void LateUpdate()
		{
			// Calculate and set camera position
			Vector3 desiredPosition = m_Target.TransformPoint(0, m_Height, -m_Distance);
			transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * m_Damping);

			// Calculate and set camera rotation
			Quaternion desiredRotation = Quaternion.LookRotation(m_Target.position - transform.position, m_Target.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * m_RotationDamping);
		}

		protected override void GameMenu(GameMenuEvent e)
		{
			ResetCamera();
		}
	}
}