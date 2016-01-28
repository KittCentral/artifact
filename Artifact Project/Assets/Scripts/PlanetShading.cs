// This script controls the atmospheric scattering for the planets it is an important part of the shaders as well
// If you don't understand don't touch

using UnityEngine;
using System.Collections;

public class PlanetShading : MonoBehaviour 
{
	public GameObject sun;
	public Material groundMaterial;
    //public Material atmoMaterial;
	
	public float hdrExposure = 0.8f;
	public Vector3 waveLength = new Vector3(0.65f,0.57f,0.475f); // Wave length of sun light
	public float ESun = 20.0f; 			// Sun brightness constant
	public float kr = 0.0025f; 			// Rayleigh scattering constant
	public float km = 0.0010f; 			// Mie scattering constant
	public float g = -0.990f;				// The Mie phase asymmetry factor, must be between 0.999 to -0.999
	
	//Dont change these
	private const float outerScaleFactor = 1.025f; // Difference between inner and ounter radius. Must be 2.5%
	private float innerRadius;		 	// Radius of the ground sphere
	private float outerRadius;		 	// Radius of the sky sphere
	private float scaleDepth = 0.25f; 	// The scale depth (i.e. the altitude at which the atmosphere's average density is found)
				
	void Start () 
	{
		//Get the radius of the sphere. This presumes that the sphere mesh is a unit sphere (radius of 1)
		//that has been scaled uniformly on the x, y and z axis
		float radius = transform.localScale.x;
		
		innerRadius = radius;
		//The outer sphere must be 2.5% larger that the inner sphere
		outerRadius = outerScaleFactor * radius;
		
		InitMaterial(groundMaterial);
        //InitMaterial(atmoMaterial);
    }
	
	void Update () 
	{
		InitMaterial(groundMaterial);
        //InitMaterial(atmoMaterial);
    }
	
	void InitMaterial(Material mat)
	{
		Vector3 invWaveLength4 = new Vector3(1.0f / Mathf.Pow(waveLength.x, 4.0f), 1.0f / Mathf.Pow(waveLength.y, 4.0f), 1.0f / Mathf.Pow(waveLength.z, 4.0f));
		float scale = 1.0f / (outerRadius - innerRadius);

		mat.SetVector("v3LightPos", sun.transform.forward*-1.0f);
		mat.SetVector("v3InvWavelength", invWaveLength4);
		mat.SetFloat("fOuterRadius", outerRadius);
		mat.SetFloat("fOuterRadius2", outerRadius*outerRadius);
		mat.SetFloat("fInnerRadius", innerRadius);
		mat.SetFloat("fInnerRadius2", innerRadius*innerRadius);
		mat.SetFloat("fKrESun", kr*ESun);
		mat.SetFloat("fKmESun", km*ESun);
		mat.SetFloat("fKr4PI", kr*4.0f*Mathf.PI);
		mat.SetFloat("fKm4PI", km*4.0f*Mathf.PI);
		mat.SetFloat("fScale", scale);
		mat.SetFloat("fScaleDepth", scaleDepth);
		mat.SetFloat("fScaleOverScaleDepth", scale/scaleDepth);
		mat.SetFloat("fHdrExposure", hdrExposure);
		mat.SetFloat("g", g);
		mat.SetFloat("g2", g*g);
		mat.SetVector("v3LightPos", sun.transform.forward*-1.0f);
		mat.SetVector("v3Translate", transform.localPosition);

	}
}