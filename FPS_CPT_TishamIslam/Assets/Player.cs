using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    //player components
    private Rigidbody rb;
    //game manager
    public GameManager manageScript;

    [Header("UI Components")]
    public Slider healthSlider;
    public Image fillImage;
    public Color fullColor = Color.green;
    public Color emptyColor = Color.red;

    [Header("Useful Numbers")]
    public int maxHealth = 100;
    private int currentHealth;
    public float sensitivityX = 5;
    public float sensitivityY = 5;
    public float maxMoveSpeed = 5;
    public float jumpheight = 5;
    public float sprintFactor = 2;
    public bool grounded = true;

    //camera ting
    private float horMove = 0;
    private float zoomLevel = 1;

    [Header("Weapon Setup")]
    public GameObject[] weapons;
    public GameObject equipped;
    Gun gunScript;
    private int equippedIndex = 0;
    public float switchDelay = 1;
    private float switchTime = 0;

    [Header("Weapon UI Setup")]
    public Slider heatSlider;
    public Image heatFillImage;
    public Text ammoTextObject;

    private void Start() {
        //intialize everything
        if (manageScript == null) {
            manageScript = FindObjectOfType<GameManager>();
        }

        fillImage.color = fullColor;
        rb = GetComponent<Rigidbody>();
        horMove = 0;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        GunSetup();
    }

    private void GunSetup() {
        if (heatSlider == null) {
            heatSlider = GameObject.Find("WeaponHeat").GetComponent<Slider>();
        }
        if (heatFillImage == null) {
            heatFillImage = GameObject.Find("HeatFill").GetComponent<Image>();
        }
        if (ammoTextObject == null) {
            ammoTextObject = GameObject.Find("WeaponAmmo").GetComponent<Text>();
        }

        equippedIndex = 0;
        equipped = weapons[equippedIndex];
        gunScript = equipped.GetComponent<Gun>();
        gunScript.Activate(this);
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
    }

    private void Update() {
        if (Time.timeScale == 0) {
            return;
        }

        //turn left and right, influenced by user set sensitivity and current weapon zoom, if any
        zoomLevel = Options.GunEnabled ? gunScript.cam.fieldOfView / gunScript.defFOV : 1f;
        sensitivityX = Options.XSens;
        //fillImage.color = Color.Lerp(fullColor, emptyColor, currentHealth / maxHealth);
        horMove += sensitivityX * Input.GetAxis("Mouse X") * zoomLevel;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, horMove, 0);

        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)) {
            SwitchWeapon(Input.GetAxis("NextWeapon"));
        }

        //if you fall under the map
        if (transform.position.y <= -5) {
            manageScript.PlayerDied("Fall");
        }
    }

    public void TakeDamage(int damage, string attacker = "") {
        //take damage, update the slider, and check if you've died
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        //so far there's only goblins that reduce health
        if (currentHealth <= 0) {
            //disable movement, and inform the management about your tragic death
            maxMoveSpeed = 0; 
            GameObject crosshairObject = GameObject.Find("Crosshair");
            Destroy(crosshairObject);
            
            manageScript.PlayerDied(attacker);
        }
    }

    public void Move() {
        //makes you move it
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * maxMoveSpeed
            //if sprinting, increase speed even further
            * (1 + sprintFactor * Input.GetAxis("Sprint")) * zoomLevel);

        //check if you're on the ground, as you can't double jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            grounded = false;
            rb.AddForce(new Vector3(0, jumpheight, 0));
        }
    }

    public void SwitchWeapon(float input) {
        //check if an amount of time has passed or is guns are enabled. if not, prevent weapon switching
        if (Time.time > switchTime && Options.GunEnabled) {
            switchTime = switchDelay + Time.time;
        } else {
            return;
        }

        //this makes it so that the guns aren't cooling more than they should
        gunScript.CancelInvoke();

        //int casting and incrementing equippedIndex, set equipped weapon to inactive
        int change = (int)input;
        equippedIndex += change;
        equipped.SetActive(false);

        //basically, if the switch goes over or under accepted index values, make it circle around
        //for example, if there's 2 weapons, and I call weapons[2], then it'll circle back to weapons[0] as weapons[2] does not exist
        try {
            equipped = weapons[equippedIndex];
        } catch {
            if (equippedIndex > weapons.Length - 1) {
                equippedIndex = 0;
                equipped = weapons[equippedIndex];
            } else if (equippedIndex < 0) {
                equippedIndex = weapons.Length - 1;
                equipped = weapons[equippedIndex];
            }
        }

        //make sure gunscript is updated, and equipped is set to active
        equipped.SetActive(true);
        gunScript = equipped.GetComponent<Gun>();
        gunScript.Activate(this);
    }
}


