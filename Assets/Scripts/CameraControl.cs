using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    private float joyStickX;
    public float sensitivity;
    public FixedJoystick fixedJoystick;
    public GameObject QuickTurn;

    private void Start()
    {
        QuickTurn.GetComponent<Button>().onClick.AddListener(HeyYa);
    }

    private void Update(){
        joyStickX = fixedJoystick.Direction.x * sensitivity * Time.deltaTime;

        // xRotation -= mouseY;
        // xRotation = Mathf.Clamp(xRotation, -10f, 10f);

        player.Rotate(Vector3.up * joyStickX);
        //Debug.Log(fixedJoystick.Direction);
        // transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void HeyYa()
    {
        player.Rotate(Vector3.up * 180);
    }
}
