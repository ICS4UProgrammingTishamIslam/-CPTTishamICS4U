using UnityEngine;
using UnityEngine.UI;


public abstract class Gun : MonoBehaviour {
    [Header("Firing")]
    public float fireRate = 0.5f;
    protected float nextFire = 0;

    [Header("Aiming")]
    public Vector3 aimPos;
    public Vector3 aimRot;
    public Vector3 restPos;
    public Vector3 restRot;
    public Camera cam;
    [SerializeField] protected Transform playerPos;
    public float aimSpeed = 5f;
    public float defFOV = 60f;
    public float aimedFOV = 40f;

    [Header("Bullet Firing")]
    public float upRecoil;
    [Tooltip("Modifies Z axis negatively")] public float upRecoilRotation;
    public Transform fireLoc;
    public GameObject muzzleFlash;
    public Image crosshairObject;
    public Sprite crosshair;
    public Color crosshairColour = Color.white;
    public Vector3 initCrosshairScale = new Vector3(1, 1, 1);
    public Vector3 initCrosshairPos = new Vector3(0, 0, 0);

    protected Player player;

    public virtual void Activate(Player player) {
        this.player = player;
        BasicNullComponentCheck();
        CrosshairSetup();
    }

    protected void BasicNullComponentCheck() {
        //if not already assigned, find them and assign them. 
        //most of these are reliable since there should be only one of each object
        if (crosshairObject == null) {
            crosshairObject = GameObject.Find("Crosshair").GetComponent<Image>();
        }
        if (cam == null) {
            cam = FindObjectOfType<Camera>();
        }
        if (playerPos == null) {
            playerPos = player.transform;
        }
        //this one should be a last resort, not the default
        if (fireLoc == null) {
            fireLoc = GameObject.Find("FireLoc").GetComponent<Transform>();
        }
    }

    protected void CrosshairSetup() {
        crosshairObject.sprite = crosshair;
        crosshairObject.color = crosshairColour;
        crosshairObject.rectTransform.localScale = initCrosshairScale;
        crosshairObject.rectTransform.localPosition = initCrosshairPos;
    }

    protected virtual void Update() {
        if (!Options.GunEnabled) {
            return;
        }

        //aim is constantly readjusted
        Aim();
        if (Input.GetAxis("Fire") == 1) {
            Fire();
        }
    }

    protected virtual void Fire() {
        //individual firing logic will differ for different weapon types
        //derived classes will contain their own logic
    }

    protected virtual void Aim() {
        if (crosshairObject == null) {
            return;
        }

        if (Input.GetAxis("Aim") == 1) {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimedFOV, Time.deltaTime * aimSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(aimRot), aimSpeed * Time.deltaTime);
            crosshairObject.gameObject.SetActive(false);
        } else {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defFOV, Time.deltaTime * aimSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, restPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(restRot), aimSpeed * Time.deltaTime);
            crosshairObject.gameObject.SetActive(true);
        }
    }
}
