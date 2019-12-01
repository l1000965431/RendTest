using UnityEngine;
using System;

//模型工厂
class LModelFactory
{
    public static LModel CreateMoel(string modelname)
    {
        GameObject g = UnityEngine.Resources.Load<GameObject>(modelname);
        MeshRenderer r = g.GetComponent<MeshRenderer>();
        LModel model = null;
        Mesh mesh = null;
        Material material = null;

        int veccount = 0;
        LVector[] lv = null;

        if (r != null)
        {
            MeshFilter f = g.GetComponent<MeshFilter>();
            mesh = f.sharedMesh;
            material = r.sharedMaterial;

            veccount = mesh.vertexCount;
            lv = new LVector[veccount];
            for (int i = 0; i < veccount; ++i)
            {
                lv[i] = new LVector();
                lv[i].SetPos(new LVector4(mesh.vertices[i]));
                lv[i].SetUV(1.0f - mesh.uv[i].x, 1.0f - mesh.uv[i].y);
                lv[i].Nor = new LVector4(mesh.normals[i]);
                lv[i].color.r = UnityEngine.Random.Range(0.0f, 1.0f);
                lv[i].color.g = UnityEngine.Random.Range(0.0f, 1.0f);
                lv[i].color.b = UnityEngine.Random.Range(0.0f, 1.0f);
            }
        }
        else
        {
            SkinnedMeshRenderer s = g.GetComponent<SkinnedMeshRenderer>();
            mesh = s.sharedMesh;
            material = s.sharedMaterial;

            veccount = mesh.vertexCount;
            lv = new LVector[veccount];
            for (int i = 0; i < veccount; ++i)
            {
                lv[i] = new LVector();
                lv[i].SetPos(new LVector4(mesh.vertices[i]));
                lv[i].SetUV(mesh.uv[i].x, mesh.uv[i].y);
                lv[i].Nor = new LVector4(mesh.normals[i]);
            }
        }

        if (material == null)
        {
            model = LModel.CreateLModel(lv, mesh.triangles, mesh.vertexCount, mesh.triangles.Length / 3, "1");
        }
        else
        {
            model = LModel.CreateLModel(lv, mesh.triangles, mesh.vertexCount, mesh.triangles.Length / 3, material.mainTexture);

        }

        return model;
    }



}

