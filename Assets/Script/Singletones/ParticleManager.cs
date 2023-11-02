using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    public static GameObject[] particles;
    public override void Awake() { base.Awake(); GetData(); }
    public static GameObject Create(string name, Vector2 position, float delay = 0f, bool isLoop = false) 
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].name == name)
            {
                GameObject particle = Instantiate(particles[i], position, Quaternion.identity);
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                var main = ps.main;
                if (!isLoop) main.stopAction = ParticleSystemStopAction.Destroy;
                ps.Play();
                return particle;
            }
        }
        return null;
    }

    private void GetData()
    {
        try
        {
            particles = Resources.LoadAll<GameObject>("Particles");
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Error loading audio assets in ParticleManager. Folder is missing or something, idk");
            throw;
        }
    }
}