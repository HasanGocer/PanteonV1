using UnityEngine;

public class MobileSkySphere : MonoBehaviour {

    private float x = 0f;
    [Range(0, 0.15f)]
    public float rotateSpeed = 0.02f;
    private float z = 0f;
	// Use this for initialization
	void Start () {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -1 * normals[i];
        mesh.normals = normals;

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int j = 0; j < tris.Length; j+=3)
            {
                int temp = tris[j];
                tris[j] = tris[j + 1];
                tris[j + 1] = temp;
            }
            mesh.SetTriangles(tris, i);
        }
	}
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(x, rotateSpeed, z));
    }
}
