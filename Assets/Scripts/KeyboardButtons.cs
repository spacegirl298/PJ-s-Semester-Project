using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class KeyboardButtons : MonoBehaviour
{
private Controls Controls;
public Button[] keypadButtons;
private int currentButtonIndex = 0;
private Vector3[] originalScales;
private int[,] navigationRules;
private const int NumberOfKeypadButtons = 12;
public void Start()
{
    Controls = new Controls();
    Controls.Keypad.Enable();
originalScales = new Vector3[keypadButtons.Length];
for (int i = 0; i < keypadButtons.Length; i++)
{
originalScales[i] = keypadButtons[i].transform.localScale;
}
navigationRules = new int[,]
{
{ 9, 3, 2, 1 },  // From button 0: Up -> 7, Down -> 4, Left -> 3, Right -> 2 - 1
{ 9, 4, 0, 2 },  // From button 1: Up -> 8, Down -> 5, Left -> 1, Right -> 3 - 2
{ 9, 5, 1, 0 },  // From button 2: Up -> 1, Down -> 0, Left -> 5, Right -> 4 - 3
{ 0, 6, 5, 4 },  // From button 3: Up -> 2, Down -> 1, Left -> 0, Right -> 5 - 4
{ 1, 7, 3, 5 },  // From button 4: Up -> 0, Down -> 1, Left -> 2, Right -> 3 - 5
{ 2, 8, 4, 3 },  // From button 5: Up -> 2, Down -> 3, Left -> 1, Right -> 0 - 6
{ 3, 9, 8, 7 },  // From button 6: Up -> 3, Down -> 9, Left -> 5, Right -> 4 - 7
{ 4, 9, 6, 8 },  // From button 7: Up -> 4, Down -> 1, Left -> 9, Right -> 8 - 8
{ 5, 9, 7, 6 },  // From button 8: Up -> 5, Down -> 2, Left -> 7, Right -> 9 - 9
{ 8, 0, 6, 8 },  // From button 9: Up -> 6, Down -> 3, Left -> 8, Right -> 7 - 10
{ 10, 10, 9, 0 },  // From Clear button (index 10), goes left to itself or to 0 on right
{ 11, 11, 0, 11 }, // From Execute button (index 11), goes right to itself or to 0 on left 
};
Controls.Keypad.Up.performed += ctx => NavigateKeypad(0);
Controls.Keypad.Down.performed += ctx => NavigateKeypad(1);
Controls.Keypad.Left.performed += ctx => NavigateKeypad(2);
Controls.Keypad.Right.performed += ctx => NavigateKeypad(3);
Controls.Keypad.Confirm.performed += ctx => SelectButton();
}
private void Update()
{
for (int i = 0; i <= NumberOfKeypadButtons; i++)
{
if (Input.GetKeyDown((KeyCode)(KeyCode.Keypad1 + i)))
{
currentButtonIndex = i;
UpdateButtonSelection();
SelectButton();
}
}
}
private void OnDisable()
{
    Controls.Keypad.Disable();
}
public void NavigateKeypad(int direction)
{
currentButtonIndex = navigationRules[currentButtonIndex, direction];
UpdateButtonSelection();
}
private void UpdateButtonSelection()
{
for (int i = 0; i < keypadButtons.Length; i++)
{
if (i == currentButtonIndex)
{
// Highlight the selected button by scaling it up
keypadButtons[i].transform.localScale = originalScales[i] * 1.2f;
}
else
{
// Reset the scale for non-selected buttons
keypadButtons[i].transform.localScale = originalScales[i];
}
}
}
public void SelectButton()
{
if(keypadButtons.Length > 0 && currentButtonIndex >= 0 && currentButtonIndex < keypadButtons.Length)
{
keypadButtons[currentButtonIndex].onClick.Invoke();
Debug.Log("Button selected:" + keypadButtons[currentButtonIndex].name);
}
}
}