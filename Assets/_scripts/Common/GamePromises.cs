using UnityEngine;
using System;
using System.Collections;
using RSG;

public class GamePromises : Singleton<GamePromises>
{
    public static IPromise<bool> PlayCard(CardId card)
    {
        Debug.Log("Returning PlayCard coroutine...");
        return new Promise<bool>((resolve, reject) => Instance.StartCoroutine(CardResult(card, resolve, reject)));
    }

    static IEnumerator CardResult(CardId card, Action<bool> resolve, Action<Exception> reject)
    {
        var player = PlayerControl.local;
        Debug.Log("Starting command...");
        player.CmdPlayCard(card);

        while(player.commandRunning)
            yield return null;

        Debug.Log("Command finished");

        var someErrorOccurred = false;
        if (someErrorOccurred)
        {
            // An error occurred, reject the promise.
            reject(new ApplicationException("My error"));
        }
        else
        {
            // Completed successfully, resolve the promise.
            resolve(player.commandSuccess);
        }
    }
}
