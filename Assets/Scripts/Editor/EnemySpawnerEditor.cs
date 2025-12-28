using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    [SerializeField] public VisualTreeAsset visualTree;

    public override VisualElement CreateInspectorGUI()
    {
        var spawner = (EnemySpawner)target;

        var root = new VisualElement();
        visualTree.CloneTree(root);

        //initializing elements
        var enemyPrefabsPropField = root.Q<PropertyField>("enemyPrefabsPropField");
        var minPropField = root.Q<IntegerField>("minPropField");
        var maxPropField = root.Q<IntegerField>("maxPropField");
        var minMaxSlider = root.Q<MinMaxSlider>("minMaxSlider");
        var maxEnemiesPropField = root.Q<PropertyField>("maxEnemiesPropField");
        var spawnIntervalPropField = root.Q<PropertyField>("spawnIntervalPropField");

        //progress bar element, and element that renders the bar of the progress bar
        var progressBar = root.Q<ProgressBar>("spawnerTotalLoad");
        var progressFill = progressBar.Q<VisualElement>(className: "unity-progress-bar__progress");

        //removing premade labels
        minPropField.label = "";
        maxPropField.label = "";

        //get values from prop fields
        var enemyPrefabsProp = serializedObject.FindProperty("enemyPrefabs");
        var minProp = serializedObject.FindProperty("minSpawnDistance");
        var maxProp = serializedObject.FindProperty("maxSpawnDistance");
        var maxEnemiesProp = serializedObject.FindProperty("maxEnemies");
        var spawnIntervalProp = serializedObject.FindProperty("spawnInterval");

        UpdateProgressBar(spawner.CurrentEnemyCount);
        spawner.OnEnemyCountChanged += UpdateProgressBar;

        //bind to prop fields
        minPropField.BindProperty(minProp);
        maxPropField.BindProperty(maxProp);
        enemyPrefabsPropField.BindProperty(enemyPrefabsProp);

        //fallback in case Level fails to load correctly
        var levelMax = 10;

        var levelEditor = FindFirstObjectByType<LevelEditor>();
        if (levelEditor != null)
        {
            //getting max spawn distance, relative to level size
            //levelMax = Mathf.Min(levelEditor.Width, levelEditor.Length);
            levelMax = Mathf.Min(10,10);
        }

        //setting up limits for the min max slider
        minMaxSlider.lowLimit = 1;
        minMaxSlider.highLimit = levelMax;

        //setting min and max value for slider
        minMaxSlider.value = new Vector2(minProp.intValue, maxProp.intValue);

        //sets props to value of slider
        minMaxSlider.RegisterValueChangedCallback(evt =>
        {
            serializedObject.Update();

            var min = Mathf.RoundToInt(evt.newValue.x);
            var max = Mathf.RoundToInt(evt.newValue.y);

            ClampMinMax(ref min, ref max, levelMax);

            minProp.intValue = min;
            maxProp.intValue = max;

            minMaxSlider.SetValueWithoutNotify(new Vector2(min, max));
            minPropField.SetValueWithoutNotify(min);
            maxPropField.SetValueWithoutNotify(max);

            serializedObject.ApplyModifiedProperties();
        });

        //handling field input
        SetupMinMaxField(minPropField);
        SetupMinMaxField(maxPropField);
        SetupMaxEnemiesField(maxEnemiesPropField, maxEnemiesProp);
        SetupSpawnIntervalField(spawnIntervalPropField, spawnIntervalProp);

        //progress bar limits
        progressBar.lowValue = 0;
        progressBar.highValue = maxEnemiesProp.intValue;

        return root;

        //checks values of max max, max enemies and spawn interval fields once not focussed anymore
        void SetupMinMaxField(IntegerField field)
        {
            field.RegisterCallback<BlurEvent>(_ =>
            {
                ApplyFieldClamp(minPropField, maxPropField, minProp, maxProp, minMaxSlider, levelMax);
            });

            field.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode is KeyCode.Return or KeyCode.KeypadEnter)
                {
                    ApplyFieldClamp(minPropField, maxPropField, minProp, maxProp, minMaxSlider, levelMax);
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
                    if (e.keyCode is not (KeyCode.Return or KeyCode.KeypadEnter)) return;
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

    //limits for values
    //min >= 1
    //max <= levelMax
    //min can't reach max
    //max can't drop below min - 1
    private static void ClampMinMax(ref int min, ref int max, int levelMax)
    {
        min = Mathf.Clamp(min, 1, levelMax - 1);
        max = Mathf.Clamp(max, min + 1, levelMax);
    }

    //clamps min max spawn distance fields
    private void ApplyFieldClamp(
        IntegerField minPropField,
        IntegerField maxPropField,
        SerializedProperty minProp,
        SerializedProperty maxProp,
        MinMaxSlider slider,
        int levelMax)
    {
        serializedObject.Update();

        var min = minPropField.value;
        var max = maxPropField.value;

        ClampMinMax(ref min, ref max, levelMax);

        minProp.intValue = min;
        maxProp.intValue = max;

        slider.SetValueWithoutNotify(new Vector2(min, max));
        minPropField.SetValueWithoutNotify(min);
        maxPropField.SetValueWithoutNotify(max);

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