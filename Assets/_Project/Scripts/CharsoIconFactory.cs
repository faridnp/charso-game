using UnityEngine;
using System.Collections.Generic;

namespace Charso
{
    public enum CharsoIcon { Coin, Hammer, Home, Star, Arrow, Book }

    /// <summary>Small original UI glyphs generated at runtime; no third-party artwork or font glyph dependency.</summary>
    public static class CharsoIconFactory
    {
        static readonly Dictionary<CharsoIcon,Sprite> Cache=new Dictionary<CharsoIcon,Sprite>();
        static readonly Color Gold = new Color(.94f,.67f,.2f,1);
        static readonly Color Ink = new Color(.12f,.25f,.34f,1);
        static readonly Color Cream = new Color(.98f,.9f,.69f,1);
        public static Sprite Create(CharsoIcon kind)
        {
            if(Cache.TryGetValue(kind,out var cached))return cached;
            const int size=64;
            var tex=new Texture2D(size,size,TextureFormat.RGBA32,false);
            tex.name="Charso_"+kind;
            tex.filterMode=FilterMode.Bilinear;
            var pixels=new Color[size*size];
            for(int i=0;i<pixels.Length;i++)pixels[i]=Color.clear;
            for(int y=0;y<size;y++)for(int x=0;x<size;x++)
            {
                float px=(x+0.5f)/size*2-1,py=(y+0.5f)/size*2-1;
                bool fill=false;Color color=Ink;
                switch(kind)
                {
                    case CharsoIcon.Coin:
                        float r=Mathf.Sqrt(px*px+py*py);
                        fill=r<.73f;
                        color=r>.58f?new Color(.72f,.43f,.1f,1):Gold;
                        if(Mathf.Abs(px)<.08f&&Mathf.Abs(py)<.42f){fill=true;color=Cream;}
                        if(Mathf.Abs(py)<.07f&&Mathf.Abs(px)<.28f){fill=true;color=Cream;}
                        break;
                    case CharsoIcon.Hammer:
                        fill=DistanceToSegment(px,py,-.34f,-.38f,.24f,.25f)<.09f ||
                             (Mathf.Abs(px-.28f)<.1f&&Mathf.Abs(py-.35f)<.12f) ||
                             (Mathf.Abs(px-.05f)<.25f&&Mathf.Abs(py-.52f)<.09f);
                        color=fill?Ink:Color.clear;
                        break;
                    case CharsoIcon.Home:
                        fill=(py<.18f&&py>-.62f&&Mathf.Abs(px)<.52f) ||
                             (py>=.05f&&py<.68f&&Mathf.Abs(px)<(.68f-py)*.95f);
                        color=py>.22f?TileColor():new Color(.91f,.78f,.58f,1);
                        break;
                    case CharsoIcon.Star:
                        float a=Mathf.Atan2(py,px),rr=Mathf.Sqrt(px*px+py*py);
                        float edge=.62f+.17f*Mathf.Cos(5*a);
                        fill=rr<edge;
                        color=Gold;
                        break;
                    case CharsoIcon.Arrow:
                        fill=(px<.12f&&px>-.62f&&Mathf.Abs(py)<.12f) ||
                             (px>=.0f&&px<.65f&&Mathf.Abs(py)<(.65f-px)*.95f);
                        color=Ink;
                        break;
                    case CharsoIcon.Book:
                        fill=(Mathf.Abs(px)<.65f&&Mathf.Abs(py)<.58f&&Mathf.Abs(px)>.06f) ||
                             (Mathf.Abs(px)<.08f&&Mathf.Abs(py)<.62f);
                        color=new Color(.17f,.39f,.53f,1);
                        break;
                }
                if(fill)pixels[y*size+x]=color;
            }
            tex.SetPixels(pixels);tex.Apply(false,true);
            var sprite=Sprite.Create(tex,new Rect(0,0,size,size),new Vector2(.5f,.5f),100);
            Cache[kind]=sprite;
            return sprite;
        }

        static float DistanceToSegment(float px,float py,float ax,float ay,float bx,float by)
        {
            float dx=bx-ax,dy=by-ay;
            float t=Mathf.Clamp01(((px-ax)*dx+(py-ay)*dy)/(dx*dx+dy*dy));
            float x=ax+t*dx,y=ay+t*dy;
            return Mathf.Sqrt((px-x)*(px-x)+(py-y)*(py-y));
        }
        static Color TileColor()=>new Color(.13f,.34f,.48f,1);
    }
}