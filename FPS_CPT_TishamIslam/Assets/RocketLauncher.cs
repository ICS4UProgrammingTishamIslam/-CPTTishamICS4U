using UnityEngine;

//might not be used?
public class RocketLauncher : ProjectileGun {
    [Header("Scope Crosshair Settings")]
    public Vector3 crosshairAimedPos;
    public Vector3 crosshairAimedScale;

    protected override void Aim() {
        if (crosshairObject == null) {
            return;
        }

        if (Input.GetAxis("Aim") == 1 && nextFire < Time.time) {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimedFOV, Time.deltaTime * aimSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(aimRot), aimSpeed * Time.deltaTime);

            crosshairObject.rectTransform.localPosition = Vector3.Lerp(crosshairObject.rectTransform.localPosition, crosshairAimedPos, aimSpeed/1.8f * Time.deltaTime);
            crosshairObject.rectTransform.localScale = Vector3.Lerp(crosshairObject.rectTransform.localScale, crosshairAimedScale, Time.deltaTime/1.8f * aimSpeed);
        } else {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defFOV, Time.deltaTime * aimSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, restPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(restRot), aimSpeed * Time.deltaTime);

            crosshairObject.rectTransform.localPosition = Vector3.Lerp(crosshairObject.rectTransform.localPosition, initCrosshairPos, aimSpeed/1.8f * Time.deltaTime);
            crosshairObject.rectTransform.localScale = Vector3.Lerp(crosshairObject.rectTransform.localScale, initCrosshairScale, Time.deltaTime/1.8f * aimSpeed);
        }
    }
}
