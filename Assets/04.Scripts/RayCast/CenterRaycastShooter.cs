using UnityEngine;
using UnityEngine.InputSystem;

public class CenterRaycastShooter : MonoBehaviour
{
    // 카메라 화면의 중점에서 Ray 쏘고자 함 -> 카메라를 알아야함
    [SerializeField]private Camera m_cam;
    
    [SerializeField]private LayerMask m_hittableMask;
    // Ray의 최대거리
    [SerializeField]private float m_maxDistance = 100f;

    private PlayerInput _pi;
    private InputAction _fire;

    private void Awake()
    {
        _pi = GetComponent<PlayerInput>();
        // 입력 액션 "Fire" 찾기 (Project Settings > Input Actions 에서 지정해야 함)
        _fire = _pi.actions.FindAction("Fire", throwIfNotFound: true);
        // 카메라가 연결 안 되어 있을 경우 메인 카메라 자동 할당
        if(m_cam == null) m_cam = Camera.main;
    }
    private void OnEnable()
    {
        _fire.performed += OnRayFire;
    }

    private void OnDisable()
    {
        _fire.performed -= OnRayFire;
    }


    // 마우스 왼쪽 클릭 _fire -> 등록 
    private void OnRayFire(InputAction.CallbackContext _)
    {
        //if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        Vector2 _screenCenter = new (Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = m_cam.ScreenPointToRay(_screenCenter);

        // QueryTriggerInteraction : Trigger Collider를 어떻게 대응할 것인지
        if (Physics.Raycast(ray, out RaycastHit hit, m_maxDistance, m_hittableMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"[CenterRaycastShooter_PI] Hit {hit.collider.name} @ {hit.point}");

            //var rend = hit.collider.GetComponent<Renderer>();
            //if (rend) rend.material.color = Color.red;

            // Scene 뷰에서 Ray가 맞은 지점까지 초록선 그려줌
            Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);
        }
        else
        {
            // Ray가 아무것도 맞지 않았다면 노란색을 Scene 뷰에서 그려줌
            Debug.DrawLine(ray.origin, ray.direction * m_maxDistance, Color.yellow, 0.5f);
        }

        SphereCastExample();
    }

    private void SphereCastExample()
    {
        float radius = 5.0f; // 구체 반지름
        float maxDistance = 10.0f;

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if(Physics.SphereCast(origin, radius, direction, out var hit, maxDistance, m_hittableMask))
        {
            Debug.Log($"Sphere Hit {hit.collider.name}");
        }

    }
    
    // 즉발 장판기
    // 결과값을, 복수개의 collider를 배열로 반환
    private void OverlapExample()
    {
        Vector3 center = transform.position;
        float radius = 2.0f;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            Debug.Log($"Detected: {hitCollider.name}");
        }
    }

    // 성능 최적화  - Overlap(방어코드 개념)
    private Collider[] results = new Collider[10];

    private void OptimizedOverlap()
    {
        // Alloc -> Allocation
        int count = Physics.OverlapSphereNonAlloc(transform.position, 5.0f, results);

        for (int i = 0; i < count; i++)
        {
            Debug.Log($"{results[i].name}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blueViolet;

        Gizmos.DrawWireSphere(transform.position + transform.forward * 10.0f, 5.0f);
    }
}
