using System;
using System.IO;
using Charso;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Charso.Editor {
 public static class BuildCommand {
  const string ScenePath="Assets/_Project/Scenes/Charso.unity",OutputPath="build/Android/Charso.apk";
  [MenuItem("Charso/Generate First Playable Scene")]
  public static void GeneratePlayableScene(){Directory.CreateDirectory("Assets/_Project/Scenes");var s=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("CharsoGame").AddComponent<CharsoGameBootstrap>();EditorSceneManager.SaveScene(s,ScenePath);AssetDatabase.Refresh();Debug.Log("Charso scene generated: "+ScenePath);}
  public static void BuildAndroidApk(){if(EditorApplication.isCompiling)throw new Exception("Scripts are still compiling.");GeneratePlayableScene();EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android,BuildTarget.Android);PlayerSettings.companyName="FaridNP Studio";PlayerSettings.productName="Charso";PlayerSettings.applicationIdentifier="ir.faridnp.charso";PlayerSettings.bundleVersion="0.1.0";PlayerSettings.Android.bundleVersionCode=1;PlayerSettings.defaultInterfaceOrientation=UIOrientation.Portrait;PlayerSettings.Android.minSdkVersion=AndroidSdkVersions.AndroidApiLevel25;PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARM64;PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Android,ScriptingImplementation.IL2CPP);Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));var o=new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=OutputPath,target=BuildTarget.Android,options=BuildOptions.None};BuildReport r=BuildPipeline.BuildPlayer(o);if(r.summary.result!=BuildResult.Succeeded)throw new Exception("Android build failed: "+r.summary.result+" errors="+r.summary.totalErrors);Debug.Log("CHARSO_APK_BUILD_SUCCESS "+Path.GetFullPath(OutputPath));}
 }
}