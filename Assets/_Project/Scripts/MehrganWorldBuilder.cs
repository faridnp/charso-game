using System;
using System.Collections.Generic;
using UnityEngine;

namespace Charso
{
    /// <summary>
    /// Procedural production blockout for Mehrgan.
    /// All geometry is original stylized environment dressing. Landmark forms remain placeholders
    /// until a researched, rights-cleared reconstruction is approved.
    /// </summary>
    public static class MehrganWorldBuilder
    {
        static readonly Dictionary<int, Material> Materials = new Dictionary<int, Material>();
        static Transform root;
        static Transform dynamicRoot;
        static readonly Color Sand = new Color(.72f,.59f,.42f);
        static readonly Color LightStone = new Color(.84f,.77f,.64f);
        static readonly Color DarkStone = new Color(.42f,.34f,.26f);
        static readonly Color Tile = new Color(.13f,.34f,.48f);
        static readonly Color TileLight = new Color(.25f,.51f,.61f);
        static readonly Color Water = new Color(.13f,.39f,.48f);
        static readonly Color Green = new Color(.22f,.36f,.23f);
        static readonly Color GreenLight = new Color(.35f,.48f,.29f);
        static readonly Color Wood = new Color(.32f,.22f,.14f);
        static readonly Color Plaster = new Color(.91f,.83f,.68f);

        public static void Build(int restorationStage = 0)
        {
            if (GameObject.Find("MehrganWorld") != null)
            {
                ApplyRestorationStage(restorationStage);
                return;
            }
            Materials.Clear();
            root = new GameObject("MehrganWorld").transform;
            root.position = Vector3.zero;
            dynamicRoot = Child(root, "Animated details");

            BuildGroundAndStreets();
            BuildCentralPlaza();
            BuildChaharBagh();
            BuildBazaar();
            BuildResidentialQuarters();
            BuildCivicCourtyard();
            BuildWorkshop();
            BuildCityEdgeAndGate();
            BuildLandmarkDistrict(restorationStage);
            BuildVegetation();
            BuildLightingAndCamera();
            ApplyRestorationStage(restorationStage);
        }

        public static void ApplyRestorationStage(int stage)
        {
            stage = Mathf.Clamp(stage, 0, 3);
            var site = GameObject.Find("MehrganLandmarkSite");
            if (site == null) return;
            var before = site.Find("RestorationStage_" + Mathf.Clamp(stage - 1, 0, 2));
            var current = site.Find("RestorationStage_" + stage);
            for (int i = 0; i < 4; i++)
            {
                var item = site.Find("RestorationStage_" + i);
                if (item != null) item.gameObject.SetActive(i == stage);
            }
        }

        static void BuildGroundAndStreets()
        {
            Box(root,"City terrain",new Vector3(0,-.55f,0),new Vector3(54,.9f,54),new Color(.69f,.59f,.45f));
            Box(root,"North-south stone road",new Vector3(0,-.045f,0),new Vector3(4,.08f,47),new Color(.57f,.48f,.37f));
            Box(root,"East-west stone road",new Vector3(0,-.035f,1),new Vector3(47,.08f,3.2f),new Color(.57f,.48f,.37f));
            Box(root,"North lane",new Vector3(-13,-.02f,0),new Vector3(2,.06f,44),new Color(.62f,.52f,.4f));
            Box(root,"South lane",new Vector3(13,-.02f,0),new Vector3(2,.06f,44),new Color(.62f,.52f,.4f));
            for(int i=-2;i<=2;i++)
            {
                Box(root,"Road paving seam",new Vector3(i*1.25f,.012f,0),new Vector3(.035f,.012f,47),new Color(.46f,.39f,.31f));
                Box(root,"Crossroad paving seam",new Vector3(0,.014f,1+i*.8f),new Vector3(47,.012f,.035f),new Color(.46f,.39f,.31f));
            }
            for(int side=-1;side<=1;side+=2)
            {
                Box(root,"Road curb",new Vector3(side*2.15f,.11f,0),new Vector3(.18f,.22f,47),LightStone);
                Box(root,"Road curb",new Vector3(0,.11f,side*2.8f+1),new Vector3(47,.22f,.18f),LightStone);
            }
        }

