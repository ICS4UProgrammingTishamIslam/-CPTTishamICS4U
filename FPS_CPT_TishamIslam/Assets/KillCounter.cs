using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public GameManager manageScript;

    private void Start()
    {
        manageScript = FindObjectOfType<GameManager>();
    }
    public void Killed()
    {
        //send a message to the management that a thing that should've been killed has been killed
        manageScript.ObjectiveKilled();
    }
}
