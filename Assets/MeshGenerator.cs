using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public Material material;

    private Mesh cubeMesh;
    private Matrix4x4 matrix;

    void Start()
    {
        CreateCubeMesh();
        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
    }

    void Update()
    {
        DecomposeMatrix(matrix, out Vector3 pos, out Quaternion rot, out Vector3 scale);

        // Movement (A = left, D = right)
        if (Input.GetKey(KeyCode.A))
            pos += Vector3.left * Time.deltaTime * 2f;
        if (Input.GetKey(KeyCode.D))
            pos += Vector3.right * Time.deltaTime * 2f;

        // Rotation (W = rotate clockwise, S = rotate counterclockwise)
        if (Input.GetKey(KeyCode.W))
            rot *= Quaternion.Euler(0f, 90f * Time.deltaTime, 0f);
        if (Input.GetKey(KeyCode.S))
            rot *= Quaternion.Euler(0f, -90f * Time.deltaTime, 0f);

        matrix = Matrix4x4.TRS(pos, rot, scale);

        Graphics.DrawMesh(cubeMesh, matrix, material, 0);
    }

    void CreateCubeMesh()
    {
        cubeMesh = new Mesh();

        Vector3[] vertices = {
            // Front face
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),

            // Back face
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
        };

        int[] triangles = {
            // Front
            0, 2, 1, 0, 3, 2,
            // Right
            1, 2, 6, 1, 6, 5,
            // Back
            5, 6, 7, 5, 7, 4,
            // Left
            4, 7, 3, 4, 3, 0,
            // Top
            3, 7, 6, 3, 6, 2,
            // Bottom
            4, 0, 1, 4, 1, 5
        };

        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;
        cubeMesh.RecalculateNormals();
    }

    void DecomposeMatrix(Matrix4x4 matrix, out Vector3 position, out Quaternion rotation, out Vector3 scale)
    {
        position = matrix.GetColumn(3);
        rotation = matrix.rotation;
        scale = matrix.lossyScale;
    }
}
