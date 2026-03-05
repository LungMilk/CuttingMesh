using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleComp : MonoBehaviour
{
    public GameObject particleObject;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ParticleStart()
    {
        FrictionGrabber FG = particleObject.GetComponent<FrictionGrabber>();
        if (FG.foundParticle == null) { return; }
        else
        {
            ParticleSystem psm = FG.foundParticle;
            psm.Play();

        }
    }

    public void ParticleStop()
    {
        FrictionGrabber FG = particleObject.GetComponent<FrictionGrabber>();
        if (FG.foundParticle == null) { return; }
        else
        {
            ParticleSystem psm = FG.foundParticle;
            psm.Stop();

        }
    }

    public void ParticleCleaner()
    {
        FrictionGrabber FG = particleObject.GetComponent<FrictionGrabber>();
        if (FG.foundParticle == null) { return; }
        else
        {
            FG.foundParticle = null;
            //SetVariableValue("foundParticle", null, FG);
            particleObject.GetComponent<FrictionGrabber>().foundParticle = null;
        }
    }
}
