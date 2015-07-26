using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.employee.EID.tests
{
    [TestFixture]
    public class SignTests
    {
        [Test]
        public void SignAuthentication()
        {
            // Sign
            Sign signTest = new Sign("beidpkcs11.dll");
            byte[] testdata = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] signeddata = signTest.DoSign(testdata, "Authentication");
            Assert.IsNotNull(signeddata);
            // Verification
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            Assert.True(integrityTest.Verify(testdata, signeddata,
                dataTest.GetCertificateAuthenticationFile()));
            //Assert.False(integrityTest.Verify(testdata, signeddata,
            //    dataTest.GetCertificateSignatureFile()));

        }

        [Test]
        public void SignSignature()
        {
            // Sign
            Sign signTest = new Sign("beidpkcs11.dll");
            byte[] testdata = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] signeddata = signTest.DoSign(testdata, "Signature");
            Assert.IsNotNull(signeddata);
            // Verification
            ReadData dataTest = new ReadData("beidpkcs11.dll");
            Integrity integrityTest = new Integrity();
            Assert.False(integrityTest.Verify(testdata, signeddata,
                dataTest.GetCertificateAuthenticationFile()));
            Assert.True(integrityTest.Verify(testdata, signeddata,
                dataTest.GetCertificateSignatureFile()));

        }

    }

}

