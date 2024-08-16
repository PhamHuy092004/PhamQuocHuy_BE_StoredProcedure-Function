using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhamQuocHuy_BE_StoredProcedure_Function.Model;
using PhamQuocHuy_BE_StoredProcedure_Function.Services;

namespace PhamQuocHuy_BE_StoredProcedure_Function.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IUser usersService;

        public UsersController(IUser usersService) =>
            this.usersService = usersService;

        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetUsers()
        {
            var users = usersService.GetUsers();

            if (users == null || !users.Any())
            {
                // Trả về 204 nếu không có dữ liệu
                return NoContent();
            }
            // Trả về 200 nếu có dữ liệu
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<Users> GetUserById(int id)
        {
            var user = usersService.GetUserID(id);

            if (user == null)
            {
                // Trả về 204 nếu không tìm thấy người dùng
                return NoContent();
            }

            // Trả về 200 nếu tìm thấy người dùng
            return Ok(user);
        }


        [HttpPost]
        public ActionResult<Users> AddUser(Users user, string repass)
        {

            if (ModelState.IsValid)
            {
                if (user.Password != repass)
                {
                    return BadRequest("Password và Repass không khớp.");
                }

                var addedUser = usersService.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { id = addedUser.Id }, addedUser);
            }
            else
            {
                //Nếu sai trạng thái sẽ trả về 406 "Không thể chấp nhận"
                return StatusCode(StatusCodes.Status406NotAcceptable, ModelState);
            }
           
        }
        [HttpPut("{id}")]
        public ActionResult<Users> UpdateUser(int id, [FromBody] Users user)
        {
            if (id != user.Id)
            {
                return BadRequest("ID không khớp với thông tin người dùng.");
            }

            if (!ModelState.IsValid)
            {           
                return StatusCode(StatusCodes.Status406NotAcceptable, ModelState);
            }
            var updatedUser = usersService.UpdateUser(id, user);
            if (updatedUser == null)
            {
                return NotFound("Người dùng không tìm thấy.");
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = usersService.DeleteUser(id);

            if (user != null)
            {
                return Ok(new { message = "Đã xóa thành công User.", user });
            }
            else
            {
                return NotFound(new { message = "User not found." });
            }
        }



    }
}
