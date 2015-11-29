using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp.Classes.Database
{
    public static class LoadDatabase
    {
        public static bool LoadCoefficient(ref List<COEFFICIENT> m_CoefficientArray)
        {
            try
            {
                KODatabase db = new KODatabase();

                foreach (COEFFICIENT coeff in db.COEFFICIENTs)
                {
                    m_CoefficientArray.Add(coeff);
                }


            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool LoadLevelUp(ref List<LEVEL_UP> m_arLevelUp)
        {
            try
            {
                KODatabase db = new KODatabase();

                foreach (LEVEL_UP level in db.LEVEL_UP)
                {
                    m_arLevelUp.Add(level);
                }


            }
            catch
            {
                return false;
            }
            return true;
        }
        
        public static bool LoadNpc(ref List<K_NPC> m_NpcList)
        {
            return true;
        }
    }
}
