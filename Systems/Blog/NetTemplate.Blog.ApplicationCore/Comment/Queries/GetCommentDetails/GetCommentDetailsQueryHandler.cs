using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetTemplate.Blog.ApplicationCore.Comment.Models;
using NetTemplate.Shared.ApplicationCore.Common.Exceptions;

namespace NetTemplate.Blog.ApplicationCore.Comment.Queries.GetCommentDetails
{
    public class GetCommentDetailsQueryHandler : IRequestHandler<GetCommentDetailsQuery, CommentDetailsModel>
    {
        private readonly IValidator<GetCommentDetailsQuery> _validator;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<GetCommentDetailsQueryHandler> _logger;

        public GetCommentDetailsQueryHandler(
            IValidator<GetCommentDetailsQuery> validator,
            ICommentRepository commentRepository,
            IMapper mapper,
            ILogger<GetCommentDetailsQueryHandler> logger)
        {
            _validator = validator;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public async Task<CommentDetailsModel> Handle(GetCommentDetailsQuery request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            IQueryable<CommentDetailsModel> query = await _commentRepository.QueryById<CommentDetailsModel>(request.Id);

            CommentDetailsModel model = query.FirstOrDefault();

            if (model == null) throw new NotFoundException();

            return model;
        }
    }
}
