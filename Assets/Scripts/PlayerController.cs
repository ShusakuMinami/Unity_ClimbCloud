using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // LoadSceneを使うときは忘れない

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 680.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 2.0f;
    float threshold = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプする
        if(Input.GetMouseButtonDown(0) && this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }
        
        // 左右移動
        int key = 0;
        if(Input.acceleration.x > this.threshold) key = 1;
        if(Input.acceleration.x < -this.threshold) key = -1;
        
        // プレイヤーの速度
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        
        // 歩行のスピード制限
        if(speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
        
        // 動く方向に応じて反転
        if(key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        
        // プレイヤーの速度に応じてアニメーション速度を変える
        // 歩行中の場合
        if(this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        // ジャンプ中の場合
        else
        {
            this.animator.speed = 1.0f;
        }
        
        // 画面外に出た場合は最初から
        if(transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    
    // ゴールに到達
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ゴール");
        SceneManager.LoadScene("ClearScene");
    }
}
