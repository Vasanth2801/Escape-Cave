using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreen;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            winScreen.SetActive(true);
        }
    }
}
