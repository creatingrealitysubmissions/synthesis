using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorChanger : MonoBehaviour {

    GameObject webCamtextureScript;
    private byte R, G, B;
    ParticleSystem particle;

	// Use this for initialization
	void Start () {

        webCamtextureScript = GameObject.Find("WebCamColorPicker");

        R = (byte)(webCamtextureScript.GetComponent<webCamtexture>().r*255);
        G = (byte)(webCamtextureScript.GetComponent<webCamtexture>().g*255);
        B = (byte)(webCamtextureScript.GetComponent<webCamtexture>().b*255);


        var main = this.gameObject.GetComponent<ParticleSystem>().main;
        main.startColor = new Color(R, G, B, 255);

        Debug.Log("particle system initiated and color changed to -> RGB " + R + " " + G + " " + B);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
