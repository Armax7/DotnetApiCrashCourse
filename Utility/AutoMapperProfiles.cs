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

            CreateMap<Author, AuthorDTOwBooks>()
            .ForMember(authorDto => authorDto.Books, options => options.MapFrom(MapAuthorDtoBooks));

            CreateMap<BookCreateDTO, Book>()
            .ForMember(book => book.AuthorBook, options => options.MapFrom(MapAuthorsBooks));

            CreateMap<Book, BookDTO>();

            CreateMap<Book, BookDTOwAuthors>()
            .ForMember(bookDto => bookDto.Authors, options => options.MapFrom(MapBookDtoAuthors));

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

        private List<AuthorDTO> MapBookDtoAuthors(Book book, BookDTO bookDto)
        {
            List<AuthorDTO> result = [];

            if (book.AuthorBook == null)
            {
                return result;
            }

            foreach (var authorBook in book.AuthorBook)
            {
                result.Add(new AuthorDTO()
                {
                    Id = authorBook.AuthorId,
                    Name = authorBook.Author.Name
                });
            }
            return result;
        }

        private List<BookDTO> MapAuthorDtoBooks(Author author, AuthorDTO authorDto)
        {
            List<BookDTO> result = [];

            if (author.AuthorBook == null)
            {
                return result;
            }

            foreach (var authorBook in author.AuthorBook)
            {
                result.Add(new BookDTO()
                {
                    Id = authorBook.BookId,
                    Title = authorBook.Book.Title
                });
            }

            return result;
        }
    }
}