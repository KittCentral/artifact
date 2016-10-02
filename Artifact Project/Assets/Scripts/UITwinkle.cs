using UnityEngine;
using UnityEngine.UI;

public class UITwinkle : MonoBehaviour
{
    public float frequency;
    public float percentAmplitude;
    public Color targetColor;
	
	// Update is called once per frame
	void Update ()
    {
        float colorFraction = percentAmplitude * Mathf.Sin(frequency * Time.time) + (1-percentAmplitude);
        transform.GetComponent<Image>().color = targetColor * colorFraction;
	}
}
