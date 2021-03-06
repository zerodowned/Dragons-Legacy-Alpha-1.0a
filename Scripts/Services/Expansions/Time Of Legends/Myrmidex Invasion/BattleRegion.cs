using System;
using Server;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Regions;
using System.Xml;

namespace Server.Engines.MyrmidexInvasion
{
    public class BattleRegion : StygianAbyssRegion
	{
        public BattleSpawner Spawner { get; set; }

        public BattleRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override void OnDeath(Mobile m)
        {
            base.OnDeath(m);

            if (BattleSpawner.Active && m is BaseCreature && ((BaseCreature)m).GetMaster() == null && Spawner != null)
            {
                Timer.DelayCall<BaseCreature>(TimeSpan.FromSeconds(.25), Spawner.RegisterDeath, (BaseCreature)m);
            }
        }

        public override void OnExit(Mobile m)
        {
            if (m is PlayerMobile && Spawner != null)
                Spawner.OnLeaveRegion((PlayerMobile)m);

            base.OnExit(m);
        }

        public override bool OnDamage(Mobile m, ref int Damage)
        {
            Mobile attacker = m.FindMostRecentDamager(false);

            if (MyrmidexInvasionSystem.IsEnemies(m, attacker) && EodonianPotion.IsUnderEffects(attacker, PotionEffect.Kurak))
            {
                Damage *= 3;

                if (Damage > 0)
                    m.FixedEffect(0x37B9, 10, 5);
            }

            return base.OnDamage(m, ref Damage);
        }
	}
}