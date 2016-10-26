using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Commands;
using System;

[CreateAssetMenu(menuName = "Command/Choice")]
public class Choice : Command {

    [SerializeField]
    Command[] choices;

    public override IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
    {
        gameData.player.choiceIndex = -1;
        var choiceNames = new string[choices.Length];
        for (int i = 0; i < choices.Length; i++)
            choiceNames[i] = choices[i].name;
        gameData.player.RpcShowChoices(choiceNames);
        while (gameData.player.choiceIndex < 0)
            yield return null;

        Debug.Log(choices[gameData.player.choiceIndex].name);
        resolve(CommandResult.success);
    }
}