        static void BuildCentralPlaza()
        {
            var p=Child(root,"Central bazaar square");
            Box(p,"Plaza stone platform",new Vector3(0,.08f,1),new Vector3(11,.2f,10),LightStone);
            Box(p,"Plaza inset",new Vector3(0,.19f,1),new Vector3(8.8f,.035f,7.8f),new Color(.72f,.64f,.51f));
            for(int i=0;i<4;i++)
            {
                float a=i*Mathf.PI*.5f;
                Vector3 pos=new Vector3(Mathf.Cos(a)*3.1f,.38f,1+Mathf.Sin(a)*2.8f);
                Cylinder(p,"Plaza bollard",pos,new Vector3(.22f,.38f,.22f),DarkStone,12);
            }
            Cylinder(p,"Fountain basin",new Vector3(0,.42f,1),new Vector3(2.35f,.38f,2.35f),Tile,32);
            Cylinder(p,"Fountain inner pool",new Vector3(0,.63f,1),new Vector3(1.78f,.08f,1.78f),Water,32);
            Cylinder(p,"Fountain central bowl",new Vector3(0,.88f,1),new Vector3(.7f,.3f,.7f),LightStone,24);
            Cylinder(p,"Fountain finial",new Vector3(0,1.18f,1),new Vector3(.2f,.38f,.2f),TileLight,16);
            for(int i=0;i<4;i++)
            {
                float a=i*Mathf.PI*.5f;
                var lamp=BuildLantern(p,new Vector3(Mathf.Cos(a)*4.6f,.4f,1+Mathf.Sin(a)*4.1f));
                lamp.name="Plaza lantern";
            }
        }

        static void BuildChaharBagh()
        {
            var p=Child(root,"Chahar Bagh Persian garden");
            Box(p,"Garden boundary",new Vector3(-9,.08f,8),new Vector3(12,.2f,12),DarkStone);
            Box(p,"Garden soil",new Vector3(-9,.2f,8),new Vector3(11.5f,.08f,11.5f),Green);
            Box(p,"North water channel",new Vector3(-9,.34f,8),new Vector3(.58f,.09f,10.8f),Water);
            Box(p,"East water channel",new Vector3(-9,.35f,8),new Vector3(10.8f,.09f,.58f),Water);
            Box(p,"Garden central crossing",new Vector3(-9,.4f,8),new Vector3(1.25f,.12f,1.25f),TileLight);
            for(int x=0;x<2;x++) for(int z=0;z<2;z++)
            {
                float cx=-11.7f+x*5.4f, cz=5.3f+z*5.4f;
                Box(p,"Garden flower bed",new Vector3(cx,.31f,cz),new Vector3(4.3f,.12f,4.3f),GreenLight);
                for(int i=0;i<8;i++)
                {
                    float a=i*Mathf.PI/4f;
                    Flower(p,new Vector3(cx+Mathf.Cos(a)*1.55f,.42f,cz+Mathf.Sin(a)*1.55f),i%2==0?new Color(.78f,.47f,.35f):new Color(.9f,.73f,.42f));
                }
                Tree(p,new Vector3(cx,.38f,cz),.82f);
            }
            for(int i=0;i<4;i++)
            {
                float x=-9+(i%2==0?-4.8f:4.8f), z=8+(i<2?-4.8f:4.8f);
                Box(p,"Garden path slab",new Vector3(x,.32f,z),new Vector3(1.2f,.1f,1.2f),LightStone);
            }
            BuildPergola(p,new Vector3(-9,.3f,14.5f),5.5f);
        }

