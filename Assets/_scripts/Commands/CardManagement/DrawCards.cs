using UnityEngine;
using System.Collections;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Draw Cards")]
    public class DrawCards : Command
    {
        public int valueSize;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
        }

        public override IEnumerator Routine(Action resolve, Action<Exception> reject)
        {
            yield return null;

            if (gameData.player.CanDrawCards)
            {
                gameData.player.ServerDrawCards(valueSize);
            }

            resolve();
        }

        public override IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
        {
            yield return null;

            if (gameData.player.CanDrawCards)
            {
                gameData.player.ServerDrawCards(valueSize);
                resolve(CommandResult.permanent);
            }
            else
                resolve(CommandResult.failure);
        }
    }
}