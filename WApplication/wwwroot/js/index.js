// Xử lý sự kiện nút bấm menu
var MobileMenu = document.getElementById("mobile-menu");
var subNavigationIconList = document.querySelector(".sub-navigation-icon_list");
if (MobileMenu != null && subNavigationIconList != null) {
    MobileMenu.onclick = function () {
        subNavigationIconList.classList.toggle("open");
    };
}

// Xử lý sự kiện nút bấm Hình ảnh và Video
var btnImage = document.getElementById("btn-imageAndVideo_image");
var btnVideo = document.getElementById("btn-imageAndVideo_video");
var image = document.querySelector(".imageAndVideo_image");
var video = document.querySelector(".imageAndVideo_video");
if (btnVideo != null) {
    btnVideo.onclick = function () {
        video.classList.add("show");
        video.classList.remove("hide");
        image.classList.add("hide");
        image.classList.remove("show");
    };
}
if (btnImage != null) {
    btnImage.onclick = function () {
        image.classList.add("show");
        image.classList.remove("hide");
        video.classList.add("hide");
        video.classList.remove("show");
    };
}

// Modal
const buyBtn = document.querySelector(".js-buy-ticket");
const modal = document.querySelector(".js-modal");
const modalClose = document.querySelector(".js-modal-close");
const modelContain = document.querySelector(".js-model-container");

function showBuyTickets() {
  modal.classList.add("show");
}
function hideBuyTickets() {
  modal.classList.remove("show");
}

if (buyBtn != null) {
    buyBtn.addEventListener("click", showBuyTickets);
}
if (modalClose != null) {
    modalClose.addEventListener("click", hideBuyTickets);
}
if (modal != null) {
    modal.addEventListener("click", hideBuyTickets);
}

// Ngăn chặn sự kiện nổi bọt ra ngoài thẻ cha
if (modelContain != null) { 
    modelContain.addEventListener("click", function (e) {
        e.stopPropagation();
    });
}

// Gửi mail cho khách hàng khi đặt vé
function sendMail() {
  var params = {
    name: document.getElementById("tickets-name").value,
    email: document.getElementById("tickets-mail").value,
    quantity: document.getElementById("tickets-quantity").value,
    phone: document.getElementById("tickets-phone").value,
  };

  const serviceID = "service_erzczjb";
  const templateID = "template_9yvz7di";

  emailjs
    .send(serviceID, templateID, params)
    .then((res) => {
      document.getElementById("tickets-name").value = "";
      document.getElementById("tickets-mail").value = "";
      document.getElementById("tickets-quantity").value = "";
      document.getElementById("tickets-phone").value = "";
      console.log(res);
    })
    .catch((err) => console.log(err));
}

// Lưu thông tin đặt vé của khách hàng xuống database
function buyTickets(event) {
    event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
    var name = $("#tickets-name").val();
    var quantity = $("#tickets-quantity").val();
    var phone = $("#tickets-phone").val();
    var email = $("#tickets-mail").val();
    $.ajax({
        url: "/Home/CallTicketInsert",
        type: "POST",
        dataType: "json",
        data: { // Dữ liệu được gửi đi
            name: name,
            quantity: quantity,
            phone: phone,
            email: email
        },
        success: function (response) {
            console.log("response", response);
            if (response == "1") {
                sendMail();
                alert("Đặt vé thành công");
            } else {
                alert("Đặt vé không thành công");
            }
        },
        error: function (error) {
            alert("Error: " + JSON.stringify(error));
        }
    });
}

// Lưu comment xuống database và đẩy lên lại web
function submitComment(event) {
    event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
    // Lấy giá trị từ các ô nhập liệu
    var name = $("#username").val();
    var email = $("#email").val();
    var content = $("#comment").val();
    $.ajax({
        url: "/HoiDap/CallCommentInsertReload",
        type: "POST",
        dataType: "json",
        data: {
            name: name,
            email: email,
            content: content
        },
        success: function (response) {
            if (response.success == true) {
                var comment = response.comment;
                var newCommentHtml = `
                        <div class="comment-user-container">
                            <div class="comment-user-info">
                                <p class="comment-user-info_name">@${comment.name}</p>
                                <p class="comment-user-info_time">Vừa mới</p>
                            </div>
                            <div class="comment-user-content">
                                ${comment.content}
                            </div>
                            <div class="comment-user-feedback">
                                <div class="thumb_up">
                                    <span class="material-symbols-outlined thumb_up-icon-like" data-id="${comment.id}" onclick="toggleLike(${comment.id})"> thumb_up </span>
                                    <span class="thumb_up-like" data-id="${comment.id}">${comment.likeC}</span>
                                </div>
                                <div class="thumb_down">
                                    <span class="material-symbols-outlined thumb_up-icon-dislike" data-id="${comment.id}" onclick="toggleDislike(${comment.id})"> thumb_down </span>
                                    <span class="thumb_up-dislike" data-id="${comment.id}">${comment.dislikeC}</span>
                                </div>
                                <p data-id="${comment.id}">Phản hồi</p>
                            </div>
                        </div>
                    `;
                $(".comment-list").append(newCommentHtml);
                $("#username").val("");
                $("#email").val("");
                $("#comment").val("");
            } else {
                alert("Có lỗi xảy ra khi thêm bình luận: " + response.message);
            }
        },
        error: function () {
            alert("Có lỗi xảy ra khi gửi yêu cầu AJAX.");
        }
    });
}

