using UnityEditor;
using UnityEngine;

public static class SetIconForScriptableObject
{
    [MenuItem("Assets/Set Custom Icon")]
    static void SetCustomIcon()
    {
        var selectedObject = Selection.activeObject;

        // カスタムアイコンのTexture2Dをロードする
        var iconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets\\Utage\\Sample\\Textures\\emoji\\1f359.png");

        // EditorGUIUtility.SetIconForObjectを使用してアイコンを設定する
        EditorGUIUtility.SetIconForObject(selectedObject, iconTexture);

        // 変更を保存
        EditorUtility.SetDirty(selectedObject);
    }
}
