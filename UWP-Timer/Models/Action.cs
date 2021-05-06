using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    public class ActionArgs<T>
    {
        public readonly ActionType Action;

        public readonly T Data;

        public ActionArgs()
        {

        }

        public ActionArgs(ActionType action, T data)
        {
            Action = action;
            Data = data;
        }
    }

    public enum ActionType
    {
        NONE,
        COMMENT,
        REPLY,
        AGREE,
        DISAGREE,
        COLLECT,
        FORWARD,
        REPORT,
        RULE,
        Like,
    }
}
