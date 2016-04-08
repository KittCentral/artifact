using UnityEngine;
using System.Collections;

public class restart : StateMachineBehaviour
{
    public void startOver(int index)
    {
        Application.LoadLevel(index);
    }
}
