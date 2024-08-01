using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ServiceShared.Models;

namespace TravelApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateDisLikeCommentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UpdateDisLikeCommentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("update-dislike")]
        public IActionResult UpdateDisLike(UpdateDislike updateDislike)
        {
            int rowsAffected = 0; // Số dòng bị ảnh hưởng

            // Chuỗi kết nối đến cơ sở dữ liệu
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Câu lệnh SQL để thực hiện truy vấn
            string query = "update SubmitComment set dislikeC = dislikeC + 1 where id = @id";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Tạo đối tượng SqlCommand với câu lệnh SQL và kết nối
                SqlCommand command = new SqlCommand(query, connection);
                // Thêm các tham số cho câu lệnh SQL
                command.Parameters.AddWithValue("@id", updateDislike.Id);
                try
                {
                    // Mở kết nối đến cơ sở dữ liệu
                    connection.Open();

                    // Thực thi câu lệnh SQL
                    rowsAffected = command.ExecuteNonQuery();

                    // Hiển thị số dòng bị ảnh hưởng (được cập nhật)
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có lỗi xảy ra
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return Ok(rowsAffected);
        }

        [HttpPost("update-dislike-decrement")]
        public IActionResult UpdateDisLikeDecrement(UpdateDislike updateDislike)
        {
            int rowsAffected = 0; // Số dòng bị ảnh hưởng

            // Chuỗi kết nối đến cơ sở dữ liệu
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Câu lệnh SQL để thực hiện truy vấn
            string query = "update SubmitComment set dislikeC = dislikeC - 1 where id = @id";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Tạo đối tượng SqlCommand với câu lệnh SQL và kết nối
                SqlCommand command = new SqlCommand(query, connection);
                // Thêm các tham số cho câu lệnh SQL
                command.Parameters.AddWithValue("@id", updateDislike.Id);
                try
                {
                    // Mở kết nối đến cơ sở dữ liệu
                    connection.Open();

                    // Thực thi câu lệnh SQL
                    rowsAffected = command.ExecuteNonQuery();

                    // Hiển thị số dòng bị ảnh hưởng (được cập nhật)
                    Console.WriteLine($"Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu có lỗi xảy ra
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return Ok(rowsAffected);
        }
    }
}
