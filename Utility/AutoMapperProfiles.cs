using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetApiCourse.DTOs;
using dotnetApiCourse.Entities;

namespace dotnetApiCourse.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AuthorCreateDTO, Author>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<BookCreateDTO, Book>()
            .ForMember(book => book.AuthorBook, options => options.MapFrom(MapAuthorsBooks));
            CreateMap<Book, BookDTO>();
            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }

        private List<AuthorBook> MapAuthorsBooks(BookCreateDTO bookCreateDTO, Book book)
        {
            List<AuthorBook> result = [];

            if (bookCreateDTO.AuthorsIds == null)
            {
                return result;
            }

            foreach (var authorId in bookCreateDTO.AuthorsIds)
            {
                result.Add(new AuthorBook() { AuthorId = authorId });
            }

            return result;
        }
    }
}