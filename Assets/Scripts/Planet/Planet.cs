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

    Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    public enum FaceRenderMask {  All , Top, Bottom, Left, Right, Front, Back};
    public FaceRenderMask faceRenderMask;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilter;
    TerrainFace[] terrainFaces;
    private void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        if (meshFilter == null || meshFilter.Length == 0)
        {
            meshFilter = new MeshFilter[directions.Length];
        }

        terrainFaces = new TerrainFace[directions.Length];

        for (int i = 0; i < directions.Length; i++)
        {
            if(meshFilter[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilter[i] = meshObj.AddComponent<MeshFilter>();
                meshFilter[i].sharedMesh = new Mesh();
            }
            meshFilter[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.material;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilter[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilter[i].gameObject.SetActive(renderFace);
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
        for (int i = 0; i < directions.Length; i++)
        {
            if (meshFilter[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
    }
}
