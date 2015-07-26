using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.employee.EID.tests
{
    [TestFixture]
    public class CertificateValidityTests
    {
        [Test]
        public void ValidityAuthenticationChain()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            List<byte[]> caCerts = new List<byte[]>();
            caCerts.Add(dataTest.GetCertificateCAFile());
            Assert.True(integrityTest.CheckCertificateChain(
                caCerts,
                dataTest.GetCertificateAuthenticationFile()));
        }
        [Test]
        public void ValiditySignatureChain()
        {
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            List<byte[]> caCerts = new List<byte[]>();
            caCerts.Add(dataTest.GetCertificateCAFile());

            Assert.True(integrityTest.CheckCertificateChain(
                caCerts,
                dataTest.GetCertificateSignatureFile()));
        }
    }

}