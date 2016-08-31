using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class GameImpl : MonoBehaviour
    {
        public Camera sharedCamera;

        

        public Rulesets.BaseRuleset rules;

        public enum State
        {
            start,
            play,
            end
        }
        private State state;
        private State previousState;

        public State GetState()
        {
            return state;
        }

        public void SetState(State newState, bool save = true)
        {
            if (save)
            {
                previousState = state;
            }

            state = newState;
        }

        public void RevertState()
        {
            SetState(previousState);
        }
    }
}