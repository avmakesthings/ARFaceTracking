using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;


public class FaceCalloutManager : MonoBehaviour {

	[SerializeField]
	private MeshFilter meshFilter;

	public GameObject calloutPrefab;
	public GameObject calloutPrefab_L;
	public GameObject calloutPrefab_R;		
	public Transform debugPrefab;
	
	List<FaceCallout> faceCalloutList;
	
	private Mesh faceDebugMesh;
	private UnityARSessionNativeInterface m_session;
	


	


	// Use this for initialization
	void Start () {
		m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

		Application.targetFrameRate = 60;
		ARKitFaceTrackingConfiguration config = new ARKitFaceTrackingConfiguration();
		config.alignment = UnityARAlignment.UnityARAlignmentGravity;
		config.enableLightEstimation = true;

		if (config.IsSupported) {
			
			m_session.RunWithConfig (config);

			UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
			UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
			UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;

		}

		initializeCallouts();

	}
	


	void FaceAdded (ARFaceAnchor anchorData)
	{
		gameObject.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform);
		gameObject.transform.localRotation = UnityARMatrixOps.GetRotation (anchorData.transform);

		
		
		updateFaceCalloutPositions(anchorData);
		
		// foreach(var k in anchorData.blendShapes){
		// 	print(k);
		// }
		
	}



	void FaceUpdated (ARFaceAnchor anchorData)
	{

		gameObject.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform);
		gameObject.transform.localRotation = UnityARMatrixOps.GetRotation (anchorData.transform);

		foreach(var f in faceCalloutList){
			f.FaceUpdated(anchorData);
		}
		
		//draw face mesh
		updateDebugFaceMesh(anchorData);
		updateFaceCalloutPositions(anchorData);

		
	}



	void FaceRemoved (ARFaceAnchor anchorData)
	{
		meshFilter.mesh = null;
		faceDebugMesh = null;
	}	


	//config object
	void initializeCallouts(){
		faceCalloutList = new List<FaceCallout>();
		FaceCallout f;

 		f = (Instantiate(calloutPrefab) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManger").transform;
		f.setTitle("Left Cheek");
		f.setDescription("some description");		
		f.pointIndex = 150;
		f.blendShapeStrings = new List<string>{
			"cheekPuff",
			"cheekSquint_L"
		};
		f.leftAligned = false;
		faceCalloutList.Add(f);

 		f = (Instantiate(calloutPrefab) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManger").transform;
		f.setTitle("Left Brow");
		f.setDescription("another descrip");
		f.pointIndex = 210;
		f.blendShapeStrings = new List<string>{
			"browDown_L",
			"browInnerUp",
			"browOuterUpLeft"
		};
		f.leftAligned = false;
		faceCalloutList.Add(f);

		f = (Instantiate(calloutPrefab) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManger").transform;
		f.setTitle("Right Eye");
		f.setDescription("eye descrip");
		f.pointIndex = 1110;
		f.blendShapeStrings = new List<string>{
			"eyeBlink_R",
			"eyeLookDown_R",
			"eyeLookIn_R",
			"eyeLookOut_R",
			"eyeLookUp_R",
			"eyeSquint_R",
			"eyeWide_R"
		};
		f.leftAligned = true;
		faceCalloutList.Add(f);

		f = (Instantiate(calloutPrefab) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManger").transform;
		f.setTitle("Left Mouth");
		f.setDescription("mouth descrip");
		f.pointIndex = 240;
		f.blendShapeStrings = new List<string>{
			"jawForward",
			"jaw_L",
			"jawOpen",
			"mouthClose",
			"mouthFunnel",
			"mouthPucker",
			"mouth_L",
			"mouthSmile_L",
			"mouthFrown_L",
			"mouthDimple_L",
			"mouthStretch_L",
			"mouthRollLower",
			"mouthRollUpper",
			"mouthShrugUpper",
			"mouthPress_L",
			"mouthLowerDown_L",
			"mouthUpperUp_L"
		};
		f.leftAligned = true;
		faceCalloutList.Add(f);
	
	}


	void updateFaceCalloutPositions(ARFaceAnchor anchorData){

		foreach(var f in faceCalloutList){
			f.setBaseLocation(anchorData.faceGeometry.vertices[f.pointIndex]);
			//f.setCalloutLine();
		}
	}



	void createDebugFaceMesh(ARFaceAnchor anchorData){
		faceDebugMesh = new Mesh ();
		meshFilter.mesh = faceDebugMesh;
		updateDebugFaceMesh(anchorData);
	}



	void updateDebugFaceMesh(ARFaceAnchor anchorData){

		if(faceDebugMesh == null){
			createDebugFaceMesh(anchorData);
		}
		faceDebugMesh.vertices = anchorData.faceGeometry.vertices;
		faceDebugMesh.uv = anchorData.faceGeometry.textureCoordinates;
		faceDebugMesh.triangles = anchorData.faceGeometry.triangleIndices;
		faceDebugMesh.RecalculateBounds();
		faceDebugMesh.RecalculateNormals();


	}

	
}
