namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(Ship))]
	public class ShipEditor : Editor
	{

		Ship ship;
		Editor shipSettingsEditor;
		Editor shootTypeSettingsEditor;

		[MenuItem("Settings")]
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			DrawSettingsEditor(ship.m_Settings, null, ref ship.OnShipSettingsFoldout, ref shipSettingsEditor);
			// DrawSettingsEditor(ship.m_Projectile.m_Settings, null, ref ship.OnProjectileSettingsFoldout, ref shootTypeSettingsEditor);
		}

		void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
		{
			if (settings != null)
			{
				foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					if (foldout)
					{
						CreateCachedEditor(settings, null, ref editor);
						editor.OnInspectorGUI();

						if (check.changed)
						{
							if (onSettingsUpdated != null)
							{
								onSettingsUpdated();
							}
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			ship = (Ship)target;
		}
	}
}