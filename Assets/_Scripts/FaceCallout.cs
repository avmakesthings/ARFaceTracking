using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using TMPro;

public class FaceCallout : MonoBehaviour {


	public string title;
	public string description;
	public int pointIndex; 
	public List<string> blendShapeStrings;
	public bool leftAligned;
	
	TextMeshProUGUI titleTextComponent;
	TextMeshProUGUI descriptionTextComponent;	

	private LineRenderer calloutLine;
	private Canvas canvas;
	private Vector3 basePosition;

	void Awake(){

		getTextComponents();
		canvas = GetComponentInChildren<Canvas>();
		calloutLine = GetComponentInChildren<LineRenderer>();

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
		setCalloutLine();
	}



	public void setCalloutLine(){
		calloutLine.SetPosition(0, canvas.GetComponent<Transform>().position);
		calloutLine.SetPosition(1, this.GetComponent<Transform>().position);
	}



	public void setTitle(string myTitle){
		titleTextComponent.text = myTitle;
		title = myTitle;
	}



	public void setDescription(string myDescription){
		descriptionTextComponent.text = myDescription;
		description = myDescription;
	}

	public void lookAtPlayer(Transform player){
		Transform t = canvas.GetComponent<Transform>();
		t.LookAt(player);
		t.rotation = Quaternion.Euler(0 , 0, 90);
		
	}


	void getTextComponents(){
				
		foreach( Transform t in GetComponentsInChildren<Transform>() ){
			if(t.name == "Title"){
				titleTextComponent = t.GetComponent<TextMeshProUGUI>();
			}
			if (t.name == "Description"){
				descriptionTextComponent = t.GetComponent<TextMeshProUGUI>();
			}
		}


		if(titleTextComponent == null || descriptionTextComponent == null ){
			throw new Exception("Unable to get text components in Face Callout");
		}
	}

}
