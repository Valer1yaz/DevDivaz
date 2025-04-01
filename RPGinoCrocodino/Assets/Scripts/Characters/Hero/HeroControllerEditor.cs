#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeroController))]
public class HeroControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        HeroController controller = (HeroController)target;
        
        // Вместо serializedObject используем прямое обращение
        Handles.color = Color.red;
        Handles.DrawWireArc(
            controller.attackPoint.position,
            Vector3.up,
            Vector3.forward,
            360,
            controller.physicalAttackRange
        );
    }
}
#endif