using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float timeBetweenBullets = 0.5f;
    public float range = 100f;

    float timer;
    LineRenderer gunLine;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    Camera thisCamera;
    Vector3 newPosition;


    void Awake ()
    {
        thisCamera = Camera.main;
        gunLine = GetComponent <LineRenderer> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                newPosition = hit.point;
                gunLine.SetPosition(1, newPosition);
            }
            
        }
    }
}
