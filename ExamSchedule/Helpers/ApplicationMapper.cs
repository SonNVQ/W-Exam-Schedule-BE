using AutoMapper;
using ExamSchedule.Data;
using ExamSchedule.Models;

namespace ExamSchedule.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Book, BookModel>().ReverseMap();
        }
    }
}
