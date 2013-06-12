using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Core.Security
{
        public class RSAClass
        {

            // Generated From http://www.opinionatedgeek.com/DotNet/Snippets/Crypto/keygen.aspx
            private static string _privateKey = "<RSAKeyValue><Modulus>om+EbDXJSJ+Q+bvIg55eXrFxPYkFDYC4s07E9XtSqHigqP+LqduS1w0M2zAeN+YCamlOwiNO4kQmarPUWVZc4eAZ+UmKZoV6ZHOUSAzkcZOq9r9HLRuuzamaEz3b+B/OKhlDbLPPkx82YPIVPrykayb6EW00Yz9oyS7UW0OEZfE=</Modulus><Exponent>AQAB</Exponent><P>1nndeCP4BL/Ro+8hpPAzCQxUQ/mt+K5cR56GN4pdlri8k3NfEkbRwi5Aorn0hxA0uxdWY2RzRSDnHgRXzlGtkQ==</P><Q>weJbwSA8cff887q6s6gftySfi4qebqeKKKUVHxpZSh1z6gjH2TvcaprBVl5pot+U+zJi3ovqHXncSf4p9o6CYQ==</Q><DP>u+pohlQVgavDdbwWoVonjRz1U92WdVXn2oRlmpIr79wKtbKXMP9F17oIvcrqGdC0Mtx+v3UKnh33AdDSUh40UQ==</DP><DQ>BBFauXeSrRV14is1xZUubSpAiq3y2wF63ZVxFwD3hJ2PlAZxIvaljjsG+WfrsXYdaxPIjUDEPbAT1h2WBc1BoQ==</DQ><InverseQ>k5pmh2AnrcV/aMBPxbMzp03SPykyjtFEmPFjO6zbOHi4386bK3gzsuqBI5M61N38BinpIr8/8Sv0y9K/HmQX7A==</InverseQ><D>LSJTZ0Xisf00aU/WPUwp6KHhVznVX5UevSPr8lHb+9fYvFyVUBMk0ABeTCEopFuS3EJ6kdMBvjk1e3yQSmqG0ALfJJjhCFO1mbJLhrXIWOBmDJ7WRV2XPlheQF7MqaJ3gLHaQveO9iauYS9KBJR/AESxmy4abo6V6GEAOjDILAE=</D></RSAKeyValue>";
            private static string _publicKey = "<RSAKeyValue><Modulus>om+EbDXJSJ+Q+bvIg55eXrFxPYkFDYC4s07E9XtSqHigqP+LqduS1w0M2zAeN+YCamlOwiNO4kQmarPUWVZc4eAZ+UmKZoV6ZHOUSAzkcZOq9r9HLRuuzamaEz3b+B/OKhlDbLPPkx82YPIVPrykayb6EW00Yz9oyS7UW0OEZfE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            private static UnicodeEncoding _encoder = new UnicodeEncoding();

            public static string Decrypt(string data)
            {
                var rsa = new RSACryptoServiceProvider();
                var dataArray = data.Split(new char[] { ',' });
                byte[] dataByte = new byte[dataArray.Length];
                for (int i = 0; i < dataArray.Length; i++)
                {
                    dataByte[i] = Convert.ToByte(dataArray[i]);
                }

                rsa.FromXmlString(_privateKey);
                var decryptedByte = rsa.Decrypt(dataByte, false);
                return _encoder.GetString(decryptedByte);
            }

            public static string Encrypt(string data)
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(_publicKey);
                var dataToEncrypt = _encoder.GetBytes(data);
                var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
                var length = encryptedByteArray.Count();
                var item = 0;
                var sb = new StringBuilder();
                foreach (var x in encryptedByteArray)
                {
                    item++;
                    sb.Append(x);

                    if (item < length)
                        sb.Append(",");
                }

                return sb.ToString();
            }

        }
    }
