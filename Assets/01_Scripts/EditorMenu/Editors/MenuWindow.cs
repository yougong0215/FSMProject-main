using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuWindow : EditorWindow
{
    private static List<Texture2D> texList = null;
    [MenuItem("Tools/Utility")]
    public static void ShowWindow() //�̸��� �� ����� ��� ��
    {
        MenuWindow win = GetWindow<MenuWindow>();

        win.minSize = new Vector2(400, 200);
        win.maxSize = new Vector2(500, 300);
    }

    //â�� ��������ڸ��� �ڵ����� ����Ǵ� �Լ�.
    private void OnEnable()
    {
        var xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/01_Scripts/EditorMenu/Editors/MenuWindow.uxml");

        TemplateContainer treeAsset = xml.CloneTree();
        rootVisualElement.Add(treeAsset);


        Image img = rootVisualElement.Q<Image>("targetImage");

        if (texList == null)
        {
            texList = new List<Texture2D>();
            string[] names = { "Mouse", "Ba", "si" };
            foreach (string name in names)
            {
                byte[] fileData = File.ReadAllBytes($"{Application.dataPath}/04_Sprites/{name}Harvester.png");
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
                texList.Add(tex);
                Debug.Log(name);
            }
        }

        //Q�� �׳� �̸��� ������ �ȴ�. #������ ����
        Button testBtn1 = rootVisualElement.Q<Button>("testBtn1");
        testBtn1.RegisterCallback<MouseUpEvent>(e => {
            img.image = texList[0];
        });

        Button testBtn2 = rootVisualElement.Q<Button>("testBtn2");
        testBtn2.RegisterCallback<MouseUpEvent>(e => {
            img.image = texList[1];
        });

        Button testBtn3 = rootVisualElement.Q<Button>("testBtn3");
        testBtn3.RegisterCallback<MouseUpEvent>(e => {
            img.image = texList[2];
        });

    }

}
