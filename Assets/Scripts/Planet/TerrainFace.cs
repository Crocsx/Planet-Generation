using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    int resolution;

    Vector3 axisY;
    Vector3 axisX;
    Vector3 axisZ;
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 axisY)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.axisY = axisY;

        axisX = new Vector3(axisY.y, axisY.z, axisY.x);
        axisZ = Vector3.Cross(axisY, axisX);
    }

    public void ConstructMesh() {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6]; // https://prnt.sc/r6if91
        int triIndex = 0;
        Vector2[] uv = mesh.uv;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = axisY + (percent.x - 0.5f) * 2 * axisX + (percent.y - 0.5f) * 2 * axisZ;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                // do not construct for the border of the mesh
                if(x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1 ;

                    triIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        if (mesh.uv.Length == uv.Length)
        {
            mesh.uv = uv;
        }
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = new Vector2[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = axisY + (percent.x - 0.5f) * 2 * axisX + (percent.y - 0.5f) * 2 * axisZ;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i] = new Vector2(colorGenerator.BiomePercentFromPoint(pointOnUnitSphere), 0);
            }
        }
        mesh.uv = uv;
    }
}
