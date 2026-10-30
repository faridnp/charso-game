using System.Collections.Generic;
using UnityEngine;

namespace Charso
{
    /// <summary>
    /// Source-led educational blockout of Hafezieh, kept separate from fictional Mehrgan.
    /// Geometry is proportional placeholder work; it is not a measured reconstruction.
    /// </summary>
    public static class HafeziehChapterBuilder
    {
        static readonly Color Limestone=new Color(.78f,.72f,.61f);
        static readonly Color PaleStone=new Color(.88f,.81f,.68f);
        static readonly Color Copper=new Color(.53f,.31f,.18f);
        static readonly Color BlueTile=new Color(.14f,.36f,.49f);
        static readonly Color Water=new Color(.15f,.4f,.48f);
        static readonly Color Garden=new Color(.22f,.38f,.25f);
        static readonly Color Orange=new Color(.78f,.38f,.12f);
        static readonly Dictionary<Color,Material> Materials=new Dictionary<Color,Material>();

        public static void Build(int reconstructionStage=0)
        {
            var existing=GameObject.Find("HafeziehChapter");
            if(existing!=null){ApplyStage(reconstructionStage);return;}
            var root=new GameObject("HafeziehChapter").transform;
            root.localScale=Vector3.one*.35f;
            Box(root,"Site ground",new Vector3(0,-.35f,0),new Vector3(72,.7f,42),new Color(.66f,.58f,.45f));
            BuildCourtyard(root,"South courtyard",new Vector3(0,0,-12));
            BuildCourtyard(root,"North courtyard",new Vector3(0,0,12));
            BuildTwentyColumnHall(root);
            BuildTombPavilion(root);
            BuildOrangeGarden(root);
            BuildPerimeter(root);
            BuildStageDetails(root);
            ApplyStage(reconstructionStage);
            SetupCamera();
        }

        public static void ApplyStage(int stage)
        {
            stage=Mathf.Clamp(stage,0,3);
            var root=GameObject.Find("HafeziehChapter");
            if(root==null)return;
            var pavilion=root.transform.Find("Eight column tomb pavilion");
            if(pavilion!=null)pavilion.gameObject.SetActive(stage>=1);
            for(int i=0;i<4;i++)
            {
                var t=root.transform.Find("ReconstructionStage_"+i);
                if(t!=null)t.gameObject.SetActive(i==stage);
            }
        }

        static void BuildTwentyColumnHall(Transform root)
        {
            var hall=Child(root,"Twenty column hall — 56m reference");
            Box(hall,"Hall floor",new Vector3(0,.12f,0),new Vector3(56f,.24f,8f),PaleStone);
            Box(hall,"Hall roof",new Vector3(0,5.2f,0),new Vector3(57f,.32f,9.4f),Limestone);
            Box(hall,"Hall blue frieze",new Vector3(0,4.9f,-4.65f),new Vector3(56.5f,.22f,.12f),BlueTile);
            for(int i=0;i<10;i++)
            {
                float x=-25.2f+i*(50.4f/9f);
                for(int side=-1;side<=1;side+=2)
                {
                    Vector3 at=new Vector3(x,2.5f,side*3.1f);
                    Cylinder(hall,"Hall stone column",at,new Vector3(.25f,2.5f,.25f),PaleStone,20);
                    Cylinder(hall,"Hall column base",at+new Vector3(0,-2.38f,0),new Vector3(.42f,.16f,.42f),Limestone,20);
                    Cylinder(hall,"Hall column capital",at+new Vector3(0,2.38f,0),new Vector3(.42f,.2f,.42f),Limestone,20);
                }
            }
            // Source describes four retained Zand-period columns and sixteen added columns.
            // Their exact individual positions/material differences are intentionally not guessed here.
        }

        static void BuildCourtyard(Transform root,string name,Vector3 at)
        {
            var court=Child(root,name);
            Box(court,"Courtyard paving",at,new Vector3(22,.16f,10),PaleStone);
            Box(court,"Central garden strip",at+new Vector3(0,.12f,0),new Vector3(18,.08f,1.6f),Garden);
            for(int side=-1;side<=1;side+=2)
            {
                Box(court,"Rectangular reflecting pool",at+new Vector3(side*6,.2f,0),new Vector3(5.4f,.12f,1.45f),Water);
                Box(court,"Pool stone rim",at+new Vector3(side*6,.26f,0),new Vector3(5.7f,.08f,1.72f),Limestone);
            }
            for(int i=0;i<7;i++)
            {
                float x=-8.0f+i*2.65f;
                Box(court,"Garden path slab",at+new Vector3(x,.18f,2.8f),new Vector3(1.25f,.09f,1.25f),Limestone);
            }
        }

