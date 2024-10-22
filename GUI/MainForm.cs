

using BUS;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form
    {

        private UserService userService;
        private ZoneService zoneService;
        private MachineService machineService;
        private ServiceService serviceService;
        private User _currentUser;
        private List<Machine> _selectedMachines;
        private List<FoodItem> _selectedServices;
        private List<Order> orders = new List<Order>();

        private CyBerModel _context;

        public MainForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            userService = new UserService();
            zoneService = new ZoneService();
            machineService = new MachineService();
            serviceService = new ServiceService();
            _selectedMachines = new List<Machine>();
            _selectedServices = new List<FoodItem>(); // Khởi tạo danh sách món ăn đã chọn
            LoadZones();
            LoadServices();
            _context = new CyBerModel();
        
        }

        private void LoadZones()
        {
            try
            {
                var zones = zoneService.GetAllZones();
                if (zones == null || zones.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy zone nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                comboBoxZones.DataSource = zones;
                comboBoxZones.DisplayMember = "Name";
                comboBoxZones.ValueMember = "ZoneId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải các zone: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxZones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxZones.SelectedValue != null)
            {
                int selectedZoneId = (int)comboBoxZones.SelectedValue;
                LoadMachines(selectedZoneId);
            }
        }

        private void LoadMachines(int zoneId)
        {
            flpDatmay.Controls.Clear();
            _selectedMachines.Clear();
            try
            {
                var machines = machineService.GetMachinesByZone(zoneId);
                if (machines.Count == 0)
                {
                    MessageBox.Show("Không có máy nào trong zone này.");
                    return;
                }

                foreach (var machine in machines)
                {
                    Button machineButton = CreateMachineButton(machine);
                    flpDatmay.Controls.Add(machineButton);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải máy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Button CreateMachineButton(Machine machine)
        {
            Button machineButton = new Button
            {
                Text = $"{machine.Name} - {machine.Status}",
                Tag = machine,
                Width = 150,
                Height = 100,
                BackColor = machine.Status == "Online" ? Color.LightGreen : Color.Gray
            };

            machineButton.Click += (s, e) => ToggleMachineSelection(machine, machineButton);

            return machineButton;
        }

        private void ToggleMachineSelection(Machine machine, Button machineButton)
        {
            if (_selectedMachines.Contains(machine))
            {
                _selectedMachines.Remove(machine);
                machineButton.BackColor = machine.Status == "Online" ? Color.LightGreen : Color.Gray;
                lblMachineInfo.Text = "Chọn một máy để xem thông tin."; // Reset khi bỏ chọn
                ClearListView();
            }
            else
            {
                _selectedMachines.Add(machine);
                machineButton.BackColor = Color.Red;

                // Cập nhật thông tin máy
                UpdateMachineInfo(machine);  // Gọi cập nhật thông tin máy
                LoadSelectedOrdersForMachine(machine);



            }
        }





        private void UpdateMachineInfo(Machine machine)
        {
            if (_selectedMachines.Contains(machine))
            {
                int sessionId = machineService.GetCurrentSessionIdForMachine(machine.MachineId);
                if (sessionId > 0)
                {
                    // Lấy thông tin phiên hoạt động
                    var session = machineService.GetSessionBy(sessionId);

                    if (session != null)
                    {
                        // Tính toán thời gian chơi và số tiền phải thanh toán
                        TimeSpan duration = DateTime.Now - session.StartTime;
                        decimal totalAmount = (decimal)duration.TotalHours * machine.PricePerHour;

                        lblMachineInfo.Text = $"Tên máy: {machine.Name}\n" +
                                              $"Trạng thái: {machine.Status}\n" +
                                              $"ID máy: {machine.MachineId}\n" +
                                              $"Giá: {machine.PricePerHour:C}\n" +
                                              $"Thời gian bắt đầu: {session.StartTime.ToString("HH:mm:ss dd/MM/yyyy")}\n" +
                                              $"Thời gian chơi: {duration.TotalHours:F2} giờ\n" +
                                              $"Tổng tiền phải thanh toán: {totalAmount:C}";
                    }
                }
                else
                {
                    lblMachineInfo.Text = $"Tên máy: {machine.Name}\n" +
                                          $"Trạng thái: {machine.Status}\n" +
                                          $"ID máy: {machine.MachineId}\n" +
                                          $"Giá: {machine.PricePerHour:C}\n" +
                                          "Chưa có phiên hoạt động.";
                }
            }
            else
            {
                lblMachineInfo.Text = "Chọn một máy để xem thông tin.";
            }
        }



        private void ToggleMachineStatus(Machine machine)
        {
            // Kiểm tra trạng thái máy
            if (machine.Status == "Online")
            {
                // Tắt máy
                machine.Status = "Offline";

                // Tìm phiên máy hiện tại
                var currentSession = _context.MachineSessions.FirstOrDefault(s => s.MachineId == machine.MachineId && s.EndTime == null);
                if (currentSession != null)
                {
                    // Cập nhật thời gian kết thúc
                    currentSession.EndTime = DateTime.Now;

                    // Lấy giá tiền của máy
                    var machinePricePerHour = machine.PricePerHour; // Giả sử Machine có thuộc tính PricePerHour lưu giá theo giờ

                    // Tính thời gian sử dụng (giờ)
                    var duration = (currentSession.EndTime.Value - currentSession.StartTime).TotalHours;

                    // Tính tổng tiền (có thể làm tròn thời gian)
                    currentSession.TotalAmount = (decimal)duration * machinePricePerHour;


                    // Cập nhật phiên làm việc vào database
                    _context.SaveChanges();
                }
            }
            else
            {
                // Bật máy
                machine.Status = "Online";

                // Tạo một phiên làm việc mới cho máy
                var newSession = new MachineSession
                {
                    MachineId = machine.MachineId,
                    StartTime = DateTime.Now,
                    TotalAmount = 0 // Khởi tạo tổng tiền là 0
                };

                _context.MachineSessions.Add(newSession);
                _context.SaveChanges();
            }

            // Cập nhật trạng thái máy vào database
            _context.SaveChanges();
        }



        private void EndMachineSession(Machine machine)
        {
            // Lấy phiên hiện tại của máy
            int sessionId = machineService.GetCurrentSessionIdForMachine(machine.MachineId);
            if (sessionId > 0)
            {
                var session = machineService.GetSessionBy(sessionId);
                if (session != null)
                {
                    // Kết thúc phiên và cập nhật trạng thái máy
                    session.EndTime = DateTime.Now; // Cập nhật thời gian kết thúc
                    session.TotalAmount = 0; // Đặt lại tổng tiền về 0

                    // Cập nhật trạng thái máy về offline
                    machineService.UpdateMachineStatus(machine.MachineId, "Offline");

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();
                }
            }
        }




        private void StartNewMachineSession(Machine machine)
        {
            var session = new MachineSession
            {
                MachineId = machine.MachineId,
                StartTime = DateTime.Now,
                EndTime = null,
                TotalAmount = 0
            };

            // Lưu phiên mới vào cơ sở dữ liệu
            machineService.StartNewSession(session);
        }




        private void btnDatmay_Click(object sender, EventArgs e)
        {
            if (_selectedMachines.Count > 0)
            {
                try
                {
                    List<string> offlineMachines = new List<string>();

                    foreach (var machine in _selectedMachines)
                    {
                        if (machine.Status == "Offline") // Nếu máy offline thì bật máy
                        {
                            machineService.TurnOnMachine(machine.MachineId); // Bật máy
                            machine.Status = "Online"; // Cập nhật trạng thái máy
                            StartNewMachineSession(machine); // Bắt đầu phiên hoạt động mới
                            offlineMachines.Add(machine.Name); // Thêm vào danh sách máy đã bật
                        }
                        else if (machine.Status == "Online")
                        {
                            // Nếu máy đã online, chỉ cập nhật thông tin.
                            UpdateMachineInfo(machine);
                        }
                    }

                    LoadMachines((int)comboBoxZones.SelectedValue); // Tải lại danh sách máy sau khi cập nhật

                    // Hiển thị thông báo nếu có máy đã được bật
                    if (offlineMachines.Count > 0)
                    {
                        MessageBox.Show($"Các máy sau đã được bật: {string.Join(", ", offlineMachines)}", "Thông báo", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi thao tác với máy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một máy để đặt.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void LoadServices()
        {
            flpService.Controls.Clear();
            try
            {
                var services = serviceService.GetFoodItems();
                if (services.Count == 0)
                {
                    MessageBox.Show("Không có dịch vụ nào.");
                    return;
                }

                foreach (var service in services)
                {
                    FlowLayoutPanel servicePanel = new FlowLayoutPanel
                    {
                        Width = 150,
                        Height = 280, // Tăng chiều cao để thêm số lượng
                        FlowDirection = FlowDirection.TopDown,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(5),
                        AutoSize = true
                    };

                    Button serviceButton = CreateServiceButton(service);
                    serviceButton.Text = "";
                    servicePanel.Controls.Add(serviceButton);

                    Label serviceNameLabel = new Label
                    {
                        Text = service.Name,
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Padding = new Padding(0, 5, 0, 0)
                    };
                    servicePanel.Controls.Add(serviceNameLabel);

                    Label servicePriceLabel = new Label
                    {
                        Text = $"Giá: {service.Price.ToString("C")}",
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Padding = new Padding(0, 0, 0, 5)
                    };
                    servicePanel.Controls.Add(servicePriceLabel);

                    // Thêm NumericUpDown để chọn số lượng
                    NumericUpDown quantitySelector = new NumericUpDown
                    {
                        Minimum = 1,
                        Maximum = 100,
                        Value = 1, // Mặc định số lượng là 1
                        Width = 140,
                        Tag = service // Đính kèm đối tượng dịch vụ
                    };
                    quantitySelector.ValueChanged += (s, e) => UpdateSelectedItemsAndTotal();
                    servicePanel.Controls.Add(quantitySelector);

                    // Thêm vào flow layout
                    flpService.Controls.Add(servicePanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Button CreateServiceButton(FoodItem service)
        {
            Button serviceButton = new Button
            {
                Width = 140,
                Height = 140,
                Tag = service,
                BackgroundImageLayout = ImageLayout.Zoom
            };

            string fullImagePath = GetFullImagePath(service.ImagePath);
            if (IsImagePathValid(fullImagePath))
            {
                try
                {
                    serviceButton.BackgroundImage = Image.FromFile(fullImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi khi tải ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            serviceButton.Click += (s, e) => ToggleServiceSelection(service, serviceButton);

            return serviceButton;
        }

        private void ToggleServiceSelection(FoodItem service, Button serviceButton)
        {
            if (_selectedServices.Contains(service))
            {
                _selectedServices.Remove(service);
                serviceButton.BackColor = Color.LightGray;
            }
            else
            {
                _selectedServices.Add(service);
                serviceButton.BackColor = Color.LightBlue;
            }

            // Cập nhật danh sách và tổng tiền
            UpdateSelectedItemsAndTotal();

        }


        private void UpdateSelectedItemsAndTotal()
        {
            // Xóa danh sách hiện tại
            lsbDichvu.Items.Clear();

            // Cập nhật danh sách các món đã chọn
            decimal totalAmount = 0;
            foreach (FlowLayoutPanel servicePanel in flpService.Controls)
            {
                var service = (FoodItem)servicePanel.Controls.OfType<Button>().FirstOrDefault()?.Tag;
                var quantitySelector = servicePanel.Controls.OfType<NumericUpDown>().FirstOrDefault();

                if (service != null && quantitySelector != null && _selectedServices.Contains(service))
                {
                    int quantity = (int)quantitySelector.Value;
                    lsbDichvu.Items.Add($"{service.Name} - {service.Price.ToString("C")} x {quantity}");

                    totalAmount += service.Price * quantity;
                }
            }

            // Hiển thị tổng tiền
            lblTotalAmount.Text = $"Tổng tiền: {totalAmount.ToString("C")}";
        }

        private bool IsImagePathValid(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                return false;

            string extension = Path.GetExtension(imagePath).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
        }

        private string GetFullImagePath(string imagePath)
        {
            if (imagePath.Contains("Images"))
            {
                return Path.Combine(Application.StartupPath, imagePath);
            }
            return imagePath;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (_selectedMachines.Count > 0) // Kiểm tra xem có máy nào đã chọn không
            {
                try
                {
                    List<string> offlineMachines = new List<string>();
                    List<string> orderedMachines = new List<string>();

                    foreach (var machine in _selectedMachines)
                    {
                        if (machine.Status != "Online")
                        {
                            offlineMachines.Add(machine.Name);
                        }
                    }

                    if (offlineMachines.Count > 0)
                    {
                        MessageBox.Show($"Máy(s): {string.Join(", ", offlineMachines)} không online, không thể đặt hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Lặp qua tất cả các máy đã chọn
                    foreach (var machine in _selectedMachines)
                    {
                        if (machine.Status == "Online")
                        {
                            int sessionId = machineService.GetCurrentSessionIdForMachine(machine.MachineId);

                            if (sessionId <= 0)
                            {
                                MessageBox.Show($"Máy {machine.Name} không có phiên hoạt động hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue;
                            }

                            // Lặp qua tất cả các FlowLayoutPanel trong flpService
                            foreach (FlowLayoutPanel servicePanel in flpService.Controls)
                            {
                                var service = (FoodItem)servicePanel.Controls.OfType<Button>().FirstOrDefault()?.Tag;
                                var quantitySelector = servicePanel.Controls.OfType<NumericUpDown>().FirstOrDefault();

                                if (service != null && quantitySelector != null && _selectedServices.Contains(service))
                                {
                                    int quantity = (int)quantitySelector.Value;
                                    // Tạo đối tượng đơn hàng
                                        Order order = new Order
                                        {
                                            SessionId = sessionId,
                                            FoodItemId = service.FoodItemId,
                                            Quantity = quantity,
                                            Price = service.Price
                                        };

                                        try
                                        {
                                            // Lưu đơn hàng
                                            serviceService.SaveOrder(order);
                                            orderedMachines.Add(machine.Name); // Thêm máy đã đặt
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show($"Có lỗi xảy ra khi đặt hàng cho máy {machine.Name}: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                    if (orderedMachines.Count > 0)
                    {
                        MessageBox.Show($"Đặt hàng thành công cho máy(s): {string.Join(", ", orderedMachines)}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một máy trước khi đặt hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnThanhtoan_Click(object sender, EventArgs e)
        {
            if (_selectedMachines.Count > 0)
            {
                decimal serviceFee = 10.00m; // Hoặc lấy từ thông tin phí dịch vụ nhập vào

                foreach (var machine in _selectedMachines)
                {
                    HandlePayment(machine, serviceFee);
                }

                // Xóa danh sách đơn hàng cũ
                ClearListView();

                // Tải lại danh sách đơn hàng cho từng máy
                foreach (var machine in _selectedMachines)
                {
                    LoadSelectedOrdersForMachine(machine);
                }

                // Tải lại danh sách máy sau khi thanh toán
                LoadMachines((int)comboBoxZones.SelectedValue); // Giả sử bạn đang dùng ComboBox để chọn vùng
            }
            else
            {
                MessageBox.Show("Vui lòng chọn máy để thanh toán.", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void ClearListView()
        {
            lsbOrders.Items.Clear(); // Xóa tất cả các mục trong ListBox
        }



        private void HandlePayment(Machine machine, decimal serviceFee)
        {
            if (machine == null)
            {
                MessageBox.Show("Máy không hợp lệ.", "Lỗi", MessageBoxButtons.OK);
                return; // Dừng thực hiện nếu máy không hợp lệ
            }

            int sessionId = machineService.GetCurrentSessionIdForMachine(machine.MachineId);
            if (sessionId <= 0)
            {
                MessageBox.Show("Không có phiên hoạt động cho máy này.", "Lỗi", MessageBoxButtons.OK);
                return; // Dừng thực hiện nếu không có phiên
            }

            var session = machineService.GetSessionBy(sessionId);
            if (session == null)
            {
                MessageBox.Show("Phiên hoạt động không tồn tại.", "Lỗi", MessageBoxButtons.OK);
                return; // Dừng thực hiện nếu phiên không tồn tại
            }

            // Tính toán thời gian chơi và tổng tiền máy
            TimeSpan duration = DateTime.Now - session.StartTime;
            decimal totalAmountForMachine = (decimal)duration.TotalHours * machine.PricePerHour;

            // Truy vấn danh sách món ăn đã đặt trong phiên này
            var orders = _context.Orders.Where(o => o.SessionId == sessionId).ToList();
            decimal totalFoodAmount = 0;

            // Tính tổng tiền các món ăn đã đặt
            foreach (var order in orders)
            {
                totalFoodAmount += order.Quantity * order.Price;
            }

            // Tính toán tổng tiền thanh toán (tiền máy + tiền món ăn + phí dịch vụ)
            decimal totalPayment = totalAmountForMachine + totalFoodAmount + serviceFee;

            // Cập nhật tổng tiền vào phiên
            session.TotalAmount = totalPayment;
            _context.SaveChanges(); // Lưu vào database

            // Hiển thị thông tin thanh toán
            MessageBox.Show($"Tổng tiền thanh toán cho máy {machine.Name} là: {totalPayment:C} (bao gồm {totalFoodAmount:C} cho dịch vụ món ăn).", "Thông báo", MessageBoxButtons.OK);

            // Kết thúc phiên
            EndMachineSession(machine);

            // Cập nhật lại trạng thái máy và thông tin phiên
            UpdateMachineInfo(machine); // Gọi hàm để cập nhật thông tin
        }


        private void LoadSelectedOrdersForMachine(Machine machine)
        {
            if (machine == null)
            {
                MessageBox.Show("Máy không hợp lệ.", "Lỗi", MessageBoxButtons.OK);
                return;
            }

            // Xóa danh sách hiện tại trong ListBox
            lsbOrders.Items.Clear();

            try
            {
                // Gọi phương thức để lấy danh sách đơn hàng từ cơ sở dữ liệu
                var ordersFromDb = serviceService.GetOrdersForMachine(machine.MachineId);

                if (ordersFromDb == null || !ordersFromDb.Any())
                {
                    MessageBox.Show("Không có đơn hàng nào cho máy này.", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                // Hiển thị danh sách món ăn đã đặt trong ListBox
                foreach (var order in ordersFromDb)
                {
                    string orderDetails = $"{order.FoodItem.Name} - Số lượng: {order.Quantity} - Tổng tiền: {order.Price * order.Quantity:C}";
                    lsbOrders.Items.Add(orderDetails); // Thêm từng đơn hàng vào ListBox
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi tải đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }



}

