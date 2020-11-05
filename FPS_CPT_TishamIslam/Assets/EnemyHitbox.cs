using UnityEngine;

public class EnemyHitbox : MonoBehaviour {

    private Enemy enemyScript;
    [Tooltip("Some areas of an enemy may take more damage than normal")] public float critMultiplier;

    void Start() {
        enemyScript = GetComponentInParent<Enemy>();
    }

    //for projectile damage considerations
    private void OnTriggerEnter(Collider other) {
        //if hit by projectile
        if (other.tag == "Projectile") {
            //get projectile damage, multiply it by the critical multiplier and send it to the enemyscript
            Projectile proScript = other.GetComponent<Projectile>();
            enemyScript.TakeDamage(proScript.damage * critMultiplier);
            Destroy(other);
        }
    }

    //for raycast gun considerations
    public void TakeDamageHandler(float damage) {
        enemyScript.TakeDamage(damage * critMultiplier);
    }
}
