using Newtonsoft.Json;
using ServiceShared.Models;

namespace WApplication.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            if (string.IsNullOrEmpty(_configuration?.GetSection("Travel:Host")?.Value?.ToString()))
            {
                throw new ArgumentException("Travel Host is null");
            }
            _httpClient.BaseAddress = new Uri(_configuration?.GetSection("Travel:Host")?.Value?.ToString());
        }
        public async Task<string> PostExternalDataAsync(string url, HttpContent content)
        {
            var response = await _httpClient.PostAsync(url, content); // Gửi một yêu cầu HTTP POST đến URL được cung cấp với nội dung được cung cấp và đợi phản hồi.
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(); // Đọc nội dung của phản hồi dưới dạng chuỗi và trả về nó.

        }
        public async Task<List<GetCommentDB>> CallApiGetAllComment(string url)
        {
            var response = await _httpClient.GetAsync(url); // Gửi yêu cầu GET đến URL
            response.EnsureSuccessStatusCode(); // Đảm bảo yêu cầu thành công

            // Đọc nội dung phản hồi dưới dạng chuỗi JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Chuyển đổi chuỗi JSON thành danh sách các đối tượng GetCommentDB
            var comments = JsonConvert.DeserializeObject<List<GetCommentDB>>(jsonResponse);

            return comments; // Trả về danh sách các comment
        }
    }
}
