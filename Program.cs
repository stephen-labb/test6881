using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

class AESGCMBouncyCastle
{
    public static void Main()
    {
        string plaintext = "Top secret message!";
        byte[] key = new byte[32]; // 256-bit key
        byte[] nonce = new byte[12]; // 96-bit nonce

        SecureRandom random = new SecureRandom();
        random.NextBytes(key);
        random.NextBytes(nonce);

        // Encrypt
        var encrypted = Encrypt(Encoding.UTF8.GetBytes(plaintext), key, nonce, out byte[] tag);
        Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));
        Console.WriteLine("Tag: " + Convert.ToBase64String(tag));

        // Decrypt
        var decrypted = Decrypt(encrypted, key, nonce, tag);
        Console.WriteLine("Decrypted: " + Encoding.UTF8.GetString(decrypted));
    }

    public static byte[] Encrypt(byte[] plaintext, byte[] key, byte[] nonce, out byte[] tag)
    {
        GcmBlockCipher cipher = new GcmBlockCipher(new Org.BouncyCastle.Crypto.Engines.AesEngine());
        AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, nonce);
        cipher.Init(true, parameters);

        byte[] output = new byte[cipher.GetOutputSize(plaintext.Length)];
        int len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, output, 0);
        cipher.DoFinal(output, len);

        int tagLength = 16; // 128 bits
        tag = new byte[tagLength];
        Array.Copy(output, output.Length - tagLength, tag, 0, tagLength);

        byte[] ciphertext = new byte[output.Length - tagLength];
        Array.Copy(output, 0, ciphertext, 0, ciphertext.Length);
        return ciphertext;
    }

    public static byte[] Decrypt(byte[] ciphertext, byte[] key, byte[] nonce, byte[] tag)
    {
        GcmBlockCipher cipher = new GcmBlockCipher(new Org.BouncyCastle.Crypto.Engines.AesEngine());
        AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, nonce);
        cipher.Init(false, parameters);

        byte[] input = new byte[ciphertext.Length + tag.Length];
        Array.Copy(ciphertext, 0, input, 0, ciphertext.Length);
        Array.Copy(tag, 0, input, ciphertext.Length, tag.Length);

        byte[] output = new byte[cipher.GetOutputSize(input.Length)];
        int len = cipher.ProcessBytes(input, 0, input.Length, output, 0);
        cipher.DoFinal(output, len);
        return output;
    }
}