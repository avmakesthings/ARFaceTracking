using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class BlendShapeValues : MonoBehaviour {

	bool enabled = false;
	Dictionary<string, float> currentBlendShapes;

	enum BlendShapeName {
		mouthSmileLeft,
		mouthSmileRight,
		mouthFrownLeft,
		mouthFrownRight,
		mouthPucker,
		mouthFunnel,
		mouthClose,
		jawOpen
	};

	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
		UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
		UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;

	}
	
	void FaceAdded (ARFaceAnchor anchorData)
	{
		enabled = true;
		print("face is enabled");
		currentBlendShapes = anchorData.blendShapes;
	}

	void FaceUpdated (ARFaceAnchor anchorData)
	{
		currentBlendShapes = anchorData.blendShapes;
	}

	void FaceRemoved (ARFaceAnchor anchorData)
	{
		print("face is removed");
		enabled = false;
	}


	// Update is called once per frame
	void Update () {
		if(enabled){
			foreach (var shapeName in Enum.GetValues(typeof(BlendShapeName)) ){
				if(currentBlendShapes.ContainsKey(shapeName.ToString())){
					//print(shapeName + "" + currentBlendShapes[shapeName.ToString()]);
					
				}			
			}
		}
	}
}
