using UnityEngine;
using TMPro; // Use this namespace if you're using TextMeshPro

public class DistanceDisplay : MonoBehaviour
{
    public Transform target; // Assign your player or camera here
    public TMP_Text distanceText; // Assign your TextMeshPro text component here

    void Update()
    {
        // Calculate the distance from the origin (0, 0, 0) to the target's position
        float distance = Vector3.Distance(target.position, Vector3.zero);

        // Convert distance to Parsecs from meters, assuming the initial distance is in meters.
        double distanceInParsecs = (distance - 1.6) * 3.28084;

        // Update the text based on distance ranges
        if (distanceInParsecs < 10)
        {
            distanceText.text = "Just stepping out into the cosmos!";
        }
        else if (distanceInParsecs < 50)
        {
            distanceText.text = "Exploring the local star cluster.";
        }
        else if (distanceInParsecs < 100)
        {
            distanceText.text = "Venturing into the galactic neighborhood!";
        }
        else if (distanceInParsecs < 500)
        {
            distanceText.text = "Crossing vast interstellar distances...";
        }
        else
        {
            distanceText.text = "In the deep void between galaxies, truly far from home.";
        }

        // Optionally, show the exact distance in Parsecs with the message
        distanceText.text += $"\n{distanceInParsecs:F2} Parsecs from home.";
    }

}

