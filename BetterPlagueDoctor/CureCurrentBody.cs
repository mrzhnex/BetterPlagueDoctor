using Exiled.API.Features;
using UnityEngine;

namespace BetterPlagueDoctor
{
    public class CureCurrentBody : MonoBehaviour
    {
        private Player scp049;
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private Ragdoll curBody;
        public Player curZombie;
        private float time_to_make_zombie = 0f;

        public void Start()
        {
            scp049 = Player.Get(gameObject);

            foreach (Ragdoll rd in FindObjectsOfType<Ragdoll>())
            {
                if (Global.damageTypes.Contains(rd.owner.DeathCause.GetDamageType()))
                {
                    if (Vector3.Distance(gameObject.transform.position, rd.transform.position) < Global.distance)
                    {
                        curBody = rd;
                        curZombie = Player.Get(rd.owner.PlayerId);
                        break;
                    }
                }
            }
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeIsUp)
            {
                timer = 0f;

                if (Vector3.Distance(gameObject.transform.position, curBody.transform.position) < Global.distance)
                {
                    curZombie = Player.Get(curZombie.Id);
                    if (curZombie != null && curZombie.Role == RoleType.Spectator)
                    {
                        time_to_make_zombie += timeIsUp;
                        scp049.ClearBroadcasts();
                        scp049.Broadcast(1, string.Concat(new object[]
                        {
                            "<color=#228b22>Вы поднимаете ",
                            curZombie.Nickname,
                            " осталось: ",
                            Global.timeToMakeZombie - time_to_make_zombie + "</color>"
                        }), Broadcast.BroadcastFlags.Normal);
                    }
                    else
                    {
                        scp049.ClearBroadcasts();
                        scp049.Broadcast(10, "<color=#ff0000>" + curZombie.Nickname + " внезапно стал неподвластен вашему лечению</color>", Broadcast.BroadcastFlags.Normal);
                        Destroy(gameObject.GetComponent<CureCurrentBody>());
                    }

                    if (time_to_make_zombie >= Global.timeToMakeZombie)
                    {
                        curZombie.SetRole(RoleType.Scp0492, true);
                        curZombie.Position = curBody.gameObject.transform.position + Vector3.up;
                        Destroy(curBody.gameObject, 0f);
                        scp049.ClearBroadcasts();
                        scp049.Broadcast(10, "<color=#228b22>" + curZombie.Nickname + " был излечен</color>", Broadcast.BroadcastFlags.Normal);
                        Destroy(gameObject.GetComponent<CureCurrentBody>());
                    }
                }
                else
                {
                    scp049.ClearBroadcasts();
                    scp049.Broadcast(10, "<color=#228b22>Вы прекратили лечение " + curZombie.Nickname + "</color>", Broadcast.BroadcastFlags.Normal);
                    Destroy(gameObject.GetComponent<CureCurrentBody>());
                }
            }
        }
    }
}