using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KOCharp
{
    public class Packet
    {

        #region Parameters

        public Socket skt;
        public byte[] send_byte;
        public int send_index = 0;
        int get_index = 0;

        byte StartPacket1 = 0xAA;
        byte StartPacket2 = 0x55;
        short PacketLen;
        byte StopPacket2 = 0xAA;
        byte StopPacket1 = 0x55;
        private byte[] ItemArray;

        #endregion

        public Packet(byte[] read_data, Socket skt)
        {
            this.skt = skt;
            send_byte = read_data;
            get_index = 0;

            byte[] Reading_First = new byte[2];
            Reading_First[0] = GetByte();
            Reading_First[1] = GetByte();

            PacketLen = GetShort();

        }
        public Packet(int len)
        {
            send_index = 0;
            send_byte = new byte[len];
        }

        public Packet(byte[] ItemArray)
        {
            this.send_byte = ItemArray;
            get_index = 0;
        }

        public Packet(byte command, byte param)
        {
            send_index = 0;
            send_byte = new byte[40 * 1024];

            SetByte(command);
            SetByte(param);
        }

        public Packet(byte command)
        {
            send_index = 0;
            send_byte = new byte[40 * 1024];

            SetByte(command);
        }

        public Packet()
        {
            send_index = 0;
            send_byte = new byte[40 * 1024];
        }

        public Packet SetByte(byte param)
        {
            try
            {
                send_byte[send_index++] = param;
            }
            catch { }

            return this;
        }

        public Packet SetByte(sbyte param)
        {
            try
            {
                send_byte[send_index++] = Convert.ToByte(param);
            }
            catch { }
            return this;
        }

        public Packet SetByte(byte[] multiparam)
        {
            for (int i = 0; i < multiparam.Length; i++)
                send_byte[send_index++] = multiparam[i];
            return this;
        }

        public Packet SetShort(Int16 param)
        {
            byte[] byt = BitConverter.GetBytes(param);

            for (int i = 0; i < byt.Length; i++)
                SetByte(byt[i]);

            return this;
        }

        public Packet SetShort(Int16[] param)
        {
            for (int j = 0; j < param.Length; j++)
            {
                byte[] byt = BitConverter.GetBytes(param[j]);

                for (int i = 0; i < byt.Length; i++)
                    SetByte(byt[i]);
            }
            return this;
        }

        public Packet SetDword(Int32 param)
        {
            byte[] byt = BitConverter.GetBytes(param);

            for (int i = 0; i < byt.Length; i++)
                SetByte(byt[i]);
            return this;
        }

        public Packet SetInt64(Int64 param)
        {
            byte[] byt = BitConverter.GetBytes(param);

            for (int i = 0; i < byt.Length; i++)
                SetByte(byt[i]);

            return this;
        }

        public Packet SetString(string param)
        {
            if (param == null || param == string.Empty)
                param = "";
            param = param.TrimEnd(' ', '/');

            byte[] txtByte = System.Text.ASCIIEncoding.GetEncoding("iso-8859-9").GetBytes(param);

            if (singleByte)
                SetByte((byte)param.Length);
            else
                SetShort((Int16)param.Length);

            for (int i = 0; i < txtByte.Length; i++)
                SetByte(txtByte[i]);

            return this;
        }

        public byte GetByte()
        {
            try
            {
                return send_byte[get_index++];
            }
            catch
            {
                return 0;
            }
        }

        public Int16 GetShort()
        {
            byte[] byt = new byte[2];
            byt[0] = GetByte();
            byt[1] = GetByte();

            return Convert.ToInt16(BitConverter.ToInt16(byt, 0));
        }

        public Int32 GetDWORD()
        {
            byte[] byt = new byte[4];
            for (int i = 0; i < byt.Length; i++)
                byt[i] = GetByte();

            return (Int32)BitConverter.ToInt32(byt, 0);
        }

        public Int64 GetInt64()
        {
            byte[] byt = new byte[8];
            for (int i = 0; i < byt.Length; i++)
                byt[i] = GetByte();

            return (Int64)BitConverter.ToInt64(byt, 0);
        }

        public string GetString()
        {
            byte[] txtByte = null;
            if (!singleByte)
                txtByte = new byte[GetShort()];
            else
                txtByte = new byte[GetByte()];

            for (int i = 0; i < txtByte.Length; i++)
                txtByte[i] = GetByte();

            return System.Text.ASCIIEncoding.GetEncoding("iso-8859-9").GetString(txtByte);
        }

        public void Clean()
        {
            send_index = 0;
            get_index = 0;
        }

        private void AddShort(ref byte[] byt, Int16 val, ref int send_index)
        {
            byte[] byts = BitConverter.GetBytes(val);

            for (int i = 0; i < byts.Length; i++)
                byt[send_index++] = (byts[i]);
        }

        private void AddDword(ref byte[] byt, Int32 val, ref int send_index)
        {
            byte[] byts = BitConverter.GetBytes(val);

            for (int i = 0; i < byts.Length; i++)
                byt[send_index++] = (byts[i]);
        }

        private void AddInt64(ref byte[] byt, Int64 val, ref int send_index)
        {
            byte[] byts = BitConverter.GetBytes(val);

            for (int i = 0; i < byts.Length; i++)
                byt[send_index++] = (byts[i]);
        }

        private void AddByte(ref byte[] byt, byte val, ref int send_index)
        {
            byt[send_index++] = val;
        }

        private void AddFull(ref byte[] byt, byte[] add, ref int send_index, int max)
        {
            for (int i = 0; i < max; i++)// (byte byts in add)
            {
                byt[send_index++] = add[i];
            }
        }

        public void Send(Socket sock)
        {
            try
            {

                byte[] send_byts = new byte[send_index + 6];
                int len = 0;

                AddByte(ref send_byts, StartPacket1, ref len);
                AddByte(ref send_byts, StartPacket2, ref len);

                AddShort(ref send_byts, Convert.ToInt16(send_index), ref len);

                AddFull(ref send_byts, send_byte, ref len, send_index);

                AddByte(ref send_byts, StopPacket1, ref len);
                AddByte(ref send_byts, StopPacket2, ref len);

                if (send_index >= 500)
                    SendCompress(sock, send_byts, len);
                else
                    sock.Send(send_byts, len, 0);
            }
            catch
            {
            }
        }

        private void SendCompress(Socket sock, byte[] send_byts, int len)
        {
            if (len <= 0 || len >= 49152)
            {
                Console.WriteLine("### SendCompressingPacket Error : len = {0} ### \n", len);
                return;
            }

            
                sock.Send(send_byts, len, 0);
            
        }

     
        /*
        internal void SetByte(short val)
        {
            byte[] byt = BitConverter.GetBytes(val);

            for (int i = 0; i < byt.Length; i++)
                SetByte(byt[i]);
        }
        */
        bool singleByte = false;
        private byte WIZ_KNIGHTS_PROCESS;
        private byte p;
        internal void SByte()
        {
            singleByte = true;
        }
        internal void DByte()
        {
            singleByte = false;
        }

        internal void append(byte[] value, int size)
        {
            for (int i = 0; i < size; i++)
                SetByte(value[i]);
        }

        internal void PutDword(int value, int pos)
        {
            byte[] bval = BitConverter.GetBytes(value);

            for (int i = pos, j = 0; i < bval.Length; i++, j++)
                send_byte[i] = bval[j];
        }

        internal void PutInt64(long value, int pos)
        {
            byte[] bval = BitConverter.GetBytes(value);

            for (int i = pos, j = 0; i < bval.Length; i++, j++)
                send_byte[i] = bval[j];
        }


        internal void PutShort(short value, int pos)
        {
            byte[] bval = BitConverter.GetBytes(value);

            for (int i = pos, j = 0; i < bval.Length; i++, j++)
                send_byte[i] = bval[j];
        }

        internal void PutByte(byte value, int pos)
        {
            send_byte[pos] = value;
        }



        internal void SetByte(bool val)
        {
            if (val)
                SetByte(1);
            else
                SetByte(0);
        }

        internal int size()
        {
            return send_index;
        }

        internal void append(char[] vals, int len)
        {
            for (int i = 0; i < len; i++)
            {
                try
                {
                    SetByte(Convert.ToByte(vals[i]));
                }
                catch { SetByte(0); }
            }
        }

        internal void SetDword(bool val)
        {
            if (val)
                SetDword(1);
            else
                SetDword(0);
        }
    }
}
