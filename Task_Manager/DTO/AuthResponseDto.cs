using System.Collections.Generic;

namespace Task_Manager.DTO
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public string Token { get; set; }

        public IList<string> Role { get; set; }
    }
}
