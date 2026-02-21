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
            var maxEnemiesPropField = root.Q<PropertyField>("maxEnemiesPropField");
            var spawnIntervalPropField = root.Q<PropertyField>("spawnIntervalPropField");

            //progress bar element, and element that renders the bar of the progress bar
            var progressBar = root.Q<ProgressBar>("spawnerTotalLoad");
            var progressFill = progressBar.Q<VisualElement>(className: "unity-progress-bar__progress");

            //get values from prop fields
            var minSpawnDistProp = serializedObject.FindProperty("minSpawnDistance");
            var maxSpawnDistProp = serializedObject.FindProperty("maxSpawnDistance");
            var maxEnemiesProp = serializedObject.FindProperty("maxEnemies");
            var spawnIntervalProp = serializedObject.FindProperty("spawnInterval");

            UpdateProgressBar(spawner.CurrentEnemyCount);
            spawner.OnEnemyCountChanged += UpdateProgressBar;

            //handling field input
            SetupMinSpawnDistanceField(minSpawnDistPropField, minSpawnDistProp, maxSpawnDistProp);
            SetupMaxSpawnDistanceField(maxSpawnDistPropField, maxSpawnDistProp, minSpawnDistProp);
            SetupMaxEnemiesField(maxEnemiesPropField, maxEnemiesProp);
            SetupSpawnIntervalField(spawnIntervalPropField, spawnIntervalProp);

            //progress bar limits
            progressBar.lowValue = 0;
            progressBar.highValue = maxEnemiesProp.intValue;

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

            void SetupMaxEnemiesField(PropertyField field, SerializedProperty property)
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
                        ApplyMaxEnemiesClamp(property);
                        UpdateProgressBarHighValue();
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }

                    void OnKeyDown(KeyDownEvent e)
                    {
                        ApplyMaxEnemiesClamp(property);
                        UpdateProgressBarHighValue();
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                });
            }

            void SetupSpawnIntervalField(PropertyField field, SerializedProperty property)
            {
                field.RegisterCallback<GeometryChangedEvent>(_ =>
                {
                    var floatField = field.Q<FloatField>();

                    floatField.UnregisterCallback<BlurEvent>(OnBlur);
                    floatField.UnregisterCallback<KeyDownEvent>(OnKeyDown);

                    floatField.RegisterCallback<BlurEvent>(OnBlur);
                    floatField.RegisterCallback<KeyDownEvent>(OnKeyDown);

                    return;

                    void OnBlur(BlurEvent e)
                    {
                        ApplySpawnIntervalClamp(property);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }

                    void OnKeyDown(KeyDownEvent e)
                    {
                        ApplySpawnIntervalClamp(property);
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                });
            }

            //updating max value in progress bar
            void UpdateProgressBarHighValue()
            {
                serializedObject.Update();
                progressBar.highValue = maxEnemiesProp.intValue;

                UpdateProgressBar(spawner.CurrentEnemyCount);
            }

            //updates values of progress bar and color accordingly
            void UpdateProgressBar(int count)
            {
                float max = spawner.MaxEnemies;
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

        //setting up smallest value for max enemies
        private void ApplyMaxEnemiesClamp(SerializedProperty property)
        {
            serializedObject.Update();

            property.intValue = Mathf.Max(1, property.intValue);

            serializedObject.ApplyModifiedProperties();
        }

        //setting up smallest value for spawn interval
        private void ApplySpawnIntervalClamp(SerializedProperty property)
        {
            serializedObject.Update();

            property.floatValue = Mathf.Max(0.001f, property.floatValue);

            serializedObject.ApplyModifiedProperties();
        }
    }
}