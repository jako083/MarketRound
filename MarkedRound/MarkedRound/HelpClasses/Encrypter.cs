using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MarkedRound.HelpClasses
{
    class Encryptor : IDisposable
    {

        #region symmetric 
        byte[] _salt;
        AesManaged _algorithm;
        public Encryptor(string salt)
        {
            if (string.IsNullOrEmpty(salt))
                throw new NullReferenceException();

            this._salt = Encoding.Unicode.GetBytes(salt);

            // TODO: 01: Instantiate the _algorithm object.
            _algorithm = new AesManaged();
        }
        public async Task<byte[]> Encrypt(byte[] bytesToEncypt, string password)
        {
            var taskResult = Task.Run(() =>
            {
                var passwordHash = this.GeneratePasswordHash(password);

                var key = this.GenerateKey(passwordHash);

                var iv = this.GenerateIV(passwordHash);

                ICryptoTransform Encryption = _algorithm.CreateEncryptor(key, iv);
                return TransformBytes(Encryption, bytesToEncypt);
            });
            await taskResult;

            return taskResult.Result;
        }
        public async Task<byte[]> Decrypt(byte[] bytesToDecypt, string password)
        {
            var taskResult = Task.Run(() =>
            {
                var passwordHash = this.GeneratePasswordHash(password);

                var key = this.GenerateKey(passwordHash);

                var iv = this.GenerateIV(passwordHash);

                var Decrypt = _algorithm.CreateDecryptor(key, iv);

                return TransformBytes(Decrypt, bytesToDecypt);
            });
            await taskResult;
            return taskResult.Result;
        }

        private Rfc2898DeriveBytes GeneratePasswordHash(string password)
        {
            return new Rfc2898DeriveBytes(password, _salt);
        }

        private byte[] GenerateKey(Rfc2898DeriveBytes passwordHash)
        {
            return passwordHash.GetBytes(_algorithm.KeySize / 8);
        }

        private byte[] GenerateIV(Rfc2898DeriveBytes passwordHash)
        {
            return passwordHash.GetBytes(_algorithm.BlockSize / 8);
        }

        private byte[] TransformBytes(ICryptoTransform transformer, byte[] bytesToTransform)
        {
            byte[] result;
            using (var bufferStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(bufferStream, transformer, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytesToTransform, 0, bytesToTransform.Length);
                    cryptoStream.FlushFinalBlock();
                    result = bufferStream.ToArray();

                    cryptoStream.Close();
                }
                bufferStream.Close();
            }
            return result;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _algorithm.Dispose();
            }

        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        #region Asymmetric
        //Initialize the byte arrays to the public key information.  
        byte[] modulus = {214,46,220,83,160,73,40,39,201,155,19,202,3,11,191,178,56,
                            74,90,36,248,103,18,144,170,163,145,87,54,61,34,220,222,
                            207,137,149,173,14,92,120,206,222,158,28,40,24,30,16,175,
                            108,128,35,230,118,40,121,113,125,216,130,11,24,90,48,194,
                            240,105,44,76,34,57,249,228,125,80,38,9,136,29,117,207,139,
                            168,181,85,137,126,10,126,242,120,247,121,8,100,12,201,171,
                            38,226,193,180,190,117,177,87,143,242,213,11,44,180,113,93,
                            106,99,179,68,175,211,164,116,64,148,226,254,172,147};
        public Asymmetric AsymmetricEncrypt()
        {
            RSA rsa = rsaMakerAsym();

            Aes aes = Aes.Create();
            var encryptedSymmetricKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            var encryptedSymmetricIV = rsa.Encrypt(aes.IV, RSAEncryptionPadding.Pkcs1);
            return new Asymmetric(encryptedSymmetricKey, encryptedSymmetricIV);
        }

        public Asymmetric AsymmetricDecrypt(Asymmetric Encrypted)
        {
            RSA rsa = rsaMakerAsym();

            rsa.ExportParameters(true);
            var symmetricKey = rsa.Decrypt(Encrypted.EncryptedSymmetricKey, RSAEncryptionPadding.Pkcs1);
            var symmetrickIV = rsa.Decrypt(Encrypted.EncryptedSymmetricIV, RSAEncryptionPadding.Pkcs1);
            return new Asymmetric(symmetricKey, symmetrickIV);

        }
        public RSA rsaMakerAsym()
        {
            RSA rsa = RSA.Create();

            //use this
            X509Certificate2 certificate = LoadCertificate("MarketRound");

            RSAParameters rsaKeyInfo = new RSAParameters();
            rsaKeyInfo.Modulus = modulus;
            byte[] exponent = { 1, 0, 1 };
            rsaKeyInfo.Exponent = exponent;
            rsa.ImportParameters(rsaKeyInfo);
            return rsa;
        }
        public X509Certificate2 LoadCertificate(string certificateName)
        {
            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificate = my.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);
            return certificate[0];
        }
    }


    //makecert -n "CN=MarketRound" -a sha1 -pe -r -sr LocalMachine -ss MarketRoundCE -sky exchange


    public class Asymmetric
    {
        public Asymmetric(byte[] encryptedSymmetricKey, byte[] encryptedSymmetricIV)
        {
            EncryptedSymmetricKey = encryptedSymmetricKey;
            EncryptedSymmetricIV = encryptedSymmetricIV;
        }

        public byte[] EncryptedSymmetricKey { get; set; }
        public byte[] EncryptedSymmetricIV { get; set; }
    }
    #endregion
}
