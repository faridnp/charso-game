using System;
using System.Collections.Generic;
namespace Charso {
 [Serializable] public sealed class PuzzleLevel {
  public int Id;
  public string Clue;
  public string Answer;
  public string[] Letters;
  public string Topic;
  public int Difficulty = 1;
 }
 public static class WordPuzzle {
  // First 20 puzzles form the initial restoration tutorial arc.
  static readonly PuzzleLevel[] OpeningLevels = {
   new PuzzleLevel{Id=1,Clue="نام کشوری که شهر خیالی مهرگان در آن روایت می‌شود.",Answer="ایران",Letters=new[]{"ا","ی","ر","ا","ن"},Topic="میراث",Difficulty=1},
   new PuzzleLevel{Id=2,Clue="شاعری که آرامگاهش در شیراز قرار دارد.",Answer="حافظ",Letters=new[]{"ح","ا","ف","ظ"},Topic="میراث",Difficulty=1},
   new PuzzleLevel{Id=3,Clue="شهری که حافظیه در آن قرار دارد.",Answer="شیراز",Letters=new[]{"ش","ی","ر","ا","ز"},Topic="میراث",Difficulty=1},
   new PuzzleLevel{Id=4,Clue="فضای سرسبز با درخت و گل و مسیرهای پیاده‌روی.",Answer="باغ",Letters=new[]{"ب","ا","غ"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=5,Clue="پوشش رنگین و لعاب‌دار روی سطح بنا.",Answer="کاشی",Letters=new[]{"ک","ا","ش","ی"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=6,Clue="فضای طاق‌دار و شاخص در ورودی یا نمای بنا.",Answer="ایوان",Letters=new[]{"ا","ی","و","ا","ن"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=7,Clue="عضو عمودی که بار سقف یا طاق را نگه می‌دارد.",Answer="ستون",Letters=new[]{"س","ت","و","ن"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=8,Clue="پوشش خمیده و قوسی‌شکل بام در بسیاری از بناهای ایرانی.",Answer="گنبد",Letters=new[]{"گ","ن","ب","د"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=9,Clue="محل خریدوفروش و راسته‌های سرپوشیده شهری.",Answer="بازار",Letters=new[]{"ب","ا","ز","ا","ر"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=10,Clue="عنصر آب‌نما که در حیاط باغ یا خانه دیده می‌شود.",Answer="حوض",Letters=new[]{"ح","و","ض"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=11,Clue="درختی همیشه‌سبز و بلند که در باغ ایرانی دیده می‌شود.",Answer="سرو",Letters=new[]{"س","ر","و"},Topic="گیاه",Difficulty=1},
   new PuzzleLevel{Id=12,Clue="درخت مرکبات با میوه‌ای خوش‌بو؛ نماد باغ‌های شیراز.",Answer="نارنج",Letters=new[]{"ن","ا","ر","ن","ج"},Topic="گیاه",Difficulty=1},
   new PuzzleLevel{Id=13,Clue="مصالح سنتی دیوارچینی که از خاک پخته ساخته می‌شود.",Answer="آجر",Letters=new[]{"آ","ج","ر"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=14,Clue="نوشته یا نقش نوشتاری روی سنگ، کاشی یا بدنه بنا.",Answer="کتیبه",Letters=new[]{"ک","ت","ی","ب","ه"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=15,Clue="بازشوی دیوار برای ورود نور و هوا.",Answer="پنجره",Letters=new[]{"پ","ن","ج","ر","ه"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=16,Clue="برجک مرتفعی برای گرفتن و هدایت باد به داخل ساختمان.",Answer="بادگیر",Letters=new[]{"ب","ا","د","گ","ی","ر"},Topic="معماری",Difficulty=2},
   new PuzzleLevel{Id=17,Clue="ساختمان میان‌راهی برای استراحت کاروان‌ها.",Answer="کاروانسرا",Letters=new[]{"ک","ا","ر","و","ا","ن","س","ر","ا"},Topic="معماری",Difficulty=2},
   new PuzzleLevel{Id=18,Clue="فضای باز میانی خانه‌های سنتی که اتاق‌ها رو به آن دارند.",Answer="حیاط",Letters=new[]{"ح","ی","ا","ط"},Topic="معماری",Difficulty=1},
   new PuzzleLevel{Id=19,Clue="بخش پوشیده و گذرگاه‌مانند در بازار سنتی.",Answer="راسته",Letters=new[]{"ر","ا","س","ت","ه"},Topic="معماری",Difficulty=2},
   new PuzzleLevel{Id=20,Clue="فضای چهاربخشی باغ ایرانی با محورهای آب و مسیر.",Answer="چهارباغ",Letters=new[]{"چ","ه","ا","ر","ب","ا","غ"},Topic="معماری",Difficulty=2}
  };
  public static readonly PuzzleLevel[] Levels = OpeningLevels;
  // The first four restoration milestones map to the four authored tutorial states.
  // Later puzzle rewards accumulate for the city-project economy; they do not charge for a capped visual stage.
  public static bool RequiresRestoration(int levelIndex) => levelIndex >= 0 && levelIndex < 20 && (levelIndex + 1) % 5 == 0;
  public static string Normalize(string s) { if(string.IsNullOrEmpty(s)) return ""; return s.Replace('ي','ی').Replace('ى','ی').Replace('ك','ک').Replace("\u0640","").Replace("\u200c","").Replace("\u200f","").Replace("\u064b","").Replace("\u064c","").Replace("\u064d","").Replace("\u064e","").Replace("\u064f","").Replace("\u0650","").Trim(); }
  public static bool IsCorrect(PuzzleLevel l,string guess)=>l!=null&&Normalize(l.Answer)==Normalize(guess);
 }
}