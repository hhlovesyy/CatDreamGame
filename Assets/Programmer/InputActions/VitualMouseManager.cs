using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.UI;

public class VirtualMouseManager : MonoBehaviour
{
    public InputActionAsset inputActions; // 你的Input Action Asset
    private InputAction moveAction;
    private InputAction clickAction;
    private Mouse virtualMouse;
    private Vector2 virtualMousePosition;

    private void OnEnable()
    {
        // 初始化虚拟鼠标
        if (virtualMouse == null)
        {
            virtualMouse = InputSystem.AddDevice<Mouse>("VirtualMouse");
            InputUser.PerformPairingWithDevice(virtualMouse);
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        // 获取Action
        moveAction = inputActions.FindAction("Navigate"); // 替换为你实际的Navigate Action路径
        clickAction = inputActions.FindAction("Submit"); // 替换为你实际的Submit Action路径

        moveAction.Enable();
        clickAction.Enable();

        // 初始化虚拟鼠标位置
        virtualMousePosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        InputState.Change(virtualMouse.position, virtualMousePosition);

        // 订阅更新事件
        InputSystem.onBeforeUpdate += UpdateVirtualMouse;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        clickAction.Disable();

        if (virtualMouse != null && virtualMouse.added)
            InputSystem.RemoveDevice(virtualMouse);

        InputSystem.onBeforeUpdate -= UpdateVirtualMouse;
    }

    private void UpdateVirtualMouse()
    {
        // 获取手柄输入
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        virtualMousePosition += moveInput * Time.deltaTime * 100f; // 调整速度
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);

        // 更新虚拟鼠标位置
        InputState.Change(virtualMouse.position, virtualMousePosition);

        // 模拟鼠标左键点击
        bool isClicking = clickAction.ReadValue<float>() > 0;
        InputState.Change(virtualMouse.leftButton, isClicking ? 1 : 0);
    }
}