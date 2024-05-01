using System.Linq;
using UnityEngine;

public class ParticleManager : Singletone<ParticleManager>
{
    public static GameObject[] particles;

    public void Start() => LoadParticles();

    private void LoadParticles()
    {
        try
        {
            particles = Resources.LoadAll<GameObject>("Particles");
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Error loading assets in ParticleManager. Folder is missing or invalid. Use Resources/Particles");
            throw;
        }
    }

    public GameObject Create(string name, Vector2 position, float delay = 0f, bool isLoop = false) 
    {
        GameObject particlePrefab = particles.FirstOrDefault(p => p.name == name);
        if (particlePrefab == null) return null;

        GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        if (!isLoop) main.stopAction = ParticleSystemStopAction.Destroy;

        particleSystem.Play();
        return particle;
    }

}