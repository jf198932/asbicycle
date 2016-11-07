using System;

namespace ASBicycle.WxPay.App
{
    [Serializable]
    public class SDKRuntimeException : Exception
    {

        private const long serialVersionUID = 1L;

        public SDKRuntimeException(String str) : base(str)
        {

        }
    }
}