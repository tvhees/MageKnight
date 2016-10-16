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

        public override IEnumerator Routine(Action<GameConstants.Location> resolve, Action<Exception> reject)
        {
            yield return null;

            if (gameData.player.CanDrawCards)
            {
                gameData.player.ServerDrawCards(valueSize);
                resolve(GameConstants.Location.Play);
            }
            else
                reject(null);
        }
    }
}