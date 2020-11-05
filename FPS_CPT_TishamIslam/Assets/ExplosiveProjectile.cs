using UnityEngine;

public class ExplosiveProjectile : Projectile {
    [Header("Explosion Settings")]
    public LayerMask enemies;
    public float exRad = 10;
    public float exForce = 1000;
    public GameObject exPrefab;

    protected override void OnCollisionEnter(Collision collision) {
        for (int i = 0; i < hittables.Length - 1; i++) {
            if (hittables[i] == collision.gameObject.layer) {
                Explode();
            }
        }
    }

    protected void Explode() {
        //collect all the colliders in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, exRad, enemies);

        //for every collider detected...
        for (int i = 0; i < colliders.Length; i++) {
            //if the collided object ain't an enemy, move on
            if (colliders[i].gameObject.tag != "Enemy") {
                continue;
            }

            //find their rigidbody component and get the Enemy script attached to the enemy
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            Enemy health = targetRigidbody.GetComponent<Enemy>();

            //if for whatever reason the script is missing, skip over the enemy
            if (!health) {
                continue;
            }

            health.TakeDamage(damage);
            targetRigidbody.AddExplosionForce(exForce, transform.position, exRad);
        }

        //instantiate explosion, destroy the prefab
        Instantiate(exPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
