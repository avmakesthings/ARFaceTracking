using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using TMPro;

public class FaceCallout : MonoBehaviour {


	public string title;
	public string description;
	public int pointIndex; 
	public List<string> blendShapeStrings;
	public bool leftAligned;
	
	TextMeshPro titleTextComponent;
	TextMeshPro descriptionTextComponent;	


	void Awake(){
		getTextComponents();
	}

	public void FaceUpdated(ARFaceAnchor anchorData){
		string t = "";
		foreach(var b in blendShapeStrings){
			if(anchorData.blendShapes.ContainsKey(b)){
				t += String.Format("{0} : {1} \n",b, anchorData.blendShapes[b].ToString("F2"));
			}
		}
		setDescription(t);
	}



	public void setBaseLocation(Vector3 point){
		Transform thisTransform = this.GetComponent<Transform>();
		thisTransform.localPosition = point;
	}


	public void setTitle(string myTitle){
		titleTextComponent.text = myTitle;
		title = myTitle;
	}


	public void setDescription(string myDescription){
		descriptionTextComponent.text = myDescription;
		description = myDescription;
	}


	void getTextComponents(){
				
		Transform thisTransform = this.GetComponent<Transform>();
		foreach (Transform t in thisTransform)
		{
			if(t.name == "Title"){
				titleTextComponent = t.GetComponent<TextMeshPro>();
			}
			if (t.name == "Description"){
				descriptionTextComponent = t.GetComponent<TextMeshPro>();
			}
		}

		if(titleTextComponent == null || descriptionTextComponent == null ){
			throw new Exception("Unable to get text components in Face Callout");
		}
	}

}
