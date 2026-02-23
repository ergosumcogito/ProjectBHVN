using Core;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : UnityEditor.Editor
    {
        [SerializeField] public VisualTreeAsset visualTree;

        public override VisualElement CreateInspectorGUI()
        {
            var spawner = (EnemySpawner)target;

            var root = new VisualElement();
            visualTree.CloneTree(root);

            //initializing elements
            var minSpawnDistPropField = root.Q<PropertyField>("minSpawnDistPropField");
            var maxSpawnDistPropField = root.Q<PropertyField>("maxSpawnDistPropField");

            //progress bar element, and element that renders the bar of the progress bar
            var progressBar = root.Q<ProgressBar>("spawnerTotalLoad");
            var progressFill = progressBar.Q<VisualElement>(className: "unity-progress-bar__progress");

            //get values from prop fields
            var minSpawnDistProp = serializedObject.FindProperty("minSpawnDistance");
            var maxSpawnDistProp = serializedObject.FindProperty("maxSpawnDistance");

            UpdateProgressBar(spawner.CurrentEnemyCount);
            spawner.OnEnemyCountChanged += UpdateProgressBar;

            root.RegisterCallback<DetachFromPanelEvent>(_ => { spawner.OnEnemyCountChanged -= UpdateProgressBar; });

            //handling field input
            SetupMinSpawnDistanceField(minSpawnDistPropField, minSpawnDistProp, maxSpawnDistProp);
            SetupMaxSpawnDistanceField(maxSpawnDistPropField, maxSpawnDistProp, minSpawnDistProp);

            //progress bar low limit
            progressBar.lowValue = 0;

            return root;

            void SetupMinSpawnDistanceField(PropertyField field, SerializedProperty property,
                SerializedProperty property2)
            {
                field.RegisterCallback<GeometryChangedEvent>(_ =>
                {
                    var intField = field.Q<IntegerField>();

                    intField.UnregisterCallback<BlurEvent>(OnBlur);
                    intField.UnregisterCallback<KeyDownEvent>(OnKeyDown);

                    intField.RegisterCallback<BlurEvent>(OnBlur);
                    intField.RegisterCallback<KeyDownEvent>(OnKeyDown);

                    return;

                    void OnBlur(BlurEvent e)
                    {
                        ClampMinSpawnDist(property, property2);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }

                    void OnKeyDown(KeyDownEvent e)
                    {
                        if (e.keyCode is not (KeyCode.Return or KeyCode.KeypadEnter)) return;
                        ClampMinSpawnDist(property, property2);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                });
            }

            void SetupMaxSpawnDistanceField(PropertyField field, SerializedProperty property,
                SerializedProperty property2)
            {
                field.RegisterCallback<GeometryChangedEvent>(_ =>
                {
                    var intField = field.Q<IntegerField>();

                    intField.UnregisterCallback<BlurEvent>(OnBlur);
                    intField.UnregisterCallback<KeyDownEvent>(OnKeyDown);

                    intField.RegisterCallback<BlurEvent>(OnBlur);
                    intField.RegisterCallback<KeyDownEvent>(OnKeyDown);

                    return;

                    void OnBlur(BlurEvent e)
                    {
                        ClampMaxSpawnDist(property, property2);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }

                    void OnKeyDown(KeyDownEvent e)
                    {
                        if (e.keyCode is not (KeyCode.Return or KeyCode.KeypadEnter)) return;
                        ClampMaxSpawnDist(property, property2);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                });
            }

            //updates values of progress bar and color accordingly
            void UpdateProgressBar(int count)
            {
                var max = Mathf.Max(1f, spawner.MaxEnemies);

                progressBar.highValue = max;

                var modifier = count / max;

                progressBar.value = count;
                progressBar.title = $"{count} / {max}";

                var speed = Mathf.Pow(modifier, 2f);
                var barColor = Color.Lerp(Color.green, Color.red, speed);

                progressFill.style.backgroundColor = barColor;
            }
        }

        private void ClampMinSpawnDist(SerializedProperty min, SerializedProperty max)
        {
            serializedObject.Update();

            if (min.intValue < 0) min.intValue = 0;
            if (min.intValue >= max.intValue) min.intValue = max.intValue - 1;

            serializedObject.ApplyModifiedProperties();
        }

        private void ClampMaxSpawnDist(SerializedProperty max, SerializedProperty min)
        {
            serializedObject.Update();

            if (max.intValue < 0) max.intValue = 0;
            if (max.intValue <= min.intValue) max.intValue = min.intValue + 1;

            serializedObject.ApplyModifiedProperties();
        }
    }
}