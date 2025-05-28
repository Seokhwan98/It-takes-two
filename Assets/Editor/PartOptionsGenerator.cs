using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PartOptionsGenerator
{
    [MenuItem("Assets/Generate Part Options From Meshes", priority = 0)]
    public static void Generate()
    {
        // 선택된 폴더 경로 가져오기
        string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("폴더를 선택해주세요.");
            return;
        }

        // 파츠 종류를 사용자에게 입력 받음
        string partTypeName = Path.GetFileName(folderPath);
        if (!System.Enum.TryParse(partTypeName, out PartType parsedType))
        {
            Debug.LogError($"폴더명이 PartType enum과 일치하지 않음: {partTypeName}");
            return;
        }

        List<MeshPartOption> options = new();
        
        // Empty 옵션 먼저 추가
        options.Add(new MeshPartOption
        {
            id = "Empty",
            mesh = null
        });

        // 폴더 내 모든 Mesh 에셋 탐색
        string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { folderPath });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
            if (mesh == null) continue;

            var option = new MeshPartOption
            {
                id = Path.GetFileNameWithoutExtension(assetPath),
                mesh = mesh,
            };

            options.Add(option);
        }

        // ScriptableObject 생성
        PartOptionsGroup asset = ScriptableObject.CreateInstance<PartOptionsGroup>();
        asset.partType = parsedType;
        asset.options = options;

        string assetPathName = $"Assets/CustomizationData/{partTypeName}_Options.asset";
        AssetDatabase.CreateAsset(asset, assetPathName);
        AssetDatabase.SaveAssets();

        Debug.Log($"[CustomizationData] {partTypeName}_Options.asset 생성 완료. 옵션 수: {options.Count}");
        Selection.activeObject = asset;
    }
}