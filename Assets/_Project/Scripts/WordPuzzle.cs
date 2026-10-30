using System;
namespace Charso {
 [Serializable] public sealed class PuzzleLevel { public int Id; public string Clue; public string Answer; public string[] Letters; }
 public static class WordPuzzle {
  public static readonly PuzzleLevel[] Levels = {
   new PuzzleLevel{Id=1,Clue="با آن داستان و دانش می‌خوانیم.",Answer="کتاب",Letters=new[]{"ک","ت","ا","ب"}},
   new PuzzleLevel{Id=2,Clue="از آسمان می‌بارد و زمین را سیراب می‌کند.",Answer="باران",Letters=new[]{"ب","ا","ر","ا","ن"}},
   new PuzzleLevel{Id=3,Clue="جایی که درختان و گل‌ها در آن می‌رویند.",Answer="باغ",Letters=new[]{"ب","ا","غ"}},
   new PuzzleLevel{Id=4,Clue="بنای بلند برای دیدبانی یا نشانه‌گذاری.",Answer="برج",Letters=new[]{"ب","ر","ج"}},
   new PuzzleLevel{Id=5,Clue="رنگ آسمان در یک روز صاف.",Answer="آبی",Letters=new[]{"آ","ب","ی"}}
  };
  public static string Normalize(string s) { if(string.IsNullOrEmpty(s)) return ""; return s.Replace('ي','ی').Replace('ى','ی').Replace('ك','ک').Replace("\u0640","").Replace("\u200c","").Replace("\u200f","").Replace("\u064b","").Replace("\u064c","").Replace("\u064d","").Replace("\u064e","").Replace("\u064f","").Replace("\u0650","").Trim(); }
  public static bool IsCorrect(PuzzleLevel l,string guess)=>l!=null&&Normalize(l.Answer)==Normalize(guess);
 }
}