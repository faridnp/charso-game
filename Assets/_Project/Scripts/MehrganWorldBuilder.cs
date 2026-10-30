using UnityEngine;

namespace Charso
{
    /// <summary>
    /// Procedural greybox for Mehrgan's first playable hub.
    /// All structures are original gameplay placeholders, not historical reconstructions.
    /// Replace landmark placeholders only after provenance and geometry review.
    /// </summary>
    public sealed class MehrganWorldBuilder : MonoBehaviour
    {
        static readonly Color Sand = new Color(0.78f, 0.68f, 0.51f);
        static readonly Color Stone = new Color(0.82f, 0.77f, 0.66f);
        static readonly Color Blue = new Color(0.18f, 0.39f, 0.55f);
        static readonly Color Green = new Color(0.25f, 0.39f, 0.27f);
        static readonly Color Water = new Color(0.23f, 0.48f, 0.57f);

        public static void Build()
        {
            if (GameObject.Find("MehrganWorld") != null) return;
            var root = new GameObject("MehrganWorld");
            var ground = Cube(root.transform, "Mehrgan ground", new Vector3(0,-0.35f,0), new Vector3(34,.5f,34), Sand);
            Cube(root.transform, "Main avenue", new Vector3(0,-.07f,-1), new Vector3(3,.08f,32), new Color(.68f,.59f,.45f));
            Cube(root.transform, "Cross avenue", new Vector3(0,-.06f,2), new Vector3(30,.08f,2.2f), new Color(.68f,.59f,.45f));
            BuildGarden(root.transform, new Vector3(-7,0,1));
            BuildCourtyard(root.transform, new Vector3(7,0,5));
            BuildDistrict(root.transform, new Vector3(-10,0,-8), 3, 2);
            BuildDistrict(root.transform, new Vector3(9,0,-9), 3, 2);
            BuildLandmarkPlaceholder(root.transform, new Vector3(1,0,10));
            BuildLandmarkPlaceholder(root.transform, new Vector3(-11,0,10));
            BuildTrees(root.transform);
            SetupCamera();
            SetupLight();
        }

        static void BuildGarden(Transform p, Vector3 at)
        {
            Cube(p,"Garden lawn",at+new Vector3(0,.08f,0),new Vector3(8,.16f,7),Green);
            Cube(p,"Garden water channel",at+new Vector3(0,.18f,0),new Vector3(.55f,.08f,6.4f),Water);
            Cube(p,"Garden cross channel",at+new Vector3(0,.19f,0),new Vector3(7.2f,.08f,.45f),Water);
            for(int i=0;i<4;i++) {
                float x=(i%2==0?-2.5f:2.5f), z=(i<2?-2f:2f);
                Cube(p,"Garden path",at+new Vector3(x,.19f,z),new Vector3(.55f,.08f,1.6f),Stone);
            }
        }

        static void BuildCourtyard(Transform p, Vector3 at)
        {
            Cube(p,"Courtyard",at+new Vector3(0,.04f,0),new Vector3(9,.12f,7),Stone);
            for(int i=0;i<4;i++) {
                float x=(i%2==0?-3.7f:3.7f), z=(i<2?-2.7f:2.7f);
                Cube(p,"Courtyard arcade pier",at+new Vector3(x,1.05f,z),new Vector3(.48f,2.1f,.48f),new Color(.72f,.65f,.53f));
            }
            Cube(p,"Courtyard rear wall",at+new Vector3(0,1.1f,3.2f),new Vector3(8,2.2f,.35f),Blue);
        }

        static void BuildDistrict(Transform p, Vector3 origin, int cols, int rows)
        {
            for(int z=0;z<rows;z++) for(int x=0;x<cols;x++) {
                Vector3 pos=origin+new Vector3(x*3.3f,0,z*3.2f);
                float h=2.2f+((x+z)%3)*.55f;
                Cube(p,"Mehrgan house",pos+new Vector3(0,h*.5f,0),new Vector3(2.45f,h,2.35f),new Color(.75f,.62f,.46f));
                Cube(p,"Flat roof",pos+new Vector3(0,h+.12f,0),new Vector3(2.6f,.24f,2.5f),new Color(.59f,.46f,.34f));
                Cube(p,"Door",pos+new Vector3(0,.65f,-1.19f),new Vector3(.55f,1.3f,.05f),new Color(.25f,.22f,.18f));
                for(int w=0;w<2;w++) Cube(p,"Window",pos+new Vector3(w==0?-.72f:.72f,1.45f,-1.2f),new Vector3(.38f,.5f,.06f),Blue);
            }
        }

        static void BuildLandmarkPlaceholder(Transform p, Vector3 at)
        {
            Cube(p,"Heritage site plinth",at+new Vector3(0,.2f,0),new Vector3(6,.4f,5),new Color(.69f,.58f,.43f));
            Cube(p,"Heritage site massing",at+new Vector3(0,1.5f,0),new Vector3(4.6f,2.6f,3.8f),Stone);
            Cube(p,"Heritage entrance",at+new Vector3(0,1.1f,-1.94f),new Vector3(1.25f,2.1f,.08f),Blue);
            var dome=GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dome.name="Unverified landmark massing";
            dome.transform.SetParent(p,false);
            dome.transform.localPosition=at+new Vector3(0,3.25f,0);
            dome.transform.localScale=new Vector3(3.1f,1.5f,3.1f);
            dome.GetComponent<Renderer>().material.color=new Color(.22f,.43f,.56f);
        }

        static void BuildTrees(Transform p)
        {
            Vector3[] pts={new Vector3(-13,0,2),new Vector3(-13,0,-2),new Vector3(13,0,2),new Vector3(13,0,-2),new Vector3(5,0,13),new Vector3(-5,0,13)};
            foreach(var v in pts) {
                var trunk=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                trunk.name="Garden tree trunk";trunk.transform.SetParent(p,false);trunk.transform.localPosition=v+new Vector3(0,.8f,0);trunk.transform.localScale=new Vector3(.32f,1.6f,.32f);trunk.GetComponent<Renderer>().material.color=new Color(.36f,.25f,.17f);
                var crown=GameObject.CreatePrimitive(PrimitiveType.Sphere);
                crown.name="Garden tree crown";crown.transform.SetParent(p,false);crown.transform.localPosition=v+new Vector3(0,2.1f,0);crown.transform.localScale=new Vector3(1.7f,1.8f,1.7f);crown.GetComponent<Renderer>().material.color=new Color(.28f,.42f,.28f);
            }
        }

        static GameObject Cube(Transform p,string name,Vector3 pos,Vector3 scale,Color color)
        {
            var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().material.color=color;return g;
        }

        static void SetupCamera()
        {
            var cam=Camera.main;
            if(cam==null) cam=new GameObject("Main Camera",typeof(Camera)).GetComponent<Camera>();
            cam.tag="MainCamera";cam.orthographic=true;cam.orthographicSize=17f;
            cam.transform.position=new Vector3(22,25,-27);
            cam.transform.rotation=Quaternion.Euler(38,-38,0);
            cam.backgroundColor=new Color(.69f,.78f,.82f);
            cam.clearFlags=CameraClearFlags.SolidColor;
        }

        static void SetupLight()
        {
            var light=Object.FindObjectOfType<Light>();
            if(light==null) light=new GameObject("Mehrgan sunlight",typeof(Light)).GetComponent<Light>();
            light.type=LightType.Directional;light.intensity=1.15f;light.transform.rotation=Quaternion.Euler(48,-32,0);
        }
    }
}