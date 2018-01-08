using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ExtrudeShape {
    public static Mesh Generate(List<Vector2> points, float depth)
    {
        float depth_offset = depth / 2.0f;

        Mesh body = extrudeShape(points, depth);
        Mesh left_cap = capMesh(points, depth_offset);
        Mesh right_cap = capMesh(points, -depth_offset, true);

        CombineInstance[] combine = new CombineInstance[3];
        Matrix4x4 matrix = Matrix4x4.identity;
        combine[0].mesh = body;
        combine[0].transform = Matrix4x4.identity;
        combine[1].mesh = left_cap;
        combine[1].transform = Matrix4x4.identity;
        combine[2].mesh = right_cap;
        combine[2].transform = Matrix4x4.identity;

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);
		combinedMesh.RecalculateNormals();
		combinedMesh.RecalculateBounds();
		combinedMesh.RecalculateTangents();
        
        return combinedMesh;
    }

    static Mesh capMesh(List<Vector2> points, float offset_x, bool flip_normals = false)
    {
        // Use the triangulator to get indices for creating triangles
        Vector2[] vertices2D = points.ToArray();
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(offset_x, vertices2D[i].y, vertices2D[i].x);
        }

        if (flip_normals)
        {
            // wind the other way to turn it inside out.
            Array.Reverse(indices);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        return msh;
    }

    static Mesh extrudeShape(List<Vector2> points, float depth)
    {
        float z = depth / 2.0f;
        List<Vector2> ShapeVertices2d = points;
        List<Vector3> ShapeVertices3d = new List<Vector3>();

        int vertsInShape = ShapeVertices2d.Count;
        int vertCount = vertsInShape * 2;
        var normals = new Vector3[vertCount];
        var uvs = new Vector2[vertCount];

        int index = 0;
        foreach (Vector2 v2 in ShapeVertices2d)
        {
            // Switch z and y for Unity coordinate system.
            ShapeVertices3d.Add(new Vector3(z, v2.y, v2.x));
            normals[index] = -Vector3.forward;
            uvs[index] = new Vector2(v2.x, 0);
            // right
            ShapeVertices3d.Add(new Vector3(-z, v2.y, v2.x));
            normals[index + vertsInShape] = -Vector3.forward;
            uvs[index + vertsInShape] = new Vector2(v2.x, 1);

            index++;
        }

        var vertices = ShapeVertices3d.ToArray();
        var triangleIndices = new List<int>(vertCount * 3);

        // Wind indices. 
        // Extremely simplified since we only need 
        // a single triangle strip.
        for (int j = 0; j < vertCount; j += 2)
        {
            int[] t1 = { j, j + 1, j + 2 };
            int[] t2 = { j + 1, j + 3, j + 2 };
            // Ensure the final triangle loops to 0 and 1.
            if (j + 3 > vertCount)
            {
                t1[2] = 0;
                t2[1] = 1;
                t2[2] = 0;
            }
            triangleIndices.AddRange(t1);
            triangleIndices.AddRange(t2);
        }
        //triangleIndices.Reverse();

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangleIndices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}