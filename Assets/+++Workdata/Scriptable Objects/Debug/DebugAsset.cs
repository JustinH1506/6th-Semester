using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Debug", menuName = "Scriptable Objects/Debug")]
public class DebugAsset : ScriptableObject
{ 
	public SceneAsset startScene;
	public bool useEditorCode;
	public bool loadGame;
}
