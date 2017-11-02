using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace V5_WinLibs.Core {
    /// <summary>
    /// Des�ԳƼ��� ����
    /// </summary>
    public class DESHelper1 {
        #region ========����========
        /// <summary>   
        /// DES���ܷ���   
        /// </summary>   
        /// <param name="strPlain">����</param>   
        /// <param name="strDESKey">��Կ</param>   
        /// <param name="strDESIV">���� ֻ��8λ</param>   
        /// <returns>����</returns>   
        public static string Encrypt(string source, string _DESKey) {
            StringBuilder sb = new StringBuilder();
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
                byte[] key = ASCIIEncoding.ASCII.GetBytes(_DESKey);
                byte[] iv = ASCIIEncoding.ASCII.GetBytes(_DESKey);
                byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
                des.Mode = System.Security.Cryptography.CipherMode.CBC;
                des.Key = key;
                des.IV = iv;
                string encrypt = "";
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write)) {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    foreach (byte b in ms.ToArray()) {
                        sb.AppendFormat("{0:X2}", b);
                    }
                    encrypt = sb.ToString();
                }
                return encrypt;
            }

        }

        #endregion

        #region ========����========
        /// <summary> 
        /// �������� 
        /// </summary> 
        /// <param name="Text">���ܵ��ַ���</param> 
        /// <param name="sKey">����Key ֻ��8λ</param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey) {
            try {

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++) {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());

            }
            catch (Exception) {
                return string.Empty;
            }
        }

        #endregion
    }
}
