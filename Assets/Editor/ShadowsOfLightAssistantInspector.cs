using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class ShadowsOfLightAssistantInspector : EditorWindow
{
    private GameObject selectedObject;

    private Vector2 hierarchyScroll;
    private Vector2 inspectorScroll;

    private Vector3 previewPosition;
    private Vector3 previewRotation;
    private Vector3 previewScale = Vector3.one;

    private bool previewInitialized;
    private string plannedChanges = "";

    [MenuItem("Tools/Shadows of Light/Assistant Inspector")]
    public static void ShowWindow()
    {
        ShadowsOfLightAssistantInspector window =
            GetWindow<ShadowsOfLightAssistantInspector>("Assistant Inspector");

        window.minSize = new Vector2(700f, 400f);
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnUnitySelectionChanged;
        OnUnitySelectionChanged();
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnUnitySelectionChanged;
    }

    private void OnUnitySelectionChanged()
    {
        if (Selection.activeGameObject != null)
        {
            SetSelectedObject(Selection.activeGameObject);
        }

        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        DrawHierarchyPanel();
        DrawInspectorPanel();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawHierarchyPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.35f));

        EditorGUILayout.LabelField("SCENE HIERARCHY", EditorStyles.boldLabel);
        EditorGUILayout.Space(4);

        hierarchyScroll = EditorGUILayout.BeginScrollView(hierarchyScroll);

        GameObject[] rootObjects =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            DrawGameObjectRecursive(rootObject, 0);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawGameObjectRecursive(GameObject gameObject, int indent)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(indent * 14f);

        bool isSelected = selectedObject == gameObject;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;

        if (GUILayout.Button(
                (isSelected ? "● " : "") + gameObject.name,
                buttonStyle))
        {
            Selection.activeGameObject = gameObject;
            SetSelectedObject(gameObject);
        }

        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            DrawGameObjectRecursive(gameObject.transform.GetChild(i).gameObject, indent + 1);
        }
    }

    private void DrawInspectorPanel()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(
            "SHADOWS OF LIGHT - ASSISTANT INSPECTOR",
            EditorStyles.boldLabel);

        EditorGUILayout.Space(6);

        inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);

        if (selectedObject == null)
        {
            EditorGUILayout.HelpBox(
                "Selecciona un GameObject en la jerarquía de esta ventana o en la Hierarchy normal de Unity.",
                MessageType.Info);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            return;
        }

        DrawSelectedObjectInfo();
        EditorGUILayout.Space(10);

        DrawComponents();
        EditorGUILayout.Space(10);

        DrawTransformPreview();
        EditorGUILayout.Space(10);

        DrawPlannedChanges();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawSelectedObjectInfo()
    {
        EditorGUILayout.LabelField("SELECTED OBJECT", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("GameObject", selectedObject, typeof(GameObject), true);
        EditorGUI.EndDisabledGroup();
    }

    private void DrawComponents()
    {
        EditorGUILayout.LabelField("CURRENT COMPONENTS", EditorStyles.boldLabel);

        UnityEngine.Component[] components =selectedObject.GetComponents<UnityEngine.Component>();

        foreach (UnityEngine.Component component in components)
        {
            if (component == null)
            {
                EditorGUILayout.HelpBox(
                    "Missing Script detectado en este GameObject.",
                    MessageType.Warning);
                continue;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(component.GetType().Name, EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
        }
    }

    private void DrawTransformPreview()
    {
        EditorGUILayout.LabelField("TRANSFORM PREVIEW", EditorStyles.boldLabel);

        if (!previewInitialized)
        {
            LoadPreviewFromSelectedObject();
        }

        EditorGUILayout.HelpBox(
            "Estos valores son una simulación. El Transform real no cambia hasta pulsar 'Aplicar preview'.",
            MessageType.Info);

        previewPosition = EditorGUILayout.Vector3Field("Position", previewPosition);
        previewRotation = EditorGUILayout.Vector3Field("Rotation", previewRotation);
        previewScale = EditorGUILayout.Vector3Field("Scale", previewScale);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Restaurar preview"))
        {
            LoadPreviewFromSelectedObject();
        }

        if (GUILayout.Button("Aplicar preview"))
        {
            ApplyPreview();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawPlannedChanges()
    {
        EditorGUILayout.LabelField("PLANNED CHANGES / NOTES", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Usaremos esta zona para anotar qué queremos añadir, eliminar o modificar antes de tocar el proyecto.",
            MessageType.None);

        plannedChanges = EditorGUILayout.TextArea(
            plannedChanges,
            GUILayout.MinHeight(120f));
    }

    private void SetSelectedObject(GameObject gameObject)
    {
        if (selectedObject == gameObject)
        {
            return;
        }

        selectedObject = gameObject;
        previewInitialized = false;
    }

    private void LoadPreviewFromSelectedObject()
    {
        if (selectedObject == null)
        {
            return;
        }

        Transform targetTransform = selectedObject.transform;

        previewPosition = targetTransform.localPosition;
        previewRotation = targetTransform.localEulerAngles;
        previewScale = targetTransform.localScale;

        previewInitialized = true;
    }

    private void ApplyPreview()
    {
        if (selectedObject == null)
        {
            return;
        }

        Transform targetTransform = selectedObject.transform;

        Undo.RecordObject(targetTransform, "Apply Assistant Inspector Preview");

        targetTransform.localPosition = previewPosition;
        targetTransform.localEulerAngles = previewRotation;
        targetTransform.localScale = previewScale;

        EditorUtility.SetDirty(targetTransform);
    }
}
