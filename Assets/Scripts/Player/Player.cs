using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Mengambil komponen PlayerMovement
        playerMovement = GetComponent<PlayerMovement>();
        
        // Mengambil Animator dari EngineEffect
        GameObject engineEffect = GameObject.Find("EngineEffect");
        if (engineEffect != null)
        {
            animator = engineEffect.GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        if (playerMovement != null)
        {
            playerMovement.Move();
        }
    }

    private void LateUpdate()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", playerMovement.IsMoving());
        }
    }
}