using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ServiceShared.Models;

namespace TravelApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CommentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("insert-data-reload")]
        public IActionResult InsertDataReload(InsertCommentRequestModel input)
        {
            int rowsAffected = 0; // Số dòng bị ảnh hưởng
            int newCommentId = 0; // Id của bình luận mới được thêm

            // Chuỗi kết nối đến cơ sở dữ liệu
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Câu lệnh SQL để thực hiện truy vấn. Sử dụng OUTPUT INSERTED.Id để lấy ra id vừa thêm vào.
            string query = "INSERT INTO SubmitComment(name, email, content, time) OUTPUT INSERTED.Id VALUES(@name, @email, @content, GETDATE())";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Tạo đối tượng SqlCommand với câu lệnh SQL và kết nối
                SqlCommand command = new SqlCommand(query, connection);
                // Thêm các tham số cho câu lệnh SQL
                command.Parameters.AddWithValue("@name", input.Name);
                command.Parameters.AddWithValue("@content", input.Content);
                command.Parameters.AddWithValue("@email", input.Email);

                try
                {
                    // Mở kết nối đến cơ sở dữ liệu
                    connection.Open();

                    // Thực thi câu lệnh SQL và lấy Id của bình luận mới được thêm
                    newCommentId = (int)command.ExecuteScalar();

                    // Hiển thị số dòng bị ảnh hưởng (được cập nhật)
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có lỗi xảy ra
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            // Tạo đối tượng phản hồi chứa thông tin bình luận mới
            var response = new
            {
                success = true,
                comment = new
                {
                    Id = newCommentId,
                    Name = input.Name,
                    Email = input.Email,
                    Content = input.Content,
                    Time = DateTime.Now,
                    ParentId = 0,
                    LikeC = 0,
                    DislikeC = 0
                }
            };
            return Ok(response);
        }


        [HttpPost("insert-feedback-reload")]
        public IActionResult InsertFeedbackReload(InsertFeedbackRequestModel input)
        {
            int rowsAffected = 0; // Số dòng bị ảnh hưởng
            int newCommentId = 0; // Id của bình luận mới được thêm

            // Chuỗi kết nối đến cơ sở dữ liệu
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Câu lệnh SQL để thực hiện truy vấn. Sử dụng OUTPUT INSERTED.Id để lấy ra id vừa thêm vào.
            string query = "INSERT INTO SubmitComment(name, email, content, time, parentID) OUTPUT INSERTED.Id VALUES(@name, @email, @content, GETDATE(), @parentId)";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Tạo đối tượng SqlCommand với câu lệnh SQL và kết nối
                SqlCommand command = new SqlCommand(query, connection);
                // Thêm các tham số cho câu lệnh SQL
                command.Parameters.AddWithValue("@parentId", input.ParentId);
                command.Parameters.AddWithValue("@name", input.Name);
                command.Parameters.AddWithValue("@content", input.Content);
                command.Parameters.AddWithValue("@email", input.Email);

                try
                {
                    // Mở kết nối đến cơ sở dữ liệu
                    connection.Open();

                    // Thực thi câu lệnh SQL và lấy Id của bình luận mới được thêm
                    newCommentId = (int)command.ExecuteScalar();

                    // Hiển thị số dòng bị ảnh hưởng (được cập nhật)
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có lỗi xảy ra
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            // Tạo đối tượng phản hồi chứa thông tin bình luận mới
            var response = new
            {
                success = true,
                comment = new
                {
                    Id = newCommentId,
                    Name = input.Name,
                    Email = input.Email,
                    Content = input.Content,
                    Time = DateTime.Now,
                    ParentId = input.ParentId,
                    LikeC = 0,
                    DislikeC = 0
                }
            };
            return Ok(response);
        }
    }
}
