using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public ColorSettings colorSettings;
    public ShapeSettings shapeSettings;
    public bool autoUpdate = true;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilter;
    TerrainFace[] terrainFaces;
    private void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        Vector3[] direction = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        if (meshFilter == null || meshFilter.Length == 0)
        {
            meshFilter = new MeshFilter[direction.Length];
        }

        terrainFaces = new TerrainFace[direction.Length];

        for (int i = 0; i < 6; i++)
        {
            if(meshFilter[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilter[i] = meshObj.AddComponent<MeshFilter>();
                meshFilter[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilter[i].sharedMesh, resolution, direction[i]);
        }
    }

    public void OnShapeSettingsUpdate()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColorSettingsUpdate()
    {
        if(autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColors()
    {
        foreach (MeshFilter m in meshFilter)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.color;
        }
    }
}
