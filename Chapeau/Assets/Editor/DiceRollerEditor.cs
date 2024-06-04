using UnityEngine;
using UnityEditor;

namespace Seacore
{
    [CustomEditor(typeof(DiceRoller))]
    public class DiceRollerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DiceRoller diceRoller = (DiceRoller)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Roll Dice"))
            {
                diceRoller.RollDice();
            }
        }
    }
}