        static void BuildBazaar()
        {
            var p=Child(root,"Covered bazaar");
            for(int row=0;row<2;row++)
            {
                float z=-9.2f+row*4.5f;
                Box(p,"Bazaar covered walkway",new Vector3(0,.25f,z),new Vector3(20,.28f,3.6f),LightStone);
                for(int i=0;i<7;i++)
                {
                    float x=-9+i*3;
                    Box(p,"Bazaar shop wall",new Vector3(x,1.35f,z+(row==0?-1.45f:1.45f)),new Vector3(2.65f,2.4f,.32f),Plaster);
                    Box(p,"Shop timber door",new Vector3(x,.98f,z+(row==0?-1.27f:1.27f)),new Vector3(.9f,1.8f,.1f),Wood);
                    Box(p,"Shop sign board",new Vector3(x,2.1f,z+(row==0?-1.3f:1.3f)),new Vector3(1.2f,.28f,.12f),Tile);
                    Box(p,"Bazaar roof bay",new Vector3(x,3.05f,z),new Vector3(2.85f,.22f,3.9f),DarkStone);
                    for(int side=-1;side<=1;side+=2)
                        Box(p,"Bazaar arcade pier",new Vector3(x+1.35f,.0f,z+side*1.5f),new Vector3(.32f,2.9f,.32f),LightStone);
                }
                Box(p,"Bazaar roof spine",new Vector3(0,3.24f,z),new Vector3(20,.2f,.28f),Tile);
            }
            for(int i=0;i<5;i++)
            {
                float x=-8+i*4;
                Box(p,"Bazaar shade awning",new Vector3(x,2.45f,-13.5f),new Vector3(2.8f,.12f,1.7f),i%2==0?new Color(.63f,.25f,.19f):new Color(.82f,.68f,.48f));
            }
        }

        static void BuildResidentialQuarters()
        {
            BuildQuarter("West residential quarter",new Vector3(-19,0,-8),3,3);
            BuildQuarter("East residential quarter",new Vector3(15,0,-8),3,3);
            BuildQuarter("South residential quarter",new Vector3(-18,0,-20),3,2);
        }

        static void BuildQuarter(string name,Vector3 origin,int cols,int rows)
        {
            var p=Child(root,name);
            for(int z=0;z<rows;z++) for(int x=0;x<cols;x++)
            {
                Vector3 at=origin+new Vector3(x*3.6f,0,z*3.7f);
                float h=2.4f+((x*3+z)%3)*.38f;
                Color wall=(x+z)%3==0?new Color(.77f,.66f,.52f):Plaster;
                Box(p,"Adobe house volume",at+new Vector3(0,h*.5f,0),new Vector3(3.0f,h,3.05f),wall);
                Box(p,"House parapet",at+new Vector3(0,h+.13f,0),new Vector3(3.12f,.26f,3.16f),Sand);
                Box(p,"Recessed entrance",at+new Vector3(0,.76f,-1.54f),new Vector3(.72f,1.52f,.08f),Wood);
                Box(p,"Door lintel",at+new Vector3(0,1.56f,-1.6f),new Vector3(1.0f,.16f,.15f),Tile);
                for(int w=-1;w<=1;w+=2)
                {
                    Box(p,"Blue glazed window",at+new Vector3(w*.92f,1.55f,-1.56f),new Vector3(.45f,.58f,.08f),TileLight);
                    Box(p,"Window sill",at+new Vector3(w*.92f,1.22f,-1.63f),new Vector3(.62f,.1f,.22f),LightStone);
                }
                if((x+z)%2==0) BuildWindcatcher(p,at+new Vector3(0,h,0),1.0f);
                if((x+z)%3==1) BuildDome(p,at+new Vector3(0,h+.2f,0),.95f,Tile);
            }
        }

        static void BuildWindcatcher(Transform p,Vector3 at,float s)
        {
            Box(p,"Windcatcher shaft",at+new Vector3(0,.8f*s,0),new Vector3(.8f*s,1.6f*s,.8f*s),LightStone);
            for(int i=0;i<4;i++)
            {
                float a=i*Mathf.PI*.5f;
                Box(p,"Windcatcher vent",at+new Vector3(Mathf.Cos(a)*.42f*s,1.25f*s,Mathf.Sin(a)*.42f*s),new Vector3(.08f*s,.65f*s,.08f*s),Tile);
            }
            Box(p,"Windcatcher cap",at+new Vector3(0,1.65f*s,0),new Vector3(1.0f*s,.16f*s,1.0f*s),Tile);
        }

        static void BuildDome(Transform p,Vector3 at,float s,Color color)
        {
            Sphere(p,"Dome shell",at+new Vector3(0,.48f*s,0),new Vector3(1.6f*s,.95f*s,1.6f*s),color);
            Cylinder(p,"Dome drum",at+new Vector3(0,.12f*s,0),new Vector3(1.35f*s,.45f*s,1.35f*s),LightStone,20);
            Cylinder(p,"Dome finial",at+new Vector3(0,1.05f*s,0),new Vector3(.12f*s,.35f*s,.12f*s),Tile,12);
        }