        static void BuildTombPavilion(Transform root)
        {
            var p=Child(root,"Eight column tomb pavilion");
            Vector3 center=new Vector3(0,.0f,12.0f);
            Cylinder(p,"Tomb stepped plinth",center+new Vector3(0,.24f,0),new Vector3(5.8f,.48f,5.8f),Limestone,40);
            Cylinder(p,"Tomb upper step",center+new Vector3(0,.52f,0),new Vector3(4.9f,.16f,4.9f),PaleStone,40);
            for(int i=0;i<8;i++)
            {
                float a=i*Mathf.PI/4f;
                Vector3 pos=center+new Vector3(Mathf.Cos(a)*2.05f,5.0f,Mathf.Sin(a)*2.05f);
                Cylinder(p,"Tomb pavilion column",pos,new Vector3(.3f,5.0f,.3f),PaleStone,20);
                Cylinder(p,"Tomb column capital",pos+new Vector3(0,4.85f,0),new Vector3(.5f,.18f,.5f),Limestone,20);
            }
            Box(p,"Pavilion entablature",center+new Vector3(0,10.2f,0),new Vector3(5.0f,.35f,5.0f),Limestone);
            var dome=CreateDome(p,"Copper ribbed dome",center+new Vector3(0,10.4f,0),2.25f,3.3f,Copper);
            var tile=Child(p,"Interior tile and inscription accents");
            for(int i=0;i<8;i++)
            {
                float a=i*Mathf.PI/4f;
                Box(tile,"Pavilion blue tile panel",center+new Vector3(Mathf.Cos(a)*2.34f,6.8f,Mathf.Sin(a)*2.34f),new Vector3(.65f,1.2f,.08f),BlueTile);
            }
            Cylinder(p,"Memorial platform",center+new Vector3(0,.68f,0),new Vector3(1.55f,.25f,1.55f),PaleStone,32);
            Box(p,"Memorial stone",center+new Vector3(0,.92f,0),new Vector3(1.25f,.22f,1.25f),new Color(.76f,.76f,.72f));
        }

        static void BuildOrangeGarden(Transform root)
        {
            var garden=Child(root,"Orange and cypress garden");
            for(int i=0;i<16;i++)
            {
                float x=-14+(i%8)*4f;
                float z=(i<8?-17:17);
                Tree(garden,new Vector3(x,0,z),i%3==0);
            }
            for(int i=0;i<8;i++)
            {
                float x=-12+i*3.4f;
                Tree(garden,new Vector3(x,0,7),true);
            }
        }

        static void BuildPerimeter(Transform root)
        {
            var p=Child(root,"Hafezieh garden edge");
            Box(p,"North garden wall",new Vector3(0,1.1f,21),new Vector3(70,2.2f,.35f),Limestone);
            Box(p,"South garden wall",new Vector3(0,1.1f,-21),new Vector3(70,2.2f,.35f),Limestone);
            Box(p,"West garden wall",new Vector3(-35,1.1f,0),new Vector3(.35f,2.2f,42),Limestone);
            Box(p,"East garden wall",new Vector3(35,1.1f,0),new Vector3(.35f,2.2f,42),Limestone);
        }

        static void BuildStageDetails(Transform root)
        {
            var pavilion=root.Find("Eight column tomb pavilion");\n            if(pavilion!=null)pavilion.gameObject.SetActive(false);\n            var stage0=Child(root,"ReconstructionStage_0");
            Box(stage0,"Survey base plan",new Vector3(0,.04f,12),new Vector3(8,.08f,7),new Color(.56f,.5f,.4f));
            var stage1=Child(root,"ReconstructionStage_1");
            Box(stage1,"Tomb pavilion temporary roof frame",new Vector3(0,10.8f,12),new Vector3(4.8f,.16f,4.8f),Wood);
            var stage2=Child(root,"ReconstructionStage_2");
            var dome=GameObject.Find("Copper ribbed dome");
            if(dome!=null)dome.transform.SetParent(stage2,false);
            var tile=GameObject.Find("Interior tile and inscription accents");
            if(tile!=null)tile.transform.SetParent(stage2,false);
            var stage3=Child(root,"ReconstructionStage_3");
            Box(stage3,"Garden renewal path",new Vector3(0,.22f,18),new Vector3(12,.1f,1.2f),Limestone);
            for(int i=0;i<4;i++)Tree(stage3,new Vector3(-4+i*2.7f,.2f,18.8f),true);
            stage0.gameObject.SetActive(false);
            stage1.gameObject.SetActive(false);
            stage2.gameObject.SetActive(false);
            stage3.gameObject.SetActive(false);
        }

