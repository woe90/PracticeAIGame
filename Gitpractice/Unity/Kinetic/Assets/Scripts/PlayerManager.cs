using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour

{
    [SerializeField] private Transform Trans;
    private float playerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 5f;

        print("Hello");

        Debug.Log("hello");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow)){
            print("LEFT");

            float x = Trans.position.x * (playerSpeed * Time.deltaTime);

            Trans.position = new Vector3(x, Trans.position.y, Trans.position.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {

            print("RIGHT");
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            print("Up");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {

            print("Down");
        }
    }
}
