﻿using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace EIP.Framework
{
    /// 
    /// Security 的摘要說明。 
    /// Security類實現.NET框架下的加密和解密。 
    /// CopyRight KangSoft@Hotmail.com@Hotmail.com@hotmail.com 
    /// 
    public class Security
    {
        string _QueryStringKey = "abcdefgh"; //URL傳輸參數加密Key 
        string _PassWordKey = "hgfedcba"; //PassWord加密Key 

        public Security()
        {
            // 
            // TODO: 在此處添加構造函數邏輯 
            // 
        }

        /// 
        /// 加密URL傳輸的字符串 
        /// 
        /// 
        /// 
        public string EncryptQueryString(string QueryString)
        {
            return Encrypt(QueryString, _QueryStringKey);
        }

        /// 
        /// 解密URL傳輸的字符串 
        /// 
        /// 
        /// 
        public string DecryptQueryString(string QueryString)
        {
            return Decrypt(QueryString, _QueryStringKey);
        }

        /// 
        /// 加密帳號口令 
        /// 
        /// 
        /// 
        public string EncryptPassWord(string PassWord)
        {
            return Encrypt(PassWord, _PassWordKey);
        }

        /// 
        /// 解密帳號口令 
        /// 
        /// 
        /// 
        public string DecryptPassWord(string PassWord)
        {
            return Decrypt(PassWord, _PassWordKey);
        }

        /// 
        /// DEC 加密過程 
        /// 
        /// 
        /// 
        /// 
        public string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte數組中 

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[] inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt); 

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密對象的密鑰和偏移量 
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey); //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
            MemoryStream ms = new MemoryStream(); //使得輸入密碼必須輸入英文文本 
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// 
        /// DEC 解密過程 
        /// 
        /// 
        /// 
        /// 
        public string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密對象的密鑰和偏移量，此值重要，不能修改
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder(); //建立StringBuild對象，CreateDecrypt使用的是流對象，必須把解密後的文本變成流對像 

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// 
        /// 檢查己加密的字符串是否與原文相同 
        /// 
        /// 
        /// 
        /// 
        /// 
        public bool ValidateString(string EnString, string FoString, int Mode)
        {
            switch (Mode)
            {
                default:
                case 1:
                    if (Decrypt(EnString, _QueryStringKey) == FoString.ToString())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case 2:
                    if (Decrypt(EnString, _PassWordKey) == FoString.ToString())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }
    }
}