using UnityEngine;
using UnityEngine.UI;

public class RaycastGun : Gun {
    [Header("Firing")]
    public float range = 100f;
    public float damage = 50f;
    public AudioSource shotSound;
    //public float fireNum = 1; repeater/shotgun functionality
    //public float burstDelay = .25; repeater functionality
    //public float fireAngle = 15; shotgun functionality

    //also should be for a future raycast gun; projectile weapons should have limited ammo
    [Header("Cooling")]
    public float maxHeat = 15;
    public float heatPerShot = 1;
    public float coolingRate = 1; //cools x amount per second
    protected float heat = 0;
    protected bool overheated = false;

    //also should be for a future raycast gun; projectile weapons wouldn't need a heatslider, they'd need an ammo counter
    [Header("Heat Slider UI")]
    [SerializeField] protected Slider heatSlider;
    [SerializeField] protected Image heatFillImage;
    public Color coolColor = Color.yellow;
    public Color hotColor = Color.red;

    public override void Activate(Player player) {
        base.Activate(player);
        HeatSliderSetup();
    }

    protected virtual void HeatSliderSetup() {
        if (heatSlider == null) {
            heatSlider = player.heatSlider;
        }
        if (heatFillImage == null) {
            heatFillImage = player.heatFillImage;
        }

        heatSlider.gameObject.SetActive(true);
        heatFillImage.color = coolColor;
        InvokeRepeating("Cooling", 0, .05f);
    }

    protected virtual void Cooling() {
        //as this will be invoked 20 times per second, decrement by coolingrate / 20
        if (heat > 0) {
            heat -= coolingRate / 20;
            heatSlider.value = heat / maxHeat;
            heatFillImage.color = Color.Lerp(coolColor, hotColor, heat / maxHeat);
        } else {
            //once done cooling, set overheated to false so you can fire again
            overheated = false;
        }
    }

    protected override void Fire() {
        //if the next shot is available and the gun is not overheated
        if (nextFire < Time.time && !overheated) {

            Quaternion rot = Quaternion.Euler(transform.eulerAngles.z, transform.eulerAngles.y - 90, 0);
            GameObject flash = Instantiate(muzzleFlash, fireLoc.position, rot);
            flash.transform.parent = gameObject.transform;

            //recoil stuff, commented out is another way to do it
            transform.position += new Vector3(0, Random.Range(upRecoil / 2, upRecoil), 0); //transform.position += Vector3.up * Random.Range(upRecoil / 2, upRecoil);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, Random.Range(-upRecoilRotation, -upRecoilRotation / 3)));

            //heat management
            nextFire = Time.time + fireRate;
            heat += heatPerShot;
            if (heat > maxHeat) {
                overheated = true;
            }

            //creates a bitmask for the enemyHibox layer
            int layerMask = 1 << 11;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, layerMask, QueryTriggerInteraction.Collide)) {

                EnemyHitbox enemyHitbox = hit.collider.transform.GetComponent<EnemyHitbox>();
                if (enemyHitbox != null) {
                    enemyHitbox.TakeDamageHandler(damage);
                }
            }
        }
    }

    protected void OnDisable() {
        heatSlider.gameObject.SetActive(false);
    }
}