// Sử lý sự kiện like và dislike
function toggleLike(id) {
    var iconLikeCountSpan = $(`.thumb_up-icon-like[data-id=${id}]`);
    var likeCountSpan = $(`.thumb_up-like[data-id=${id}]`);
    var likeCount = parseInt(likeCountSpan[0].innerHTML);
    if (iconLikeCountSpan) {
        if (iconLikeCountSpan[0].classList.contains('liked')) {
            likeCountSpan[0].innerText = likeCount - 1;
            iconLikeCountSpan[0].classList.remove('liked');
            Decrement_Like(event, iconLikeCountSpan);
        }
        else {
            likeCountSpan[0].innerText = likeCount + 1;
            iconLikeCountSpan[0].classList.add('liked');
            Increment_Like(event, iconLikeCountSpan);

            // Nếu đã dislike trước đó, xóa dislike
            var iconDisLikeCountSpan = $(`.thumb_up-icon-dislike[data-id=${id}]`);
            if (iconDisLikeCountSpan) {
                if (iconDisLikeCountSpan[0].classList.contains('disliked')) {
                    var dislikeCountSpan = $(`.thumb_up-dislike[data-id=${id}]`);
                    var dislikeCount = parseInt(dislikeCountSpan[0].innerHTML);
                    dislikeCountSpan[0].innerText = dislikeCount - 1;
                    iconDisLikeCountSpan[0].classList.remove('disliked');
                    Decrement_Dislike(event, iconDisLikeCountSpan);
                }
            }
        }
    } else {
        console.log("Lỗi");
    }
}
function toggleDislike(id) {
    var iconDisLikeCountSpan = $(`.thumb_up-icon-dislike[data-id=${id}]`);
    var dislikeCountSpan = $(`.thumb_up-dislike[data-id=${id}]`);
    var dislikeCount = parseInt(dislikeCountSpan[0].innerHTML);
    if (iconDisLikeCountSpan) {
        if (iconDisLikeCountSpan[0].classList.contains('disliked')) {
            dislikeCountSpan[0].innerText = dislikeCount - 1;
            iconDisLikeCountSpan[0].classList.remove('disliked');
            Decrement_Dislike(event, iconDisLikeCountSpan);
        } else {
            dislikeCountSpan[0].innerText = dislikeCount + 1;
            iconDisLikeCountSpan[0].classList.add('disliked');
            Increment_Dislike(event, iconDisLikeCountSpan);

            // Nếu đã like trước đó, xóa like
            var iconLikeCountSpan = $(`.thumb_up-icon-like[data-id=${id}]`);
            if (iconLikeCountSpan[0].classList.contains('liked')) {
                var likeCountSpan = $(`.thumb_up-like[data-id= ${id}]`);
                var likeCount = parseInt(likeCountSpan[0].innerHTML);
                likeCountSpan[0].innerText = likeCount - 1;
                iconLikeCountSpan[0].classList.remove('liked');
                Decrement_Like(event, iconLikeCountSpan);
            }
        }
    } else {
        console.log("Lỗi");
    }
}

// Lưa xuống data khi có lượt like hay dislike
function Increment_Like(event, element) { // => Tăng lượt like
    event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
    var id = $(element).attr("data-id") // Lấy ra giá trị của "data-id" của element có thuộc attribute là "data-id"
    $.ajax({
        url: "/HoiDap/IncrementLike",
        type: "POST",
        dataType: "json",
        data: { // Dữ liệu được gửi đi
            id: id,
        },
        success: function (response) {

        },
        error: function (error) {
            alert("Error: " + JSON.stringify(error));
        }
    });
}
function Decrement_Like(event, element) { // => Giảm lượt like
    event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
    var id = $(element).attr("data-id")
    $.ajax({
        url: "/HoiDap/DecrementLike",
        type: "POST",
        dataType: "json",
        data: { // Dữ liệu được gửi đi
            id: id,
        },
        success: function (response) {

        },
        error: function (error) {
            alert("Error: " + JSON.stringify(error));
        }
    });
}
function Increment_Dislike(event, element) { // => Tăng lượt dislike
    event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
    var id = $(element).attr("data-id")
    $.ajax({
        url: "/HoiDap/IncrementDisLike",
        type: "POST",
        dataType: "json",
        data: { // Dữ liệu được gửi đi
            id: id,
        },
        success: function (response) {

        },
        error: function (error) {
            alert("Error: " + JSON.stringify(error));
        }
    });
}
function Decrement_Dislike(event, element) {
    event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
    var id = $(element).attr("data-id")
    $.ajax({
        url: "/HoiDap/DecrementDisLike",
        type: "POST",
        dataType: "json",
        data: { // Dữ liệu được gửi đi
            id: id,
        },
        success: function (response) {

        },
        error: function (error) {
            alert("Error: " + JSON.stringify(error));
        }
    });
}

