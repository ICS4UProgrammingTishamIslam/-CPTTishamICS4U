using UnityEngine;

public class GroundChecker : MonoBehaviour {
    private Player playerScript;

    void Start() {
        //get the player script
        playerScript = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other) {
        //if you hit anything tagged as being jumpable, set grounded to true for the player
        //was gonna have th eplayer raise the chairs but sadly not enough time to do that
        if (other.tag == "Floor" || other.tag == "Chair" || other.tag == "Jumpables") {
            playerScript.grounded = true;
        }
    }
}
