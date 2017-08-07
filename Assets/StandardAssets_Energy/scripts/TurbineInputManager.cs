﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using goedle_sdk;

public class TurbineInputManager : MonoBehaviour {
    
	private Text popUpText;
	private Image backgroundImage;
	private TurbineController turbineController;
	private bool displayPopUpText = false;
	private Vector2 mousePos;
	private Vector2 canvasPos;

	private GameObject infoQuad;
	private GameObject infoQuadText;


	Material defaultMat;
	Material highlightedMat;
	Material transparentMat;

	public Canvas PopUpCanvas;
	GameObject turbine_Fan, turbine_Main;

    void Start(){

		PopUpCanvas = transform.Find ("canvas_mouse").GetComponent<Canvas>();

		PopUpCanvas.enabled = false;
		PopUpCanvas.gameObject.SetActive (true);
		turbineController = GetComponent<TurbineController>();
		InitializePopUpText();
		// We have to talk about this tracking point
		//GoedleAnalytics.track ("add.turbineDetailed",null,turbineController.turbineEnergyOutput.ToString());

		infoQuad = transform.Find("InfoQuad").gameObject;
		infoQuadText = infoQuad.transform.Find("InfoQuadText").gameObject;

		defaultMat = new Material (Shader.Find("Standard"));
		highlightedMat = new Material (Shader.Find("Standard"));
		transparentMat = new Material (Shader.Find("Standard"));


		Material original_material;

		foreach (Transform tr in transform)
			if (tr.gameObject.tag == "producer_mesh") {
				turbine_Fan = tr.Find ("Turbine_Fan").gameObject;
				turbine_Main= tr.Find ("Turbine_Main").gameObject;	 
			}

		original_material = turbine_Fan.GetComponent<Renderer> ().material;
		defaultMat.CopyPropertiesFromMaterial (original_material);
		highlightedMat.color = Color.cyan; 
		transparentMat.color = Color.black;

	}

	void Update(){
		//used for highlighting the turbine when is damaged.ο
		if(turbineController.isDamaged)
			HighlightTurbine(turbineController.isDamaged);


		if (infoQuad)
			infoQuad.transform.LookAt( GameObject.Find("camera").GetComponent<Camera>().transform.position);
	}

	//when user clicks the turbine.
	void OnMouseDown () {
		//clicks while turbine is rotating.
		if (turbineController.isConstructed) {
			if (turbineController.isRotating && !turbineController.isDamaged) {
				turbineController.DisableTurbine ();
			} else if (!turbineController.isRotating && !turbineController.isDamaged) {
				turbineController.EnableTurbine ("onTurbine");
			} else if (turbineController.isDamaged) {
				turbineController.repairTurbine ();
			}
		}
	}

	
	//when mouse is hovered over a wind turbine the turbine is highlighted.
	void HighlightTurbine(bool isDamaged){
		turbine_Fan.GetComponent<Renderer>().material = displayPopUpText ? highlightedMat : (isDamaged ? transparentMat : defaultMat);
		turbine_Main.GetComponent<Renderer>().material = displayPopUpText ? highlightedMat : (isDamaged ? transparentMat : defaultMat);
	}

	//when mouse is over a wind turbine without being clicked.
	void OnMouseEnter(){
		PlaceCanvasToMouse();
		displayPopUpText = true;
		DisplayPopUpText();
		HighlightTurbine(turbineController.isDamaged);


		infoQuad.GetComponent<MeshRenderer> ().enabled = true;
		infoQuadText.GetComponent<MeshRenderer>().enabled = true;


	}

	//when mouse is not over a wind turbine (specifically on the collider of the turbine).
	void OnMouseExit(){
		displayPopUpText = false;
		DisplayPopUpText();
		HighlightTurbine(turbineController.isDamaged);


		infoQuad.GetComponent<MeshRenderer> ().enabled = false;
		infoQuadText.GetComponent<MeshRenderer>().enabled = false;
	}


	void InitializePopUpText(){
		popUpText = PopUpCanvas.GetComponentInChildren<Image>().GetComponentInChildren<Text>();
		backgroundImage = PopUpCanvas.GetComponentInChildren<Image>();
		popUpText.enabled = false;
		backgroundImage.enabled = false;
	}

	void DisplayPopUpText(){
		if(displayPopUpText == true){
			if(turbineController.isRotating){
				popUpText.color = Color.red;
				popUpText.text = "Turn off";
			}
			else if(!turbineController.isRotating && !turbineController.isDamaged){
				popUpText.color = Color.blue;
				popUpText.text = "Turn on";
			}
			else {
				popUpText.text = "repair";
				popUpText.color = Color.black;
			}
			popUpText.enabled = true;
			backgroundImage.enabled = true;
		}
		else{
			popUpText.enabled = false;
			backgroundImage.enabled = false;
		} 
	}

	// used to move the text next to the selected turbine.
	void PlaceCanvasToMouse(){
		mousePos = Input.mousePosition;
		canvasPos.x = mousePos.x;
		canvasPos.y = mousePos.y+20;
		PopUpCanvas.GetComponentInChildren<Image>().transform.position = canvasPos;
	}
}
