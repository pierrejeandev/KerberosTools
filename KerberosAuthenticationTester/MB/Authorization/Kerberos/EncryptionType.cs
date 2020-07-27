// Decompiled with JetBrains decompiler
// Type: MB.Authorization.Kerberos.EncryptionType
// Assembly: KerberosAuthenticationTester, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B413C8B6-5019-4556-A1A9-A4BD8CCAC83A
// Assembly location: P:\Software\Utils\KerberosAuthenticationTester.exe

namespace MB.Authorization.Kerberos
{
  public enum EncryptionType
  {
    RC4_HMAC_OLD_EXP = -135, // 0xFFFFFF79
    RC4_HMAC_OLD = -133, // 0xFFFFFF7B
    RC4_MD4 = -128, // 0xFFFFFF80
    UNKNOWN = -1, // 0xFFFFFFFF
    NULL = 0,
    DES_CBC_CRC = 1,
    DES_CBC_MD4 = 2,
    DES_CBC_MD5 = 3,
    RESERVED4 = 4,
    DES3_CBC_MD5 = 5,
    RESERVED6 = 6,
    DES3_CBC_SHA1 = 7,
    DSAWITHSHA1_CMSOID = 9,
    MD5WITHRSAENCRYPTION_CMSOID = 10, // 0x0000000A
    SHA1WITHRSAENCRYPTION_CMSOID = 11, // 0x0000000B
    RC2CBC_ENVOID = 12, // 0x0000000C
    RSAENCRYPTION_ENVOID = 13, // 0x0000000D
    RSAES_OAEP_ENV_OID = 14, // 0x0000000E
    DES_EDE3_CBC_ENV_OID = 15, // 0x0000000F
    DES3_CBC_SHA1_KD = 16, // 0x00000010
    AES128_CTS_HMAC_SHA1_96 = 17, // 0x00000011
    AES256_CTS_HMAC_SHA1_96 = 18, // 0x00000012
    RC4_HMAC = 23, // 0x00000017
    RC4_HMAC_EXP = 24, // 0x00000018
    SUBKEY_KEYMATERIAL = 65, // 0x00000041
  }
}
