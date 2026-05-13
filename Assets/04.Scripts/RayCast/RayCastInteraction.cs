using UnityEngine;
using UnityEngine.InputSystem;


public class RayCastInteraction : MonoBehaviour
{

    // 카메라 화면의 중점에서 Ray 쏘고자 함 -> 카메라를 알아야함
    [SerializeField] private Camera m_cam;

    [SerializeField] private LayerMask m_hittableMask;
    // Ray의 최대거리
    [SerializeField] private float m_maxDistance = 3f;

    [SerializeField] public Transform attackPoint;

    private PlayerInput playerInput;
    private InputAction interact;
    private InputAction fire;

    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interact = playerInput.actions.FindAction("Interaction", throwIfNotFound: true);
        fire = playerInput.actions.FindAction("Fire", throwIfNotFound: true);
        if (m_cam == null) m_cam = Camera.main;
    }

    private void Update()
    {
        CheckRaycast();        
    }

    private void OnEnable()
    {
        interact.performed += Interact;
        fire.performed += Attack;
    }

    private void OnDisable()
    {
        interact.performed -= Interact;
        fire.performed -= Attack;
    }

    private void CheckRaycast()
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = m_cam.ScreenPointToRay(screenCenter);

        if(Physics.Raycast(ray, out RaycastHit hit, m_maxDistance, m_hittableMask))
        {
            int targetLayer = hit.collider.gameObject.layer;

            string targetName = hit.collider.gameObject.name;

            if(targetLayer == 7)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);

                InteractableController interactableController = hit.collider.GetComponent<InteractableController>();

                if (interactableController.CheckInteract())
                {
                    switch (targetName)
                    {
                        case "NPC": Debug.Log("[E] NPC와 대화"); break;
                        case "Chest": Debug.Log("[E] 상자 열기"); break;
                        case "Hub": Debug.Log("[E] 약초 채집"); break;
                    }
                }
            }
        }
    }

    private void Interact(InputAction.CallbackContext _)
    {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = m_cam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance, m_hittableMask))
        {
            int targetLayer = hit.collider.gameObject.layer;

            string targetName = hit.collider.gameObject.name;

            if (targetLayer == 7)
            {
                InteractableController interactableController = hit.collider.GetComponent<InteractableController>();

                if (interactableController.CheckInteract())
                {
                    interactableController.UseInteract();
                    Debug.Log($"{targetName} 과의 상호작용을 완료하였습니다.");
                }
                else
                {
                    Debug.Log("이미 상호작용을 완료하였습니다.");
                }
            }
        }
        else
        {
            Debug.Log("상호작용 대상이 없습니다.");
        }
    }

    private void Attack(InputAction.CallbackContext _)
    {
        Vector3 center = transform.position;
        float radius = 1.3f;
        int attackDamage = 60;

        Debug.Log("공격");

        Collider[] hitColliders = Physics.OverlapSphere(center + attackPoint.forward * 3.0f, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.layer == 6)
            {
                EnemyHealth targetHealth = hitCollider.gameObject.GetComponent<EnemyHealth>();

                targetHealth.TakeDamage(attackDamage);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blueViolet;

        Gizmos.DrawWireSphere(transform.position + attackPoint.forward * 3.0f, 1.3f);
    }
}
