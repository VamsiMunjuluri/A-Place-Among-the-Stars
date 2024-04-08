using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExoCosmos : MonoBehaviour


{
    private ParticleSystem ps;
    //public TextAsset constFile;
    private List<GameObject> constellationLines = new List<GameObject>();
    ParticleSystem.Particle[] particleStars;
    public TextAsset starCSV;
    public int maxParticles = 100;
    
    public Dictionary<int, Vector3> starPositionsByHIP = new Dictionary<int, Vector3>();
    private int counter = 1;
    private Vector3[] originalPositions;

    // Start is called before the first frame update
    void Awake ()
    {

        


    }

    void OnEnable()
    {

        InitializeParticles();
        Debug.Log("CosmosJourney script was enabled.");

    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
    }

    public void AdjustParticleScale(float scaleFactor)
    {
        // Ensure originalPositions has been initialized
        if (originalPositions == null)
        {
            Debug.LogError("Original positions not initialized.");
            return;
        }

        // Loop through each particle and adjust its position based on the scaleFactor
        for (int i = 0; i < particleStars.Length; i++)
        {
            // Adjust the position by multiplying the original position by the scaleFactor
            Vector3 adjustedPosition = new Vector3(
                originalPositions[i].x * scaleFactor,
                originalPositions[i].y * scaleFactor,
                originalPositions[i].z * scaleFactor
            );

            particleStars[i].position = adjustedPosition;
        }

        // Apply the adjusted particles back to the particle system
        ps.SetParticles(particleStars, particleStars.Length);
    }
    float GetSizeFromSpectralClass(string spectralClass)
    {
        // Define relative size for each spectral class based on the image provided.
        // These are example values and should be adjusted based on your visual needs and scene scale.
        Dictionary<string, float> spectralSizes = new Dictionary<string, float>()
    {
        { "O", 6.6f }, // Larger than 6.6 solar radii
        { "B", 1.8f }, // Between 1.8 and 6.6 solar radii
        { "A", 1.4f }, // Between 1.4 and 1.8 solar radii
        { "F", 1.1f }, // Between 1.1 and 1.4 solar radii
        { "G", 1.0f }, // Between 0.9 and 1.1 solar radii, let's take 1 as an average
        { "K", 0.8f }, // Between 0.7 and 0.9 solar radii
        { "M", 0.7f }  // Less than 0.7 solar radii
    };

        if (spectralSizes.TryGetValue(spectralClass, out float relativeSize))
        {
            // Assuming 1 unit in the scene represents the size of the sun
            // Adjust this base size to fit the scale of your scene appropriately.
            float baseSize = 1.0f;

            // Use a logarithmic scale to reduce the disparity in sizes.
            // The constant 0.1f is arbitrary and can be adjusted to suit your needs.
            return baseSize * Mathf.Log(1 + relativeSize) / 0.1f;
        }
        else
        {
           // Debug.LogWarning("Spectral class '" + spectralClass + "' not recognized. Defaulting to base size.");
            return 1.0f; // Default size if spectral class is unknown
        }
    }



    public Color GetColorBasedOnPlanetCount(int planetCount)
    {
        switch (planetCount)
        {
            case 1: return new Color(0.0f, 0.5f, 1.0f); // Cool blue
            case 2: return new Color(0.5f, 0.7f, 1.0f); // Light blue
            case 3: return new Color(0.5f, 1.0f, 0.5f); // Pale green
            case 4: return new Color(1.0f, 1.0f, 0.0f); // Yellow
            case 5: return new Color(1.0f, 0.64f, 0.0f); // Orange
            case 6: return Color.white; // White for 6 or more planets
            default: return Color.grey; // Grey to indicate data outside the expected range
        }
    }


    float GetBrightnessFromAbsMag(float absMag)
    {
        // Define the range of absolute magnitudes visible in your scene.
        // These are example values and should be adjusted based on your visual needs.
        float brightestMag = -10f; // Brightest star (Sirius)
        float dimmestMag = 16f; // Adjust this based on the dimmest star you want to be visible

        // Clamp the absolute magnitude to the range we're working with
        absMag = Mathf.Clamp(absMag, brightestMag, dimmestMag);

        // Convert the magnitude to a linear scale between 0 and 1, where 0 is the dimmest and 1 is the brightest.
        // This formula can be adjusted based on how you want the scale to work visually.
        float brightness = 1f - ((absMag - brightestMag) / (dimmestMag - brightestMag));

        // Since the scale is logarithmic, we might want to apply a power to simulate this effect
        // You can adjust the power to control how the brightness scales.
        brightness = Mathf.Pow(brightness, 2.5f);

        return brightness;
    }
    void Start()
    {
        InitializeParticles();
    }

    void InitializeParticles()
    {

        ps = GetComponent<ParticleSystem>();
        particleStars = new ParticleSystem.Particle[maxParticles];

        var main = ps.main;
        main.maxParticles = maxParticles;
        //main.startSize = 3.0F;



        var em = ps.emission;
        em.enabled = true;
        em.rateOverTime = 0;


        em.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0.0f, (short)maxParticles, (short)maxParticles)

            });

        string[] lines = starCSV.text.Split('\n');
        int numParticlesAlive = ps.GetParticles(particleStars);
        Debug.Log("Particles before initialization: " + numParticlesAlive); // Should be 0

        for (int i = 1; i < maxParticles; i++)
        {
            string[] components = lines[i].Split(',');
            if (components.Length >= 5)
            {
                Vector3 position = new Vector3(float.Parse(components[2]) * (float)3.28084,
                                                        float.Parse(components[4]) * (float)3.28084,
                                                        float.Parse(components[3]) * (float)3.28084);    // tried dividing by / (float)3.086)but it was very blurry
                particleStars[i].position = position;
                if (counter == 1)
                {
                    if (int.TryParse(components[0], out int hipId))
                    {
                        starPositionsByHIP.Add(hipId, position);
                    }
                }
                

                string spectralType = components[10];
                int planetCount = int.Parse(components[11]);
                float starSize = GetSizeFromSpectralClass(spectralType);
                float brightness = GetBrightnessFromAbsMag(float.Parse(components[5]));
                Color starColor = GetColorBasedOnPlanetCount(planetCount);
                // starColor *= brightness;
                // particleStars[i].startSize = 1.0f; // Make sure the size is large enough to be visible.
                particleStars[i].startLifetime = Mathf.Infinity;
                particleStars[i].startColor = starColor; // Use a color that will be visible.
                particleStars[i].startSize = 0.4F * starSize;
            }
            else
            {
                Debug.LogError("Invalid line in CSV: " + lines[i]);
            }
        }

        ps.SetParticles(particleStars, maxParticles);
        numParticlesAlive = ps.GetParticles(particleStars);
        Debug.Log("Particles after initialization: " + numParticlesAlive); // Should be maxParticles
        //CreateConstellations();
       // Debug.Log("Counter: " + counter);
        counter++;
        // Debug.Log("Counter: " + counter);
        originalPositions = new Vector3[maxParticles];
        for (int i = 0; i < maxParticles; i++)
        {
            if (particleStars[i].position != Vector3.zero) // Assuming particles are initially placed
            {
                originalPositions[i] = particleStars[i].position;
            }
        }
    }

    public Dictionary<int, Vector3> StarPositionsByHIP
    {
        get
        {
            return starPositionsByHIP;
        }
    }

   

}
