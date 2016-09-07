using AcademyPlatform.Models.Base;

namespace AcademyPlatform.Models.Payments
{
    public class OnlineTransaction : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }

        public string TransactionId { get; set; }

        public OnlineTransactionResultType Result { get; set; }

        /// <summary>
        /// Status of the result on the payment server
        /// </summary>
        public string ResultPs { get; set; }

        public string ResultCode { get; set; }

        /// <summary>
        /// Result from 3D Secure authentication
        /// </summary>
        public string ThreeDSecure { get; set; }

        /// <summary>
        /// Reference to the card payment on the paymnet server
        /// </summary>
        public string Rrn { get; set; }

        /// <summary>
        /// Authorization code of the card payment 
        /// </summary>
        public string ApprovalCode { get; set; }

        /// <summary>
        /// Masked Credit Card Number
        /// </summary>
        public string CardNumber { get; set; }

        public int PaymentId { get; set; }

        public virtual Payment Payment { get; set; }

    }
}