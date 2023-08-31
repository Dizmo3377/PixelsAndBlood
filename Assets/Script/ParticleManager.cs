using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static GameObject[] particles;
    public static Vector3 standartRotation = Vector3.zero;
    private void Awake()
    {
        GetData();
    }
    public static GameObject Create(string name, Vector2 position, float delay = 0f, bool isLoop = false) 
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].name == name)
            {
                GameObject particle = Instantiate(particles[i], position, Quaternion.identity);
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                if (!isLoop)
                    Destroy(particle, ps.main.duration + delay);
                ps.Play();
                return particle;
            }
        }
        return null;
    }

    public static GameObject Create(string name, Vector2 position, Vector3 rotation, float delay = 0f)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].name == name)
            {
                GameObject particle = Instantiate(particles[i], position, Quaternion.Euler(rotation));
                ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                Destroy(particle, ps.main.duration + delay);
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