namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Foe))]
	public class FoeEditor : ShipEditor
	{

		[MenuItem("Settings")]
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}