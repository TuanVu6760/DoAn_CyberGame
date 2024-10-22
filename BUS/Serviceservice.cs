using DAL.Entities; // Đảm bảo bạn đã thêm namespace chứa các thực thể của DAL
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BUS
{
    public class ServiceService
    {
        private readonly CyBerModel context; // Đối tượng DbContext của bạn
        private readonly MachineService machineService; // Tham chiếu đến MachineService

        public ServiceService()
        {
            context = new CyBerModel(); // Khởi tạo DbContext
            machineService= new MachineService(); // Khởi tạo MachineService
        }

        // Lấy tất cả món ăn
        public List<FoodItem> GetFoodItems()
        {
            return context.FoodItems.ToList(); // Sử dụng Entity Framework để lấy tất cả các món ăn
        }

        // Phương thức tạo đơn hàng
        public void CreateOrder(int sessionId, int foodItemId, int quantity, decimal price)
        {
            // Kiểm tra đầu vào có hợp lệ không
            if (sessionId <= 0 || foodItemId <= 0 || quantity <= 0 || price <= 0)
            {
                throw new ArgumentException("Các giá trị đầu vào không hợp lệ.");
            }

            // Kiểm tra sessionId có tồn tại không
            var sessionExists = context.MachineSessions.Any(s => s.SessionId == sessionId);
            if (!sessionExists)
            {
                throw new Exception("Session không tồn tại.");
            }

            // Kiểm tra foodItemId có tồn tại không
            var foodItemExists = context.FoodItems.Any(f => f.FoodItemId == foodItemId);
            if (!foodItemExists)
            {
                throw new Exception("Món ăn không tồn tại.");
            }

            // Tạo đơn hàng mới nếu tất cả các điều kiện đều hợp lệ
            var order = new Order
            {
                SessionId = sessionId,
                FoodItemId = foodItemId,
                Quantity = quantity,
                Price = price
            };

            context.Orders.Add(order); // Thêm đơn hàng mới vào DbContext
            context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }
        public List<string> GetFoodItemNamesBySessionId(int sessionId)
        {
            using (var context = new CyBerModel())
            {
                return context.Orders
                    .Where(o => o.SessionId == sessionId)
                    .Select(o => o.FoodItem.Name)
                    .ToList();
            }
        }
        public FoodItem GetFoodItemById(int foodItemId)
        {
            return context.FoodItems.FirstOrDefault(fi => fi.FoodItemId == foodItemId);
        }






        public void SaveOrder(Order order)
        {
            try
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết
                Console.WriteLine($"Lỗi khi lưu đơn hàng: {ex.Message}");
                throw; // Ném lại ngoại lệ để xử lý ở nơi gọi
            }
        }

        public List<Order> GetOrdersForMachine(int machineId)
        {
            // Lấy danh sách đơn hàng cho máy dựa vào machineId
            return context.Orders
                           .Where(o => o.MachineSession.MachineId == machineId)
                           .ToList();
        }

    }
}
