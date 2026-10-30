using System;
using System.IO;
using UnityEngine;
namespace Charso {
 [Serializable] public sealed class PlayerProgress {
  public int SchemaVersion=1; public int LevelIndex; public int Coins=20; public int RestorationStage; public int SolvedLevels;
  public bool CurrentLevelSolved; public bool CurrentLevelRestored;
 }
 public static class SaveSystem {
  static string FilePath=>Path.Combine(Application.persistentDataPath,"charso-save.json");
  public static PlayerProgress Load(){try{if(File.Exists(FilePath)){var p=JsonUtility.FromJson<PlayerProgress>(File.ReadAllText(FilePath));if(p!=null&&p.SchemaVersion==1)return Clean(p);}}catch(Exception e){Debug.LogWarning("Save read failed: "+e.Message);}return new PlayerProgress();}
  public static void Store(PlayerProgress p){if(p==null)return;Clean(p);Directory.CreateDirectory(Application.persistentDataPath);string tmp=FilePath+".tmp",bak=FilePath+".bak";File.WriteAllText(tmp,JsonUtility.ToJson(p,true));try{if(File.Exists(FilePath))File.Copy(FilePath,bak,true);if(File.Exists(FilePath))File.Delete(FilePath);File.Move(tmp,FilePath);}catch(Exception e){Debug.LogError("Save write failed: "+e.Message);}}
  static PlayerProgress Clean(PlayerProgress p){p.LevelIndex=Mathf.Clamp(p.LevelIndex,0,WordPuzzle.Levels.Length-1);p.Coins=Mathf.Clamp(p.Coins,0,999999);p.RestorationStage=Mathf.Clamp(p.RestorationStage,0,3);p.SolvedLevels=Mathf.Max(0,p.SolvedLevels);p.SchemaVersion=1;return p;}
 }
}