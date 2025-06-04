using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class lockmanager : MonoBehaviour
{
    public static int count = 0;
    public static int scores = 0;

    internal static void Chancetodropkeys(TextMeshProUGUI txt, TextMeshProUGUI score, GameObject itemPrefab, GameObject health, Vector3 dropPosition, Vector3 dp, int enemycount)
    {
        scores += 100;
        int dropChance = Random.Range(0, 100);

        if (enemycount < 3)
        {
            if (count <= 2)
            {
                Instantiate(itemPrefab, dropPosition, Quaternion.identity);
                count++;
                txt.text = count.ToString();
            }
        }

        else
        {
            if (dropChance <= 10 && count <= 2) 
            {
                Instantiate(itemPrefab, dropPosition, Quaternion.identity);
                count++;
                txt.text = count.ToString();
            }
            else if (dropChance >= 30 && dropChance <= 50) 
            {
                Instantiate(health, dp, Quaternion.identity);
            }
            else
            {
                Debug.Log("No item drop.");
            }
        }

        score.text = "Final Score: " + scores;
    }

}
