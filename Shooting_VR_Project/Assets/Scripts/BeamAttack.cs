using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    public ParticleSystem beam;
    float BeamTime;
    [SerializeField]
    protected LayerMask layer = 0;
    [SerializeField]
    protected GameObject explosion;
    protected float damege = 5;

    private void Start()
    {
        BeamTime = 0;
        beam = this.GetComponent<ParticleSystem>();
        beam.Stop();
    }

    //private void OnParticleTrigger()
    private void OnParticleCollision(GameObject obj)
    {
        //GameObject obj = beam.trigger.GetCollider(0).gameObject;
        Debug.Log("obj: " + obj);

        var fighter = obj.gameObject.GetComponent<IShootingDown>();
        Debug.Log("is: " + fighter);


        if (fighter != null)
        {
            //ダメージを与える
            fighter.Damage(damege);
            Explosion(obj);
            //Destroy(this.gameObject);
        }
    }

    //LayerMaskに対象のLayerが含まれているかチェックする
    protected bool CompareLayer(LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    protected void Explosion(GameObject gameObject)
    {
        if (explosion != null)
        { Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation); }
    }

    private void Update()
    {
        //beam.Play();
        
    }

}
