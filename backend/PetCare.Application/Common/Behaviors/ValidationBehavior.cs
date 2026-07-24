using FluentValidation;
using MediatR;


namespace PetCare.Application.Common.Behaviors
{    
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
           
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);   

            //ejecuto toda los validators y espero el resultado
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            //capturo todos los errores en una lista
            var errors = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (errors.Count != 0) 
            {
                throw new ValidationException(errors);  //si hay errores, lanzo la exception y no se ejecuta el handler
            }
            else
            {
                return await next();  //si no hay errores, paso al handler
            }

        }
    }
}
