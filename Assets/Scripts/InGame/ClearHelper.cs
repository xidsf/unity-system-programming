using UnityEngine;

public class ClearHelper : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InGameManager.Instance.coinCount++;
        }
    }
}
