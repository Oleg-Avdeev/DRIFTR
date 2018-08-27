using System.Collections.Generic;
using UnityEngine;

namespace Game.MeshGeneration
{
    [ExecuteInEditMode]
    public class PlayerMesh : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshFilter meshFilter;

        private Mesh mesh;

        void Awake()
        {
            GenerateMesh();
        }

        public void GenerateMesh()
        {
            mesh = new Mesh();
            List<Vector3> veritces = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indexes = new List<int>();

            veritces.Add(new Vector3(0, 0.8f, 0)); normals.Add(veritces[0]);
            veritces.Add(new Vector3(0.3f, 0, 0.3f)); normals.Add(veritces[1]);
            veritces.Add(new Vector3(-0.3f, 0, 0.3f)); normals.Add(veritces[2]);
            veritces.Add(new Vector3(0.3f, 0, -0.3f)); normals.Add(veritces[3]);
            veritces.Add(new Vector3(-0.3f, 0, -0.3f)); normals.Add(veritces[4]);
            veritces.Add(new Vector3(0, -0.1f, 0)); normals.Add(veritces[5]);

            indexes.Add(0);
            indexes.Add(2);
            indexes.Add(1);

            indexes.Add(0);
            indexes.Add(3);
            indexes.Add(4);
            
            indexes.Add(0);
            indexes.Add(1);
            indexes.Add(3);
            
            indexes.Add(0);
            indexes.Add(4);
            indexes.Add(2);

            indexes.Add(5);
            indexes.Add(1);
            indexes.Add(2);
            
            indexes.Add(5);
            indexes.Add(4);
            indexes.Add(3);
            
            indexes.Add(5);
            indexes.Add(3);
            indexes.Add(1);
            
            indexes.Add(5);
            indexes.Add(2);
            indexes.Add(4);

            mesh.SetVertices(veritces);
            mesh.SetTriangles(indexes.ToArray(), 0);
            mesh.SetNormals(normals);
            meshFilter.mesh = mesh;

        }
    }
}