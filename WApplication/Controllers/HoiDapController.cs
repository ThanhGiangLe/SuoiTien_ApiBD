using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceShared.Const;
using ServiceShared.Models;
using System.Text;
using WApplication.Services;

namespace WebApi.Controllers
{
    public class HoiDapController : Controller
    {
        private readonly ApiService _apiService;
        public HoiDapController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await getAllComment();
            return View(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CallCommentInsertReload(InsertCommentRequestModel input)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(CommentConst.urlInsertCommentReload, content);

                // Chuyển đổi data(string) sang đối tượng response(Object) có kiểu dữ liệu ResponseModel được chỉ định
                var response = JsonConvert.DeserializeObject<ResponseModel>(data);

                if (response != null && response.Success)
                {
                    // Một đối tượng response gồm có Success và Comment. Lấy ra comment
                    var comment = response.Comment;

                    // Trả về đối tượng JSON với thông tin bình luận
                    return Ok(new
                    {
                        success = true,
                        comment = new
                        {
                            Id = comment.Id,
                            Name = comment.Name,
                            Email = comment.Email,
                            Content = comment.Content,
                            Time = comment.Time,
                            ParentId = comment.ParentID,
                            LikeC = comment.LikeC,
                            DislikeC = comment.DislikeC
                        }
                    });
                }

                return Ok(new { success = false, message = "Không thể thêm bình luận" });
            }
            catch (HttpRequestException ex)
            {
                return Ok(new { success = false, error = ex.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CallFeedbackInsertReload(InsertFeedbackRequestModel input)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(CommentConst.urlInsertFeedbackReload, content);

                // Chuyển đổi data(string) sang đối tượng Object được chỉ định
                var response = JsonConvert.DeserializeObject<ResponseModel>(data);

                if (response != null && response.Success)
                {
                    // Một đối tượng response gồm có Success và Comment. Lấy ra comment
                    var comment = response.Comment;

                    // Trả về đối tượng JSON với thông tin bình luận
                    return Ok(new
                    {
                        success = true,
                        comment = new
                        {
                            Id = comment.Id,
                            Name = comment.Name,
                            Email = comment.Email,
                            Content = comment.Content,
                            Time = comment.Time,
                            ParentId = comment.ParentID,
                            LikeC = comment.LikeC,
                            DislikeC = comment.DislikeC
                        }
                    });
                }

                return Ok(new { success = false, message = "Không thể thêm bình luận" });
            }
            catch (HttpRequestException ex)
            {
                return Ok(new { success = false, error = ex.ToString() });
            }
        }

        [HttpGet]
        public async Task<List<GetCommentDB>> getAllComment()
        {
            List<GetCommentDB> datas = await _apiService.CallApiGetAllComment(CommentConst.urlGetAllComment);
            datas = datas?.OrderByDescending(x => x.LikeC).ThenByDescending(x => x.Time)?.ToList()??new();
            return datas;
        }
        [HttpPost]
        public async Task<string> IncrementLike(UpdateLike updateLike)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(updateLike), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(CommentConst.urlUpdateLike, content);
                return data;
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }
        [HttpPost]
        public async Task<string> DecrementLike(UpdateLike updateLike)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(updateLike), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(CommentConst.urlUpdateLikeDecrement, content);
                return data;
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }
        [HttpPost]
        public async Task<string> IncrementDisLike(UpdateDislike updateDislike)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(updateDislike), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(CommentConst.urlUpdateDislike, content);
                return data;
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }
        [HttpPost]
        public async Task<string> DecrementDisLike(UpdateDislike updateDislike)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(updateDislike), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(CommentConst.urlUpdateDislikeDecrement, content);
                return data;
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }
    }
}
