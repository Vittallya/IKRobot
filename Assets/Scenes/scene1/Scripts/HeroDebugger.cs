using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeroManager))]
public class HeroDebugger : Editor
{
    private void OnSceneGUI()
    {
        var hero = (HeroManager)target;

        var pos = hero.transform.position;
        Handles.DrawWireDisc(pos, Vector3.up, hero.FOV);
    }
}
