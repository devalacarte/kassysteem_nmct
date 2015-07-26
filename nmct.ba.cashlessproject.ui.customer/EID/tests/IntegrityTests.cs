using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.customer.EID.tests
{
    [TestFixture]
    public class IntegrityTests
    {
        [Test]
        public void IntegrityFails()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            byte[] idFile = dataTest.GetIdFile();
            byte[] idSignatureFile = dataTest.GetIdSignatureFile();
            byte[] certificateRRN = null;
            Assert.False(integrityTest.Verify(idFile, idSignatureFile, certificateRRN));
        }
        [Test]
        public void IntegrityIdentityFile()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            byte[] idFile = dataTest.GetIdFile();
            byte[] idSignatureFile = dataTest.GetIdSignatureFile();
            byte[] certificateRRN = dataTest.GetCertificateRNFile();
            Assert.True(integrityTest.Verify(idFile, idSignatureFile, certificateRRN));
        }
        [Test]
        public void IntegrityIdentityFileWrongSignature()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            byte[] idFile = dataTest.GetIdFile();
            byte[] idSignatureFile = dataTest.GetAddressSignatureFile();
            byte[] certificateRRN = dataTest.GetCertificateRNFile();
            Assert.False(integrityTest.Verify(idFile, idSignatureFile, certificateRRN));
        }
        [Test]
        public void IntegrityIdentityFileWrongCertificate()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            byte[] idFile = dataTest.GetIdFile();
            byte[] idSignatureFile = dataTest.GetIdSignatureFile();
            byte[] certificateRoot = dataTest.GetCertificateRootFile();
            Assert.False(integrityTest.Verify(idFile, idSignatureFile, certificateRoot));
        }
        [Test]
        public void IntegrityAddressFile()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            byte[] addressFile = trimRight(dataTest.GetAddressFile());
            byte[] idSignatureFile = dataTest.GetIdSignatureFile();
            byte[] concatFiles = new byte[addressFile.Length + idSignatureFile.Length];
            Array.Copy(addressFile, 0, concatFiles, 0, addressFile.Length);
            Array.Copy(idSignatureFile, 0, concatFiles, addressFile.Length, idSignatureFile.Length);
            byte[] addressSignatureFile = dataTest.GetAddressSignatureFile();
            byte[] certificateRRN = dataTest.GetCertificateRNFile();
            Assert.True(integrityTest.Verify(concatFiles, addressSignatureFile, certificateRRN));
        }
        private byte[] trimRight(byte[] addressFile)
        {
            int idx;
            for (idx = 0; idx < addressFile.Length; idx++)
            {
                if (0 == addressFile[idx])
                {
                    break;
                }
            }
            byte[] result = new byte[idx];
            Array.Copy(addressFile, 0, result, 0, idx);
            return result;
        }

    }

}
