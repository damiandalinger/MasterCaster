using ProjectCeros;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyRuntimeSet enemySet;

    [ContextMenu("Deactivate All Enemies")]
    public void DeactivateAllEnemies()
    {
        for (int i = enemySet.Items.Count - 1; i >= 0; i--)
        {
            if (enemySet.Items[i] != null)
                enemySet.Items[i].gameObject.SetActive(false);
        }
    }
}