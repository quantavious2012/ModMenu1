using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

[BepInPlugin("com.coder2.privatebase", "Coder2 Sandbox Menu", "1.0.0")]
public class MyModMenu : BaseUnityPlugin
{
    private bool menuOpen = true;
    private bool speedEnabled = false;
    private bool gravityEnabled = false;

    void Awake()
    {
        var harmony = new Harmony("com.coder2.privatebase");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    void OnGUI()
    {
        // Only shows if you press a specific button or keep it toggled
        if (menuOpen)
        {
            GUI.Box(new Rect(10, 10, 200, 150), "Coder2 Private Menu");

            if (GUI.Button(new Rect(20, 40, 180, 30), speedEnabled ? "Speed: ON" : "Speed: OFF"))
            {
                speedEnabled = !speedEnabled;
            }

            if (GUI.Button(new Rect(20, 80, 180, 30), gravityEnabled ? "Low Grav: ON" : "Low Grav: OFF"))
            {
                gravityEnabled = !gravityEnabled;
            }
        }
    }

    void Update()
    {
        // Safety check: Only runs if you are in a PRIVATE room
        if (PhotonNetwork.InRoom && !PhotonNetwork.CurrentRoom.IsVisible)
        {
            if (speedEnabled) 
                GorillaLocomotion.Player.Instance.jumpMultiplier = 1.5f;
            
            if (gravityEnabled)
                Physics.gravity = new Vector3(0, -4.9f, 0); // Half gravity
        }
        else
        {
            // Reset to defaults if you leave or enter a public room
            GorillaLocomotion.Player.Instance.jumpMultiplier = 1.1f;
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }
}
