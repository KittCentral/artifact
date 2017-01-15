using UnityEngine;

public class pongPainter : MonoBehaviour
{
    #region "Public Instantiations"
    public GameObject brushPrefab;
    public GameObject brushContainer, ball; //The cursor that overlaps the model and our container for the brushes painted
    public Camera sceneCamera, canvasCam;  //The camera that looks at the model, and the camera that looks at the canvas.
    ParticleSystem sys;
    public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
    public Material baseMaterial; // The material of our base texture (Were we will save the painted texture)
    #endregion

    const int maxBrushes = 50;
    ParticleSystem.Particle[] particles;
    Color[] colors = new Color[6] { Color.blue, Color.cyan, Color.green, Color.yellow, Color.red, Color.magenta };
    int counter;
    int wait;

    void Update()
    {
        if(wait == 3)
        { 
            if (GameObject.Find("Explosion(Clone)") != null)
                sys = GameObject.Find("Explosion(Clone)").GetComponent<ParticleSystem>();
            else
                sys = null;
            //if (Input.GetMouseButton(0))
            //    AddPaint();
            //UpdateBrushCursor();
            if (sys != null)
            {
                particles = new ParticleSystem.Particle[sys.particleCount];
                sys.GetParticles(particles);
                for (int i = 0; i < particles.Length; i += 100)
                {
                    int ind = i + counter;
                    ind = ind >= particles.Length ? ind % particles.Length : ind;
                    if (particles[ind].remainingLifetime > 4)
                    {
                        int index;
                        if ((int)((particles[ind].velocity.magnitude - 5) / 3) < 7 && (int)((particles[ind].velocity.magnitude - 5) / 3) > -1)
                            index = (int)((particles[ind].velocity.magnitude - 5) / 3);
                        else
                            index = 5;
                        Color randColor = colors[index];
                        Vector3 pos = sys.gameObject.transform.TransformPoint(particles[ind].position);
                        TexturePainter.AddPaintPoint(pos, brushPrefab, brushContainer, sceneCamera.gameObject, particles[ind].GetCurrentColor(sys), particles[ind].GetCurrentSize(sys) * 1.5f);
                    }
                }
                TexturePainter.brushCounter++; //Add to the max brushes
                if (TexturePainter.brushCounter >= maxBrushes)
                { //If we reach the max brushes available, flatten the texture and clear the brushes
                    TexturePainter.saving = true;
                    Invoke("SaveTexture", 0.1f);
                    Invoke("ShowCursor", 0.2f);
                }
            }
            if (counter == 100)
            {
                counter = 0;
            }
            counter++;
            wait = 0;
        }
        wait++;
    }

    void SaveTexture() {TexturePainter.SaveTexture(canvasTexture, baseMaterial, brushContainer);}
    void ShowCursor() {TexturePainter.ShowCursor();}
}