        static GameObject CreateDome(Transform parent,string name,Vector3 position,float radius,float height,Color color)
        {
            int segments=32,rings=10;
            var vertices=new List<Vector3>();
            var triangles=new List<int>();
            for(int y=0;y<=rings;y++)
            {
                float v=(float)y/rings;
                float theta=v*Mathf.PI*.5f;
                float ringRadius=Mathf.Cos(theta)*radius;
                float h=Mathf.Sin(theta)*height;
                for(int x=0;x<=segments;x++)
                {
                    float a=(float)x/segments*Mathf.PI*2f;
                    vertices.Add(new Vector3(Mathf.Cos(a)*ringRadius,h,Mathf.Sin(a)*ringRadius));
                }
            }
            for(int y=0;y<rings;y++)for(int x=0;x<segments;x++)
            {
                int a=y*(segments+1)+x,b=a+segments+1;
                triangles.AddRange(new[]{a,b,a+1,a+1,b,b+1});
            }
            var mesh=new Mesh();mesh.name=name+" mesh";mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();
            var g=new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer));
            g.transform.SetParent(parent,false);g.transform.localPosition=position;
            g.GetComponent<MeshFilter>().sharedMesh=mesh;g.GetComponent<MeshRenderer>().sharedMaterial=Mat(color);
            for(int i=0;i<16;i++)
            {
                float a=i*Mathf.PI*2f/16f;
                var ribObject=new GameObject("Dome copper rib");
                ribObject.transform.SetParent(g.transform,false);
                var rib=ribObject.AddComponent<LineRenderer>();
                rib.useWorldSpace=false;rib.positionCount=9;rib.startWidth=.055f;rib.endWidth=.025f;
                rib.numCapVertices=2;rib.alignment=LineAlignment.View;rib.sharedMaterial=Mat(new Color(.68f,.46f,.26f));
                for(int j=0;j<=8;j++)
                {
                    float theta=(float)j/8f*Mathf.PI*.5f;
                    float rr=Mathf.Cos(theta)*radius*1.012f;
                    float yy=Mathf.Sin(theta)*height*1.012f;
                    rib.SetPosition(j,new Vector3(Mathf.Cos(a)*rr,yy,Mathf.Sin(a)*rr));
                }
            }
            return g;
        }

        static void Tree(Transform p,Vector3 at,bool cypress)
        {
            Cylinder(p,"Tree trunk",at+new Vector3(0,.55f,0),new Vector3(.2f,1.1f,.2f),new Color(.36f,.25f,.16f),10);
            if(cypress)
            {
                for(int i=0;i<4;i++)
                    Sphere(p,"Cypress foliage tier",at+new Vector3(0,1.15f+i*.48f,0),new Vector3(1.25f-i*.22f,1.0f-i*.12f,1.25f-i*.22f),Garden);
            }
            else
            {
                Sphere(p,"Orange tree canopy",at+new Vector3(0,1.75f,0),new Vector3(1.8f,1.6f,1.8f),new Color(.27f,.4f,.24f));
                for(int i=0;i<5;i++)
                {
                    float a=i*Mathf.PI*2f/5f;
                    Sphere(p,"Orange fruit",at+new Vector3(Mathf.Cos(a)*.55f,1.7f,Mathf.Sin(a)*.55f),new Vector3(.22f,.22f,.22f),Orange);
                }
            }
        }

        static Transform Child(Transform p,string name)
        {
            var g=new GameObject(name);g.transform.SetParent(p,false);return g.transform;
        }
        static Material Mat(Color color)
        {
            if(Materials.TryGetValue(color,out var m))return m;
            Shader shader=Shader.Find("Universal Render Pipeline/Lit");if(shader==null)shader=Shader.Find("Standard");
            m=new Material(shader);m.color=color;Materials[color]=m;return m;
        }
        static GameObject Primitive(Transform p,string name,PrimitiveType type,Vector3 pos,Vector3 scale,Color color)
        {
            var g=GameObject.CreatePrimitive(type);g.name=name;
            var col=g.GetComponent<Collider>();if(col!=null)Object.Destroy(col);
            g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=scale;
            g.GetComponent<Renderer>().sharedMaterial=Mat(color);return g;
        }
        static GameObject Box(Transform p,string n,Vector3 pos,Vector3 scale,Color c)=>Primitive(p,n,PrimitiveType.Cube,pos,scale,c);
        static GameObject Sphere(Transform p,string n,Vector3 pos,Vector3 scale,Color c)=>Primitive(p,n,PrimitiveType.Sphere,pos,scale,c);
        static GameObject Cylinder(Transform p,string n,Vector3 pos,Vector3 scale,Color c,int sides=16)=>Primitive(p,n,PrimitiveType.Cylinder,pos,scale,c);
        static void SetupCamera()
        {
            var cam=Camera.main;if(cam==null)cam=new GameObject("Main Camera",typeof(Camera)).GetComponent<Camera>();
            cam.tag="MainCamera";cam.orthographic=true;cam.orthographicSize=17f;
            cam.transform.position=new Vector3(20,25,-22);cam.transform.rotation=Quaternion.Euler(42,-40,0);
            cam.backgroundColor=new Color(.62f,.74f,.78f);cam.clearFlags=CameraClearFlags.SolidColor;
            var explorer=cam.GetComponent<MehrganCameraController>();if(explorer!=null)explorer.enabled=false;
        }
    }
}