using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Amazon.Kinesis;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon;
using System.Text;
using Amazon.Kinesis.Model;
using System.IO;

public class Amazon_config : MonoBehaviour {


	public string IdentityPoolId = "";

	// public string CognitoIdentityRegion = RegionEndpoint.USWest2.SystemName;

	// private RegionEndpoint _CognitoIdentityRegion
	// {
	// 	get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
	// }
	
	// Initialize the Amazon Cognito credentials provider
	
	CognitoAWSCredentials credentials = new CognitoAWSCredentials(
		"us-west-2:13e0e993-0cc5-48c9-b4c3-4430cadad5f0", // Identity pool ID
		RegionEndpoint.USWest2 // Region
	); 
	


	public string KinesisRegion = RegionEndpoint.USWest2.SystemName;

	private RegionEndpoint _KinesisRegion
	{
		get { return RegionEndpoint.GetBySystemName(KinesisRegion); }
	}

	// Use this for initialization
	void Awake () {
		UnityInitializer.AttachToGameObject(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
