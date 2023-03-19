using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CryptoModule
{
    private static readonly string _PASSWORD = "GGMisGreatestOfAllTime1234abcdefgqwnjebaslwqe12"; //�ּ� 32���� ��ȣŰ �ʿ��ϴ�.
    private byte[] _saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

    public string AESEncrypt256(string plainText)
    {
        byte[] plainByte = Encoding.UTF8.GetBytes(plainText);

        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_PASSWORD, _saltBytes);

        Aes myAES = Aes.Create();
        myAES.Mode = CipherMode.CBC;
        myAES.Padding = PaddingMode.PKCS7;
        myAES.KeySize = 256;
        myAES.BlockSize = 128;
        myAES.Key = key.GetBytes(myAES.KeySize / 8);
        myAES.IV = key.GetBytes(myAES.BlockSize / 8);

        MemoryStream ms = new MemoryStream();
        ICryptoTransform encrypter = myAES.CreateEncryptor(myAES.Key, myAES.IV); //��ȣ�Ⱑ �������
        CryptoStream cs = new CryptoStream(ms, encrypter, CryptoStreamMode.Write);

        cs.Write(plainByte, 0, plainByte.Length);
        cs.FlushFinalBlock(); //�̰� ������ ���ϱ��� ���� �������°� 80����Ʈ 

        byte[] encryptedBytes = ms.ToArray(); //�޸𸮽�Ʈ���� ���� ��ȣȭ�� �����Ͱ� ���´�.

        string encryptedString = Convert.ToBase64String(encryptedBytes);

        cs.Close();
        ms.Close(); //�� ������ try catch�� ����� ��.

        return encryptedString;
    }

    public string Decrypt(string cryptedText)
    {
        var key = new Rfc2898DeriveBytes(_PASSWORD, _saltBytes);
        Aes myAES = Aes.Create();
        myAES.Mode = CipherMode.CBC;
        myAES.Padding = PaddingMode.PKCS7;
        myAES.KeySize = 256;
        myAES.BlockSize = 128;
        myAES.Key = key.GetBytes(myAES.KeySize / 8);
        myAES.IV = key.GetBytes(myAES.BlockSize / 8);

        MemoryStream ms = new MemoryStream();
        ICryptoTransform decryptor = myAES.CreateDecryptor();
        CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

        byte[] crytedBytes = Convert.FromBase64String(cryptedText);

        cs.Write(crytedBytes, 0, crytedBytes.Length);

        byte[] buffer = ms.ToArray();
        string output = Encoding.UTF8.GetString(buffer);

        return output;
    }
}