        static void BuildCivicCourtyard()
        {
            var p=Child(root,"Civic courtyard");
            Box(p,"Courtyard platform",new Vector3(9,.1f,10),new Vector3(11,.25f,9),LightStone);
            Box(p,"Courtyard paving",new Vector3(9,.24f,10),new Vector3(9.8f,.06f,7.8f),new Color(.73f,.65f,.52f));
            Box(p,"Rear iwan mass",new Vector3(9,2.0f,13.9f),new Vector3(8,3.6f,.5f),Plaster);
            Box(p,"Iwan deep recess",new Vector3(9,1.7f,13.62f),new Vector3(3.2f,2.8f,.12f),Tile);
            Box(p,"Iwan inner door",new Vector3(9,1.1f,13.52f),new Vector3(1.4f,2.2f,.1f),Wood);
            for(int side=-1;side<=1;side+=2)
            {
                for(int i=0;i<3;i++)
                {
                    float z=7.4f+i*2.5f;
                    Box(p,"Courtyard arcade column",new Vector3(9+side*4.3f,1.35f,z),new Vector3(.42f,2.7f,.42f),LightStone);
                }
            }
            BuildDome(p,new Vector3(9,3.85f,10),1.8f,Tile);
            for(int i=0;i<4;i++)
            {
                float x=5.5f+(i%2)*7f,z=7.2f+(i/2)*5.6f;
                BuildLantern(p,new Vector3(x,.35f,z));
            }
        }

        static void BuildWorkshop()
        {
            var p=Child(root,"Restoration workshop");
            Box(p,"Workshop floor",new Vector3(18,.12f,10),new Vector3(7,.25f,6),DarkStone);
            Box(p,"Workshop wall",new Vector3(18,1.6f,12.7f),new Vector3(7,3.1f,.3f),Plaster);
            Box(p,"Workshop door",new Vector3(18,1.0f,12.5f),new Vector3(1.25f,2.0f,.12f),Wood);
            for(int i=0;i<3;i++)
            {
                Box(p,"Timber restoration beam",new Vector3(16.2f+i*1.8f,2.9f,9.2f),new Vector3(.18f,.18f,3.8f),Wood);
                Box(p,"Stone material stack",new Vector3(15.6f+i*2.0f,.55f,8.2f),new Vector3(1.1f,.75f,.8f),LightStone);
            }
            BuildLantern(p,new Vector3(18,.5f,7.3f));
        }

        static void BuildCityEdgeAndGate()
        {
            var p=Child(root,"Old city edge");
            Box(p,"North defensive wall",new Vector3(0,1.7f,25),new Vector3(52,3.4f,.8f),Sand);
            Box(p,"West defensive wall",new Vector3(-25,1.7f,0),new Vector3(.8f,3.4f,50),Sand);
            Box(p,"East defensive wall",new Vector3(25,1.7f,0),new Vector3(.8f,3.4f,50),Sand);
            for(int x=-24;x<=24;x+=4)
            {
                Box(p,"North wall merlon",new Vector3(x,3.7f,25),new Vector3(1.0f,.55f,1.0f),LightStone);
                Box(p,"South wall merlon",new Vector3(x,3.7f,-25),new Vector3(1.0f,.55f,1.0f),LightStone);
            }
            Box(p,"South gate tower left",new Vector3(-4,2.4f,-25),new Vector3(2,4.8f,1.2f),LightStone);
            Box(p,"South gate tower right",new Vector3(4,2.4f,-25),new Vector3(2,4.8f,1.2f),LightStone);
            Box(p,"South gate lintel",new Vector3(0,4.5f,-25),new Vector3(6,.7f,1.5f),Tile);
            Box(p,"South gate passage",new Vector3(0,1.8f,-25),new Vector3(4.8f,3.5f,.25f),Wood);
        }

