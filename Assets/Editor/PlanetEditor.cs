using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;

    Editor shapeEditor;
    Editor colorEditor;
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdate, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdate, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref Editor editor)
    {
        if(settings != null)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.InspectorTitlebar(true, settings);

                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    onSettingsUpdated?.Invoke();
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
