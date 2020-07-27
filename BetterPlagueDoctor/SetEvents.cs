using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using UnityEngine;

namespace BetterPlagueDoctor
{
    public class SetEvents
    {
        internal void OnRoundStarted()
        {
            Global.can_use_commands = true;
            Door armory049 = null;
            Global.SCP049GateIsLock = false;
            foreach (Door door in Map.Doors)
            {
                if (door.DoorName.ToLower().Contains("049") && door.DoorName.ToLower().Contains("armory"))
                {
                    armory049 = door;
                    break;
                }
            }
            foreach (Door door in Map.Doors)
            {
                if (Vector3.Distance(armory049.gameObject.transform.position, door.gameObject.transform.position) < 15.0f)
                {
                    if (door.DoorName == armory049.DoorName)
                        continue;
                    Global.SCP049Gate = door;
                    break;
                }
            }
            foreach (WorkStation workStation in UnityEngine.Object.FindObjectsOfType<WorkStation>())
            {
                if (Vector3.Distance(armory049.gameObject.transform.position, workStation.gameObject.transform.position) < 15.0f)
                {
                    Global.SCP049GateWorkstation = workStation;
                }
            }
        }

        internal void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            if (ev.Name == "cure")
            {
                if (ev.Player.Role != RoleType.Scp049)
                {
                    ev.ReturnMessage = Global._not049;
                    return;
                }
                GameObject gameObject = ev.Player.GameObject;
                if (gameObject.GetComponent<CureCurrentBody>() != null)
                {
                    ev.ReturnMessage = Global._isalreadycure + gameObject.GetComponent<CureCurrentBody>().curZombie.Nickname;
                    return;
                }
                foreach (Ragdoll rd in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
                {
                    if (Global.damageTypes.Contains(rd.owner.DeathCause.GetDamageType()))
                    {
                        if (Vector3.Distance(gameObject.transform.position, rd.transform.position) <= Global.distance)
                        {
                            if (Player.Get(rd.owner.PlayerId) != null && Player.Get(rd.owner.PlayerId).Role == RoleType.Spectator)
                            {
                                gameObject.AddComponent<CureCurrentBody>();
                                ev.ReturnMessage = Global._successstart + Player.Get(rd.owner.PlayerId).Nickname;
                                return;
                            }
                        }
                    }
                }
                ev.ReturnMessage = Global._istoolargefor049_2;
                return;
            }
            else if (ev.Name == "stopcure")
            {
                if (ev.Player.Role != RoleType.Scp049)
                {
                    ev.ReturnMessage = Global._not049;
                    return;
                }
                GameObject gameObject = ev.Player.GameObject;
                if (gameObject.GetComponent<CureCurrentBody>() != null)
                {
                    ev.ReturnMessage = Global._stopcure + gameObject.GetComponent<CureCurrentBody>().curZombie.Nickname;
                    UnityEngine.Object.Destroy(gameObject.GetComponent<CureCurrentBody>());
                    return;
                }
                else
                {
                    ev.ReturnMessage = Global._isnotcure;
                    return;
                }
            }
            else if (ev.Name.Contains("gate049"))
            {
                if (ev.Player.Team == Team.SCP && ev.Player.Role != RoleType.Scp049 || ev.Player.Role == RoleType.Spectator)
                {
                    ev.ReturnMessage = "Вы не можете использовать эту команду";
                    return;
                }
                if (Vector3.Distance(Global.SCP049GateWorkstation.transform.position, ev.Player.Position) < 3.0f)
                {
                    if (ev.Name.Contains("open"))
                    {
                        if (Global.SCP049Gate.NetworkisOpen)
                        {
                            ev.ReturnMessage = "Гермоворота уже открыты";
                            return;
                        }
                        else
                        {
                            Global.SCP049Gate.NetworkisOpen = true;
                            ev.ReturnMessage = "Вы открыли гермоворота";
                            return;
                        }
                    }
                    else if (ev.Name.Contains("close"))
                    {
                        if (Global.SCP049Gate.NetworkisOpen)
                        {
                            Global.SCP049Gate.NetworkisOpen = false;
                            ev.ReturnMessage = "Вы закрыли гермоворота";
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = "Гермоворота уже закрыты";
                            return;
                        }
                    }
                    else if (ev.Name.Contains("unlock"))
                    {
                        if (Global.SCP049GateIsLock)
                        {
                            Global.SCP049GateIsLock = false;
                            ev.ReturnMessage = "Вы разблокировали гермоворота";
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = "Гермоворота уже разблокированы";
                            return;
                        }
                    }
                    else if (ev.Name.Contains("lock"))
                    {
                        if (Global.SCP049GateIsLock)
                        {
                            ev.ReturnMessage = "Гермоворота уже заблокированы";
                            return;
                        }
                        else
                        {
                            Global.SCP049GateIsLock = true;
                            ev.ReturnMessage = "Вы заблокировали гермоворота";
                            return;
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = "Использование: gate049 <open|close|unlock|lock>";
                        return;
                    }
                }
                else
                {
                    ev.ReturnMessage = "Вы слишком далеко от рычага";
                    return;
                }
            }
        }

        internal void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Door == Global.SCP049Gate && Global.SCP049GateIsLock)
            {
                ev.IsAllowed = false;
            }
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
        }
    }
}