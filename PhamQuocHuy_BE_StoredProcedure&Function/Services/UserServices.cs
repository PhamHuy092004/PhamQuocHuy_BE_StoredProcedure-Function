using Dapper;
using Microsoft.Data.SqlClient;
using PhamQuocHuy_BE_StoredProcedure_Function.Model;
using System.Data;

namespace PhamQuocHuy_BE_StoredProcedure_Function.Services
{
    public class UsersService : IUser
    {
        private readonly IConfiguration _configuration;

        public UsersService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Users> GetUsers()
        {
            // Kết nối database
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                // Gọi stored procedure GETALLUSERS để lấy tất cả người dùng
                var items = connection.Query<Users>("GETALLUSERS", commandType: CommandType.StoredProcedure);

                return items;
            }
        }

        public Users AddUser(Users user)
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                //Sử dụng parameter để truyền tham số cho DynamicParameters 
                parameters.Add("@hoVaTen", user.hoVaTen);
                parameters.Add("@soDienThoai", user.soDienThoai);
                parameters.Add("@ngaySinh", user.ngaySinh.ToString("yyyy-MM-dd"));
                parameters.Add("@gioiTinh", user.gioiTinh);
                parameters.Add("@tinhThanh", user.tinhThanh);
                parameters.Add("@Username", user.Username);
                parameters.Add("@Password", user.Password);
                parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                // Gọi stored procedure AddUser để thêm người dùng
                connection.Execute("AddUser", parameters, commandType: CommandType.StoredProcedure);
                // Lấy giá trị của UserId được trả về từ stored procedure
                var userId = parameters.Get<int>("@UserId");
                user.Id = userId;
                return user;
            }
        }



        public Users UpdateUser(int id, Users user)
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@hoVaTen", user.hoVaTen);
                parameters.Add("@soDienThoai", user.soDienThoai);
                parameters.Add("@ngaySinh", user.ngaySinh.ToString("yyyy-MM-dd"));
                parameters.Add("@gioiTinh", user.gioiTinh);
                parameters.Add("@tinhThanh", user.tinhThanh);
                parameters.Add("@Username", user.Username);
                parameters.Add("@Password", user.Password);
                parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);
                // Gọi stored procedure UpdateUser
                connection.Execute("UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                var rowsAffected = parameters.Get<int>("@RowsAffected");

                if (rowsAffected == 0)
                {
                    return null;
                }

                user.Id = id;
                return user;
            }
        }

        public Users DeleteUser(int id)
        {
            using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", id);
                // Gọi stored procedure DeleteUser và nhận kết quả
                var user = connection.QuerySingleOrDefault<Users>(
                    "DeleteUser",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return user;
            }
        }



    }

}
