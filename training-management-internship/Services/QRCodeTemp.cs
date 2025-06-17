namespace training_management_internship.Services
{
    public class QRCodeTemp
    {
        public int QRCodeTempId { get; set; }
        public Guid Token { get; set; } 
        public DateTime CreatedAt { get; set; }
        public QRCodeType Type { get; set; } // Checkin | Checkout
        public int ChiTietLopId { get; set; }

        public enum QRCodeType
        {
            CheckIn,
            CheckOut
        }
    }

}