        static void BuildLandmarkDistrict(int stage)
        {
            var site=Child(root,"MehrganLandmarkSite");
            Box(site,"Landmark square",new Vector3(0,.12f,17),new Vector3(13,.25f,8),LightStone);
            Box(site,"Landmark plinth",new Vector3(0,.38f,17),new Vector3(8,.28f,5.5f),DarkStone);
            for(int i=0;i<4;i++)
            {
                float x=i<2?-3.2f:3.2f,z=i%2==0?15.2f:18.8f;
                Box(site,"Restoration boundary post",new Vector3(x,.8f,z),new Vector3(.22f,1.2f,.22f),Wood);
            }
            Stage(site,0,"Before restoration",false);
            Stage(site,1,"Structural stabilization",true);
            Stage(site,2,"Facade and tile restoration",true);
            Stage(site,3,"Courtyard and garden renewal",true);
            ApplyRestorationStage(stage);
        }

        static void Stage(Transform p,int index,string label,bool active)
        {
            var s=Child(p,"RestorationStage_"+index);
            s.gameObject.SetActive(active);
            if(index==0)
            {
                Box(s,"Damaged heritage volume",new Vector3(0,1.35f,17),new Vector3(5.2f,2.5f,3.6f),new Color(.48f,.4f,.31f));
                Box(s,"Temporary timber brace",new Vector3(-2.8f,1.3f,17),new Vector3(.18f,2.8f,.18f),Wood);
                Box(s,"Temporary timber brace",new Vector3(2.8f,1.3f,17),new Vector3(.18f,2.8f,.18f),Wood);
                return;
            }
            Box(s,"Restored heritage walls",new Vector3(0,1.65f,17),new Vector3(5.8f,3.1f,3.8f),index>=2?Plaster:LightStone);
            Box(s,"Main iwan recess",new Vector3(0,1.55f,15.04f),new Vector3(2.2f,2.5f,.12f),index>=2?Tile:DarkStone);
            Box(s,"Main entrance",new Vector3(0,1.0f,14.96f),new Vector3(1.0f,2.0f,.1f),Wood);
            if(index>=2)
            {
                Box(s,"Blue tile frieze",new Vector3(0,3.12f,14.9f),new Vector3(5.9f,.32f,.12f),Tile);
                for(int i=-2;i<=2;i++)
                    Box(s,"Facade tile panel",new Vector3(i*1.05f,2.2f,14.91f),new Vector3(.55f,.7f,.1f),TileLight);
                BuildDome(s,new Vector3(0,3.35f,17),1.9f,Tile);
            }
            if(index>=3)
            {
                Box(s,"Renewed courtyard grass",new Vector3(0,.56f,20.1f),new Vector3(5.6f,.08f,1.6f),Green);
                Box(s,"Renewed water rill",new Vector3(0,.64f,19.5f),new Vector3(.32f,.08f,2.5f),Water);
                for(int i=-1;i<=1;i++) Tree(s,new Vector3(i*2.1f,.55f,20.5f),.55f);
                BuildLantern(s,new Vector3(-2.7f,.6f,19.7f));
                BuildLantern(s,new Vector3(2.7f,.6f,19.7f));
            }
        }

        static void BuildVegetation()
        {
            var p=Child(root,"Urban vegetation");
            Vector3[] positions={
                new Vector3(-21,.0f,17),new Vector3(-17,.0f,17),new Vector3(17,.0f,18),new Vector3(21,.0f,18),
                new Vector3(-21,.0f,1),new Vector3(21,.0f,1),new Vector3(-21,.0f,-15),new Vector3(21,.0f,-15),
                new Vector3(-5,.0f,22),new Vector3(5,.0f,22),new Vector3(-15,.0f,13),new Vector3(15,.0f,13)
            };
            for(int i=0;i<positions.Length;i++) Tree(p,positions[i],i%3==0?1.0f:.78f);
        }

        static void BuildPergola(Transform p,Vector3 at,float width)
        {
            for(int side=-1;side<=1;side+=2)
                for(int i=0;i<4;i++)
                    Box(p,"Garden pergola post",at+new Vector3(side*width*.5f,1.25f,i*1.25f-1.8f),new Vector3(.16f,2.5f,.16f),Wood);
            for(int i=0;i<4;i++)
                Box(p,"Garden pergola beam",at+new Vector3(0,2.5f,i*1.25f-1.8f),new Vector3(width,.16f,.16f),Wood);
        }

