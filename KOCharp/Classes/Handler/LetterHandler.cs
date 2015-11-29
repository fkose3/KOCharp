using System;

namespace KOCharp
{
    using static Define;
    internal static class LetterHandler
    {
        #region opcodes 
        public const byte LETTER_UNREAD = 1;
        public const byte LETTER_LIST = 2;
        public const byte LETTER_HISTORY = 3;
        public const byte LETTER_GET_ITEM = 4;
        public const byte LETTER_READ = 5;
        public const byte LETTER_SEND = 6;
        public const byte LETTER_DELETE = 7;
        public const byte LETTER_ITEM_CHECK = 8;

        public const byte STORE_OPEN = 1;
        public const byte STORE_CLOSE = 2;
        public const byte STORE_BUY = 3;
        public const byte STORE_MINI = 4;
        public const byte STORE_PROCESS = 5;
        public const byte STORE_LETTER = 6;
        #endregion

        public static void HandleShoppingMall(Packet pkt, User pUser)
        {
            if (pkt.GetByte() != STORE_LETTER)
                return;
            byte opcode = pkt.GetByte();
               
            switch (opcode)
            {
                // Are there any letters to be read?
                // This is for the notification at the top of the screen.
                case LETTER_UNREAD:
                    ReqLetterUnread(pUser);
                    break;

                // Lists all the new mail.
                case LETTER_LIST:
                    ReqLetterList(true,pUser);
                    break;

                // Lists all the old mail.
                case LETTER_HISTORY:
                    ReqLetterList(false, pUser);
                    break;

                // Opens up the letter & marks it as read.
                case LETTER_READ:
                    ReqLetterRead(pkt, pUser);
                    break;

                // Used to send a letter & any coins/items (coins are disabled though)
                case LETTER_SEND:
                    ReqLetterSend(pkt, pUser);
                    break;

                // Used to take an item from a letter. 
                case LETTER_GET_ITEM:
                    ReqLetterGetItem(pkt, pUser);
                    break;

                // Deletes up to 5 old letters at a time.
                case LETTER_DELETE:
                    ReqLetterDelete(pkt, pUser);
                    break;
            }   
        }

        private static void ReqLetterDelete(Packet pkt, User pUser)
        {
            throw new NotImplementedException();
        }

        private static void ReqLetterGetItem(Packet pkt, User pUser)
        {
            throw new NotImplementedException();
        }

        private static void ReqLetterSend(Packet pkt, User pUser)
        {
            Packet result = new Packet(WIZ_SHOPPING_MALL, STORE_LETTER);
            User ptUser = null;
            String strRecipient = String.Empty, strSubject = String.Empty, strMessage = String.Empty;
            _ITEM_DATA pItem = null;

            Int32 nItemID = 0, nCoins = 0, nCoinRequirement = 1000;
            Byte bType, bSrcPos;
            SByte bResult = 1;
            Int64 Serial = 0;
            
           //if(pUser.isTrading() || pUser.isMerchanting())
           //{
           //    bResult = -1;
           //    goto send_packet;
           //}

            pkt.SByte();
            strRecipient = pkt.GetString(); strSubject = pkt.GetString(); bType = pkt.GetByte();

            // invalid recipient name lenght
            if (strRecipient == String.Empty || strRecipient.Length > MAX_ID_SIZE
                // Invalid subject lenght
                || strSubject == String.Empty || strSubject.Length > 31
                // Invalid type (as far we're concerned) 
                || bType == 0 || bType > 2)
                bResult = -1;
            else if (STRCMP(strRecipient, pUser.strCharID))
                bResult = -6;

            if (bResult != 1)
                goto send_packet;

            if(bType == 2)
            {
                if (nItemID != 0)
                    nCoinRequirement = 10000;
                else
                    nCoinRequirement = 5000;

                // Item alma fonksiyonu eklenecek
            }
            send_packet:

            result.SetByte(LETTER_SEND).SetByte(bResult);
            pUser.Send(result);
        }

        private static void ReqLetterRead(Packet pkt, User pUser)
        {
            Packet result = new Packet(WIZ_SHOPPING_MALL, STORE_LETTER);
            Int32 nLetterID = pkt.GetDWORD();
            String strMessage = String.Empty;

            result.SetByte(LETTER_READ);
            if (!DBAgent.ReadLetter(pUser.GetAccountID(), nLetterID, strMessage))
                result.SetByte(0);
            else
            {
                result.SByte();
                result.SetByte(1).SetDword(nLetterID).SetString(strMessage);
            }

            pUser.Send(result);
        }

        private static void ReqLetterList(bool IsHistory, User pUser)
        {
            Packet result = new Packet(WIZ_SHOPPING_MALL, STORE_LETTER);

            result.SetByte(IsHistory ? LETTER_LIST : LETTER_HISTORY);

            if (!DBAgent.GetLetterList(pUser.GetAccountID(), ref result, IsHistory))
                result.SetByte(-1);

            pUser.Send(result);
        }
        
        private static void ReqLetterUnread(User pUser)
        {
            Packet result = new Packet(WIZ_SHOPPING_MALL, STORE_LETTER);
            result.SetByte(LETTER_UNREAD).SetByte(DBAgent.GetUnreadLetter(pUser.GetAccountID()));
            pUser.Send(result);
        }
    }
}