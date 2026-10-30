using System.Collections.Generic;
using UnityEngine;

namespace Charso
{
    /// <summary>Subtle, allocation-free ambience for foliage and water highlights.</summary>
    public sealed class MehrganAmbientAnimator : MonoBehaviour
    {
        readonly List<Transform> foliage=new List<Transform>();
        readonly List<Quaternion> foliageRotations=new List<Quaternion>();
        readonly List<Transform> water=new List<Transform>();
        readonly List<Vector3> waterScales=new List<Vector3>();

        void Awake()
        {
            foreach(Transform t in GetComponentsInChildren<Transform>(true))
            {
                if(t.name=="Tree canopy"||t.name=="Tree canopy highlight")
                {
                    foliage.Add(t);foliageRotations.Add(t.localRotation);
                }
                else if(t.name=="Fountain water shimmer")
                {
                    water.Add(t);waterScales.Add(t.localScale);
                }
            }
        }

        void Update()
        {
            float time=Time.time;
            for(int i=0;i<foliage.Count;i++)
            {
                float phase=time*.65f+i*.71f;
                foliage[i].localRotation=foliageRotations[i]*Quaternion.Euler(Mathf.Sin(phase)*1.1f,0,Mathf.Cos(phase*.83f)*1.35f);
            }
            for(int i=0;i<water.Count;i++)
            {
                float pulse=1f+Mathf.Sin(time*1.15f+i*1.9f)*.055f;
                Vector3 s=waterScales[i];water[i].localScale=new Vector3(s.x*pulse,s.y,s.z*pulse);
            }
        }
    }
}