using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceShared.Models;

namespace TravelApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetAllCommentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment env;
        public GetAllCommentController(IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            this.env = env;
        }

        [HttpGet]
        public IActionResult getAllComment()
        {
            string envvvv = env.EnvironmentName;
            var constss = _configuration.GetSection("test").Value;
            // Tạo danh sách để chứa các Comment được lấy từ database
            List<GetCommentDB> comments = new List<GetCommentDB>();

            // Chuỗi kết nối đến cơ sở dữ liệu
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Câu lệnh SQL để thực hiện truy vấn
            string query = "select * from SubmitComment";

            // Tạo kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối đến cơ sở dữ liệu
                connection.Open();

                // Tạo đối tượng SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thực thi truy vấn và lấy SqlDataReader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Đọc kết quả truy vấn
                        while (reader.Read())
                        {
                            // Tạo đối tượng GetCommentDb với các thông tin duyệt được
                            GetCommentDB cmt = new GetCommentDB {
                                Id = Int32.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Content = reader["Content"].ToString(),
                                Time = DateTime.Parse(reader["Time"].ToString()),
                                ParentID = Int32.Parse(reader["ParentID"].ToString()),
                                LikeC = Int32.Parse(reader["LikeC"].ToString()),
                                DislikeC = Int32.Parse(reader["DislikeC"].ToString()),
                            };
                            // Add vào list comments
                            comments.Add(cmt);
                        }
                    }
                }
            }
            return Ok(comments);
        }


    }
}
