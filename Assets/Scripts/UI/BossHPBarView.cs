using UnityEngine;
using UnityEngine.UI;
using Core.Enemy_Logic;

public class BossHPBarView : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Vector3 offset = new Vector3(0f, -1.5f, 0f);

    private EnemyAbstract enemy;

    public void Initialize(EnemyAbstract target)
    {
        enemy = target;
        enemy.OnHealthChanged += UpdateBar;
    }

    private void OnDestroy()
    {
        if (enemy != null)
            enemy.OnHealthChanged -= UpdateBar;
    }

    private void LateUpdate()
    {
        if (enemy != null)
        {
            transform.position = enemy.transform.position + offset;
        }
    }

    private void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}