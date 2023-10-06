namespace ExamScheduleSystem.DTO
{
    public class PaginationUserDTO
    {
        public string Status { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; }
    }
    public class PaginatedUser<User>
    {
        public Pagination Pagination { get; set; }
        public List<PaginationUserDTO> Data { get; set; }
    }
}
