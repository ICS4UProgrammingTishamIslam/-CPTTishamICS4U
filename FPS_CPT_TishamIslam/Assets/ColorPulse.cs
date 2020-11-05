using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPulse : MonoBehaviour
{
    public float fxDis = 40f;
    public float fxXInt = 1f;
    public float fxYInt = 1.5f;

    public float fxRInt = 6.66f;
    public float fxGInt = 0.69f;
    public float fxBInt = 4.20f;

    private bool xInc = true;
    private bool yInc = true;
    private bool rInc = true;
    private bool gInc = true;
    private bool bInc = true;

    private Shadow fx;

    void Start()
    {
        //get fx component to modify its values
        fx = GetComponent<Outline>();
    }

    void Update()
    {
        //move the x and y distances of the outline, separately for added coolness
        if (xInc)
        {
            fx.effectDistance = new Vector2(Mathf.Lerp(fx.effectDistance.x, fxDis * 1.6f, Time.deltaTime * fxXInt),
                fx.effectDistance.y);
        }
        else
        {
            fx.effectDistance = new Vector2(Mathf.Lerp(fx.effectDistance.x, -fxDis * 1.6f, Time.deltaTime * fxXInt),
            fx.effectDistance.y);
        }
        if (yInc)
        {
            fx.effectDistance = new Vector2(fx.effectDistance.x,
                Mathf.Lerp(fx.effectDistance.y, fxDis, Time.deltaTime * fxYInt));
        }
        else
        {
            fx.effectDistance = new Vector2(fx.effectDistance.x,
                Mathf.Lerp(fx.effectDistance.y, -fxDis, Time.deltaTime * fxYInt));
        }

        //if they hit their max distance (or close enough, really), make them go back
        if (fx.effectDistance.x <= (-fxDis + fxDis / 5) * 1.6)
            xInc = true;
        if (fx.effectDistance.x >= (fxDis - fxDis / 5) * 1.6)
            xInc = false;

        if (fx.effectDistance.y <= -fxDis + fxDis / 5)
            yInc = true;
        if (fx.effectDistance.y >= fxDis - fxDis / 5)
            yInc = false;

        //switch around the color values
        if (rInc)
        {
            fx.effectColor = new Color(Mathf.Lerp(fx.effectColor.r, .8f, Time.deltaTime * fxRInt),
                fx.effectColor.g, fx.effectColor.b);
        }
        else
        {
            fx.effectColor = new Color(Mathf.Lerp(fx.effectColor.r, .6f, Time.deltaTime * fxRInt),
                fx.effectColor.g, fx.effectColor.b);
        }
        if (gInc)
        {
            fx.effectColor = new Color(fx.effectColor.r,
                Mathf.Lerp(fx.effectColor.g, .8f, Time.deltaTime * fxGInt), fx.effectColor.b);
        }
        else
        {
            fx.effectColor = new Color(fx.effectColor.r,
                Mathf.Lerp(fx.effectColor.g, .6f, Time.deltaTime * fxGInt), fx.effectColor.b);
        }
        if (bInc)
        {
            fx.effectColor = new Color(fx.effectColor.r, fx.effectColor.g,
                Mathf.Lerp(fx.effectColor.b, .8f, Time.deltaTime * fxBInt));
        }
        else
        {
            fx.effectColor = new Color(fx.effectColor.r, fx.effectColor.g,
                Mathf.Lerp(fx.effectColor.b, .6f, Time.deltaTime * fxBInt));
        }

        //if they hit their min distance (or close enough, really), make them go up while another colour goes back
        if (fx.effectColor.r <= 0 + .3f)
        {
            rInc = true;
            bInc = false;
        }
        if (fx.effectColor.g <= 0 + .3f)
        {
            gInc = true;
            rInc = false;
        }
        if (fx.effectColor.b <= 0 + .3f)
        {
            bInc = true;
            gInc = false;
        }
    }
}
