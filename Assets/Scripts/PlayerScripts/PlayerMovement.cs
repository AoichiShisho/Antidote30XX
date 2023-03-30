using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public float speed = 2.5f;
    public float jumpForce = 300.0f;
    private bool isFrozen = false;

    private Animator anim;
    private SpriteRenderer sprite;

    void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        playerMovement();
#if UNITY_EDITOR
        playerJump();
#endif
    }

    public void SetFrozenStatus(bool status) {
        isFrozen = status;
        if (isFrozen) anim.SetBool("isWalking", false);
    }

    public bool IsPlayerFrozen()
    {
        return isFrozen;
    }

    public void Teleport(Transform location)
    {
        transform.position = location.position;
    }

    public void WaitAndTeleport(int seconds, Transform location)
    {
        StartCoroutine(WaitAndTeleportCoroutine(seconds, location));
    }

    private IEnumerator WaitAndTeleportCoroutine(int seconds, Transform location)
    {
        yield return new WaitForSeconds(seconds);
        Teleport(location);
    }
    
    public void TeleportToLab()
    {
        WaitAndTeleport(1, GameObject.Find("PresetLocations/Lab").transform);
    }

    private void playerMovement() {
        if (isFrozen) {
            return;
        }

         // left
        if (Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.back * speed * Time.deltaTime; 
            sprite.flipX = true;
            anim.SetBool("isWalking", true);
            anim.SetBool("isFront", true);
        }
        // right
        if (Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.forward * speed * Time.deltaTime;
            sprite.flipX = false;
            anim.SetBool("isWalking", true);
            anim.SetBool("isFront", true);
        }
        // forward
        if (Input.GetKey(KeyCode.W)) {
            transform.position += Vector3.left * speed * Time.deltaTime;
            anim.SetBool("isWalking", true);
            anim.SetBool("isFront", false);
        }
        // back
        if (Input.GetKey(KeyCode.S)) {
            transform.position += Vector3.right * speed * Time.deltaTime;
            anim.SetBool("isWalking", true);
            anim.SetBool("isFront", true);
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) 
        && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) {
            anim.SetBool("isWalking", false);
        }
    }

    private void playerJump() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        }
    }
}
