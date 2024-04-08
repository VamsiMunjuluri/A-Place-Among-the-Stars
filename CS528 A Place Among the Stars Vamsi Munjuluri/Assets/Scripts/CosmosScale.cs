using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CosmosScale : MonoBehaviour
{
    public Text label;
    public Slider slider;
    public CosmosJourney cosmosJourney;
    public ExoCosmos exoCosmos;
    public List<ConstellationDrawer> constellationDrawers; // List to hold multiple constellation drawers
    public CAVE2WandNavigator navController;

    void Start()
    {
        navController = GetComponentInParent<CAVE2WandNavigator>();

        float sliderVal = slider.value; // Assuming this is the initial scale value
        label.text = "CosmosScale: " + sliderVal + "Par/Feet";

        UpdateScale(); // Initialize scale on start
    }

    public void UpdateScale()
    {
        float sliderVal = slider.value;
        float scaleFactor = sliderVal; // This is your scale factor based on the slider value

        cosmosJourney.AdjustParticleScale(scaleFactor);
        exoCosmos.AdjustParticleScale(scaleFactor);

        // Update all constellation drawers
        foreach (ConstellationDrawer drawer in constellationDrawers)
        {
            if (drawer != null)
            {
                drawer.UpdateConstellationLines(scaleFactor);
            }
        }

        label.text = "CosmosScale: " + sliderVal + "Par/Feet";
    }

    // Call this method to add a new ConstellationDrawer to be managed
    public void AddConstellationDrawer(ConstellationDrawer newDrawer)
    {
        if (newDrawer != null && !constellationDrawers.Contains(newDrawer))
        {
            constellationDrawers.Add(newDrawer);
        }
    }

    // Call this method to remove a ConstellationDrawer from being managed
    public void RemoveConstellationDrawer(ConstellationDrawer drawerToRemove)
    {
        if (drawerToRemove != null)
        {
            constellationDrawers.Remove(drawerToRemove);
        }
    }
}
