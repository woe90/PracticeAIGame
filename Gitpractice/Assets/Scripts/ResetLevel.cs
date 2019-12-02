using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(1 << other.gameObject.layer == 13)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
