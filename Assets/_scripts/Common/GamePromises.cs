using UnityEngine;
using System;
using System.Collections;
using RSG;
using Commands;

public class GamePromises : Singleton<GamePromises>
{
    // We use a promise on the client side so that we can delay moving cards to the appropriate area until after
    // the effect has completed resolution on the server side.
    public static IPromise<CommandResult> PlayCard(CardId card)
    {
        return new Promise<CommandResult>((resolve, reject) => Instance.StartCoroutine(CardResult(card, resolve, reject)));
    }

    static IEnumerator CardResult(CardId card, Action<CommandResult> resolve, Action<Exception> reject)
    {
        var player = PlayerControl.local;
        Debug.Log("Starting command... " + card.name);
        player.CmdPlayCard(card);

        while(player.commandRunning)
            yield return null;

        Debug.Log("Command finished");

        var someErrorOccurred = false;
        if (someErrorOccurred)
        {
            reject(new ApplicationException("My error"));
        }
        else
        {
            resolve(player.commandSuccess);
        }
    }
}