// Show form Feedback, gửi xuống database và đẩy lên trang web
function showFormComment(commentId) {
    
    // Lấy ra nơi lưu form feedback theo Id của comment
    var container = document.querySelector('#feedback-form-container-' + commentId);
    // Nếu như đang ẩn
    if (container.style.display === "none") {
        // Xóa hết tất cả các form feedback trước đó
        document.querySelectorAll('.feedback-form').forEach(function (form) {
            form.remove();
        });
        // Ẩn tất cả các thẻ chứa form feedback trước đó
        document.querySelectorAll('.feedback-form-container').forEach(function (form) {
            form.style.display = "none";
        });
        // Tạo ra form điền thông tin feedback
        var formHTML = `
        <form id="feedback-form" class="feedback-form comment-form">
            <div class="form-group">
                <label for="username_feedback_${commentId}">Name:</label>
                <input type="text" id="username_feedback_${commentId}" name="username" required />
            </div>
            <div class="form-group">
                <label for="email_feedback_${commentId}">Email:</label>
                <input type="email" id="email_feedback_${commentId}" name="email" required />
            </div>
            <div class="form-group">
                <label for="comment_feedback_${commentId}">Comment:</label>
                <textarea id="comment_feedback_${commentId}"
                          name="comment"
                          rows="4"
                          required></textarea>
            </div>
            <button type="submit" id="submit-comment_feedback">Submit</button>
        </form>
        `;
        // Thêm vào nơi lưu form feedback và cho nó hiển thị lên
        container.innerHTML = formHTML;
        container.style.display = "block";

        // Bắt sự kiện kiện submit trên form feedback
        $("#submit-comment_feedback").on('click', function (event) {
            event.preventDefault(); // Ngăn chặn hành vi submit mặc định của form
            // Lấy giá trị từ các ô nhập liệu
            var parentId = commentId;
            var name = $(`#username_feedback_${commentId}`).val();
            var email = $(`#email_feedback_${commentId}`).val();
            var content = $(`#comment_feedback_${commentId}`).val();
            $.ajax({
                url: "/HoiDap/CallFeedbackInsertReload",
                type: "POST",
                dataType: "json",
                data: {
                    parentId: parentId,
                    name: name,
                    email: email,
                    content: content
                },
                success: function (response) { // Nếu thành công
                    if (response.success == true) {
                        var comment = response.comment;
                        // Tạo thêm comment mới
                        var newCommentHtml = `
                        <div class="comment-user-container">
                            <div class="comment-user-info">
                                <p class="comment-user-info_name">@${comment.name}</p>
                                <p class="comment-user-info_time">Vừa mới</p>
                            </div>
                            <div class="comment-user-content">
                                ${comment.content}
                            </div>
                            <div class="comment-user-feedback">
                                <div class="thumb_up">
                                    <span class="material-symbols-outlined thumb_up-icon-like" data-id="${comment.id}" onclick="toggleLike(${comment.id})"> thumb_up </span>
                                    <span class="thumb_up-like" data-id="${comment.id}">${comment.likeC}</span>
                                </div>
                                <div class="thumb_down">
                                    <span class="material-symbols-outlined thumb_up-icon-dislike" data-id="${comment.id}" onclick="toggleDislike(${comment.id})"> thumb_down </span>
                                    <span class="thumb_up-dislike" data-id="${comment.id}">${comment.dislikeC}</span>
                                </div>
                            </div>
                        </div>
                        `;
                        // Thêm comment mới vào list_feedback của comment cha
                        $(`.feedback_list[data-id=${commentId}]`).append(newCommentHtml);
                        // Form đưa về các giá trị rỗng
                        $(`#username_feedback_${commentId}`).val("");
                        $(`#email_feedback_${commentId}`).val("");
                        $(`#comment_feedback_${commentId}`).val("");
                        // Ẩn đi form bình luận con
                        const selector = `#feedback-form-container-${commentId}`;
                        document.querySelector(selector).style.display = "none";
                    } else {
                        alert("Có lỗi xảy ra khi thêm bình luận: " + response.message);
                    }
                },
                error: function () {
                    alert("Có lỗi xảy ra khi gửi yêu cầu AJAX.");
                }
            });
        });

    } else {
        container.style.display = "none";
    }
}

