using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp.Classes.Database
{
    public static class LoadDatabase
    {
        public static bool LoadItemTable(ref List<_ITEM_TABLE> m_ItemTable)
        {
            try
            {
                KODatabase db = new KODatabase();

                foreach(ITEM item in db.ITEMs)
                {
                    _ITEM_TABLE pItem = new _ITEM_TABLE();

                    pItem.m_iNum = item.Num;
                    pItem.m_sName = item.strName;
                    pItem.m_bKind = item.Kind;
                    pItem.m_bSlot = item.Slot;
                    pItem.m_bRace = item.Race;
                    pItem.m_bClass = item.Class;
                    pItem.m_sDamage = item.Damage;
                    pItem.m_sDelay = item.Delay;
                    pItem.m_sRange = item.Range;
                    pItem.m_sWeight = item.Weight;
                    pItem.m_sDuration = item.Duration;
                    pItem.m_iBuyPrice = item.BuyPrice;
                    pItem.m_iSellPrice = item.SellPrice;
                    pItem.m_sAc = item.Ac;
                    pItem.m_bCountable = item.Countable;
                    pItem.m_iEffect1 = item.Effect1;
                    pItem.m_iEffect2 = item.Effect2;
                    pItem.m_bReqLevel = item.ReqLevel;
                    pItem.m_bReqLevelMax = item.ReqLevelMax;
                    pItem.m_bReqRank = item.ReqRank;
                    pItem.m_bReqTitle = item.ReqTitle;
                    pItem.m_bReqStr = item.ReqStr;
                    pItem.m_bReqSta = item.ReqSta;
                    pItem.m_bReqDex = item.ReqDex;
                    pItem.m_bReqIntel = item.ReqIntel;
                    pItem.m_bReqCha = item.ReqCha;
                    pItem.m_bSellingGroup = item.SellingGroup;
                    pItem.m_ItemType = item.ItemType;
                    pItem.m_sHitrate = item.Hitrate;
                    pItem.m_sEvarate = item.Evasionrate;
                    pItem.m_sDaggerAc = item.DaggerAc;
                    pItem.m_sSwordAc = item.SwordAc;
                    pItem.m_sMaceAc = item.MaceAc;
                    pItem.m_sAxeAc = item.AxeAc;
                    pItem.m_sSpearAc = item.SpearAc;
                    pItem.m_sBowAc = item.BowAc;
                    pItem.m_bFireDamage = item.FireDamage;
                    pItem.m_bIceDamage = item.IceDamage;
                    pItem.m_bLightningDamage = item.LightningDamage;
                    pItem.m_bPoisonDamage = item.PoisonDamage;
                    pItem.m_bHPDrain = item.HPDrain;
                    pItem.m_bMPDamage = item.MPDamage;
                    pItem.m_bMPDrain = item.MPDrain;
                    pItem.m_bMirrorDamage = item.MirrorDamage;
                    pItem.m_sStrB = item.StrB;
                    pItem.m_sStaB = item.StaB;
                    pItem.m_sDexB = item.DexB;
                    pItem.m_sIntelB = item.IntelB;
                    pItem.m_sChaB = item.ChaB;
                    pItem.m_MaxHpB = item.MaxHpB;
                    pItem.m_MaxMpB = item.MaxMpB;
                    pItem.m_bFireR = item.FireR;
                    pItem.m_bColdR = item.ColdR;
                    pItem.m_bLightningR = item.LightningR;
                    pItem.m_bMagicR = item.MagicR;
                    pItem.m_bPoisonR = item.PoisonR;
                    pItem.m_bCurseR = item.CurseR;
                    pItem.ItemClass = (short)item.ItemClass;
                    pItem.ItemExt = (short)item.ItemExt;

                    m_ItemTable.Add(pItem);
                }
            }catch
            {
                return false;
            }
            return true;
        }

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
