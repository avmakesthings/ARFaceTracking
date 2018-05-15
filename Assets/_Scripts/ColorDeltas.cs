using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDeltas : MonoBehaviour {

    public float m_ColorMultiplier;

    private MeshFilter filter;
    private Vector3[] vertices;
    private Vector3[] lastVertices;
    private Color[] colors;

    void Update(){
		filter = GetComponent<MeshFilter>();
		if(filter!= null && filter.sharedMesh != null){
        	vertices = filter.sharedMesh.vertices;
        	lastVertices = (Vector3[])vertices.Clone ();
		}

    }

    void LateUpdate(){
		if(filter!= null && filter.sharedMesh != null){
       		ColorMesh ();
		}
    }

    void ColorMesh(){
        filter = GetComponent<MeshFilter>();
        vertices = filter.sharedMesh.vertices;

        if (colors == null) {
            colors = new Color[vertices.Length];
        }

        for (int i = 0; i < vertices.Length; i++) {
            //float delta = (vertices[i].magnitudez / lastVertices[i].magnitude);
			// float delta = (vertices[i].z - lastVertices[i].z);
			float delta = (vertices[i].z)/10f;
            colors[i] = Color.Lerp(Color.clear, Color.cyan, delta);
        }

        filter.sharedMesh.colors = colors;
        filter.sharedMesh.RecalculateBounds();
    }
}