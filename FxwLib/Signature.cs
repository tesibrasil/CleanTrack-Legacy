namespace It.IDnova.Fxw
{
    internal class Signature
    {
        private const int POLY_ISO15693 = 33800;
        private const int PRESET_ISO15693 = 65535;

        public static ushort SignatureCalcHfApriporta(byte[] buffer)
        {
            ushort num1 = 33800;
            ushort num2 = ushort.MaxValue;
            int length = buffer.Length;
            for (int index1 = 0; index1 < length; ++index1)
            {
                num2 ^= (ushort)buffer[index1];
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    if (((uint)num2 & 1U) > 0U)
                        num2 = (ushort)((uint)num2 >> 1 ^ (uint)num1);
                    else
                        num2 >>= 1;
                }
            }
            // return ~num2;
            return (ushort)~num2;
        }
    }
}
