using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.customer.EID
{
    /// Example Integrity checking class
    /** Some examples on how to verify certificates and signatures
     */
    class Integrity
    {
        public Integrity()
        {
        }
        /// <summary>
        /// Verify a signature with a given certificate. It is assumed that
        /// the signature is made from a SHA1 hash of the data.
        /// </summary>
        /// <param name="data">Signed data</param>
        /// <param name="signature">Signature to be verified</param>
        /// <param name="certificate">Certificate containing the public key used to verify the code</param>
        /// <returns>True if the verification succeeds</returns>
        public bool Verify(byte[] data, byte[] signature, byte[] certificate) 
        {
            try
            {
                X509Certificate2 x509Certificate;

                // create certificate object from byte 'file' 
                x509Certificate = new X509Certificate2(certificate);

                // use public key from certificate during verification
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;

                // verify signature. assume that the data was SHA1 hashed.
                return rsa.VerifyData(data,"SHA1",signature);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        }
       
        /// <summary>
        /// Check a certificate chain. In order to trust the certficate, the root certificate must be in
        /// stored in the trusted root certificates store. An online CRL check of the chain will be carried out.
        /// </summary>
        /// <param name="CACertificates">CA certificates</param>
        /// <param name="leafCertificate">The certificate whose chain will be checked</param>
        /// <returns>True if the certificate is trusted according the system settings</returns>
        public bool CheckCertificateChain(List <byte[]> CACertificates, byte[] leafCertificate)
        {
            X509Chain chain = new X509Chain();
            // check CRL of certificates online
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

            // add intermediate CA certificates in order to build the correct chain
            foreach (byte[] CACert in CACertificates)
                chain.ChainPolicy.ExtraStore.Add(new X509Certificate2(CACert));

            // do chain validation
            bool chainIsValid = chain.Build(new X509Certificate2(leafCertificate));

            // write some more information if anything went wrong
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                Console.WriteLine("Chain status: " + chain.ChainStatus[i].Status 
                    + " (" + chain.ChainStatus[i].StatusInformation + ")");
            }
            return chainIsValid;
        }
   
    }
}
