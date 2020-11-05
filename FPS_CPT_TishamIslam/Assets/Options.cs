using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Options
{
    private static float xSens = 5, ySens = 5, volume = 1; // mouse sensitivity + volume
    private static string lastScene = "ComputerLab", deathMessage = "You managed to die before the game started, huh?"; //last scene loaded, death message for death scene
    private static bool mute = false, gunEnabled = true; //muting the game, preventing gun fire

    public static float XSens
    {
        get
        {
            return xSens;
        }
        set
        {
            xSens = value;
        }
    }
    public static float YSens
    {
        get
        {
            return ySens;
        }
        set
        {
            ySens = value;
        }
    }
    public static float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            if (value > 1)
            {
                value = 1;
            }
            else if (value < 0) 
            {
                value = 0;
            }
            volume = value;          
        }
    }
    //technically not an option, but needed to be kept somewhere unchanging
    public static string LastScene
    {
        get
        {
            return lastScene;
        }
        set
        {
            lastScene = value;
        }
    }
    public static bool Mute
    {
        get
        {
            return mute;
        }
        set
        {
            mute = value;
        }
    }
    public static bool GunEnabled
    {
        get
        {
            return mute;
        }
        set
        {
            mute = value;
        }
    }

    public static string DeathMessage
    {
        get
        {
            return deathMessage;
        }
        set
        {
            deathMessage = value;
        }
    }

}
