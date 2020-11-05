using UnityEngine;

public class Projectile : MonoBehaviour {
    [Header("Generic Projectile Settings")]
    [Tooltip("Layers as individual integer values")] public int[] hittables;
    public float velocity = 50;
    public float damage = 20;
    public float lifetime = 2;
    protected AudioSource shotSound;

    protected virtual void Start() {
        //destroy the object later, get audio
        Destroy(gameObject, lifetime);
        shotSound = GetComponent<AudioSource>();
    }

    protected virtual void Update() {
        //move projectile, set volume
        transform.Translate(new Vector3(0, 0, velocity) * Time.deltaTime);
        shotSound.volume = Options.Volume / 5;
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        for (int i = 0; i < hittables.Length - 1; i++) {
            if (hittables[i] == collision.gameObject.layer) {
                Destroy(gameObject);
            }
        }
    }
}
