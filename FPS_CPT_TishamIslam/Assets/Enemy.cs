using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("Explosion Attacks")]
    public GameObject exPar;
    public float maxDamage = 25;
    public float exDelay = 2;
    public float exForce = 400;
    public float exRad = 3;
    public float triggerDis = 3;
    public bool defenceAttacker;

    [Header("Health")]
    public float startingHealth = 100;
    [SerializeField] private float currentHealth;
    public GameObject killedNoise;

    [Header("Movement")]
    public Transform waypoint;  //waypoint can be both player or other location, if needed
    public float Speed = 5;
    public Animator anim;

    private GameManager manageScript;


    void Start() {
        manageScript = FindObjectOfType<GameManager>();

        //get animator, and init health
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;

        //set waypoint if not automatically set
        if (waypoint == null) {
            Spawn spawnScript = GetComponentInParent<Spawn>();
            waypoint = spawnScript.destination;
        }
    }

    public void TakeDamage(float damage) {
        //subtract health and check if goblin's still alive
        currentHealth -= damage;
        if (currentHealth <= 0) {
            //if there's a killScript on the goblin, send a kill to the game manager
            KillCounter killScript = GetComponent<KillCounter>();
            if (killScript != null) {
                killScript.Killed();
            }

            Explode(true);
        }
    }

    void Update() {
        //move towards the objective
        transform.position = Vector3.MoveTowards(transform.position, waypoint.position, Time.deltaTime * Speed);
        //you don't want the goblins to look up or down, as then it rotates their entire bodies, which can also mess up their movement when close to the player,
        //so set the y direction to the current position
        transform.LookAt(new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z));


        if (Vector3.Distance(transform.position, waypoint.position) < triggerDis) {
            Explode(false);
        }

    }

    //enemy explodes to attack or when it dies
    public void Explode(bool killed) {

        //if it is killed, it will not damage the player
        if (killed != true) {
            if (defenceAttacker) {
                manageScript.DefenceHit();
            }

            //collect all the colliders in the explosion radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, exRad);

            //for every collider detected...
            for (int i = 0; i < colliders.Length; i++) {
                //if the collided object ain't a player, move on
                if (colliders[i].gameObject.tag != "Player") {
                    continue;
                }

                //find their rigidbody component
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

                //get the Player script attached to the player
                Player playerHealth = targetRigidbody.GetComponent<Player>();

                //add an explosion force to the object hit
                targetRigidbody.AddExplosionForce(exForce, transform.position, exRad);

                //calculate damage based on the target's position
                int damage = (int)CalculateDamage(targetRigidbody.position);

                //deal damage to the player
                playerHealth.TakeDamage(damage, "Goblin");
            }
        }

        //play the particles
        Instantiate(exPar, transform.position, Quaternion.identity);

        if (killed == true) {
            //instantiate the killed noise as the goblin object will be destroyed
            Instantiate(killedNoise, transform.position, Quaternion.identity);
        }

        //destroy the goblin as it is dead and gone
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition) {
        //create a vector from the goblin to the target
        Vector3 explosionToTarget = targetPosition - transform.position;

        //calculate the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;

        //find the proportion of maximum distance to the actual distance
        float relativeDistance = (exRad - explosionDistance) / exRad;

        //multiply the distance by max damage
        float damage = relativeDistance * maxDamage;

        //make sure the damage is always 0 or greater
        damage = Mathf.Max(0f, damage);

        //round damage off
        Mathf.Round(damage);

        return damage;
    }
}