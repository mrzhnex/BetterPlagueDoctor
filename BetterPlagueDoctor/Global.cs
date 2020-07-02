using System.Collections.Generic;

namespace BetterPlagueDoctor
{
    public static class Global
    {
        public static Door SCP049Gate;
        public static WorkStation SCP049GateWorkstation;
        public static bool SCP049GateIsLock = false;
        public static float distance = 2.5f;
        public static float timeToMakeZombie = 15.0f;

        public static string _istoolargefor049_2 = "Рядом нету доступных трупов";
        public static string _successstart = "Вы начинаете лечить ";
        public static string _isalreadycure = "Вы уже лечите ";

        public static string _isnotcure = "Сейчас вы никого не лечите";
        public static string _stopcure = "Вы прекратили лечение ";

        public static string _not049 = "Вы не можете лечить, так как вы не SCP-049";
        internal static bool can_use_commands;
        public static readonly List<DamageTypes.DamageType> damageTypes = new List<DamageTypes.DamageType>
        {
            DamageTypes.Scp049
        };
    }
}