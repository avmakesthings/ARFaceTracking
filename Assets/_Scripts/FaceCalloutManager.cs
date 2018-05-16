using System;
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
	public Transform player;
	
	List<FaceCallout> faceCalloutList;
	Animator anim;

	public ParticleSystem smilePartSys;
	public AudioSource smileAudio;
	public AudioSource blinkAudio;
	public AudioSource makeupAudio;
	
	private Mesh faceDebugMesh;
	private UnityARSessionNativeInterface m_session;
	private bool isPlaying;
	private bool isSmiling;
	

	// Use this for initialization
	void Start () {

		isPlaying = false;
		isSmiling = false;

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
		anim = GetComponent<Animator>();
	}
	


	void FaceAdded (ARFaceAnchor anchorData)
	{
		gameObject.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform);
		gameObject.transform.localRotation = UnityARMatrixOps.GetRotation (anchorData.transform);

		updateFaceCalloutPositions(anchorData);		
	}



	void FaceUpdated (ARFaceAnchor anchorData)
	{

		gameObject.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform);
		gameObject.transform.localRotation = UnityARMatrixOps.GetRotation (anchorData.transform);

		foreach(var f in faceCalloutList){
			f.FaceUpdated(anchorData);
			f.lookAtPlayer(player);
			//iterate through thresholds
			bool show = false;
			
			foreach(KeyValuePair<string, float> threshold in f.activationThresholds){
				if(anchorData.blendShapes[threshold.Key]> threshold.Value){
					show = true;
				}
			}
			f.gameObject.SetActive(show);

		}
		
		//draw face mesh
		updateDebugFaceMesh(anchorData);
		updateFaceCalloutPositions(anchorData);		

		if(anchorData.blendShapes["mouthSmile_L"]> 0.9f && !isSmiling && !isPlaying){
			 isSmiling = true;
			 //StartCoroutine(Smiling());
		}
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

 		f = (Instantiate(calloutPrefab_R) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManager").transform;
		f.setTitle("Right Cheek");
		f.setDescription("some description");		
		f.pointIndex = 630;
		f.activationThresholds = new Dictionary<string, float>() { 
			{"cheekPuff", 0.5f},
			{"jawOpen", 0.8f} 
		
		};
		f.blendShapeStrings = new List<string>{
			"cheekPuff",
			"cheekSquint_R",
			"jawForward",
			"jaw_R",
			"jawOpen"
		};
		f.leftAligned = false;
		faceCalloutList.Add(f);
		f.gameObject.SetActive(false);


 		f = (Instantiate(calloutPrefab) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManager").transform;
		f.setTitle("Left Brow");
		f.setDescription("another descrip");
		f.pointIndex = 210;
		f.activationThresholds = new Dictionary<string, float>() { {"browDown_L", 0.8f} };
		f.blendShapeStrings = new List<string>{
			"browDown_L",
			"browInnerUp",
			"browOuterUpLeft"
		};
		f.leftAligned = true;
		faceCalloutList.Add(f);
		f.gameObject.SetActive(false);


		f = (Instantiate(calloutPrefab_R) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManager").transform;
		f.setTitle("Right Eye");
		f.setDescription("eye descrip");
		f.pointIndex = 1110;
		f.activationThresholds = new Dictionary<string, float>() { 
			{"eyeLookOut_R", 0.4f},
			{"eyeLookUp_R", 0.4f},
			 };
		f.blendShapeStrings = new List<string>{
			"eyeBlink_R",
			"eyeLookDown_R",
			"eyeLookIn_R",
			"eyeLookOut_R",
			"eyeLookUp_R",
			"eyeSquint_R",
			"eyeWide_R"
		};
		f.leftAligned = false;
		faceCalloutList.Add(f);
		f.gameObject.SetActive(false);


		f = (Instantiate(calloutPrefab_L) as GameObject).GetComponent<FaceCallout>();
		f.transform.parent = GameObject.Find("FaceCalloutManager").transform;
		f.setTitle("Left Mouth");
		f.setDescription("mouth descrip");
		f.pointIndex = 240;
		f.activationThresholds = new Dictionary<string, float>() { {"mouthLowerDown_L", 0.33f} };
		f.blendShapeStrings = new List<string>{
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
		f.gameObject.SetActive(false);
	}


	void updateFaceCalloutPositions(ARFaceAnchor anchorData){

		foreach(var f in faceCalloutList){
			f.setBaseLocation(anchorData.faceGeometry.vertices[f.pointIndex]);
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

	
	public IEnumerator Smiling(){
		print("smile coroutine started");
		if(!isPlaying){
			isPlaying = true;
			smilePartSys.Play();
			smileAudio.Play();
			yield return new WaitForSeconds(10.0f);
			isPlaying = false;
			StartCoroutine(MakeUp());
			isSmiling = false;
		}
    }

	// public IEnumerator Blinking(){
	// 	print("blink coroutine started");
	// 	if(!isPlaying){
			
	// 		isPlaying = true;
	// 		print(isPlaying);
	// 		yield return new WaitForSeconds(5.0f);
	// 	}
		
	// }

	public IEnumerator MakeUp(){
		print("makeup coroutine started");
		if(!isPlaying){
			isPlaying = true;
			anim.Play("MakeUp");
			makeupAudio.Play();
			yield return new WaitForSeconds(10.0f);
		}
		
	}




}
