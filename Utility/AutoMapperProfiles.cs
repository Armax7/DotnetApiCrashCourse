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
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorCreateDTO, Author>();

            CreateMap<Author, AuthorDTOwBooks>()
            .ForMember(authorDto => authorDto.Books, options => options.MapFrom(MapAuthorDtoBooks));

            CreateMap<AuthorUpdateDTO, Author>();


            CreateMap<Book, BookDTO>();

            CreateMap<Book, BookDTOwAuthors>()
            .ForMember(bookDto => bookDto.Authors, options => options.MapFrom(MapBookDtoAuthors));

            CreateMap<BookCreateDTO, Book>()
            .ForMember(book => book.AuthorBook, options => options.MapFrom(MapAuthorsBooksOnCreate));

            CreateMap<BookUpdateDTO, Book>()
            .ForMember(book => book.AuthorBook, options => options.MapFrom(MapAuthorsBooksOnUpdate));

            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<CommentUpdateDTO, Comment>();
        }

        private List<AuthorBook> MapAuthorsBooksOnCreate(BookCreateDTO bookCreateDTO, Book book)
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

        private List<AuthorBook> MapAuthorsBooksOnUpdate(BookUpdateDTO bookUpdateDTO, Book book)
        {
            List<AuthorBook> result = [];

            if (bookUpdateDTO.AuthorsIds == null)
            {
                return result;
            }

            foreach (var authorId in bookUpdateDTO.AuthorsIds)
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