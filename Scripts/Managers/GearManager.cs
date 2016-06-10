using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GearManager : MonoBehaviour 
{
    public enum GearMaterial
    {
        greyMetal,
        mossyMetal,
        shinyMetal,
        paintedMetal,
        wood
    }

    [Serializable]
    public class GearMaterialSet
    {
        public string name = "Gear Material";
        public Material threeSpoke;
        public Material fourSpoke;
        public Material fiveSpoke;
        public Material pin;
    }

    [Serializable]
    public class GearSet
    {
        public string name = "Gear Set 01";
        public GameObject gearSetRoot;
    }

    public static GearManager instance;
    public static float globalSpeedMultiplier = 3;
    public static bool gearsReady = false;

    internal List<Gear> driveGears;
    internal List<Gear> gears;

    public GearMaterialSet[] gearMaterials;
    public GearSet[] gearSets;

    private GearMaterial currentMaterial;

    void Awake()
    {
        if (instance == null) instance = this;
        gears = new List<Gear>();
        driveGears = new List<Gear>();
        SetGearSettings();
    }

    public void SetGearSettings()
    {
        List<Gear> gearsToPopulate = new List<Gear>(gears);

        do
        {
            foreach (Gear gear in gearsToPopulate)
            {
                if (!gear.isParentSettingsSet())
                {
                    gear.SetParentSettings();
                    gearsToPopulate.Remove(gear);
                    break;
                }
            }
        } while (gearsToPopulate.Count > 0);

        gearsReady = true;
    }

    #region Android Inputs
    public void SetGearRotationDirection(string direction)
    {
        switch (direction)
        {
            case "forward":
                foreach (Gear gear in driveGears){
                    gear.settings.rotation.rotateDirection = Gear.RotateDirection.forward;
                }
                break;

            case "reverse":
                foreach (Gear gear in driveGears){
                    gear.settings.rotation.rotateDirection = Gear.RotateDirection.reverse;
                }
                break;
        }
    }

    public void SetGearSpeed(string speed)
    {
        int gearSpeed = 3;

        try{
            gearSpeed = int.Parse(speed);
        }
        catch (System.Exception e){
            Debug.Log(e.StackTrace);
        }

        if (gearSpeed > 300) { gearSpeed = 300; }
        globalSpeedMultiplier = gearSpeed;
    }

    public void SetGearsMaterial(string material)
    {
        switch (material)
        {
            case "greyMetal":
                SetGearMaterials(GearMaterial.greyMetal);
                break;

            case "mossyMetal":
                SetGearMaterials(GearMaterial.mossyMetal);
                break;

            case "shinyMetal":
                SetGearMaterials(GearMaterial.shinyMetal);
                break;

            case "paintedMetal":
                SetGearMaterials(GearMaterial.paintedMetal);
                break;

            case "wood":
                SetGearMaterials(GearMaterial.wood);
                break;
        }
    }

    public void SetGearSet(string gearSet)
    {
        switch (gearSet)
        {
            case "gearSet01":
                SetGearSet(1);
                break;
            
            case "gearSet02":
                SetGearSet(2);
                break;

            case "gearSet03":
                SetGearSet(3);
                break;

            case "gearSet04":
                SetGearSet(4);
                break;

            case "gearSet05":
                SetGearSet(5);
                break;

        }
    }
    #endregion

    #region Android Methods
    private void SetGearMaterials(GearMaterial gearMaterial)
    {
        currentMaterial = gearMaterial;
        foreach (Gear gear in gears)
        {
            switch (gear.settings.dimensions.spokeCount)
            {
                case 3:
                    gear.settings.gearMesh.renderer.material = gearMaterials[(int)gearMaterial].threeSpoke;
                    break;

                case 4:
                    gear.settings.gearMesh.renderer.material = gearMaterials[(int)gearMaterial].fourSpoke;
                    break;

                case 5:
                    gear.settings.gearMesh.renderer.material = gearMaterials[(int)gearMaterial].fiveSpoke;
                    break;
            }
            gear.settings.pinMesh.renderer.material = gearMaterials[(int)gearMaterial].pin;
        }
    }

    private void SetGearSet(int index)
    {
        foreach (GearSet gearSet in gearSets)
        {
            gearSet.gearSetRoot.SetActive(false);
        }

        gearSets[index - 1].gearSetRoot.SetActive(true);

        SetGearMaterials(currentMaterial);
    }
    #endregion

}
