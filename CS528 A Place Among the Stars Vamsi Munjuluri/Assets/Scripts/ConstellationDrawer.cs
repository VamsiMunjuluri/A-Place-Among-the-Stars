using UnityEngine;
using System.Collections.Generic;

public class ConstellationDrawer : MonoBehaviour
{
    public CosmosJourney cosmosJourney;
    public CosmosScale cosmosScale;
    public TextAsset constFile;

    private Dictionary<GameObject, (Vector3, Vector3)> originalLinePositions = new Dictionary<GameObject, (Vector3, Vector3)>();

    private void Start()
    {
        if (cosmosJourney != null)
        {
            CreateConstellations(cosmosJourney.StarPositionsByHIP);
            if (cosmosScale != null && cosmosScale.slider != null)
            {
                cosmosScale.slider.onValueChanged.AddListener(delegate { UpdateConstellationLines(cosmosScale.slider.value); });
            }
        }
        else
        {
            Debug.LogError("CosmosJourney reference not set.");
        }
    }

    public void CreateConstellations(Dictionary<int, Vector3> starPositionsByHIP)
    {
        ClearConstellationLines();

        string[] lines = constFile.text.Split('\n');
        for (int j = 0; j < lines.Length; j++)
        {
            string[] values = lines[j].Split(' ');
            for (int i = 3; i < values.Length - 1; i += 2)
            {
                if (int.TryParse(values[i], out int hipId1) && int.TryParse(values[i + 1], out int hipId2))
                {
                    if (starPositionsByHIP.TryGetValue(hipId1, out Vector3 position1) && 
                        starPositionsByHIP.TryGetValue(hipId2, out Vector3 position2))
                    {
                        GameObject line = DrawLineBetweenStars(position1, position2);
                        // Store the original positions
                        originalLinePositions[line] = (position1, position2);
                    }
                }
            }
        }
    }

    private GameObject DrawLineBetweenStars(Vector3 position1, Vector3 position2)
    {
        GameObject line = new GameObject("ConstellationLine");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, position1);
        lineRenderer.SetPosition(1, position2);
        lineRenderer.startWidth = 0.05f; // Adjust as necessary
        lineRenderer.endWidth = 0.05f; // Adjust as necessary
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        line.transform.SetParent(this.transform, false);

        return line;
    }

    private void ClearConstellationLines()
    {
        foreach (GameObject line in originalLinePositions.Keys)
        {
            Destroy(line);
        }
        originalLinePositions.Clear();
    }

    public void UpdateConstellationLines(float scaleFactor)
    {
        foreach (var lineEntry in originalLinePositions)
        {
            LineRenderer lineRenderer = lineEntry.Key.GetComponent<LineRenderer>();
            // Apply scaling to the original positions
            Vector3 startPosition = lineEntry.Value.Item1 * scaleFactor;
            Vector3 endPosition = lineEntry.Value.Item2 * scaleFactor;

            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
