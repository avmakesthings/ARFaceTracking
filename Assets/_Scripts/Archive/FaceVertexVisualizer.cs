using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using TMPro;

//Based on Unity ARKit plugin UnityARFaceMeshManager Class
public class FaceVertexVisualizer : MonoBehaviour {

	[SerializeField]
	private MeshFilter meshFilter;

	private UnityARSessionNativeInterface m_session;
	private Mesh faceMesh;

	public Transform prefab;
	public Camera camera;

	Transform [] points;
	Transform [] pointsCulled;


	

	// Use this for initialization
	void Start () {
		m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

		Application.targetFrameRate = 60;
		ARKitFaceTrackingConfiguration config = new ARKitFaceTrackingConfiguration();
		config.alignment = UnityARAlignment.UnityARAlignmentGravity;
		config.enableLightEstimation = true;

		if (config.IsSupported && meshFilter != null) {
			
			m_session.RunWithConfig (config);

			UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
			UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
			UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;

		}
	}

	void FaceAdded (ARFaceAnchor anchorData)
	{
		gameObject.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform);
		gameObject.transform.localRotation = UnityARMatrixOps.GetRotation (anchorData.transform);

		faceMesh = new Mesh ();
		faceMesh.vertices = anchorData.faceGeometry.vertices;
		
		points = new Transform[faceMesh.vertices.Length]; 
		for(int i=0; i<faceMesh.vertices.Length; i++){
			// if(i%30==0){
			// 	points[i] = Instantiate(prefab);
			// 	points[i].transform.parent = GameObject.Find("Vertices").transform;
			// 	points[i].localPosition =  faceMesh.vertices[i];
			// 	TextMeshPro label = points[i].GetComponentInChildren<TextMeshPro>();
			// 	label.text = i.ToString();
			// 	//points[i].LookAt(camera.transform);
			// }
			if(i==150){
				points[i] = Instantiate(prefab);
				points[i].transform.parent = GameObject.Find("Vertices").transform;
				points[i].localPosition =  faceMesh.vertices[i];
				TextMeshPro label = points[i].GetComponentInChildren<TextMeshPro>();
				label.text = i.ToString();
				print("right cheek");
			}
			if(i==210){

				print("right brow");
			}	
			if(i==240){

				print("right mouth");
			}
			if(i==1110){
				points[i] = Instantiate(prefab);
				points[i].transform.parent = GameObject.Find("Vertices").transform;
				points[i].localPosition =  faceMesh.vertices[i];
				TextMeshPro label = points[i].GetComponentInChildren<TextMeshPro>();
				label.text = i.ToString();
				print("left eye");
			}						
		}

		// faceMesh.uv = anchorData.faceGeometry.textureCoordinates;
		// faceMesh.triangles = anchorData.faceGeometry.triangleIndices;

		// Assign the mesh object and update it.
		// faceMesh.RecalculateBounds();
		// faceMesh.RecalculateNormals();
		// meshFilter.mesh = faceMesh;
	}

	void FaceUpdated (ARFaceAnchor anchorData)
	{
		if (faceMesh != null) {
			gameObject.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform);
			gameObject.transform.localRotation = UnityARMatrixOps.GetRotation (anchorData.transform);
			faceMesh.vertices = anchorData.faceGeometry.vertices;
			
			points[150].localPosition = faceMesh.vertices[150];
			points[1110].localPosition = faceMesh.vertices[1110];

			//this is too slow!!! 
			// for(int i=0; i<faceMesh.vertices.Length; i++){
			// 	if(points[i] != null){
			// 		if(i==150){					
			// 			points[i].localPosition =  faceMesh.vertices[i];
			// 		}
			// 		if(i==1110){
			// 			points[i].localPosition =  faceMesh.vertices[i];
			// 		}					
			// 	}
			// }	



			// faceMesh.uv = anchorData.faceGeometry.textureCoordinates;
			// faceMesh.triangles = anchorData.faceGeometry.triangleIndices;
			// faceMesh.RecalculateBounds();
			// faceMesh.RecalculateNormals();
		}

	}

	void FaceRemoved (ARFaceAnchor anchorData)
	{
		meshFilter.mesh = null;
		faceMesh = null;
	}


	// void instantiateFacePoint(string strLocation, Transform point){
	// 	Transform thisPoint = Instantiate(prefab);
	// 	thisPoint.transform.parent = GameObject.Find("Vertices").transform;
	// 	thisPoint.localPosition =  point;
	// 	TextMeshPro label = points[index].GetComponentInChildren<TextMeshPro>();
	// 	label.text = strLocation;
	// 	print("just completed"+ strLocation);
	// }

}
