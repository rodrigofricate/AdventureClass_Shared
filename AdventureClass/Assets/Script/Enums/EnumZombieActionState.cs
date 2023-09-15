using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Enums
{
    public enum EnumZombieActionState : int
    {
        Patrol,
        ChasingPlayer,
        AngryChasingPlayer,
        AttackPlayer,
        Death,
        Null,

    }
}
