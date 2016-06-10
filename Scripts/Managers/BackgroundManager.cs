using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour 
{
    enum BackgroundType
    {
        brownMetal,
        concrete,
        plateMetal,
        stones,
        wood
    }

    [System.Serializable]
    public class Background
    {
        public string name = "Background";
        public Material background;
    }

    public GameObject backgroundPlane;
    public Background[] backgrounds;

    public void SetBackground(string background)
    {
        BackgroundType backgroundType = BackgroundType.brownMetal;
        switch (background)
        {
            case "brownMetal":
                backgroundType = BackgroundType.brownMetal;
                break;

            case "concrete":
                backgroundType = BackgroundType.concrete;
                break;

            case "plateMetal":
                backgroundType = BackgroundType.plateMetal;
                break;

            case "stones":
                backgroundType = BackgroundType.stones;
                break;

            case "wood":
                backgroundType = BackgroundType.wood;
                break;
        }

        backgroundPlane.renderer.material = backgrounds[(int)backgroundType].background;

    }
}
