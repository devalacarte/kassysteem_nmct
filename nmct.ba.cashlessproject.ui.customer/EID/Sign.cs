using Net.Sf.Pkcs11;
using Net.Sf.Pkcs11.Objects;
using Net.Sf.Pkcs11.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.ui.customer.EID
{
    class Sign
    {
        private Module m = null;
        private String mFileName;
        /// <summary>
        /// Default constructor. Will instantiate the beidpkcs11.dll pkcs11 module
        /// </summary>
        public Sign()
        {
            mFileName = "beidpkcs11.dll";
        }
        public Sign(String moduleFileName)
        {
            mFileName = moduleFileName;
        }
        /// <summary>
        /// Sign data with a named private key
        /// </summary>
        /// <param name="data">Data to be signed</param>
        /// <param name="privatekeylabel">Label for private key. Can be "Signature" or "Authentication"</param>
        /// <returns>Signed data.</returns>
        public byte[] DoSign(byte[] data, string privatekeylabel)
        {
            if (m == null)
            {
                // link with the pkcs11 DLL
                m = Module.GetInstance(mFileName);
            } //m.Initialize();

            byte[] encryptedData = null;
            try
            {
                Slot slot = m.GetSlotList(true)[0];
                Session session = slot.Token.OpenSession(true);
                ObjectClassAttribute classAttribute = new ObjectClassAttribute(CKO.PRIVATE_KEY);
                ByteArrayAttribute keyLabelAttribute = new ByteArrayAttribute(CKA.LABEL);
                keyLabelAttribute.Value = System.Text.Encoding.UTF8.GetBytes(privatekeylabel);

                session.FindObjectsInit(new P11Attribute[] {
                     classAttribute,
                     keyLabelAttribute
                    }
                );
                P11Object[] privatekeys = session.FindObjects(1) as P11Object[];
                session.FindObjectsFinal();

                if (privatekeys.Length >= 1)
                {
                    session.SignInit(new Mechanism(CKM.SHA1_RSA_PKCS), (PrivateKey)privatekeys[0]);
                    encryptedData = session.Sign(data);
                }

            }
            finally
            {
                m.Dispose();
            }
            return encryptedData;
        }

    }
}
