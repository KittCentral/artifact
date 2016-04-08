using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int Score;


    Text text;

    
    public void increment(int x)
    {
        Score += x;
    }


    void Update ()
    {
        text.text = "Score: " + Score;
    }
}
