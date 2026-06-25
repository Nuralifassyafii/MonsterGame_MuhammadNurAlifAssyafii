using System.Collections;
using UnityEngine;

public class MovingGate : MonoBehaviour
{
    private PlayerManager player;
    [SerializeField] Vector3 newPlayerPosition;
    GameObject camera;
    [SerializeField] Vector3 newCameraPosition;
    private bool isCameraMoving = false;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerManager>();
        camera = FindFirstObjectByType<Camera>().gameObject;
    }
    private void Update()
    {
        if (isCameraMoving)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, newCameraPosition, 10 * Time.deltaTime);
            StartCoroutine(CheckIfCameraDoneMoving());
        }
    }
    public void SetMovementPlayer()
    {
        player.gameObject.transform.position = newPlayerPosition;
    }

    public IEnumerator MoveCamera()
    {
        player.SetPermissionMoving(false);
        yield return new WaitForSeconds(1f);
        isCameraMoving = true;
    }

    private IEnumerator CheckIfCameraDoneMoving()
    {
        yield return new WaitForSeconds(3f);
        isCameraMoving = false;
        player.SetPermissionMoving(true);
    }

    public void Moving()
    {
        SetMovementPlayer();
        StartCoroutine(MoveCamera());
    }

}
