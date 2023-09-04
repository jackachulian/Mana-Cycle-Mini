using System.Collections;
using TMPro;
using UnityEngine;

public class WavyText : MonoBehaviour
{
    TMP_Text text;

    Mesh mesh;

    Vector3[] vertices;

    [SerializeField] private Vector2 wobbleNormal = new Vector2(2.5f, 10f);

    [SerializeField] private float frequency = 1.5f;

    [SerializeField] private float followOffset = 0.1f;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.ForceMeshUpdate();

        mesh = text.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < text.textInfo.characterCount; i++)
        {
            var charInfo = text.textInfo.characterInfo[i];

            Vector3 offset = Wobble(Time.time + i * followOffset);

            int index = charInfo.vertexIndex;

            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }

        mesh.vertices = vertices;
        text.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time)
    {
        return wobbleNormal * Mathf.Sin(time * frequency * Mathf.PI * 2);
    }
}