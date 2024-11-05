using Dating.Controllers;
using Dating.Data;
using Dating.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Dating.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext; // Thay thế AppDbContext bằng tên DbContext của bạn
        private readonly IHubContext<ChatHub> _hubContext;


        private readonly UsersController _usersController;

        public ChatHub(AppDbContext dbContext, IHubContext<ChatHub> hubContext, UsersController usersController)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
            _usersController = usersController; // Khởi tạo UsersController thông qua DI
        }


    }
}