        static GameObject BuildLantern(Transform p,Vector3 at)
        {
            var g=Child(p,"Lantern").gameObject;
            Cylinder(g.transform,"Lantern base",new Vector3(0,.2f,0),new Vector3(.22f,.4f,.22f),DarkStone,12);
            Box(g.transform,"Lantern glass",new Vector3(0,.75f,0),new Vector3(.42f,.65f,.42f),new Color(.92f,.66f,.29f));
            Box(g.transform,"Lantern cap",new Vector3(0,1.12f,0),new Vector3(.55f,.12f,.55f),Wood);
            return g;
        }

        static void Tree(Transform p,Vector3 at,float scale)
        {
            Cylinder(p,"Tree trunk",at+new Vector3(0,.75f*scale,0),new Vector3(.22f*scale,1.5f*scale,.22f*scale),Wood,10);
            Sphere(p,"Tree canopy",at+new Vector3(0,1.9f*scale,0),new Vector3(1.5f*scale,1.65f*scale,1.5f*scale),GreenLight);
            Sphere(p,"Tree canopy highlight",at+new Vector3(.28f*scale,2.15f*scale,-.18f*scale),new Vector3(.8f*scale,.85f*scale,.8f*scale),Green);
        }

        static void Flower(Transform p,Vector3 at,Color color)
        {
            Cylinder(p,"Flower stem",at+new Vector3(0,.12f,0),new Vector3(.035f,.24f,.035f),Green,6);
            Sphere(p,"Flower head",at+new Vector3(0,.28f,0),new Vector3(.2f,.12f,.2f),color);
        }

        static void LightingAndCamera()
        {
            var camera=Camera.main;
            if(camera==null) camera=new GameObject("Main Camera",typeof(Camera)).GetComponent<Camera>();
            camera.tag="MainCamera";
            camera.orthographic=true;
            camera.orthographicSize=27;
            camera.transform.position=new Vector3(34,39,-42);
            camera.transform.rotation=Quaternion.Euler(39,-39,0);
            camera.backgroundColor=new Color(.55f,.69f,.75f);
            camera.clearFlags=CameraClearFlags.SolidColor;
            camera.nearClipPlane=.1f;
            camera.farClipPlane=180;
            var light=UnityEngine.Object.FindObjectOfType<Light>();
            if(light==null) light=new GameObject("Mehrgan sun",typeof(Light)).GetComponent<Light>();
            light.type=LightType.Directional;
            light.intensity=1.15f;
            light.color=new Color(1.0f,.91f,.77f);
            light.transform.rotation=Quaternion.Euler(48,-32,0);
            RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight=new Color(.62f,.59f,.52f);
        }

        static Transform Child(Transform parent,string name)
        {
            var g=new GameObject(name);
            g.transform.SetParent(parent,false);
            return g.transform;
        }

        static Material Mat(Color color)
        {
            int key=color.GetHashCode();
            if(Materials.TryGetValue(key,out var m)) return m;
            Shader shader=Shader.Find("Universal Render Pipeline/Lit");
            if(shader==null) shader=Shader.Find("Standard");
            m=new Material(shader);
            m.color=color;
            Materials[key]=m;
            return m;
        }

        static GameObject Primitive(Transform parent,string name,PrimitiveType type,Vector3 position,Vector3 scale,Color color,int sides=16)
        {
            var g=GameObject.CreatePrimitive(type);
            g.name=name;
            var collider=g.GetComponent<Collider>();
            if(collider!=null) UnityEngine.Object.Destroy(collider);
            g.transform.SetParent(parent,false);
            g.transform.localPosition=position;
            g.transform.localScale=scale;
            var renderer=g.GetComponent<Renderer>();
            renderer.sharedMaterial=Mat(color);
            return g;
        }

        static GameObject Box(Transform p,string n,Vector3 pos,Vector3 scale,Color c)=>Primitive(p,n,PrimitiveType.Cube,pos,scale,c);
        static GameObject Sphere(Transform p,string n,Vector3 pos,Vector3 scale,Color c)=>Primitive(p,n,PrimitiveType.Sphere,pos,scale,c);
        static GameObject Cylinder(Transform p,string n,Vector3 pos,Vector3 scale,Color c,int sides=16)=>Primitive(p,n,PrimitiveType.Cylinder,pos,scale,c,sides);
    }
}