using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        void IOrderHeaderRepository.UpdateStatus(int id, string orderStatus, string? paymentStatus)
        {
            var orderDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderDb != null)
            {
                orderDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus)) orderDb.PaymentStatus = paymentStatus;
            }
        }

        void IOrderHeaderRepository.UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var orderDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (!string.IsNullOrEmpty(sessionId)) orderDb.SessionId = sessionId;
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderDb.PaymentIntentId = paymentIntentId;
                orderDb.PaymentDate = DateTime.Now;
            }

        }
    }
}
