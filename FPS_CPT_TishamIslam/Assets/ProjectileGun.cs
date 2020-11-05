using UnityEngine;
using UnityEngine.UI;

public class ProjectileGun : Gun {
    //ammo will store range and damage logic
    [Header("Ammo Settings")]
    public GameObject ammo;
    public int ammoCapacity = 5;
    protected int ammoLeft;

    [Header("Ammo UI Settings")]
    public Text ammoTextObject;
    public string ammoText = "Rockets Left:";
    public Color textColor = Color.red;

    protected void Start() {
        ammoLeft = ammoCapacity;
    }

    public override void Activate(Player player) {
        base.Activate(player);
        AmmoCounterSetup();
    }

    protected virtual void AmmoCounterSetup() {
        if (ammoTextObject == null) {
            ammoTextObject = player.ammoTextObject;
        }
        ammoTextObject.gameObject.SetActive(true);
        ammoTextObject.text = ammoText + " " + ammoLeft;
        ammoTextObject.color = textColor;
    }

    protected override void Fire() {
        //if the next shot is available and the gun is not overheated
        if (nextFire < Time.time && ammoLeft > 0) {
            Quaternion rot = Quaternion.Euler(transform.eulerAngles.z, transform.eulerAngles.y - 90, 0);
            Instantiate(ammo, fireLoc.position, rot);
            GameObject flash = Instantiate(muzzleFlash, fireLoc.position, rot);
            flash.transform.parent = gameObject.transform;

            //recoil stuff, commented out is another way to do it
            transform.position += new Vector3(0, Random.Range(upRecoil / 2, upRecoil), 0); //transform.position += Vector3.up * Random.Range(upRecoil / 2, upRecoil);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, Random.Range(-upRecoilRotation, -upRecoilRotation / 3)));

            nextFire = Time.time + fireRate;
            ammoLeft--;
        } else if (ammoLeft <= 0) {
            //play empty ammo noise
        }
    }

    protected virtual void OnDisable() {
        ammoTextObject.gameObject.SetActive(false);
    }
}
